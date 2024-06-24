using Microsoft.EntityFrameworkCore;
using Project.Context;
using Project.DTOs;
using Project.Models;

namespace Project.Services;

public class ClientService : IClientService
{
    private readonly MyDbContext _context;

    public ClientService(MyDbContext context)
    {
        _context = context;
    }

    public async Task<bool> AddClient(IndividualClientPostDto clientPostDto)
    {
        Client client = new IndividualClient
        {
            Address = clientPostDto.Address,
            Email = clientPostDto.Email,
            PhoneNumber = clientPostDto.PhoneNumber,
            ClientType = ClientType.Individual,
            FirstName = clientPostDto.FirstName,
            LastName = clientPostDto.LastName,
            PESEL = clientPostDto.PESEL
        };
        
        await _context.Clients.AddAsync(client);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> AddClient(CompanyClientPostDto clientPostDto)
    {
        Client client = new CompanyClient
        {
            Address = clientPostDto.Address,
            Email = clientPostDto.Email,
            PhoneNumber = clientPostDto.PhoneNumber,
            ClientType = ClientType.Company,
            CompanyName = clientPostDto.CompanyName,
            KRS = clientPostDto.KRS
        };
        
        await _context.Clients.AddAsync(client);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<int> RemoveClient(int id)
    {
        var client = await _context.Clients.FindAsync(id);
        if (client == null)
        {
            return -1;
        }

        if (client.ClientType != ClientType.Individual)
        {
            return 0;
        }

        var individualClient = (IndividualClient)client;
        if (individualClient.IsDeleted)
        {
            return -1;
        }

        individualClient.IsDeleted = true;
        await _context.SaveChangesAsync();
        return 1;
    }

    public async Task<bool> UpdateClient(int id, IndividualClientPutDto clientPutDto)
    {
        var client = await _context.Clients.Where(c => c.ClientType == ClientType.Individual).FirstOrDefaultAsync(c => c.Id == id);
        if (client == null)
        {
            return false;
        }
        
        var individualClient = (IndividualClient)client;
        individualClient.Address = clientPutDto.Address;
        individualClient.Email = clientPutDto.Email;
        individualClient.PhoneNumber = clientPutDto.PhoneNumber;
        individualClient.FirstName = clientPutDto.FirstName;
        individualClient.LastName = clientPutDto.LastName;
        
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateClient(int id, CompanyClientPutDto clientPutDto)
    {
        var client = await _context.Clients.Where(c => c.ClientType == ClientType.Company).FirstOrDefaultAsync(c => c.Id == id);
        if (client == null)
        {
            return false;
        }
        
        var companyClient = (CompanyClient)client;
        companyClient.Address = clientPutDto.Address;
        companyClient.Email = clientPutDto.Email;
        companyClient.PhoneNumber = clientPutDto.PhoneNumber;
        companyClient.CompanyName = clientPutDto.CompanyName;
        
        await _context.SaveChangesAsync();
        return true;
    }
}