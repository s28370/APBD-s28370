using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.DTOs;
using Project.Services;

namespace Project.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class RevenueController : ControllerBase
{
    private readonly IRevenueService _revenueService;
    
    public RevenueController(IRevenueService service)
    {
        _revenueService = service;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetRevenue([FromQuery] RevenueRequestDto revenueRequestDto)
    {
        return Ok(await _revenueService.GetRevenue(revenueRequestDto));
    }
}