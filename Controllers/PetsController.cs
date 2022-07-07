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
        public PetsController(ApplicationContext context) {
            _context = context;
        }

        // This is just a stub for GET / to prevent any weird frontend errors that 
        // occur when the route is missing in this controller
        [HttpGet]
        public IEnumerable<Pet> GetPets() {
            return new List<Pet>();
        }

        [HttpGet("{id}")]
        public Pet getPetById(int id){
            return _context.Pets
                .Include(pet => pet.ownedBy)
                .SingleOrDefault(pet => pet.id == id);
        }

        [HttpDelete("{id}")]
        public IActionResult deletePetById(int id){
            Pet pet = _context.Pets.Find(id);
            if (pet == null){
                return NotFound();
            }
            _context.Pets.Remove(pet);
            return NoContent();
        } 

        [HttpPost]
        public IActionResult addPet([FromBody] Pet pet ){
            _context.Pets.Add(pet);
            _context.SaveChanges();
            return CreatedAtAction(nameof(getPetById), new { id = pet.id }, pet);
        }

        [HttpPut("{id}/In")]
        public IActionResult checkInPet(int id){
            Pet pet = _context.Pets.Find(id);
            if (pet == null){
                return NotFound();
            }
            pet.petCheckIn();
            _context.Update(pet);
            _context.SaveChanges();
            return Ok();

        }

        [HttpPut("{id}/In")]
        public IActionResult checkOutPet(int id){
            Pet pet = _context.Pets.Find(id);
            if (pet == null){
                return NotFound();
            }
            pet.petCheckOut();
            _context.Update(pet);
            _context.SaveChanges();
            return Ok();

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
