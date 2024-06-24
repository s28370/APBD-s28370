using Microsoft.EntityFrameworkCore;
using Project.Context;
using Project.DTOs;
using Project.Models;

namespace Project.Services;

public class ContractService : IContractService
{
    private readonly MyDbContext _context;

    public ContractService(MyDbContext context)
    {
        _context = context;
    }
    
    public async Task<string?> CreateContract(ContractDto contractDto)
    {
        if ((contractDto.DateEnd - contractDto.DateStart).TotalDays < 3 || 
            (contractDto.DateEnd - contractDto.DateStart).TotalDays > 30)
        {
            return "The contract duration must be between 3 and 30 days";
        }

        var activeContract = await _context.Contracts
            .Include(c => c.Client)
            .Include(c => c.Software)
            .FirstOrDefaultAsync(c => c.Client.Id == contractDto.ClientId
                                 && c.Software.Id == contractDto.SoftwareId
                                 && c.DateEnd > DateTime.Now);
        if (activeContract != null)
        {
            return "The client already has an active contract for this software";
        }
        
        var client = await _context.Clients.FindAsync(contractDto.ClientId);
        var software = await _context.Softwares.FindAsync(contractDto.SoftwareId);

        if (client == null || software == null)
        {
            return "Invalid client or software ID.";
        }
        
        var finalPrice = software.Cost;
        if (contractDto.ExtendedSupportFor != null && contractDto.ExtendedSupportFor >= 1 && contractDto.ExtendedSupportFor <= 3)
        {
            finalPrice += 1000 * contractDto.ExtendedSupportFor.Value;
        }
        
        var highestDiscount = await _context.Discounts
            .Where(d => d.Client.Id == contractDto.ClientId && d.DateStart <= DateTime.Now && d.DateEnd >= DateTime.Now)
            .OrderByDescending(d => d.Value)
            .FirstOrDefaultAsync();
        if (highestDiscount != null)
        {
            finalPrice -= (finalPrice * highestDiscount.Value / 100);
        }
        
        var previousContracts = await _context.Contracts
            .Where(c => c.Client.Id == contractDto.ClientId && c.DateEnd < DateTime.Now)
            .ToListAsync();
        if (previousContracts.Count != 0)
        {
            finalPrice -= (finalPrice * 0.05);
        }
        
        var contract = new Contract
        {
            Price = finalPrice,
            DateStart = contractDto.DateStart,
            DateEnd = contractDto.DateEnd,
            ExtendedSupportFor = contractDto.ExtendedSupportFor,
            Version = software.Version,
            Client = client,
            Software = software,
            Payments = new List<Payment>()
        };
        
        await _context.Contracts.AddAsync(contract);
        await _context.SaveChangesAsync();

        return null;
    }
}