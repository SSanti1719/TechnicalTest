using Domain.Entities.HR;
using Domain.Entities.Production;
using Domain.Entities.Sales;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Shipper> Shippers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<PredictedOrder> PredictedOrders { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Mapeo completo de Sales.Customers
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("Customers", "Sales");

                entity.HasKey(e => e.CustId);

                entity.Property(e => e.CustId).HasColumnName("custid");
                entity.Property(e => e.CompanyName).HasColumnName("companyname");
                entity.Property(e => e.ContactName).HasColumnName("contactname");
                entity.Property(e => e.ContactTitle).HasColumnName("contacttitle");
                entity.Property(e => e.Address).HasColumnName("address");
                entity.Property(e => e.City).HasColumnName("city");
                entity.Property(e => e.Region).HasColumnName("region");
                entity.Property(e => e.PostalCode).HasColumnName("postalcode");
                entity.Property(e => e.Country).HasColumnName("country");
                entity.Property(e => e.Phone).HasColumnName("phone");
                entity.Property(e => e.Fax).HasColumnName("fax");
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.ToTable("Employees", "HR");

                entity.HasKey(e => e.Empid);

                entity.Property(e => e.Empid).HasColumnName("empid");
                entity.Property(e => e.FirstName).HasColumnName("firstname");
                entity.Property(e => e.LastName).HasColumnName("lastname");
            });

            modelBuilder.Entity<Shipper>(entity =>
            {
                entity.ToTable("Shippers", "Sales");

                entity.HasKey(e => e.ShipperId);

                entity.Property(e => e.ShipperId).HasColumnName("shipperid");
                entity.Property(e => e.Companyname).HasColumnName("companyname");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Products", "Production");

                entity.HasKey(e => e.Productid);

                entity.Property(e => e.Productid).HasColumnName("productid");
                entity.Property(e => e.Productname).HasColumnName("productname");
            });

            // Orders
            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Orders", "Sales");
                entity.HasKey(e => e.OrderId).HasName("PK_Orders");

                entity.Property(e => e.OrderId).HasColumnName("orderid");
                entity.Property(e => e.CustId).HasColumnName("custid");
                entity.Property(e => e.EmpId).HasColumnName("empid");
                entity.Property(e => e.OrderDate).HasColumnName("orderdate");
                entity.Property(e => e.RequiredDate).HasColumnName("requireddate");
                entity.Property(e => e.ShippedDate).HasColumnName("shippeddate");
                entity.Property(e => e.ShipperId).HasColumnName("shipperid");
                entity.Property(e => e.Freight).HasColumnName("freight");
                entity.Property(e => e.ShipName).HasColumnName("shipname").HasMaxLength(40).IsRequired();
                entity.Property(e => e.ShipAddress).HasColumnName("shipaddress").HasMaxLength(60).IsRequired();
                entity.Property(e => e.ShipCity).HasColumnName("shipcity").HasMaxLength(15).IsRequired();
                entity.Property(e => e.ShipRegion).HasColumnName("shipregion").HasMaxLength(15);
                entity.Property(e => e.ShipPostalCode).HasColumnName("shippostalcode").HasMaxLength(10);
                entity.Property(e => e.ShipCountry).HasColumnName("shipcountry").HasMaxLength(15).IsRequired();
            });

            // OrderDetails
            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.ToTable("OrderDetails", "Sales");
                entity.HasKey(e => new { e.OrderId, e.ProductId }).HasName("PK_OrderDetails");

                entity.Property(e => e.OrderId).HasColumnName("orderid");
                entity.Property(e => e.ProductId).HasColumnName("productid");
                entity.Property(e => e.UnitPrice).HasColumnName("unitprice");
                entity.Property(e => e.Qty).HasColumnName("qty");
                entity.Property(e => e.Discount).HasColumnName("discount");
            });

            modelBuilder.Entity<PredictedOrder>().HasNoKey();

        }
    }
}
