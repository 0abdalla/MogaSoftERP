using Microsoft.Extensions.Logging;
using mogaERP.Domain.Contracts.HR.Staff;
using mogaERP.Domain.Interfaces.HR_Module;

namespace mogaERP.Services.Services.HR_Module;
public class StaffService(IUnitOfWork unitOfWork, ILogger<StaffService> logger, IFileService fileService) : IStaffService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ILogger<StaffService> _logger = logger;
    private readonly IFileService _fileService = fileService;

    public async Task<ApiResponse<string>> CreateAsync(StaffRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.FullName))
                return ApiResponse<string>.Failure(new ErrorModel("Full name is required.", AppStatusCode.Failed));

            if (string.IsNullOrWhiteSpace(request.PhoneNumber))
                return ApiResponse<string>.Failure(new ErrorModel("Phone number is required.", AppStatusCode.Failed));

            if (string.IsNullOrWhiteSpace(request.Email))
                return ApiResponse<string>.Failure(new ErrorModel("Email is required.", AppStatusCode.Failed));

            if (!Enum.TryParse<StaffStatus>(request.Status, true, out var status))
                return ApiResponse<string>.Failure(new ErrorModel("Invalid staff status.", AppStatusCode.Failed));

            if (!Enum.TryParse<Gender>(request.Gender, true, out var gender))
                return ApiResponse<string>.Failure(new ErrorModel("Invalid gender.", AppStatusCode.Failed));


            if (!Enum.TryParse<MaritalStatus>(request.MaritalStatus, true, out var maritalStatus))
                return ApiResponse<string>.Failure(new ErrorModel("Invalid Marital Status.", AppStatusCode.Failed));

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
                VacationDays = request.VacationDays,
                VariableSalary = request.VariableSalary,
                VisaCode = request.VisaCode?.Trim(),
                Allowances = request.Allowances,
                Rewards = request.Rewards
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

            return ApiResponse<string>.Success(ErrorModel.None, staff.Id.ToString());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating staff");
            return ApiResponse<string>.Failure(new ErrorModel("Failed to create staff.", AppStatusCode.Failed));
        }
    }

    public Task<ApiResponse<PagedResponse<StaffResponse>>> GetAllAsync(SearchRequest searchRequest, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<ApiResponse<StaffResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<ApiResponse<StaffCountsResponse>> GetStaffCountsAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<ApiResponse<string>> InActiveStaffAsync(int id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<ApiResponse<string>> UpdateAsync(int id, StaffRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
