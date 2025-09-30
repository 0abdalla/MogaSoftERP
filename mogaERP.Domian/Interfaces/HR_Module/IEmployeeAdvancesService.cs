using mogaERP.Domain.Contracts.AccountingModule.AccountTree;
using mogaERP.Domain.Contracts.HR.EmployeeAdvance;

namespace mogaERP.Domain.Interfaces.HR_Module;
public interface IEmployeeAdvancesService
{
    List<EmployeeAdvanceModel> GetAdvancesByEmployeeId(SearchRequest searchRequest, int EmployeeId);
    Task<ApiResponse<string>> AddNewEmployeeAdvance(int EmployeeId, EmployeeAdvanceModel model, CancellationToken cancellationToken = default);
    Task<ApiResponse<string>> EditEmployeeAdvance(int EmployeeId, EmployeeAdvanceModel model);
    Task<ApiResponse<string>> ApproveEmployeeAdvance(int EmployeeAdvanceId, bool IsApproved);
    Task<ApiResponse<string>> DeleteEmployeeAdvance(int EmployeeAdvanceId);
    List<SelectorDataModel> GetAdvanceTypesSelector();
}
