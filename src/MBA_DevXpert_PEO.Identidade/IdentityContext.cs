using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MBA_DevXpert_PEO.Identity;
using Microsoft.AspNetCore.Identity;

namespace MBA_DevXpert_PEO.Identidade.Data
{
    public class IdentityContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public IdentityContext(DbContextOptions<IdentityContext> options)
            : base(options) { }
    }
}
