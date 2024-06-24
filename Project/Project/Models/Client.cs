using System.ComponentModel.DataAnnotations;

namespace Project.Models;

public enum ClientType
{
    Individual,
    Company
}

public abstract class Client
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public string Address { get; set; }
    
    [Required]
    public string Email { get; set; }
    
    [Required]
    public string PhoneNumber { get; set; }
    
    [Required]
    public ClientType ClientType { get; set; }
    
    public ICollection<Discount> Discounts { get; set; }
    
    public ICollection<Contract> Contracts { get; set; }
}

public class IndividualClient : Client
{
    [Required]
    public string FirstName { get; set; }
    
    [Required]
    public string LastName { get; set; }
    
    [Required]
    public string PESEL { get; set; }
    
    public bool IsDeleted { get; set; }
}

public class CompanyClient : Client
{
    [Required]
    public string CompanyName { get; set; }
    
    [Required]
    public string KRS { get; set; }
}