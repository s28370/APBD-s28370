using Microsoft.AspNetCore.Mvc;
using Moq;
using Project.Controllers;
using Project.DTOs;
using Project.Services;

namespace Tests;

public class RevenueControllerTests
{
    private readonly Mock<IRevenueService> _mockRevenueService;
    private readonly RevenueController _controller;

    public RevenueControllerTests()
    {
        _mockRevenueService = new Mock<IRevenueService>();
        _controller = new RevenueController(_mockRevenueService.Object);
    }

    [Fact]
    public async Task GetRevenue_ReturnsOk()
    {
        var revenueRequestDto = new RevenueRequestDto();
        _mockRevenueService.Setup(service => service.GetRevenue(It.IsAny<RevenueRequestDto>())).ReturnsAsync(new RevenueResponseDto{ Revenue = 1000 });

        var result = await _controller.GetRevenue(revenueRequestDto);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<RevenueResponseDto>(okResult.Value);
        Assert.Equal(1000, response.Revenue);
    }
}