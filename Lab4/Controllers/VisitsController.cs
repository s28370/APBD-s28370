using Microsoft.AspNetCore.Mvc;
using Task4.models;

namespace Task4.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VisitsController : ControllerBase
    {
        // List of visits
        private static List<Visit> _visits = new()
        {
            new Visit(){DateOfVisit = DateTime.Today, Description = "Everything ok", AnimalId = Guid.NewGuid(), Price = 20.5},
        };
        
        //Retrieve all visits
        [HttpGet]
        public IActionResult Get() => Ok(_visits);

        //Retrieve visists with specific animal by the Id
        [HttpGet("{animalId}")]
        public IActionResult Get(Guid animalId)
        {
            var visit = _visits.Where(a => a.AnimalId == animalId).ToList();
            if (visit.Count == 0) return NotFound();
            return Ok(visit);
        }

        //Add a new visit
        [HttpPost]
        public IActionResult Post(Visit visit)
        {
            _visits.Add(visit);
            return CreatedAtAction(nameof(Get), new { id = visit.IdVisit }, visit);
        }

    }
}