using mogaERP.Domain.Common;

namespace mogaERP.Infrastructure._Data.Configurations;
public class RoleConfiguration : IEntityTypeConfiguration<ApplicationRole>
{
    public void Configure(EntityTypeBuilder<ApplicationRole> builder)
    {
        builder.HasData(DefaultRoles.All.Select(role => new ApplicationRole
        {
            Id = role.Id,
            Name = role.Name,
            NormalizedName = role.Name.ToUpper(),
            ConcurrencyStamp = role.ConcurrencyStamp
        }));
    }
}