namespace Project.DTOs;

public class RevenueRequestDto
{
    public int? ProductId { get; set; }
    public string? Currency { get; set; }
    
    public bool Predicted { get; set; }
}