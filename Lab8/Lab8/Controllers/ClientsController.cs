using Lab8.Context;
using Lab8.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace Lab8.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ClientsController : ControllerBase
{
    private readonly ApbdContext _context;

        public ClientsController(ApbdContext context)
        {
            _context = context;
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClient(int id)
        {
            if (!_context.Clients.Any())
            {
                return NotFound();
            }
            var client = await _context.Clients.FindAsync(id);
            if (client == null)
            {
                return NotFound();
            }

            var cl = _context.Clients.Where(client1 => client1.IdClient == id)
                .Where(client1 => client1.ClientTrips.Any());
            
            if (cl.Any())
            {
                return BadRequest("Client has assigned trips");
            }

            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    
}