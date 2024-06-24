using Microsoft.EntityFrameworkCore;
using Moq;
using Project.Context;
using Project.DTOs;
using Project.Models;
using Project.Services;

namespace Tests;

public class RevenueServiceTests
{
    private readonly MyDbContext _context;
    private readonly Mock<IRevenueService> _revenueServiceMock;

    public RevenueServiceTests()
    {
        var options = new DbContextOptionsBuilder<MyDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
        _context = new MyDbContext(options);
        _revenueServiceMock = new Mock<IRevenueService>();
    }

    [Fact]
    public async Task GetRevenue_ShouldReturnCorrectRevenue()
    {
        var contract = new Contract { Id = 2, Price = 1000, IsSigned = true };
        await _context.Contracts.AddAsync(contract);
        await _context.SaveChangesAsync();

        var revenueRequestDto = new RevenueRequestDto
        {
            ProductId = null,
            Predicted = false,
            Currency = "pln"
        };

        var revenueService = new RevenueService(_context);
        var response = await revenueService.GetRevenue(revenueRequestDto);

        Assert.Equal(1000, response.Revenue);
        Assert.Equal("pln", response.Currency);
    }
}