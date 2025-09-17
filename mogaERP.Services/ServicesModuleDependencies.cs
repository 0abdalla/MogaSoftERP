using Mapster;
using Microsoft.Extensions.DependencyInjection;
using mogaERP.Domain.Interfaces.AccountingModule;
using mogaERP.Domain.Interfaces.Auth;
using mogaERP.Domain.Interfaces.InventoryModule;
using mogaERP.Domain.Interfaces.ProcurementModule;
using mogaERP.Services.Services.AccountingModule;
using mogaERP.Services.Services.Auth;
using mogaERP.Services.Services.InventoryModule;
using mogaERP.Services.Services.ProcurementModule;
using System.Reflection;

namespace mogaERP.Services;
public static class ServicesModuleDependencies
{
    public static IServiceCollection AddServicesModuleDependencies(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();

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