using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shop_Core.Models;

namespace Shop_Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<Users, IdentityRole<int>, int>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // تعريف العلاقات بين الكيانات المرتبطة
            builder.Entity<ItemsUnits>()
                .HasKey(i => new { i.Unit_Id, i.Item_Id });

            builder.Entity<CustomerStores>()
                .HasKey(x => new { x.Store_Id, x.Cus_Id });

            builder.Entity<InvItemStores>()
                .HasKey(x => new { x.Store_Id, x.Item_Id });

            builder.Entity<ShoppingCartItems>()
                .HasKey(x => new { x.Store_Id, x.Item_Id, x.Cus_Id });

            builder.Entity<InvoiceDetails>()
                .HasKey(x => new { x.Invoice_Id, x.Item_Id });

            // ضبط العلاقة بين Users و Cities
            builder.Entity<Users>()
                .HasOne(u => u.Cities)
                .WithMany(c => c.Users)
                .HasForeignKey(u => u.City_Id)
                .OnDelete(DeleteBehavior.Restrict);

            // ضبط العلاقة بين Users و Classifications
            builder.Entity<Users>()
                .HasOne(u => u.Classifications)
                .WithMany()
                .HasForeignKey(u => u.Class_Id)
                .OnDelete(DeleteBehavior.Restrict);

            // ضبط العلاقة بين Users و Governments
            builder.Entity<Users>()
                .HasOne(u => u.Governments)
                .WithMany()
                .HasForeignKey(u => u.Gov_Id)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Stores>()
       .HasOne(s => s.Cities) // العلاقة مع Cities
       .WithMany(c => c.Stores)
       .HasForeignKey(s => s.City_Id)
       .OnDelete(DeleteBehavior.NoAction);


            // ضبط العلاقة بين Stores و Governments
            builder.Entity<Stores>()
                .HasOne(s => s.Governments)
                .WithMany()
                .HasForeignKey(s => s.Gov_Id)
                .OnDelete(DeleteBehavior.NoAction);

            // ضبط العلاقة بين Items و MainGroup
            builder.Entity<Items>()
                .HasOne(i => i.MainGroup)
                .WithMany()
                .HasForeignKey(i => i.MG_Id)
                .OnDelete(DeleteBehavior.NoAction);

            // ضبط العلاقة بين Items و SubGroup
            builder.Entity<Items>()
                .HasOne(i => i.SubGroup)
                .WithMany()
                .HasForeignKey(i => i.Sub_Id)
                .OnDelete(DeleteBehavior.NoAction);

            
        }

        // تعريف DbSets لجميع الكيانات
        public DbSet<Users> Users { get; set; }
        public DbSet<Cities> Cities { get; set; }
        public DbSet<Governments> Governments { get; set; }
        public DbSet<Classifications> Classifications { get; set; }
        public DbSet<Units> Units { get; set; }
        public DbSet<Items> Items { get; set; }
        public DbSet<MainGroup> MainGroup { get; set; }
        public DbSet<SubGroup> SubGroup { get; set; }
        public DbSet<Invoice> Invoice { get; set; }
        public DbSet<InvoiceDetails> InvoiceDetails { get; set; }
        public DbSet<ShoppingCartItems> ShoppingCartItems { get; set; }
        public DbSet<InvItemStores> InvItemStores { get; set; }
        public DbSet<ItemsUnits> ItemsUnits { get; set; }
        public DbSet<CustomerStores> CustomerStores { get; set; }
        public DbSet<Stores> Stores { get; set; }
    }
}
