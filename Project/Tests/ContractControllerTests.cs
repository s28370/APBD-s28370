using Microsoft.AspNetCore.Mvc;
using Moq;
using Project.Controllers;
using Project.DTOs;
using Project.Services;

namespace Tests;

public class ContractControllerTests
{
    private readonly Mock<IContractService> _mockContractService;
    private readonly ContractController _controller;

    public ContractControllerTests()
    {
        _mockContractService = new Mock<IContractService>();
        _controller = new ContractController(_mockContractService.Object);
    }

    [Fact]
    public async Task CreateContract_ReturnsOk()
    {
        var contractDto = new ContractDto();

        var result = await _controller.CreateContract(contractDto);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Contract created", okResult.Value);
    }

    [Fact]
    public async Task CreateContract_ReturnsBadRequest_IfServiceReturnsError()
    {
        _mockContractService.Setup(service => service.CreateContract(It.IsAny<ContractDto>())).ReturnsAsync("Error");

        var contractDto = new ContractDto();

        var result = await _controller.CreateContract(contractDto);

        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Error", badRequestResult.Value);
    }
}