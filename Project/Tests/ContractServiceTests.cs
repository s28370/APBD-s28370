using Microsoft.EntityFrameworkCore;
using Project.Context;
using Project.DTOs;
using Project.Models;
using Project.Services;

namespace Tests;

public class ContractServiceTests
{
    private readonly MyDbContext _context;
    private readonly ContractService _contractService;

    public ContractServiceTests()
    {
        var options = new DbContextOptionsBuilder<MyDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
        _context = new MyDbContext(options);
        _contractService = new ContractService(_context);
    }

    [Fact]
    public async Task CreateContract_ShouldReturnNull()
    {
        var client = new IndividualClient { Id = 1, ClientType = ClientType.Individual, Address = "test", Email = "test", PhoneNumber = "test", FirstName = "test", LastName = "test", PESEL = "test"};
        var software = new Software { Id = 1, Cost = 1000, Version = 1.0, Description = "test", Name = "test"};

        await _context.Clients.AddAsync(client);
        await _context.Softwares.AddAsync(software);
        await _context.SaveChangesAsync();

        var contractDto = new ContractDto
        {
            ClientId = client.Id,
            SoftwareId = software.Id,
            DateStart = DateTime.Now,
            DateEnd = DateTime.Now.AddDays(5)
        };

        var result = await _contractService.CreateContract(contractDto);

        Assert.Null(result);
    }
}