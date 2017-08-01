using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using NgStore.API.Data;

namespace NgStore.API.Migrations
{
    [DbContext(typeof(NgStoreDBContext))]
    [Migration("20170728101820_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("NgStore.API.Entities.Customer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("City")
                        .HasMaxLength(40);

                    b.Property<string>("Country")
                        .HasMaxLength(40);

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(40);

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(40);

                    b.Property<string>("Phone")
                        .HasMaxLength(20);

                    b.HasKey("Id");

                    b.HasIndex("LastName", "FirstName")
                        .HasName("IndexCustomerName");

                    b.ToTable("Customer");
                });

            modelBuilder.Entity("NgStore.API.Entities.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CustomerId");

                    b.Property<DateTime>("OrderDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("getdate()");

                    b.Property<string>("OrderNumber")
                        .HasMaxLength(10);

                    b.Property<decimal?>("TotalAmount")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("decimal")
                        .HasDefaultValueSql("0");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId")
                        .HasName("IndexOrderCustomerId");

                    b.HasIndex("OrderDate")
                        .HasName("IndexOrderOrderDate");

                    b.ToTable("Order");
                });

            modelBuilder.Entity("NgStore.API.Entities.OrderItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("OrderId");

                    b.Property<int>("ProductId");

                    b.Property<int>("Quantity")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("1");

                    b.Property<decimal>("UnitPrice")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("decimal")
                        .HasDefaultValueSql("0");

                    b.HasKey("Id");

                    b.HasIndex("OrderId")
                        .HasName("IndexOrderItemOrderId");

                    b.HasIndex("ProductId")
                        .HasName("IndexOrderItemProductId");

                    b.ToTable("OrderItem");
                });

            modelBuilder.Entity("NgStore.API.Entities.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("IsDiscontinued")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("0");

                    b.Property<string>("Package")
                        .HasMaxLength(30);

                    b.Property<string>("ProductName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<int>("SupplierId");

                    b.Property<decimal?>("UnitPrice")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("decimal")
                        .HasDefaultValueSql("0");

                    b.HasKey("Id");

                    b.HasIndex("ProductName")
                        .HasName("IndexProductName");

                    b.HasIndex("SupplierId")
                        .HasName("IndexProductSupplierId");

                    b.ToTable("Product");
                });

            modelBuilder.Entity("NgStore.API.Entities.Supplier", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("City")
                        .HasMaxLength(40);

                    b.Property<string>("CompanyName")
                        .IsRequired()
                        .HasMaxLength(40);

                    b.Property<string>("ContactName")
                        .HasMaxLength(50);

                    b.Property<string>("ContactTitle")
                        .HasMaxLength(40);

                    b.Property<string>("Country")
                        .HasMaxLength(40);

                    b.Property<string>("Fax")
                        .HasMaxLength(30);

                    b.Property<string>("Phone")
                        .HasMaxLength(30);

                    b.HasKey("Id");

                    b.HasIndex("CompanyName")
                        .HasName("IndexSupplierName");

                    b.HasIndex("Country")
                        .HasName("IndexSupplierCountry");

                    b.ToTable("Supplier");
                });

            modelBuilder.Entity("NgStore.API.Entities.Order", b =>
                {
                    b.HasOne("NgStore.API.Entities.Customer", "Customer")
                        .WithMany("Order")
                        .HasForeignKey("CustomerId")
                        .HasConstraintName("FK_ORDER_REFERENCE_CUSTOMER");
                });

            modelBuilder.Entity("NgStore.API.Entities.OrderItem", b =>
                {
                    b.HasOne("NgStore.API.Entities.Order", "Order")
                        .WithMany("OrderItem")
                        .HasForeignKey("OrderId")
                        .HasConstraintName("FK_ORDERITE_REFERENCE_ORDER");

                    b.HasOne("NgStore.API.Entities.Product", "Product")
                        .WithMany("OrderItem")
                        .HasForeignKey("ProductId")
                        .HasConstraintName("FK_ORDERITE_REFERENCE_PRODUCT");
                });

            modelBuilder.Entity("NgStore.API.Entities.Product", b =>
                {
                    b.HasOne("NgStore.API.Entities.Supplier", "Supplier")
                        .WithMany("Product")
                        .HasForeignKey("SupplierId")
                        .HasConstraintName("FK_PRODUCT_REFERENCE_SUPPLIER");
                });
        }
    }
}
