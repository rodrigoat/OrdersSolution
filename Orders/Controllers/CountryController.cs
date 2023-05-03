using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;
using Orders.Entities;
using Orders.Interfaces;
using Orders.Repository;

namespace Orders.Controllers;

[Route("[controller]")]
[ApiController]
public class CountryController : ControllerBase
{
    private readonly ICountryRepository _countryRepository;
    public CountryController(ICountryRepository countryRepository)
    {
        _countryRepository = countryRepository;
    }
    [EnableQuery]
    [HttpGet]
    public IQueryable<Country> Get()
    {
        return _countryRepository.GetAll();
    }

    [EnableQuery]
    [HttpGet("{id}")]
    public SingleResult<Country> Get([FromODataUri] int key)
    {
        return SingleResult.Create(_countryRepository.GetById(key));
    }

    [HttpPost]
    public IActionResult Post([FromBody] Country order)
    {
        _countryRepository.Create(order);
        return Created("order", order);
    }

    [HttpPatch]
    public IActionResult Patch([FromODataUri] int key, [FromBody] Country country)
    {
        var countryToEdit = _countryRepository.GetById(key);
        if (!countryToEdit.Any())
        {
            return NotFound();
        }

        _countryRepository.Update(key, country);

        return NoContent();
    }

    [HttpDelete]
    public IActionResult Delete([FromODataUri] int key)
    {
        var order = _countryRepository.GetById(key);

        _countryRepository.Delete(order.First());

        return NoContent();
    }

}
