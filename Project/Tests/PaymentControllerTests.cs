using Microsoft.AspNetCore.Mvc;
using Moq;
using Project.Controllers;
using Project.DTOs;
using Project.Services;

namespace Tests;

public class PaymentControllerTests
{
    private readonly Mock<IPaymentService> _mockPaymentService;
    private readonly PaymentController _controller;

    public PaymentControllerTests()
    {
        _mockPaymentService = new Mock<IPaymentService>();
        _controller = new PaymentController(_mockPaymentService.Object);
    }

    [Fact]
    public async Task CreatePayment_ReturnsOk()
    {
        var paymentDto = new PaymentDto();

        var result = await _controller.CreatePayment(paymentDto);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Payment created", okResult.Value);
    }

    [Fact]
    public async Task CreatePayment_ReturnsBadRequest_IfServiceReturnsError()
    {
        _mockPaymentService.Setup(service => service.CreatePayment(It.IsAny<PaymentDto>())).ReturnsAsync("Error");

        var paymentDto = new PaymentDto();

        var result = await _controller.CreatePayment(paymentDto);

        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Error", badRequestResult.Value);
    }
}