using Microsoft.EntityFrameworkCore;
using Project.Context;
using Project.DTOs;
using Project.Models;

namespace Project.Services;

public class PaymentService : IPaymentService
{
    private readonly MyDbContext _context;

    public PaymentService(MyDbContext context)
    {
        _context = context;
    }
    
    public async Task<string?> CreatePayment(PaymentDto paymentDto)
    {
        var contract = await _context.Contracts
            .Include(c => c.Payments)
            .FirstOrDefaultAsync(c => c.Id == paymentDto.ContractId);

        if (contract == null)
        {
            return "Invalid contract ID";
        }
        
        if (DateTime.Now > contract.DateEnd)
        {
            return "Payment cannot be accepted after the contract end date";
        }
        
        var totalPayments = contract.Payments.Sum(p => p.Amount);
        if (totalPayments + paymentDto.Amount > contract.Price)
        {
            return "Total payments exceed the contract price";
        }
        
        if (totalPayments + paymentDto.Amount == contract.Price)
        {
            contract.IsSigned = true;
        }
        
        var payment = new Payment
        {
            Amount = paymentDto.Amount,
            Contract = contract
        };
        
        await _context.Payments.AddAsync(payment);
        await _context.SaveChangesAsync();

        return null;
    }
}