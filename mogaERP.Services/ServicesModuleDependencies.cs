using FluentValidation;
using FluentValidation.AspNetCore;
using Mapster;
using Microsoft.Extensions.DependencyInjection;
using mogaERP.Domain.Interfaces.AccountingModule;
using mogaERP.Domain.Interfaces.Auth;
using mogaERP.Domain.Interfaces.HR_Module;
using mogaERP.Domain.Interfaces.InventoryModule;
using mogaERP.Domain.Interfaces.ProcurementModule;
using mogaERP.Domain.Interfaces.SalesModule;
using mogaERP.Services.Services.AccountingModule;
using mogaERP.Services.Services.Auth;
using mogaERP.Services.Services.Common;
using mogaERP.Services.Services.HR_Module;
using mogaERP.Services.Services.InventoryModule;
using mogaERP.Services.Services.ProcurementModule;
using mogaERP.Services.Services.SalesModule;
using System.Reflection;

namespace mogaERP.Services;
public static class ServicesModuleDependencies
{
    public static IServiceCollection AddServicesModuleDependencies(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ISQLHelper, SQLHelper>();

        services.AddScoped<IStoreService, StoreService>();
        services.AddScoped<IItemService, ItemService>();
        services.AddScoped<IItemUnitService, ItemUnitService>();
        services.AddScoped<IItemGroupService, ItemGroupService>();
        services.AddScoped<IMainGroupService, MainGroupService>();
        services.AddScoped<IPurchaseRequestService, PurchaseRequestService>();
        services.AddScoped<ISupplierService, SupplierService>();
        services.AddScoped<IPriceQuotationService, PriceQuotationService>();
        services.AddScoped<IPurchaseOrderService, PurchaseOrderService>();
        services.AddScoped<IStoreTypeService, StoreTypeService>();
        services.AddScoped<IReceiptPermissionService, ReceiptPermissionService>();
        services.AddScoped<IDailyRestrictionService, DailyRestrictionService>();
        services.AddScoped<IJobDepartmentService, JobDepartmentService>();
        services.AddScoped<IDisbursementRequestService, DisbursementRequestService>();
        services.AddScoped<IMaterialIssuePermissionService, MaterialIssuePermissionService>();
        services.AddScoped<IBankService, BankService>();
        services.AddScoped<IAdditionNotificationService, AdditionNotificationService>();
        services.AddScoped<IDebitNoticeService, DebitNoticeService>();
        services.AddScoped<IDailyRestrictionService, DailyRestrictionService>();
        services.AddScoped<IRestrictionTypeService, RestrictionTypeService>();
        services.AddScoped<IJobLevelService, JobLevelService>();
        services.AddScoped<IJobTitleService, JobTitleService>();
        services.AddScoped<IJobTypeService, JobTypeService>();
        services.AddScoped<IStaffService, StaffService>();
        services.AddScoped<IFileService, FileService>();
        services.AddScoped<ISalesQuotationService, SalesQuotationService>();
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<IAccountTreeService, AccountTreeService>();
        services.AddScoped<ICostCenterTreeService, CostCenterTreeService>();
        services.AddScoped<IEmployeeAdvancesService, EmployeeAdvancesService>();
        services.AddScoped<IFiscalYearService, FiscalYearService>();
        services.AddScoped<ITreasuryService, TreasuryService>();

        // add fluent validation config
        services
            .AddFluentValidationAutoValidation()
            .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        // add mapster configuration
        services.AddMapsterConfig();

        return services;
    }

    private static IServiceCollection AddMapsterConfig(this IServiceCollection services)
    {
        var mappingConfig = TypeAdapterConfig.GlobalSettings;
        mappingConfig.Scan(Assembly.GetExecutingAssembly());

        services.AddSingleton<IMapper>(new Mapper(mappingConfig));

        return services;
    }
}