using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Orders.Entities;

public class Order
{
    [Key]
    [DatabaseGeneratedAttribute(DatabaseGeneratedOption.None)]
    [DisplayNameAttribute("Order ID")]
    public int OrderId { get; set; }
    [DisplayNameAttribute("Customer ID")]
    public string? CustoumerId { get; set; }
    public double Freight { get; set; }
    [DisplayNameAttribute("Ship Country")]
    [ForeignKey("Country")]
    public int ShipCountryID { get; set; }
    
    public Country? Country { get; set; }
}
