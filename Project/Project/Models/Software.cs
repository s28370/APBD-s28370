using System.ComponentModel.DataAnnotations;

namespace Project.Models;

public class Software
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }
    
    [Required]
    public string Description { get; set; }

    [Required] 
    public double Version { get; set; }
    
    [Required]
    public double Cost { get; set; }
    
    public Category Category { get; set; }
    
    public ICollection<Contract> Contracts { get; set; }
}