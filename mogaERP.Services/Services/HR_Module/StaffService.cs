using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using mogaERP.Domain.Common;
using mogaERP.Domain.Contracts.HR.Staff;
using mogaERP.Domain.Interfaces.HR_Module;

namespace mogaERP.Services.Services.HR_Module;
public class StaffService(
    IUnitOfWork unitOfWork,
    ILogger<StaffService> logger,
    IFileService fileService,
    UserManager<ApplicationUser> userManager) : IStaffService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ILogger<StaffService> _logger = logger;
    private readonly IFileService _fileService = fileService;
    private readonly UserManager<ApplicationUser> _userManager = userManager;

    public async Task<ApiResponse<string>> CreateAsync(StaffRequest request, CancellationToken cancellationToken = default)
    {
        try
        {

            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            var status = Enum.Parse<StaffStatus>(request.Status, true);
            var gender = Enum.Parse<Gender>(request.Gender, true);
            var maritalStatus = Enum.Parse<MaritalStatus>(request.MaritalStatus, true);


            var basicSalary = request.BasicSalary;
            var taxRate = (request.Tax) / 100m;
            var insuranceRate = (request.Insurance) / 100m;
            var allowances = request.Allowances;

            var taxDeduction = basicSalary * taxRate;
            var insuranceDeduction = basicSalary * insuranceRate;
            var totalDeductions = taxDeduction + insuranceDeduction;

            var netSalary = basicSalary - totalDeductions + allowances;

            var staff = new Staff
            {
                FullName = request.FullName.Trim(),
                Code = request.Code?.Trim(),
                Email = request.Email.Trim(),
                PhoneNumber = request.PhoneNumber.Trim(),
                HireDate = request.HireDate,
                Status = status,
                NationalId = request.NationalId?.Trim(),
                MaritalStatus = maritalStatus,
                Address = request.Address?.Trim(),
                Gender = gender,
                Notes = request.Notes?.Trim(),
                IsAuthorized = request.IsAuthorized,
                JobTitleId = request.JobTitleId,
                JobTypeId = request.JobTypeId,
                JobLevelId = request.JobLevelId,
                JobDepartmentId = request.JobDepartmentId,
                BranchId = request.BranchId,
                BasicSalary = request.BasicSalary,
                Tax = request.Tax,
                Insurance = request.Insurance,
                AnnualDays = request.AnnualDays,
                VisaCode = request.VisaCode?.Trim(),
                Allowances = request.Allowances,
                BirthDate = request.BirthDate,
                NetSalary = netSalary,
                Deductions = totalDeductions
            };

            await _unitOfWork.Repository<Staff>().AddAsync(staff, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            // Handle file attachments if any
            if (request.Files != null && request.Files.Count > 0)
            {
                foreach (var file in request.Files)
                {
                    var fileUrl = await _fileService.UploadFileAsync(file, "staff");

                    var attachment = new StaffAttachments
                    {
                        StaffId = staff.Id,
                        FileUrl = fileUrl
                    };
                    await _unitOfWork.Repository<StaffAttachments>().AddAsync(attachment, cancellationToken);
                }
                await _unitOfWork.CompleteAsync(cancellationToken);
            }

            // Handle user account 
            if (request.IsAuthorized && !string.IsNullOrWhiteSpace(request.UserName))
            {
                var user = await _userManager.FindByNameAsync(request.UserName);

                if (user != null)
                {
                    user.Email = request.Email;
                    user.Name = request.FullName;
                    //user.BranchId = request.BranchId;
                    user.IsActive = true;

                    var updateResult = await _userManager.UpdateAsync(user);

                    if (!updateResult.Succeeded)
                    {
                        await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                        return ApiResponse<string>.Failure(AppErrors.TransactionFailed, string.Join("; ", updateResult.Errors.Select(e => e.Description)));
                    }

                    if (!string.IsNullOrWhiteSpace(request.Password))
                    {
                        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                        var passResult = await _userManager.ResetPasswordAsync(user, token, request.Password);

                        if (!passResult.Succeeded)
                        {
                            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                            return ApiResponse<string>.Failure(AppErrors.TransactionFailed, string.Join("; ", passResult.Errors.Select(e => e.Description)));
                        }
                    }
                }
                else if (!string.IsNullOrWhiteSpace(request.Password))
                {
                    var newUser = new ApplicationUser
                    {
                        UserName = request.UserName,
                        Email = request.Email,
                        Name = request.FullName,
                        //BranchId = request.BranchId,
                        IsActive = true,
                    };
                    var createResult = await _userManager.CreateAsync(newUser, request.Password);
                    if (!createResult.Succeeded)
                    {
                        await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                        return ApiResponse<string>.Failure(AppErrors.TransactionFailed, string.Join("; ", createResult.Errors.Select(e => e.Description)));
                    }

                    await _userManager.AddToRoleAsync(newUser, DefaultRoles.Employee.Name);
                }
            }

            await _unitOfWork.CommitTransactionAsync(cancellationToken);

            return ApiResponse<string>.Success(ErrorModel.None, staff.Id.ToString());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating staff");
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            return ApiResponse<string>.Failure(new ErrorModel("Failed to create staff.", AppStatusCode.Failed));
        }
    }

    public async Task<ApiResponse<string>> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        try
        {
            var staff = await _unitOfWork.Repository<Staff>().GetByIdAsync(id, cancellationToken);

            if (staff == null)
                return ApiResponse<string>.Failure(AppErrors.NotFound);

            staff.IsDeleted = true;

            _unitOfWork.Repository<Staff>().Update(staff);

            await _unitOfWork.CompleteAsync(cancellationToken);

            return ApiResponse<string>.Success(AppErrors.DeleteSuccess);
        }
        catch (Exception)
        {
            return ApiResponse<string>.Failure(AppErrors.TransactionFailed);
        }
    }

    public async Task<ApiResponse<PagedResponse<StaffListResponse>>> GetAllAsync(SearchRequest searchRequest, CancellationToken cancellationToken = default)
    {
        try
        {
            var spec = new StaffSpecification(searchRequest);

            var staff = await _unitOfWork.Repository<Staff>().ListAsync(spec, cancellationToken);

            var countSpec = new StaffSpecification(searchRequest);
            countSpec.DisablePagination();
            var totalCount = await _unitOfWork.Repository<Staff>().CountBySpecAsync(countSpec, cancellationToken);

            var staffResponses = staff.Select(s => new StaffListResponse
            {
                Id = s.Id,
                FullName = s.FullName,
                Status = s.Status.ToString(),
                JobTitleId = s.JobTitleId,
                JobTitleName = s.JobTitle?.Name,
                JobDepartmentId = s.JobDepartmentId,
                JobDepartmentName = s.JobDepartment?.Name,

            }).ToList();

            var pagedResponse = new PagedResponse<StaffListResponse>(staffResponses, totalCount, searchRequest.PageNumber, searchRequest.PageSize);

            return ApiResponse<PagedResponse<StaffListResponse>>.Success(AppErrors.Success, pagedResponse);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all staff members");
            return ApiResponse<PagedResponse<StaffListResponse>>.Failure(AppErrors.TransactionFailed);
        }
    }

    public async Task<ApiResponse<StaffResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var spec = new StaffSpecification(id);
            var staff = await _unitOfWork.Repository<Staff>().GetEntityWithSpecAsync(spec, cancellationToken);

            if (staff == null)
                return ApiResponse<StaffResponse>.Failure(AppErrors.NotFound);

            var response = new StaffResponse
            {
                Id = staff.Id,
                FullName = staff.FullName,
                Code = staff.Code,
                Email = staff.Email,
                PhoneNumber = staff.PhoneNumber,
                HireDate = staff.HireDate,
                Status = staff.Status.ToString(),
                NationalId = staff.NationalId,
                MaritalStatus = staff.MaritalStatus?.ToString(),
                Address = staff.Address,
                Gender = staff.Gender.ToString(),
                Notes = staff.Notes,
                AttachmentsUrls = staff.StaffAttachments?.Select(a => a.FileUrl).ToList() ?? [],
                IsAuthorized = staff.IsAuthorized,
                JobTitleId = staff.JobTitleId,
                JobTitleName = staff.JobTitle?.Name,
                JobTypeId = staff.JobTypeId,
                JobTypeName = staff.JobType?.Name,
                JobDepartmentName = staff.JobDepartment?.Name,
                JobLevelName = staff.JobLevel?.Name,
                JobLevelId = staff.JobLevelId,
                JobDepartmentId = staff.JobDepartmentId,
                BranchId = staff.BranchId,
                BranchName = staff.Branch?.Name,
                AnnualDays = staff.AnnualDays,
                Tax = staff.Tax,
                Insurance = staff.Insurance,
                VisaCode = staff.VisaCode,
                Allowances = staff.Allowances,
                CreatedById = staff.CreatedById,
                CreatedBy = staff.CreatedBy?.UserName,
                CreatedOn = staff.CreatedOn,
                UpdatedById = staff.UpdatedById,
                UpdatedBy = staff.UpdatedBy?.UserName,
                UpdatedOn = staff.UpdatedOn,
                BasicSalary = staff.BasicSalary,
                BirthDate = staff.BirthDate,
                NetSalary = staff.NetSalary,
                Deductions = staff.Deductions,
            };

            return ApiResponse<StaffResponse>.Success(AppErrors.Success, response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting staff member {Id}", id);
            return ApiResponse<StaffResponse>.Failure(AppErrors.TransactionFailed);
        }
    }

    public async Task<ApiResponse<StaffCountsResponse>> GetStaffCountsAsync(CancellationToken cancellationToken = default)
    {
        var jobTitleCounts = await _unitOfWork.Repository<Staff>()
        .Query()
        .Include(s => s.JobTitle)
        .GroupBy(s => s.JobTitle.Name)
        .Select(g => new { JobTitleName = g.Key, Count = g.Count() })
        .ToDictionaryAsync(x => x.JobTitleName, x => x.Count, cancellationToken);

        var response = new StaffCountsResponse
        {
            JobTitleCounts = jobTitleCounts
        };

        return ApiResponse<StaffCountsResponse>.Success(AppErrors.Success, response);
    }

    public async Task<ApiResponse<string>> InActiveStaffAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var staff = await _unitOfWork.Repository<Staff>().GetByIdAsync(id, cancellationToken);
            if (staff == null || staff.IsDeleted)
                return ApiResponse<string>.Failure(new ErrorModel("Staff not found.", AppStatusCode.NotFound));

            if (staff.Status == StaffStatus.Inactive)
                return ApiResponse<string>.Failure(new ErrorModel("Staff is already inactive.", AppStatusCode.Failed));

            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            staff.Status = StaffStatus.Inactive;

            _unitOfWork.Repository<Staff>().Update(staff);
            await _unitOfWork.CompleteAsync(cancellationToken);

            await _unitOfWork.CommitTransactionAsync(cancellationToken);
            return ApiResponse<string>.Success(ErrorModel.None, staff.Id.ToString());
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            _logger.LogError(ex, "Error deactivating staff {Id}", id);
            return ApiResponse<string>.Failure(new ErrorModel("Failed to deactivate staff.", AppStatusCode.Failed));
        }
    }

    public async Task<ApiResponse<string>> UpdateAsync(int id, StaffRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            var staff = await _unitOfWork.Repository<Staff>().GetByIdAsync(id, cancellationToken);
            if (staff == null || staff.IsDeleted)
                return ApiResponse<string>.Failure(new ErrorModel("Staff not found.", AppStatusCode.NotFound));

            var status = Enum.Parse<StaffStatus>(request.Status, true);
            var gender = Enum.Parse<Gender>(request.Gender, true);
            var maritalStatus = Enum.Parse<MaritalStatus>(request.MaritalStatus, true);

            // Update staff properties
            staff.FullName = request.FullName.Trim();
            staff.Code = request.Code?.Trim();
            staff.Email = request.Email.Trim();
            staff.PhoneNumber = request.PhoneNumber.Trim();
            staff.HireDate = request.HireDate;
            staff.Status = status;
            staff.NationalId = request.NationalId?.Trim();
            staff.MaritalStatus = maritalStatus;
            staff.Address = request.Address?.Trim();
            staff.Gender = gender;
            staff.Notes = request.Notes?.Trim();
            staff.IsAuthorized = request.IsAuthorized;
            staff.JobTitleId = request.JobTitleId;
            staff.JobTypeId = request.JobTypeId;
            staff.JobLevelId = request.JobLevelId;
            staff.JobDepartmentId = request.JobDepartmentId;
            staff.BranchId = request.BranchId;
            staff.BasicSalary = request.BasicSalary;
            staff.Tax = request.Tax;
            staff.Insurance = request.Insurance;
            staff.AnnualDays = request.AnnualDays;
            staff.VisaCode = request.VisaCode?.Trim();
            staff.Allowances = request.Allowances;

            staff.NetSalary = request.BasicSalary
                          - (request.BasicSalary * (request.Tax / 100))
                          - (request.BasicSalary * (request.Insurance / 100))
                          + (request.Allowances);

            staff.Deductions = (request.BasicSalary * (request.Tax / 100))
                              + (request.BasicSalary * (request.Insurance / 100));

            _unitOfWork.Repository<Staff>().Update(staff);
            await _unitOfWork.CompleteAsync(cancellationToken);

            // Handle attachments
            if (staff.StaffAttachments != null && staff.StaffAttachments.Count > 0)
            {
                _unitOfWork.Repository<StaffAttachments>().RemoveRange(staff.StaffAttachments);
            }

            var newAttachments = new List<StaffAttachments>();
            foreach (var file in request.Files)
            {
                if (file.Length > 0)
                {
                    var fileUrl = await _fileService.UploadFileAsync(file, "staff");
                    newAttachments.Add(new StaffAttachments
                    {
                        FileUrl = fileUrl,
                        StaffId = staff.Id
                    });
                }
            }
            await _unitOfWork.Repository<StaffAttachments>().AddRangeAsync(newAttachments, cancellationToken);

            await _unitOfWork.CommitTransactionAsync(cancellationToken);
            return ApiResponse<string>.Success(ErrorModel.None, staff.Id.ToString());
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            _logger.LogError(ex, "Error updating staff {Id}", id);
            return ApiResponse<string>.Failure(new ErrorModel("Failed to update staff.", AppStatusCode.Failed));
        }
    }
}
