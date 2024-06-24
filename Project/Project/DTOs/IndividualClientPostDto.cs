using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Project.DTOs;
public class IndividualClientPostDto
{
    public string Address { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PESEL { get; set; }
}