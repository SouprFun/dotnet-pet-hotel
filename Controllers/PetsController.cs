using System.Net.NetworkInformation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using pet_hotel.Models;
using Microsoft.EntityFrameworkCore;

namespace pet_hotel.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PetsController : ControllerBase
    {
        private readonly ApplicationContext _context;
        public PetsController(ApplicationContext context)
        {
            _context = context;
        }

        // This is just a stub for GET / to prevent any weird frontend errors that 
        // occur when the route is missing in this controller
        [HttpGet]
        public List<Pet> GetPets()
        {
            return _context.Pets
                .Include(pet => pet.petOwner).ToList();
        }

        [HttpGet("{id}")]
        public Pet getPetById(int id)
        {
            Console.WriteLine("get Pet by id: ", id);
            return _context.Pets
                .Include(pet => pet.petOwner)
                .SingleOrDefault(pet => pet.id == id);
        }


        [HttpDelete("{id}")]
        public IActionResult deletePetById(int id)
        {
            Console.WriteLine("delete Pet by id: ", id);
            Pet pet = _context.Pets.Find(id);
            Console.WriteLine(pet);
            if (pet == null)
            {
                return NotFound();
            }
            _context.Pets.Remove(pet);
            _context.SaveChanges();
            return NoContent();
        }



        [HttpPost] // POST /api/Pets
        public IActionResult Post([FromBody] Pet pet)
        { // how to access the http body?
            PetOwner petOwner = _context.PetOwners.SingleOrDefault(m => m.id == pet.petOwnerid);
            if (petOwner == null)
            {
                return NotFound();
            }
            _context.Add(pet);
            _context.SaveChanges();

            Pet newPet = _context.Pets.Include(p => p.petOwner).SingleOrDefault(p => p.id == pet.id);

            return CreatedAtAction(nameof(getPetById), new { id = newPet.id }, newPet);
        }

        [HttpPut("{id}/checkin")]
        public IActionResult checkIn(int id)
        {
            Pet pet = _context.Pets.SingleOrDefault(p => p.id == id);
            if (pet == null)
            {
                return NotFound();
            }
            pet.checkedInAt = DateTime.UtcNow;
            _context.Update(pet);
            _context.SaveChanges();
            return Ok(pet);
        }

        [HttpPut("{id}/checkout")]
        public IActionResult checkOut(int id)
        {
            Pet pet = _context.Pets.SingleOrDefault(p => p.id == id);
            if (pet == null)
            {
                return NotFound();
            }
            pet.checkedInAt = null;
            _context.Update(pet);
            _context.SaveChanges();
            return Ok(pet);
        }



        [HttpPut("{id}")]
        public IActionResult updatePet(int id, [FromBody] Pet pet)
        {
            if (!_context.Pets.Any(p => p.id == id)) return NotFound();
            // pet.checkedInAt = null;
            _context.Update(pet);
            _context.SaveChanges();
            //pet.checkedInAt(null);
            return Ok(_context.Pets.Include(p => p.petOwner).SingleOrDefault(p => p.id == id));

        }
        // [HttpGet]
        // [Route("test")]
        // public IEnumerable<Pet> GetPets() {
        //     PetOwner blaine = new PetOwner{
        //         name = "Blaine"
        //     };

        //     Pet newPet1 = new Pet {
        //         name = "Big Dog",
        //         petOwner = blaine,
        //         color = PetColorType.Black,
        //         breed = PetBreedType.Poodle,
        //     };

        //     Pet newPet2 = new Pet {
        //         name = "Little Dog",
        //         petOwner = blaine,
        //         color = PetColorType.Golden,
        //         breed = PetBreedType.Labrador,
        //     };

        //     return new List<Pet>{ newPet1, newPet2};
        // }
    }
}
