using mogaERP.Domain.Contracts.AccountingModule.AccountTree;

namespace mogaERP.Domain.Interfaces.AccountingModule;
public interface IAccountTreeService
{
    Task<ApiResponse<string>> AddNewAccount(AccountTreeModel Model, CancellationToken cancellationToken = default);
    Task<ApiResponse<string>> EditAccountTree(int AccountId, AccountTreeModel Model, CancellationToken cancellationToken = default);
    Task<ApiResponse<string>> DeleteAccountTree(int AccountId, CancellationToken cancellationToken = default);
    List<AccountTreeModel> GetAccountTreeHierarchicalData(string SearchText);
    string GenerateAccountNumber(int? parentAccountId);
    List<SelectorDataModel> GetAccountTypes();
    List<SelectorDataModel> GetCurrencySelector();
    List<SelectorDataModel> GetAccountsSelector(bool? IsGroup);
    List<SelectorDataModel> GetCostCenterSelector(bool IsParent);
    List<AccountTreeModel> GetAccountTreeData(string SearchText);
}
