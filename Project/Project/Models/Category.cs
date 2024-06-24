using System.ComponentModel.DataAnnotations;

namespace Project.Models;

public class Category
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public string Name { get; set; }
    
    public ICollection<Software> Softwares { get; set; }
}