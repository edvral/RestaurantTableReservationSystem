using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantTableReservationSystem.Data;
using RestaurantTableReservationSystem.Models;
using RestaurantTableReservationSystem.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace RestaurantTableReservationSystem.Controllers
{
    [ApiController]
    [Route("api/restaurants")]
    public class RestaurantsController : ControllerBase
    {
        private readonly RestaurantReservationContext _context;

        public RestaurantsController(RestaurantReservationContext context)
        {
            _context = context;
        }

        // GET: api/restaurants
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Restaurant>>> GetRestaurants()
        {
            var restaurants = await _context.Restaurants.ToListAsync();

            if (restaurants == null || !restaurants.Any())
            {
                return NotFound("No restaurants found.");
            }

            return Ok(restaurants);
        }

        // GET: api/restaurants/{id}
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<Restaurant>> GetRestaurant(int id)
        {
            var restaurant = await _context.Restaurants.FindAsync(id);

            if (restaurant == null)
            {
                return NotFound($"Restaurant with ID {id} not found.");
            }

            return restaurant;
        }

        // POST: api/restaurants
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Restaurant>> PostRestaurant([FromBody] RestaurantDTO restaurantCreateDTO)
        {

            if (!restaurantCreateDTO.Email.Contains("@"))
            {
                return UnprocessableEntity("Email must contain an '@' symbol.");
            }

            var restaurantExists = await _context.Restaurants
                .AnyAsync(r => r.Name == restaurantCreateDTO.Name && r.Location == restaurantCreateDTO.Location);
            
            if (restaurantExists)
            {
                return UnprocessableEntity($"A restaurant with the name '{restaurantCreateDTO.Name}' already exists at location '{restaurantCreateDTO.Location}'.");
            }

            var restaurant = new Restaurant
            {
                Name = restaurantCreateDTO.Name,
                Location = restaurantCreateDTO.Location,
                PhoneNumber = restaurantCreateDTO.PhoneNumber,
                Email = restaurantCreateDTO.Email,
                OpeningHours = restaurantCreateDTO.OpeningHours,
                Description = restaurantCreateDTO.Description
            };

            _context.Restaurants.Add(restaurant);
            await _context.SaveChangesAsync();
         
            return CreatedAtAction(nameof(GetRestaurant), new { id = restaurant.RestaurantId }, restaurant);
        }

        // PUT: api/restaurants/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutRestaurant(int id, [FromBody] RestaurantDTO restaurantUpdateDTO)
        {
            var restaurant = await _context.Restaurants.FindAsync(id);
            if (restaurant == null)
            {
                return NotFound($"Restaurant with ID {id} not found.");
            }

            if (!restaurantUpdateDTO.Email.Contains("@"))
            {
                return UnprocessableEntity("Email must contain an '@' symbol.");
            }

            restaurant.Name = restaurantUpdateDTO.Name;
            restaurant.Location = restaurantUpdateDTO.Location;
            restaurant.PhoneNumber = restaurantUpdateDTO.PhoneNumber;
            restaurant.Email = restaurantUpdateDTO.Email;
            restaurant.OpeningHours = restaurantUpdateDTO.OpeningHours;
            restaurant.Description = restaurantUpdateDTO.Description;

            _context.Entry(restaurant).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/restaurants/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteRestaurant(int id)
        {
            var restaurant = await _context.Restaurants.FindAsync(id);
            if (restaurant == null)
            {
                return NotFound($"Restaurant with ID {id} not found.");
            }

            _context.Restaurants.Remove(restaurant);
            await _context.SaveChangesAsync();

            return NoContent();
        }      
    }
}
