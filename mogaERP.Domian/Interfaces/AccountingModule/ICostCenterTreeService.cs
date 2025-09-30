using mogaERP.Domain.Contracts.AccountingModule.CostCenterTree;
using mogaERP.Domain.Entities;

namespace mogaERP.Domain.Interfaces.AccountingModule;
public interface ICostCenterTreeService
{
    Task<ApiResponse<string>> CreateNewCostCenter(CostCenterTreeModel Model, CancellationToken cancellationToken = default);
    Task<ApiResponse<string>> UpdateCostCenterTree(int CostCenterId, CostCenterTreeModel Model, CancellationToken cancellationToken = default);
    string GenerateCostCenterNumber(int? ParentCostCenterId);
    Task<ApiResponse<string>> DeleteCostCenterTree(int CostCenterId, CancellationToken cancellationToken = default);
    Task<List<CostCenterTree>> GetCostCenterTreeDataBySearch(string? SearchText);
    List<CostCenterTreeModel> GetCostCenterTreeHierarchicalData(string SearchText);
}
