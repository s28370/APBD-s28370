namespace Project.DTOs;

public class ContractDto
{
    public DateTime DateStart { get; set; }
    public DateTime DateEnd { get; set; }
    public int? ExtendedSupportFor { get; set; }
    public int SoftwareId { get; set; }
    public int ClientId { get; set; }
}