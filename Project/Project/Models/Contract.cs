using System.ComponentModel.DataAnnotations;

namespace Project.Models;

public class Contract
{
    [Key]
    public int Id { get; set; }

    [Required]
    public double Price { get; set; }
    
    [Required]
    public DateTime DateStart { get; set; }
    
    [Required]
    public DateTime DateEnd { get; set; }
    
    public int? ExtendedSupportFor { get; set; }

    public bool IsSigned { get; set; }
    
    [Required]
    public double Version { get; set; }

    public ICollection<Payment> Payments { get; set; }
    
    public Software Software { get; set; }
    
    public Client Client { get; set; }
}