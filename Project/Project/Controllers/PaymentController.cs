using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.DTOs;
using Project.Services;

namespace Project.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class PaymentController : ControllerBase
{
    private readonly IPaymentService _paymentService;
    
    public PaymentController(IPaymentService service)
    {
        _paymentService = service;
    }
    
    [HttpPost]
    public async Task<IActionResult> CreatePayment([FromBody] PaymentDto paymentDto)
    {
        var result = await _paymentService.CreatePayment(paymentDto);
        if (result != null)
        {
            return BadRequest(result);
        }

        return Ok("Payment created");
    }
}