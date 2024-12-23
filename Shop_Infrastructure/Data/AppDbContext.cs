using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shop_Core.Models;
using System;

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

            // تعريف العلاقات بين Users و Cities، Classifications، و Governments
            builder.Entity<Users>()
                .HasOne(u => u.Cities)  // علاقة مع Cities
                .WithMany()
                .HasForeignKey(u => u.City_Id)
                .OnDelete(DeleteBehavior.Restrict);  // لا يسمح بحذف المدن إذا كان هناك مستخدم مرتبط بها

            builder.Entity<Users>()
                .HasOne(u => u.Classifications)  // علاقة مع Classifications
                .WithMany()
                .HasForeignKey(u => u.Class_Id)
                .OnDelete(DeleteBehavior.Restrict);  // لا يسمح بحذف التصنيفات إذا كان هناك مستخدم مرتبط بها

            builder.Entity<Users>()
                .HasOne(u => u.Governments)  // علاقة مع Governments
                .WithMany()
                .HasForeignKey(u => u.Gov_Id)
                .OnDelete(DeleteBehavior.Restrict);  // لا يسمح بحذف الحكومات إذا كان هناك مستخدم مرتبط بها

            // تحديد العلاقات بين Stores و Cities و Governments
            builder.Entity<Stores>()
                .HasOne(s => s.Cities)  // علاقة مع Cities
                .WithMany()
                .HasForeignKey(s => s.City_Id)
                .OnDelete(DeleteBehavior.NoAction);  // تغيير إلى NoAction لتجنب تعدد مسارات الحذف

            builder.Entity<Stores>()
                .HasOne(s => s.Governments)  // علاقة مع Governments
                .WithMany()
                .HasForeignKey(s => s.Gov_Id)
                .OnDelete(DeleteBehavior.NoAction);  // تغيير إلى NoAction لتجنب تعدد مسارات الحذف

            // تعريف العلاقات بين Items و MainGroup و SubGroup
            builder.Entity<Items>()
                .HasOne(i => i.MainGroup)  // علاقة مع MainGroup
                .WithMany()
                .HasForeignKey(i => i.MG_Id)
                .OnDelete(DeleteBehavior.NoAction);  // تغيير إلى NoAction لتجنب تعدد مسارات الحذف

            builder.Entity<Items>()
                .HasOne(i => i.SubGroup)  // علاقة مع SubGroup
                .WithMany()
                .HasForeignKey(i => i.Sub_Id)
                .OnDelete(DeleteBehavior.NoAction);  // تغيير إلى NoAction لتجنب تعدد مسارات الحذف

            // تعريف باقي العلاقات مع الـ DbSets الأخرى حسب الحاجة
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
