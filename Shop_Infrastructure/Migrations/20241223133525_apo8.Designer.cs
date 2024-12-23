﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Shop_Infrastructure.Data;

#nullable disable

namespace Shop_Infrastructure.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20241223133525_apo8")]
    partial class apo8
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<int>", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("Shop_Core.Models.Cities", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Gov_Id")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("Gov_Id");

                    b.ToTable("Cities");
                });

            modelBuilder.Entity("Shop_Core.Models.Classifications", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Classifications");
                });

            modelBuilder.Entity("Shop_Core.Models.CustomerStores", b =>
                {
                    b.Property<int>("Store_Id")
                        .HasColumnType("int");

                    b.Property<int>("Cus_Id")
                        .HasColumnType("int");

                    b.HasKey("Store_Id", "Cus_Id");

                    b.HasIndex("Cus_Id");

                    b.ToTable("CustomerStores");
                });

            modelBuilder.Entity("Shop_Core.Models.Governments", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Governments");
                });

            modelBuilder.Entity("Shop_Core.Models.InvItemStores", b =>
                {
                    b.Property<int>("Store_Id")
                        .HasColumnType("int");

                    b.Property<int>("Item_Id")
                        .HasColumnType("int");

                    b.Property<double>("Balance")
                        .HasColumnType("float");

                    b.Property<int>("Factor")
                        .HasColumnType("int");

                    b.Property<DateTime>("LastUpdated")
                        .HasColumnType("datetime2");

                    b.Property<double>("ReservedQuantity")
                        .HasColumnType("float");

                    b.HasKey("Store_Id", "Item_Id");

                    b.HasIndex("Item_Id");

                    b.ToTable("InvItemStores");
                });

            modelBuilder.Entity("Shop_Core.Models.Invoice", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("Cus_Id")
                        .HasColumnType("int");

                    b.Property<double>("NetPrice")
                        .HasColumnType("float");

                    b.Property<int>("Payment_Type")
                        .HasColumnType("int");

                    b.Property<int>("Transaction_Type")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("isClosed")
                        .HasColumnType("bit");

                    b.Property<bool>("isPosted")
                        .HasColumnType("bit");

                    b.Property<bool>("isReviewed")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("Cus_Id");

                    b.ToTable("Invoice");
                });

            modelBuilder.Entity("Shop_Core.Models.InvoiceDetails", b =>
                {
                    b.Property<int>("Invoice_Id")
                        .HasColumnType("int");

                    b.Property<int>("Item_Id")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<double>("Factor")
                        .HasColumnType("float");

                    b.Property<double>("Quantity")
                        .HasColumnType("float");

                    b.Property<int>("Unit_Id")
                        .HasColumnType("int");

                    b.Property<int>("price")
                        .HasColumnType("int");

                    b.HasKey("Invoice_Id", "Item_Id");

                    b.HasIndex("Item_Id");

                    b.ToTable("InvoiceDetails");
                });

            modelBuilder.Entity("Shop_Core.Models.Items", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("MG_Id")
                        .HasColumnType("int");

                    b.Property<int?>("MainGroupId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("SubGroupId")
                        .HasColumnType("int");

                    b.Property<int>("Sub_Id")
                        .HasColumnType("int");

                    b.Property<double>("price")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("MG_Id");

                    b.HasIndex("MainGroupId");

                    b.HasIndex("SubGroupId");

                    b.HasIndex("Sub_Id");

                    b.ToTable("Items");
                });

            modelBuilder.Entity("Shop_Core.Models.ItemsUnits", b =>
                {
                    b.Property<int>("Unit_Id")
                        .HasColumnType("int");

                    b.Property<int>("Item_Id")
                        .HasColumnType("int");

                    b.Property<int>("Factor")
                        .HasColumnType("int");

                    b.HasKey("Unit_Id", "Item_Id");

                    b.HasIndex("Item_Id");

                    b.ToTable("ItemsUnits");
                });

            modelBuilder.Entity("Shop_Core.Models.MainGroup", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("MainGroup");
                });

            modelBuilder.Entity("Shop_Core.Models.ShoppingCartItems", b =>
                {
                    b.Property<int>("Store_Id")
                        .HasColumnType("int");

                    b.Property<int>("Item_Id")
                        .HasColumnType("int");

                    b.Property<int?>("Cus_Id")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<double>("Quantity")
                        .HasColumnType("float");

                    b.Property<int>("Unit_Id")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Store_Id", "Item_Id", "Cus_Id");

                    b.HasIndex("Cus_Id");

                    b.HasIndex("Item_Id");

                    b.ToTable("ShoppingCartItems");
                });

            modelBuilder.Entity("Shop_Core.Models.Stores", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("CitiesId")
                        .HasColumnType("int");

                    b.Property<int>("City_Id")
                        .HasColumnType("int");

                    b.Property<int>("Gov_Id")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CitiesId");

                    b.HasIndex("City_Id");

                    b.HasIndex("Gov_Id");

                    b.ToTable("Stores");
                });

            modelBuilder.Entity("Shop_Core.Models.SubGroup", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("MG_Id")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("MG_Id");

                    b.ToTable("SubGroup");
                });

            modelBuilder.Entity("Shop_Core.Models.Units", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Units");
                });

            modelBuilder.Entity("Shop_Core.Models.Users", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<int?>("CitiesId")
                        .HasColumnType("int");

                    b.Property<int>("City_Id")
                        .HasColumnType("int");

                    b.Property<int>("Class_Id")
                        .HasColumnType("int");

                    b.Property<int?>("ClassificationsId")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<int>("Gov_Id")
                        .HasColumnType("int");

                    b.Property<int?>("GovernmentsId")
                        .HasColumnType("int");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("CitiesId");

                    b.HasIndex("City_Id");

                    b.HasIndex("Class_Id");

                    b.HasIndex("ClassificationsId");

                    b.HasIndex("Gov_Id");

                    b.HasIndex("GovernmentsId");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<int>", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.HasOne("Shop_Core.Models.Users", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.HasOne("Shop_Core.Models.Users", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<int>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<int>", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Shop_Core.Models.Users", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.HasOne("Shop_Core.Models.Users", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Shop_Core.Models.Cities", b =>
                {
                    b.HasOne("Shop_Core.Models.Governments", "Governments")
                        .WithMany("Cities")
                        .HasForeignKey("Gov_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Governments");
                });

            modelBuilder.Entity("Shop_Core.Models.CustomerStores", b =>
                {
                    b.HasOne("Shop_Core.Models.Users", "Users")
                        .WithMany("CustomerStores")
                        .HasForeignKey("Cus_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Shop_Core.Models.Stores", "Stores")
                        .WithMany("CustomerStores")
                        .HasForeignKey("Store_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Stores");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("Shop_Core.Models.InvItemStores", b =>
                {
                    b.HasOne("Shop_Core.Models.Items", "Items")
                        .WithMany("InvItemStores")
                        .HasForeignKey("Item_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Shop_Core.Models.Stores", "Stores")
                        .WithMany("InvItemStores")
                        .HasForeignKey("Store_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Items");

                    b.Navigation("Stores");
                });

            modelBuilder.Entity("Shop_Core.Models.Invoice", b =>
                {
                    b.HasOne("Shop_Core.Models.Users", "Users")
                        .WithMany()
                        .HasForeignKey("Cus_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Users");
                });

            modelBuilder.Entity("Shop_Core.Models.InvoiceDetails", b =>
                {
                    b.HasOne("Shop_Core.Models.Invoice", "Invoice")
                        .WithMany("InvoiceDetails")
                        .HasForeignKey("Invoice_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Shop_Core.Models.Items", "Items")
                        .WithMany()
                        .HasForeignKey("Item_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Invoice");

                    b.Navigation("Items");
                });

            modelBuilder.Entity("Shop_Core.Models.Items", b =>
                {
                    b.HasOne("Shop_Core.Models.MainGroup", "MainGroup")
                        .WithMany()
                        .HasForeignKey("MG_Id")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Shop_Core.Models.MainGroup", null)
                        .WithMany("Items")
                        .HasForeignKey("MainGroupId");

                    b.HasOne("Shop_Core.Models.SubGroup", null)
                        .WithMany("Items")
                        .HasForeignKey("SubGroupId");

                    b.HasOne("Shop_Core.Models.SubGroup", "SubGroup")
                        .WithMany()
                        .HasForeignKey("Sub_Id")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("MainGroup");

                    b.Navigation("SubGroup");
                });

            modelBuilder.Entity("Shop_Core.Models.ItemsUnits", b =>
                {
                    b.HasOne("Shop_Core.Models.Items", "Items")
                        .WithMany("ItemsUnits")
                        .HasForeignKey("Item_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Shop_Core.Models.Units", "Units")
                        .WithMany("ItemsUnits")
                        .HasForeignKey("Unit_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Items");

                    b.Navigation("Units");
                });

            modelBuilder.Entity("Shop_Core.Models.ShoppingCartItems", b =>
                {
                    b.HasOne("Shop_Core.Models.Users", "Users")
                        .WithMany()
                        .HasForeignKey("Cus_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Shop_Core.Models.Items", "Items")
                        .WithMany()
                        .HasForeignKey("Item_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Shop_Core.Models.Stores", "Stores")
                        .WithMany()
                        .HasForeignKey("Store_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Items");

                    b.Navigation("Stores");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("Shop_Core.Models.Stores", b =>
                {
                    b.HasOne("Shop_Core.Models.Cities", null)
                        .WithMany("Stores")
                        .HasForeignKey("CitiesId");

                    b.HasOne("Shop_Core.Models.Cities", "Cities")
                        .WithMany()
                        .HasForeignKey("City_Id")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Shop_Core.Models.Governments", "Governments")
                        .WithMany()
                        .HasForeignKey("Gov_Id")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Cities");

                    b.Navigation("Governments");
                });

            modelBuilder.Entity("Shop_Core.Models.SubGroup", b =>
                {
                    b.HasOne("Shop_Core.Models.MainGroup", "MainGroup")
                        .WithMany("SubGroup")
                        .HasForeignKey("MG_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MainGroup");
                });

            modelBuilder.Entity("Shop_Core.Models.Users", b =>
                {
                    b.HasOne("Shop_Core.Models.Cities", null)
                        .WithMany("Users")
                        .HasForeignKey("CitiesId");

                    b.HasOne("Shop_Core.Models.Cities", "Cities")
                        .WithMany()
                        .HasForeignKey("City_Id")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Shop_Core.Models.Classifications", "Classifications")
                        .WithMany()
                        .HasForeignKey("Class_Id")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Shop_Core.Models.Classifications", null)
                        .WithMany("Users")
                        .HasForeignKey("ClassificationsId");

                    b.HasOne("Shop_Core.Models.Governments", "Governments")
                        .WithMany()
                        .HasForeignKey("Gov_Id")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Shop_Core.Models.Governments", null)
                        .WithMany("Users")
                        .HasForeignKey("GovernmentsId");

                    b.Navigation("Cities");

                    b.Navigation("Classifications");

                    b.Navigation("Governments");
                });

            modelBuilder.Entity("Shop_Core.Models.Cities", b =>
                {
                    b.Navigation("Stores");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("Shop_Core.Models.Classifications", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("Shop_Core.Models.Governments", b =>
                {
                    b.Navigation("Cities");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("Shop_Core.Models.Invoice", b =>
                {
                    b.Navigation("InvoiceDetails");
                });

            modelBuilder.Entity("Shop_Core.Models.Items", b =>
                {
                    b.Navigation("InvItemStores");

                    b.Navigation("ItemsUnits");
                });

            modelBuilder.Entity("Shop_Core.Models.MainGroup", b =>
                {
                    b.Navigation("Items");

                    b.Navigation("SubGroup");
                });

            modelBuilder.Entity("Shop_Core.Models.Stores", b =>
                {
                    b.Navigation("CustomerStores");

                    b.Navigation("InvItemStores");
                });

            modelBuilder.Entity("Shop_Core.Models.SubGroup", b =>
                {
                    b.Navigation("Items");
                });

            modelBuilder.Entity("Shop_Core.Models.Units", b =>
                {
                    b.Navigation("ItemsUnits");
                });

            modelBuilder.Entity("Shop_Core.Models.Users", b =>
                {
                    b.Navigation("CustomerStores");
                });
#pragma warning restore 612, 618
        }
    }
}
