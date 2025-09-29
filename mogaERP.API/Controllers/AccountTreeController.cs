using mogaERP.Domain.Contracts.AccountingModule.AccountTree;
using mogaERP.Domain.Interfaces.AccountingModule;

namespace mogaERP.API.Controllers;

public class AccountTreeController : BaseApiController
{
    private readonly IAccountTreeService _accountTreeService;
    public AccountTreeController(IAccountTreeService accountTreeService)
    {
        _accountTreeService = accountTreeService;
    }

    [HttpPost]
    [Route("AddNewAccount")]
    public async Task<IActionResult> AddNewAccount(AccountTreeModel Model)
    {
        var results = await _accountTreeService.AddNewAccount(Model);
        return results.IsSuccess ? Ok(results) : BadRequest(results);
    }
    [HttpGet]
    [Route("GenerateAccountNumber")]

    public IActionResult GenerateAccountNumber(int? ParentAccountId)
    {
        int? id = ParentAccountId == 0 ? null : ParentAccountId;
        var results = _accountTreeService.GenerateAccountNumber(id);
        return Ok(results);
    }

    [HttpPost]
    [Route("EditAccountTree")]

    public async Task<IActionResult> EditAccountTree(int AccountId, AccountTreeModel Model)
    {
        var results = await _accountTreeService.EditAccountTree(AccountId, Model);
        return results.IsSuccess ? Ok(results) : BadRequest(results);
    }
    [HttpGet]
    [Route("DeleteAccountTree")]

    public async Task<IActionResult> DeleteAccountTree(int AccountId)
    {
        var results = await _accountTreeService.DeleteAccountTree(AccountId);
        return results.IsSuccess ? Ok(results) : BadRequest(results);
    }

    [HttpGet]
    [Route("GetAccountTreeData")]
    public IActionResult GetAccountTreeData(string? SearchText)
    {
        var results = _accountTreeService.GetAccountTreeData(SearchText);
        return Ok(results);
    }

    [HttpGet]
    [Route("GetAccountTreeHierarchicalData")]
    public IActionResult GetAccountTreeHierarchicalData(string? SearchText)
    {
        var results = _accountTreeService.GetAccountTreeHierarchicalData(SearchText);
        return Ok(results);
    }

    [HttpGet]
    [Route("GetAccountsSelector")]
    public List<SelectorDataModel> GetAccountsSelector(bool? IsGroup)
    {
        return _accountTreeService.GetAccountsSelector(IsGroup);
    }

    [HttpGet]
    [Route("GetAccountTypes")]
    public List<SelectorDataModel> GetAccountTypes()
    {
        return _accountTreeService.GetAccountTypes();
    }

    [HttpGet]
    [Route("GetCurrencySelector")]
    public List<SelectorDataModel> GetCurrencySelector()
    {
        return _accountTreeService.GetCurrencySelector();
    }

    [HttpGet]
    [Route("GetCostCenterSelector")]
    public List<SelectorDataModel> GetCostCenterSelector(bool IsParent)
    {
        return _accountTreeService.GetCostCenterSelector(IsParent);
    }
}
