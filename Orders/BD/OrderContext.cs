using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Orders.DTO;
using Orders.Entities;
using System.Data;

namespace Orders.BD;

public class OrderContext : DbContext
{
    private readonly IConfiguration _configuration;
    private readonly string _connectionString;
    public OrderContext(DbContextOptions<OrderContext> options, IConfiguration configuration) : base(options)
    {
        _configuration = configuration;
        _connectionString = _configuration.GetConnectionString("OrderCS");
    }

    public DbSet<Order> Orders { get; set; }
    public DbSet<Country> Countries { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) 
    {
        modelBuilder.Entity<Order>().ToTable("Order");
        modelBuilder.Entity<Country>().ToTable("Country");
    }

    public IDbConnection CreateConnection()
    => new SqlConnection(_connectionString);
}
