using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using pet_hotel.Models;
using Microsoft.EntityFrameworkCore;

namespace pet_hotel.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PetOwnersController : ControllerBase
    {
        private readonly ApplicationContext _context;
        public PetOwnersController(ApplicationContext context)
        {
            _context = context;
        }

        // This is just a stub for GET / to prevent any weird frontend errors that 
        // occur when the route is missing in this controller
        [HttpGet]
        public List<PetOwner> GetPetOwners()
        {
            return _context.PetOwners.Include(owner => owner.pets).ToList();
        }

        [HttpGet("{id}")]
        public PetOwner GetPetOwnerById(int id)
        {
            return _context.PetOwners
                .Include(owner => owner.pets)
                .SingleOrDefault(owner => owner.id == id);
        }

        [HttpPost]
        public IActionResult addPetOwner([FromBody] PetOwner owner)
        {
            _context.PetOwners.Add(owner);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetPetOwnerById), new { id = owner.id }, owner);
        }

        [HttpDelete("{id}")] 
        public IActionResult deletePetOwnerById(int id)
        {
            PetOwner owner = _context.PetOwners.Find(id);
            if (owner == null)
            {
                return NotFound();
            }
            _context.PetOwners.Remove(owner);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpPut("{id}")]
        public IActionResult updatePetOwnerById(int id, [FromBody] PetOwner owner)
        {
            if(!_context.PetOwners.Any(b => b.id == id )) return NotFound();
            _context.Update(owner);
            _context.SaveChanges();
            return Ok(_context.PetOwners.Include(p => p.pets).SingleOrDefault(p => p.id == id));

        }
    }
}
