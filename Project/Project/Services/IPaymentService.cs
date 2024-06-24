using Project.DTOs;

namespace Project.Services;

public interface IPaymentService
{
    Task<string?> CreatePayment(PaymentDto paymentDto);
}