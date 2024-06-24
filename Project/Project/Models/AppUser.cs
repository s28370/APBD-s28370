using System.ComponentModel.DataAnnotations;

namespace Project.Models;

public class AppUser
{
    [Key]
    public int IdUser { get; set; }
        
    [Required]
    public string Username { get; set; }
    
    [Required]
    public string Password { get; set; }
    
    [Required]
    public string Role { get; set; }
    
    [Required]
    public string Salt { get; set; }
    
    [Required]
    public string RefreshToken { get; set; }
    
    [Required]
    public DateTime? RefreshTokenExp { get; set; }
}