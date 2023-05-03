using Dapper;
using Orders.BD;
using Orders.DTO;
using Orders.Entities;
using Orders.Interfaces;
using System.Data;

namespace Orders.Repository;

public class OrderRepository : IOrderRepository
{
    private readonly OrderContext _context;
    private readonly ICountryRepository _countryRepository;

    public OrderRepository(OrderContext context, ICountryRepository countryRepository)
    {
        _context = context;
        _countryRepository = countryRepository;
    }

    public void CreateOrders(IEnumerable<Order> orders)
    {
        var query = "INSERT INTO [dbo].[Order] ([OrderId],[CustoumerId],[Freight],[ShipCountryID]) VALUES(@OrderId, @CustoumerId, @Freight, @ShipCountryID)";

        using (var connection = _context.CreateConnection())
        {
            connection.Open();

            using (var transaction = connection.BeginTransaction())
            {
                foreach (var order in orders)
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("OrderId", order.OrderId, DbType.Int32);
                    parameters.Add("CustoumerId", order.CustoumerId, DbType.String);
                    parameters.Add("Freight", order.Freight, DbType.Double);
                    parameters.Add("ShipCountryID", order.ShipCountryID, DbType.String);

                    connection.Execute(query, parameters, transaction: transaction);
                }

                transaction.Commit();
            }
        }
    }

    public IQueryable<Order> GetAll()
    {
        var query = "SELECT * FROM [dbo].[Order] JOIN [dbo].[Country] on [Order].[ShipCountryID] = [Country].[CountryID]";

        using (var connection = _context.CreateConnection())
        {
            var orders = connection.Query<Order, Country, Order>(query, (order, country) => { order.Country = country; return order; }, splitOn: "CountryId");
            return orders.ToList().AsQueryable();
        }
    }

    public IQueryable<Order> GetById(int id)
    {
        var query = "SELECT * FROM [dbo].[Order] JOIN [dbo].[Country] on [Order].[ShipCountryID] = [Country].[CountryID]  WHERE [OrderId] = @OrderId";

        using (var connection = _context.CreateConnection())
        {
            var parameters = new DynamicParameters();
            parameters.Add("OrderId", id, DbType.Int32);

            var orders = connection.Query<Order, Country, Order>(query,(order, country) => { order.Country = country; return order; },parameters, splitOn: "CountryId");

            return orders.AsQueryable();
        }
    }

    public void Create(Order order)
    {
        var query = "INSERT INTO [dbo].[Order] ([OrderId],[CustoumerId],[Freight],[ShipCountryID]) VALUES(@OrderId, @CustoumerId, @Freight, @ShipCountryID)";

        using (var connection = _context.CreateConnection())
        {
            connection.Open();

            using (var transaction = connection.BeginTransaction())
            {
                var parameters = new DynamicParameters();
                parameters.Add("OrderId", order.OrderId, DbType.Int32);
                parameters.Add("CustoumerId", order.CustoumerId, DbType.String);
                parameters.Add("Freight", order.Freight, DbType.Double);
                parameters.Add("ShipCountryID", order.ShipCountryID, DbType.String);

                connection.Execute(query, parameters, transaction: transaction);
                transaction.Commit();
            }
        }
    }

    public void Update(int key, Order order)
    {
        var query = "UPDATE [dbo].[Order] SET [CustoumerId] = @CustoumerId, [Freight] = @Freight, [ShipCountryID] = @ShipCountryID WHERE [OrderID] = @OrderId";

        using (var connection = _context.CreateConnection())
        {
            connection.Open();

            using (var transaction = connection.BeginTransaction())
            {
                var parameters = new DynamicParameters();
                parameters.Add("OrderID", key, DbType.Int32);
                parameters.Add("CustoumerId", order.CustoumerId, DbType.String);
                parameters.Add("Freight", order.Freight, DbType.Double);
                parameters.Add("ShipCountryID", order.ShipCountryID, DbType.String);

                connection.Execute(query, parameters, transaction: transaction);
                transaction.Commit();
            }
        }
    }

    public void Delete(Order order)
    {
        var query = "DELETE FROM [dbo].[Order] WHERE [OrderID] = @OrderID";

        using (var connection = _context.CreateConnection())
        {
            var parameters = new DynamicParameters();
            parameters.Add("OrderID", order.OrderId, DbType.Int32);

            connection.Execute(query, parameters);
        }
    }
}
