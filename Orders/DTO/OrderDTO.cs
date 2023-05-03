using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.ComponentModel;

namespace Orders.DTO
{
    public class OrderDTO
    {

        public int OrderID { get; set; }

        public string? CustoumerId { get; set; }
        public double Freight { get; set; }

        public CountryDTO Country { get; set; }

        public OrderDTO() { 
        Country= new CountryDTO();
        }

    }
    public class CountryDTO
    {
        public string? CountryId { get; set; }
        public string? CountryName { get; set; }
    }
}
