using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.DTOs;
using Project.Services;

namespace Project.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class ContractController : ControllerBase
{
    private readonly IContractService _contractService;
    
    public ContractController(IContractService service)
    {
        _contractService = service;
    }


    [HttpPost]
    public async Task<IActionResult> CreateContract([FromBody] ContractDto contractDto)
    {
        var result = await _contractService.CreateContract(contractDto);
        if (result != null)
        {
            return BadRequest(result);
        }

        return Ok("Contract created");
    }
}