using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Project.Context;
using Project.DTOs;

namespace Project.Services;

public class RevenueService : IRevenueService
{
    private readonly MyDbContext _context;

    public RevenueService(MyDbContext context)
    {
        _context = context;
    }
    
    public async Task<RevenueResponseDto> GetRevenue(RevenueRequestDto revenueRequestDto)
    {
        double revenue = 0;

        var contractsQuery = _context.Contracts.Include(c => c.Payments).AsQueryable();

        if (revenueRequestDto.ProductId != null)
        {
            contractsQuery = contractsQuery.Where(c => c.Software.Id == revenueRequestDto.ProductId.Value);
        }
        

        var signedContracts = contractsQuery.Where(c => c.IsSigned);

        if (revenueRequestDto.Predicted)
        {
            signedContracts = contractsQuery.Where(c => c.IsSigned || DateTime.Now < c.DateEnd);
        }

        foreach (var contract in signedContracts)
        {
            revenue += contract.Price;
        }
        
        var currency = "pln";
        if (revenueRequestDto.Currency != null && revenueRequestDto.Currency.ToLower() != "pln")
        {
            currency = revenueRequestDto.Currency;
            var exchangeRate = await GetExchangeRateAsync(currency);
            revenue /= exchangeRate;
        }
        
        return new RevenueResponseDto
        {
            Revenue = revenue,
            Currency = currency
        };
    }
    
    private async Task<double> GetExchangeRateAsync(string currency)
    {
        HttpClient client = new HttpClient();
        var response = await client.GetStringAsync($"http://api.nbp.pl/api/exchangerates/rates/a/" + currency);
        var json = JObject.Parse(response);
        var exchangeRate = json["rates"][0]["mid"].Value<double>();
        return exchangeRate;
    }
}