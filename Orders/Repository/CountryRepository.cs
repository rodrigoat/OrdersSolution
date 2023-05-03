using Dapper;
using Orders.BD;
using Orders.Entities;
using Orders.Interfaces;
using System.Data;

namespace Orders.Repository;

public class CountryRepository : ICountryRepository
{
    private readonly OrderContext _context;

    public CountryRepository(OrderContext context )
    {
        _context = context;
    }

    public void CreateCountries(IEnumerable<Country> countries)
    {
        var query = "INSERT INTO [dbo].[Country] ([CountryName]) VALUES(@CountryName)";

        using (var connection = _context.CreateConnection())
        {
            connection.Open();

            using (var transaction = connection.BeginTransaction())
            {
                foreach (var country in countries)
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("CountryName", country.CountryName, DbType.String);

                    connection.Execute(query, parameters, transaction: transaction);
                }

                transaction.Commit();
            }
        }
    }

    public IQueryable<Country> GetAll()
    {
        var query = "SELECT [CountryId],[CountryName] FROM [dbo].[Country]";

        using (var connection = _context.CreateConnection())
        {
            var countries = connection.Query<Country>(query);
            return countries.ToList().AsQueryable();
        }
    }

    public IQueryable<Country> GetById(int id)
    {
        var query = "SELECT [CountryId],[CountryName] FROM [dbo].[Country] WHERE [CountryId] = @CountryId";

        using (var connection = _context.CreateConnection())
        {
            var parameters = new DynamicParameters();
            parameters.Add("CountryId", id, DbType.Int32);

            var countries = connection.Query<Country>(query, parameters);

            return countries.AsQueryable();
        }
    }

    public IQueryable<Country> GetByName(string countryName)
    {
        var query = "SELECT [CountryId],[CountryName] FROM [dbo].[Country] WHERE [CountryName] = @CountryName";

        using (var connection = _context.CreateConnection())
        {
            var parameters = new DynamicParameters();
            parameters.Add("CountryName", countryName, DbType.String);

            var countries = connection.Query<Country>(query, parameters);

            return countries.AsQueryable();
        }
    }
    public void Create(Country order)
    {
        throw new NotImplementedException();
    }

    public void Delete(Country order)
    {
        throw new NotImplementedException();
    }

    public void Update(int key, Country order)
    {
        throw new NotImplementedException();
    }
}
