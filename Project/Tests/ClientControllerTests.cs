using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Project.Context;
using Project.Controllers;
using Project.DTOs;
using Project.Services;

namespace Tests;

public class ClientControllerTests
{
    private readonly Mock<IClientService> _mockClientService;
    private readonly ClientController _controller;

    public ClientControllerTests()
    {
        _mockClientService = new Mock<IClientService>();
        _controller = new ClientController(_mockClientService.Object);

        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.Role, "Admin")
        }, "mock"));

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };
    }

    [Fact]
    public async Task AddIndividualClient_ReturnsOk()
    {
        var clientDto = new IndividualClientPostDto();

        var result = await _controller.AddClient(clientDto);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Client has been added", okResult.Value);
    }

    [Fact]
    public async Task RemoveClient_ReturnsUnauthorized_IfNotAdmin()
    {
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.Role, "Employee")
        }, "mock"));

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };

        var result = await _controller.RemoveClient(1);

        Assert.IsType<UnauthorizedResult>(result);
    }

    [Fact]
    public async Task RemoveClient_ReturnsNotFound_IfClientNotFound()
    {
        _mockClientService.Setup(service => service.RemoveClient(It.IsAny<int>())).ReturnsAsync(-1);

        var result = await _controller.RemoveClient(1);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("There is no client with this id", notFoundResult.Value);
    }
}
