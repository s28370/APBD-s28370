using Project.DTOs;

namespace Project.Services;

public interface IRevenueService
{
    Task<RevenueResponseDto> GetRevenue(RevenueRequestDto revenueRequestDto);
}