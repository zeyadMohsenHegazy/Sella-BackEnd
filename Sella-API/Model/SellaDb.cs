using Microsoft.EntityFrameworkCore;

namespace Sella_API.Model
{
    public class SellaDb : DbContext
    {
        public SellaDb():base()
        {

        }
        public SellaDb(DbContextOptions options):base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CartProducts>()
                .HasKey(cp => new { cp.CartID, cp.ProductID }); // define composite primary key

            modelBuilder.Entity<CartProducts>()
           .HasOne(cp => cp.Cart)
           .WithMany(c => c.CartProducts)
           .IsRequired();

            modelBuilder.Entity<CartProducts>()
            .HasOne(cp => cp.Product)
            .WithMany(p => p.CartProducts)
            .IsRequired();


            modelBuilder.Entity<OrderedProducts>()
               .HasKey(cp => new { cp.OrderID, cp.ProductID }); // define composite primary key

            modelBuilder.Entity<OrderedProducts>()
           .HasOne(cp => cp.Order)
           .WithMany(c => c.OrderedProducts)
           .IsRequired();

            modelBuilder.Entity<OrderedProducts>()
            .HasOne(cp => cp.Product)
            .WithMany(p => p.OrderedProducts)
            .IsRequired();
        }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (!optionsBuilder.IsConfigured)
        //    {
        //        optionsBuilder.UseSqlServer("Data Source=.; Initial Catalog=Sella; Integrated Security=true;TrustServerCertificate=true; Trusted_Connection=true; ");

        //    }
        //}

        public  DbSet<User> Users { get; set; }
        public  DbSet<Order> Orders { get; set; }
        public  DbSet<Category> Categories { get; set; }
        public  DbSet<Product> Products { get; set; }
        public  DbSet<ProductImages> ProductImages { get; set; }
        public  DbSet<OrderedProducts> OrderedProducts { get; set; }
        public  DbSet<Customer> Customers { get; set; }
        public  DbSet<Cart> Carts { get; set; }
        public  DbSet<CartProducts> CartProducts { get; set; }

    }
}
