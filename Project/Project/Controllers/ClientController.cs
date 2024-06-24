using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.DTOs;
using Project.Services;

namespace Project.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class ClientController : ControllerBase
{
    private readonly IClientService _clientService;
    
    public ClientController(IClientService service)
    {
        _clientService = service;
    }
    
    [HttpPost("individual")]
    public async Task<ActionResult> AddClient([FromBody] IndividualClientPostDto clientPostDto)
    {
        await _clientService.AddClient(clientPostDto);
        return Ok("Client has been added");
    }
    
    [HttpPost("company")]
    public async Task<ActionResult> AddClient([FromBody] CompanyClientPostDto clientPostDto)
    {
        await _clientService.AddClient(clientPostDto);
        return Ok("Client has been added");
    }
    
    [HttpDelete("{id}")]
    public async Task<ActionResult> RemoveClient(int id)
    {
        var userRole = User.Claims.FirstOrDefault(c => c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role")?.Value;

        if (userRole != "Admin")
        {
            return Unauthorized();
        }
        
        var response = await _clientService.RemoveClient(id);
        if (response == -1)
        {
            return NotFound("There is no client with this id");
        }

        if (response == 0)
        {
            return BadRequest("Only individual clients can be deleted");
        }

        return Ok("Client has been deleted");
    }
    
    [HttpPut("individual/{id}")]
    public async Task<ActionResult> UpdateClient(int id, [FromBody] IndividualClientPutDto clientPutDto)
    {
        if (!await _clientService.UpdateClient(id, clientPutDto))
        {
            return NotFound("There is no client with this id");
        }

        return Ok("Client has been updated");
    }
    
    
    [HttpPut("company/{id}")]
    public async Task<ActionResult> UpdateClient(int id, [FromBody] CompanyClientPutDto clientPutDto)
    {
        var userRole = User.Claims.FirstOrDefault(c => c.Type == "role")?.Value;

        if (userRole != "Admin")
        {
            return Unauthorized();
        }
        
        if (!await _clientService.UpdateClient(id, clientPutDto))
        {
            return NotFound("There is no client with this id");
        }

        return Ok("Client has been updated");
    }
}