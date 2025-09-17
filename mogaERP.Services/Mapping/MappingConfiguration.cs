using Mapster;
using mogaERP.Domain.Contracts.InventoryModule.Items;
using mogaERP.Domain.Contracts.ProcurementModule.Supplier;

namespace mogaERP.Services.Mapping;
public class MappingConfiguration : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<ItemRequest, Item>()
            .Ignore(dest => dest.Id)
            .Ignore(dest => dest.CreatedOn)
            .Ignore(dest => dest.UpdatedOn)
            .Ignore(dest => dest.CreatedById)
            .Ignore(dest => dest.UpdatedById);

        config.NewConfig<Item, ItemResponse>()
            .Map(dest => dest.UnitName, src => src.Unit != null ? src.Unit.Name : string.Empty)
            .Map(dest => dest.GroupName, src => src.Group != null ? src.Group.Name : string.Empty)
            .Map(dest => dest.CreatedBy, src => src.CreatedBy != null ? src.CreatedBy.UserName : string.Empty)
            .Map(dest => dest.UpdatedBy, src => src.UpdatedBy != null ? src.UpdatedBy.UserName : string.Empty)
            .Map(dest => dest.MainGroupId, src => src.Group != null ? src.Group.MainGroupId : null)
            .Map(dest => dest.MainGroupName, src => src.Group != null ? src.Group.MainGroup.Name : string.Empty);


        config.NewConfig<SupplierRequest, Supplier>()
            .Ignore(dest => dest.Id)
            .Ignore(dest => dest.PaymentType)
            .Ignore(dest => dest.CreatedOn)
            .Ignore(dest => dest.UpdatedOn)
            .Ignore(dest => dest.CreatedById)
            .Ignore(dest => dest.UpdatedById);

        config.NewConfig<Supplier, SupplierResponse>()
            .Map(dest => dest.CreatedBy, src => src.CreatedBy != null ? src.CreatedBy.UserName : string.Empty)
            .Map(dest => dest.UpdatedBy, src => src.UpdatedBy != null ? src.UpdatedBy.UserName : string.Empty)
            .Map(dest => dest.PaymentType, src => src.PaymentType.ToString());

    }
}
