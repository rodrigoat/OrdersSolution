using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;
using Orders.DTO;
using Orders.Entities;
using Orders.Interfaces;
using Orders.Repository;
using Syncfusion.EJ2.Linq;
using Syncfusion.EJ2.Spreadsheet;
using Syncfusion.XlsIO;
using System.Linq;

namespace Orders.Controllers;

[Route("[controller]")]

[ApiController]
public class OrderController : ControllerBase
{
    private readonly IOrderRepository _orderRepository;
    private readonly ICountryRepository _countryRepository;
    public OrderController(IOrderRepository orderRepository, ICountryRepository countryRepository)
    {
        _orderRepository = orderRepository;
        _countryRepository = countryRepository;
    }

    [AcceptVerbs("Post")]
    [HttpPost]
    [Route("Open")]
    public IActionResult Open(IFormCollection openRequest)
    {
        OpenRequest open = new OpenRequest();
        open.File = openRequest.Files[0];
        return Content(Workbook.Open(open));
    }

    [AcceptVerbs("Post")]
    [HttpPost]
    [Route("Save")]
    public void Save([FromForm] SaveSettings saveSettings)
    {
        ExcelEngine excelEngine = new ExcelEngine();
        IApplication application = excelEngine.Excel;

        Stream inputStream = Workbook.Save(saveSettings).FileStream;
        IWorkbook workbook = application.Workbooks.Open(inputStream);
        IWorksheet worksheet = workbook.Worksheets[0];
        IRange usedCells = worksheet.UsedRange;

        List<OrderRowModel> orderRows = worksheet.ExportData<OrderRowModel>(1, 1, usedCells.LastRow, usedCells.LastColumn);

        var countries = (from orderRow in orderRows
                         where orderRow.OrderID != 0
                         select new Country() { CountryName = orderRow.ShipCountry }).DistinctBy(c => c.CountryName);

        _countryRepository.CreateCountries(countries);

        var newCountries = _countryRepository.GetAll();

        var orders = from orderRow in orderRows
                     where orderRow.OrderID != 0
                     select new Order()
                     {
                         OrderId = orderRow.OrderID,
                         CustoumerId = orderRow.CustoumerId,
                         Freight = orderRow.Freight,
                         ShipCountryID = newCountries.First(country => country.CountryName == orderRow.ShipCountry).CountryId
                     };
        _orderRepository.CreateOrders(orders);
    }

    [EnableQuery]
    [HttpGet]
    public IQueryable<Order> Get()
    {
        return _orderRepository.GetAll();
    }

    [EnableQuery]
    [HttpGet("{id}")]
    public SingleResult<Order> Get([FromODataUri] int key)
    {
        return SingleResult.Create(_orderRepository.GetById(key));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Post([FromBody] OrderDTO order)
    {
        Order _order = new Order();
        if(order.OrderID == 0 || order.CustoumerId==null || order.Freight==0 || order.Country.CountryName==null)
            return BadRequest("All Fields are required to add new order.");
        if (order != null)
        {
            var _country = _countryRepository.GetByName(order.Country.CountryName);
            _order.ShipCountryID = _country.FirstOrDefault().CountryId;
            _order.Freight = order.Freight;
            _order.OrderId = order.OrderID;
            _order.CustoumerId = order.CustoumerId;
        }
        _orderRepository.Create(_order);
        return Created("order", order);
    }

    [HttpPatch]
    public IActionResult Patch([FromODataUri] int key, [FromBody] OrderDTO order)
    {
        Order _order = new Order();

        var orderToEdit = _orderRepository.GetById(key);
        if (!orderToEdit.Any())
        {
            return NotFound();
        }
        var _odr = orderToEdit.FirstOrDefault();
        if (!string.IsNullOrEmpty(order.Country.CountryName))
        {
            var _country = _countryRepository.GetByName(order.Country.CountryName);
            _order.ShipCountryID = _country.FirstOrDefault().CountryId;
        }
        else
        {
            _order.ShipCountryID = _odr.ShipCountryID;
        }
       


        _order.Freight =order.Freight > 0?order.Freight:_odr.Freight;
        _order.OrderId = order.OrderID;
        _order.CustoumerId = order.CustoumerId!=null?order.CustoumerId:_odr.CustoumerId;

        _orderRepository.Update(key, _order);

        order.Freight = _order.Freight;
        order.CustoumerId = _order.CustoumerId;
        order.Country.CountryId = _order.ShipCountryID.ToString();

        return new ObjectResult(order);
    }

    [HttpDelete]
    public IActionResult Delete([FromODataUri] int key)
    {
        var order = _orderRepository.GetById(key);
        if (!order.Any())
        {
            return BadRequest("Order not found");
        }

        _orderRepository.Delete(order.First());

        return NoContent();
    }
}