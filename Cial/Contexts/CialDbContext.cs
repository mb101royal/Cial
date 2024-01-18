using Cial.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Cial.Contexts
{
    public class CialDbContext : IdentityDbContext
    {
        public CialDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<OurTeam> OurTeams { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
    }
}
