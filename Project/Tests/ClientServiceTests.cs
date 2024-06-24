using Microsoft.EntityFrameworkCore;
using Project.Context;
using Project.DTOs;
using Project.Models;
using Project.Services;

namespace Tests;

public class ClientServiceTests
{
    private readonly MyDbContext _context;
    private readonly ClientService _clientService;

    public ClientServiceTests()
    {
        var options = new DbContextOptionsBuilder<MyDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
        _context = new MyDbContext(options);
        _clientService = new ClientService(_context);
    }

    [Fact]
    public async Task AddIndividualClient_ShouldReturnTrue()
    {
        var clientDto = new IndividualClientPostDto
        {
            Address = "Test Address",
            Email = "test@example.com",
            PhoneNumber = "123456789",
            FirstName = "John",
            LastName = "Doe",
            PESEL = "12345678901"
        };

        var result = await _clientService.AddClient(clientDto);

        Assert.True(result);
    }

    [Fact]
    public async Task AddCompanyClient_ShouldReturnTrue()
    {
        var clientDto = new CompanyClientPostDto
        {
            Address = "Test Address",
            Email = "test@example.com",
            PhoneNumber = "123456789",
            CompanyName = "Test Company",
            KRS = "1234567890"
        };

        var result = await _clientService.AddClient(clientDto);

        Assert.True(result);
    }

    [Fact]
    public async Task RemoveClient_ShouldReturnOne()
    {
        var client = new IndividualClient
        {
            Address = "Test Address",
            Email = "test@example.com",
            PhoneNumber = "123456789",
            ClientType = ClientType.Individual,
            FirstName = "John",
            LastName = "Doe",
            PESEL = "12345678901"
        };

        await _context.Clients.AddAsync(client);
        await _context.SaveChangesAsync();

        var result = await _clientService.RemoveClient(client.Id);

        Assert.Equal(1, result);
    }

    [Fact]
    public async Task UpdateIndividualClient_ShouldReturnTrue()
    {
        var client = new IndividualClient
        {
            Address = "Test Address",
            Email = "test@example.com",
            PhoneNumber = "123456789",
            ClientType = ClientType.Individual,
            FirstName = "John",
            LastName = "Doe",
            PESEL = "12345678901"
        };

        await _context.Clients.AddAsync(client);
        await _context.SaveChangesAsync();

        var clientDto = new IndividualClientPutDto
        {
            Address = "New Address",
            Email = "new@example.com",
            PhoneNumber = "987654321",
            FirstName = "Jane",
            LastName = "Doe"
        };

        var result = await _clientService.UpdateClient(client.Id, clientDto);

        Assert.True(result);
    }

    [Fact]
    public async Task UpdateCompanyClient_ShouldReturnTrue()
    {
        var client = new CompanyClient
        {
            Address = "Test Address",
            Email = "test@example.com",
            PhoneNumber = "123456789",
            ClientType = ClientType.Company,
            CompanyName = "Test Company",
            KRS = "1234567890"
        };

        await _context.Clients.AddAsync(client);
        await _context.SaveChangesAsync();

        var clientDto = new CompanyClientPutDto
        {
            Address = "New Address",
            Email = "new@example.com",
            PhoneNumber = "987654321",
            CompanyName = "New Company"
        };

        var result = await _clientService.UpdateClient(client.Id, clientDto);

        Assert.True(result);
    }
}