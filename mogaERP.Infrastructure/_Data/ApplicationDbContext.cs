using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using mogaERP.Domain.Interfaces.Common;
using System.Reflection;

namespace mogaERP.Infrastructure._Data;
public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ICurrentUserService currentUserService) : IdentityDbContext<ApplicationUser>(options)
{
    private readonly ICurrentUserService _currentUserService = currentUserService;

    public DbSet<Store> Stores { get; set; }
    public DbSet<Item> Items { get; set; }
    public DbSet<ItemUnit> ItemUnits { get; set; }
    public DbSet<ItemGroup> ItemGroups { get; set; }
    public DbSet<MainGroup> MainGroups { get; set; }
    public DbSet<PurchaseRequest> PurchaseRequests { get; set; }
    public DbSet<PurchaseRequestItem> PurchaseRequestItems { get; set; }
    public DbSet<Supplier> Suppliers { get; set; }
    public DbSet<PriceQuotation> PriceQuotations { get; set; }
    public DbSet<PriceQuotationItem> PriceQuotationItems { get; set; }
    public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
    public DbSet<PurchaseOrderItem> PurchaseOrderItems { get; set; }
    public DbSet<ReceiptPermission> ReceiptPermissions { get; set; }
    public DbSet<ReceiptPermissionItem> ReceiptPermissionItems { get; set; }
    public DbSet<DailyRestriction> DailyRestrictions { get; set; }
    public DbSet<DailyRestrictionDetail> DailyRestrictionDetails { get; set; }
    public DbSet<StoreType> StoreTypes { get; set; }
    public DbSet<AccountTree> AccountTrees { get; set; }
    public DbSet<AccountingGuidance> AccountingGuidances { get; set; }
    public DbSet<CostCenterTree> CostCenterTrees { get; set; }
    public DbSet<JobDepartment> JobDepartments { get; set; }
    public DbSet<DisbursementRequestItem> DisbursementRequestItems { get; set; }
    public DbSet<DisbursementRequest> DisbursementRequests { get; set; }
    public DbSet<MaterialIssuePermission> MaterialIssuePermissions { get; set; }
    public DbSet<MaterialIssueItem> MaterialIssueItems { get; set; }
    public DbSet<Bank> Banks { get; set; }
    public DbSet<AdditionNotice> AdditionNotices { get; set; }




    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        var cascadeFKs = modelBuilder.Model.GetEntityTypes()
       .SelectMany(t => t.GetForeignKeys())
       .Where(fk => fk.DeleteBehavior == DeleteBehavior.Cascade && !fk.IsOwnership);

        foreach (var fk in cascadeFKs)
            fk.DeleteBehavior = DeleteBehavior.Restrict;

        base.OnModelCreating(modelBuilder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries<BaseEntity>();

        foreach (var entityEntry in entries)
        {
            var currentUserId = _currentUserService.UserId!;

            if (entityEntry.State == EntityState.Added)
            {
                entityEntry.Property(e => e.CreatedById).CurrentValue = currentUserId;
            }
            else if (entityEntry.State == EntityState.Modified)
            {
                entityEntry.Property(e => e.UpdatedById).CurrentValue = currentUserId;
                entityEntry.Property(e => e.UpdatedOn).CurrentValue = DateTime.UtcNow;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}
