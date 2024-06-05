using MSA.Common.PostgresMassTransit.PostgresDB;
using MSA.OrderService.Domain;
using Microsoft.EntityFrameworkCore;

namespace MSA.OrderService.Infrastructure.Data
{
    public class MainDbContext : AppDbContextBase
    {
        private readonly string _uuidGenerator = "uuid-ossp";
        private readonly string _uuidAlgorithm = "uuid_generate_v4()";
        private readonly IConfiguration configuration;

        public MainDbContext(
            IConfiguration configuration,
            DbContextOptions<MainDbContext> options) : base(configuration, options)
        {
            this.configuration = configuration;
        }

        public DbSet<Order> Orders { get; set; } = default!;
        public DbSet<OrderDetail> OrderDetails { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasPostgresExtension(_uuidGenerator);

            //Orders
            modelBuilder.Entity<Order>().ToTable("orders");
            modelBuilder.Entity<Order>().HasKey(x => x.Id);
            modelBuilder.Entity<Order>().Property(x => x.Id)
                .HasColumnType("uuid");
                //.HasDefaultValueSql(_uuidAlgorithm);

            //Order Details
            modelBuilder.Entity<OrderDetail>().ToTable("order_details");
            modelBuilder.Entity<OrderDetail>().HasKey(x => x.Id);
            modelBuilder.Entity<OrderDetail>().Property(x => x.Id)
                .HasColumnType("uuid")
                .HasDefaultValueSql(_uuidAlgorithm);

            //Product
            modelBuilder.Entity<Product>().ToTable("products");
            modelBuilder.Entity<Product>().HasKey(x => x.Id);
            modelBuilder.Entity<Product>().Property(x => x.Id)
                .HasColumnType("uuid");

            //Relationship
            modelBuilder.Entity<Order>()
                .HasMany(x => x.OrderDetails);
        }
    }
}