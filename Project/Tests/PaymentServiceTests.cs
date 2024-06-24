using Microsoft.EntityFrameworkCore;
using Project.Context;
using Project.DTOs;
using Project.Models;
using Project.Services;

namespace Tests;

public class PaymentServiceTests
{
    private readonly MyDbContext _context;
    private readonly PaymentService _paymentService;

    public PaymentServiceTests()
    {
        var options = new DbContextOptionsBuilder<MyDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
        _context = new MyDbContext(options);
        _paymentService = new PaymentService(_context);
    }

    [Fact]
    public async Task CreatePayment_ShouldReturnNull()
    {
        var contract = new Contract { Id = 1, DateEnd = DateTime.Now.AddDays(10), Price = 1000 };

        await _context.Contracts.AddAsync(contract);
        await _context.SaveChangesAsync();

        var paymentDto = new PaymentDto
        {
            ContractId = contract.Id,
            Amount = 500
        };

        var result = await _paymentService.CreatePayment(paymentDto);

        Assert.Null(result);
    }
}