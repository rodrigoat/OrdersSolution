using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Orders.Entities;

namespace Orders.DTO;

public class OrderRowModel
{
    [DisplayName("Order ID")]
    public int OrderID { get; set; }
    [DisplayName("Customer ID")]
    public string? CustoumerId { get; set; }
    public double Freight { get; set; }
    [DisplayName("Ship Country")]
    public string? ShipCountry { get; set; }
    
}
