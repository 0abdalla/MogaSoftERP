using Microsoft.Data.SqlClient;
using mogaERP.Domain.Contracts.AccountingModule.AccountTree;
using mogaERP.Domain.Contracts.HR.EmployeeAdvance;
using mogaERP.Domain.Interfaces.HR_Module;

namespace mogaERP.Services.Services.HR_Module;
public class EmployeeAdvancesService : IEmployeeAdvancesService
{
    private IUnitOfWork _unitOfWork;
    private readonly ISQLHelper SQLHelper;

    public EmployeeAdvancesService(IUnitOfWork unitOfWork, ISQLHelper sQLHelper)
    {
        _unitOfWork = unitOfWork;
        SQLHelper = sQLHelper;
    }

    public List<EmployeeAdvanceModel> GetAdvancesByEmployeeId(SearchRequest SearchModel, int EmployeeId)
    {
        SqlParameter[] param = new SqlParameter[3];
        param[0] = new SqlParameter("@StaffId", EmployeeId);
        param[1] = new SqlParameter("@CurrentPage", SearchModel.PageNumber);
        param[2] = new SqlParameter("@PageSize", SearchModel.PageSize);
        var result = SQLHelper.SQLQuery<EmployeeAdvanceModel>("[dbo].[SP_GetEmployeeAdvancesData]", param);
        return result;
    }

    public async Task<ApiResponse<string>> AddNewEmployeeAdvance(int EmployeeId, EmployeeAdvanceModel model, CancellationToken cancellationToken = default)
    {
        try
        {
            var advance = new EmployeeAdvance()
            {
                StaffId = EmployeeId,
                AdvanceName = string.Empty,
                AdvanceTypeId = model.AdvanceTypeId.Value,
                PaymentFromDate = model.PaymentFromDate.Value,
                PaymentToDate = CalcAdvancePaymentToDate(model.AdvanceAmount.Value, model.PaymentAmount.Value, model.PaymentFromDate.Value),
                AdvanceAmount = model.AdvanceAmount.Value,
                PaymentAmount = model.PaymentAmount.Value,
                Notes = model.Notes,
                WorkflowStatusId = model.WorkflowStatusId,
                CreatedBy = model.CreatedBy,
                CreatedDate = DateTime.Now
            };

            var AdvanceNumber = await _unitOfWork.Repository<EmployeeAdvance>().MaxAsync(i => (int?)i.AdvanceNumber) ?? 0;
            advance.AdvanceNumber = AdvanceNumber + 1;

            await _unitOfWork.Repository<EmployeeAdvance>().AddAsync(advance, cancellationToken);
            await _unitOfWork.CompleteAsync();

            return ApiResponse<string>.Success(AppErrors.AddSuccess);
        }
        catch (Exception ex)
        {
            return ApiResponse<string>.Failure(AppErrors.TransactionFailed);
        }

    }

    public async Task<ApiResponse<string>> EditEmployeeAdvance(int EmployeeId, EmployeeAdvanceModel model)
    {
        try
        {
            var advance = await _unitOfWork.Repository<EmployeeAdvance>().GetByIdAsync(model.StaffAdvanceId.Value);
            if (advance != null)
            {
                advance.AdvanceTypeId = model.AdvanceTypeId.Value;
                advance.PaymentFromDate = model.PaymentFromDate.Value;
                advance.PaymentToDate = CalcAdvancePaymentToDate(model.AdvanceAmount.Value, model.PaymentAmount.Value, model.PaymentFromDate.Value);
                advance.AdvanceAmount = model.AdvanceAmount.Value;
                advance.PaymentAmount = model.PaymentAmount.Value;
                advance.Notes = model.Notes;
                advance.WorkflowStatusId = model.WorkflowStatusId;
                advance.ModifiedBy = model.ModifiedBy;
                advance.ModifiedDate = DateTime.Now;

                await _unitOfWork.CompleteAsync();


                return ApiResponse<string>.Success(AppErrors.UpdateSuccess);
            }
            else
                return ApiResponse<string>.Failure(AppErrors.NotFound);
        }
        catch (Exception ex)
        {
            return ApiResponse<string>.Failure(AppErrors.TransactionFailed);
        }
    }

    public async Task<ApiResponse<string>> ApproveEmployeeAdvance(int EmployeeAdvanceId, bool IsApproved)
    {

        try
        {
            var advance = await _unitOfWork.Repository<EmployeeAdvance>().GetByIdAsync(EmployeeAdvanceId);
            if (advance != null)
            {
                advance.WorkflowStatusId = IsApproved ? (int)HRWorkflowStatus.Approved : (int)HRWorkflowStatus.Rejected;
                advance.ModifiedBy = string.Empty;
                advance.ModifiedDate = DateTime.Now;
                await _unitOfWork.CompleteAsync();
                return ApiResponse<string>.Success(AppErrors.StatusChangedSuccess);
            }
            else
                return ApiResponse<string>.Failure(AppErrors.NotFound);
        }
        catch (Exception ex)
        {
            return ApiResponse<string>.Failure(AppErrors.TransactionFailed);
        }

    }
    public async Task<ApiResponse<string>> DeleteEmployeeAdvance(int EmployeeAdvanceId)
    {

        try
        {
            var advance = await _unitOfWork.Repository<EmployeeAdvance>().GetByIdAsync(EmployeeAdvanceId);
            if (advance != null)
            {
                _unitOfWork.Repository<EmployeeAdvance>().Remove(advance);
                await _unitOfWork.CompleteAsync();
                return ApiResponse<string>.Success(AppErrors.DeleteSuccess);
            }
            else
                return ApiResponse<string>.Failure(AppErrors.NotFound);
        }
        catch (Exception ex)
        {
            return ApiResponse<string>.Failure(AppErrors.TransactionFailed);
        }

    }

    public List<SelectorDataModel> GetAdvanceTypesSelector()
    {
        var results = _unitOfWork.Repository<AdvanceType>().Query().Select(b => new SelectorDataModel
        {
            Id = b.AdvanceTypeId,
            Name = b.NameAR,
        }).ToList();
        return results;
    }

    private DateTime CalcAdvancePaymentToDate(double AdvanceAmount, double MonthelyPaymentAmount, DateTime PaymentFromDate)
    {
        double remainingAmount = AdvanceAmount;
        DateTime paymentDate = PaymentFromDate;

        while (remainingAmount > 0)
        {
            remainingAmount -= MonthelyPaymentAmount;
            if (remainingAmount > 0)
            {
                paymentDate = paymentDate.AddMonths(1);
            }
        }

        return paymentDate;
    }
}
