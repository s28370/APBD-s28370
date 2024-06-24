using System.ComponentModel.DataAnnotations;

namespace Project.Models;

public class Discount
{
    [Key]
    public int Id { get; set; }
    
    [Required] 
    public string Name { get; set; }

    [Required]
    public int Value { get; set; }
    
    [Required]
    public DateTime DateStart { get; set; }
    
    [Required]
    public DateTime DateEnd { get; set; }
    
    public Client Client { get; set; }
}