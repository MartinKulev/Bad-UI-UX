using Microsoft.EntityFrameworkCore;
using BadUI.Data.Entities;

namespace BadUI.Data
{
    public class BadUIDbContext : DbContext
    {
        public BadUIDbContext(DbContextOptions<BadUIDbContext> options)
            : base(options)
        {
        }

        public DbSet<CustomUser> Users { get; set; }
    }
}
