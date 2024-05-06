using System.ComponentModel.DataAnnotations;

namespace apbd6.Models;

public class ProductWarehouse
{
    [Required]
    public int IdProduct { get; set; }
    [Required]
    public int IdWarehouse { get; set; }
    [Required]
    [Range(1,Int32.MaxValue, ErrorMessage = "amount musi byc > 0")]
    public int Amount { get; set; }
    [Required]
    public DateTime CreatedAt { get; set; }
    
}