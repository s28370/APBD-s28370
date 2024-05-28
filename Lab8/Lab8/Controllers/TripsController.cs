using Lab8.Context;
using Lab8.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lab8.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TripsController : ControllerBase
{
    private readonly ApbdContext _context;

    public TripsController(ApbdContext context)
    {
        _context = context;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Trip>>> GetTrips()
    {
        if (!_context.Trips.Any())
        {
            return NotFound();
        }
        return await _context.Trips.OrderByDescending(trip => trip.DateFrom).ToListAsync();
    }
    
    [HttpPost("{id}/clients")]
    public async Task<ActionResult<Client>> PostClient(TripRequest tripRequest)
    {
        if (!_context.Trips.Any())
        {
            return NotFound();
        }

        if (_context.Clients.Any(client => client.Pesel == tripRequest.Pesel))
        {
            return BadRequest("Person with given PESEL already exists");
        }

        if (!_context.Trips.Any(trip => trip.IdTrip == tripRequest.IdTrip && trip.DateFrom > DateTime.Now))
        {
            return BadRequest("Trip does not exist or DateFrom has already passed");
        }

        var client = new Client
        {
            FirstName = tripRequest.FirstName,
            LastName = tripRequest.LastName,
            Email = tripRequest.Email,
            Telephone = tripRequest.Telephone,
            Pesel = tripRequest.Pesel
        };
        
        
        await _context.Clients.AddAsync(client);
        await _context.SaveChangesAsync();

        var clientTrip = new ClientTrip
        {
            IdClient = client.IdClient,
            IdTrip = _context.Trips.First(trip => trip.IdTrip == tripRequest.IdTrip).IdTrip,
            RegisteredAt = DateTime.Now,
            PaymentDate = tripRequest.PaymentDate
        };
        
        await _context.ClientTrips.AddAsync(clientTrip);
        await _context.SaveChangesAsync();

        return Ok();
    }
}