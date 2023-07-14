using Microsoft.EntityFrameworkCore;
using API_Sella.Models;

namespace API_Sella.Context
{
    public class SellaDBContext : DbContext
    {
        public SellaDBContext( DbContextOptions<SellaDBContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("users");
        }
    }
}
