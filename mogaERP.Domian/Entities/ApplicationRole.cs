using Microsoft.AspNetCore.Identity;

namespace mogaERP.Domain.Entities;
public class ApplicationRole : IdentityRole
{
    public bool IsDeleted { get; set; }
}

