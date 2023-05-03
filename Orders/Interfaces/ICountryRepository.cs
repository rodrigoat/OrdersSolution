using Orders.Entities;

namespace Orders.Interfaces;

public interface ICountryRepository
{
    public void CreateCountries(IEnumerable<Country> countries);
    public IQueryable<Country> GetAll();
    public IQueryable<Country> GetById(int id);
    public IQueryable<Country> GetByName(string countryName);
    public void Create(Country order);
    public void Update(int key, Country order);
    public void Delete(Country order);
}
