using System.ComponentModel.DataAnnotations;

namespace Project.Models;

public class Payment
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public double Amount { get; set; }
    
    public Contract Contract { get; set; }
}