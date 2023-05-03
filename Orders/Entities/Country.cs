using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Orders.Entities;

public class Country
{
    [Key]
    [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
    [DisplayNameAttribute("Country Id")]
    public int CountryId { get; set; }
    [DisplayNameAttribute("Country Name")]
    public string? CountryName { get; set; }
}
