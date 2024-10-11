using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantTableReservationSystem.Data;
using RestaurantTableReservationSystem.Models;
using RestaurantTableReservationSystem.DTOs;

namespace RestaurantTableReservationSystem.Controllers
{
    [ApiController]
    [Route("api/restaurants/{restaurantId}/tables")]
    public class TablesController : ControllerBase
    {
        private readonly RestaurantReservationContext _context;

        public TablesController(RestaurantReservationContext context)
        {
            _context = context;
        }

        // GET: api/restaurants/{restaurantId}/tables
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Table>>> GetTables(int restaurantId)
        {
            var restaurantExists = await _context.Restaurants.AnyAsync(r => r.RestaurantId == restaurantId);
            if (!restaurantExists)
            {
                return NotFound($"Restaurant with ID {restaurantId} not found.");
            }

            var tables = await _context.Tables.Where(t => t.RestaurantId == restaurantId).ToListAsync();
            if (!tables.Any())
            {
                return NotFound($"Restaurant with ID {restaurantId} has no tables.");
            }
         
            return Ok(tables);
        }

        // GET: api/restaurants/{restaurantId}/tables/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Table>> GetTable(int restaurantId, int id)
        {
            var restaurantExists = await _context.Restaurants.AnyAsync(r => r.RestaurantId == restaurantId);
            if (!restaurantExists)
            {
                return NotFound($"Restaurant with ID {restaurantId} not found.");
            }

            var table = await _context.Tables.FirstOrDefaultAsync(t => t.TableId == id && t.RestaurantId == restaurantId);
            if (table == null)
            {
                return NotFound($"Table with ID {id} not found for restaurant with ID {restaurantId}.");
            }

            return table;
        }

        // POST: api/restaurants/{restaurantId}/tables
        [HttpPost]
        public async Task<ActionResult<Table>> PostTable(int restaurantId, TableDTO tableCreateDTO)
        {
            var restaurantExists = await _context.Restaurants.AnyAsync(r => r.RestaurantId == restaurantId);
            if (!restaurantExists)
            {
                return NotFound($"Restaurant with ID {restaurantId} not found.");
            }

            var tableNumberExists = await _context.Tables
                .AnyAsync(t => t.RestaurantId == restaurantId && t.TableNumber == tableCreateDTO.TableNumber);

            if (tableNumberExists)
            {
                return UnprocessableEntity($"Table with number {tableCreateDTO.TableNumber} already exists in this restaurant.");
            }

            if (tableCreateDTO.Capacity <= 0)
            {
                return UnprocessableEntity("Capacity can't be 0 or less.");
            }

            if (tableCreateDTO.TableNumber < 0)
            {
                return UnprocessableEntity("Table number can't be negative.");
            }

            var table = new Table
            {
                RestaurantId = restaurantId,  
                TableNumber = tableCreateDTO.TableNumber,
                Capacity = tableCreateDTO.Capacity
            };

            _context.Tables.Add(table);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTable", new { restaurantId = restaurantId, id = table.TableId }, table);
        }

        // PUT: api/restaurants/{restaurantId}/tables/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTable(int restaurantId, int id, TableDTO tableUpdateDTO)
        {
            var restaurantExists = await _context.Restaurants.AnyAsync(r => r.RestaurantId == restaurantId);
            if (!restaurantExists)
            {
                return NotFound($"Restaurant with ID {restaurantId} not found.");
            }

            var table = await _context.Tables.FirstOrDefaultAsync(t => t.TableId == id && t.RestaurantId == restaurantId);
            if (table == null)
            {
                return NotFound($"Table with ID {id} not found for restaurant with ID {restaurantId}.");
            }

            var tableNumberExists = await _context.Tables
                 .AnyAsync(t => t.RestaurantId == restaurantId && t.TableNumber == tableUpdateDTO.TableNumber && t.TableId != id);

            if (tableNumberExists)
            {
                return UnprocessableEntity($"Another table with number {tableUpdateDTO.TableNumber} already exists in this restaurant.");
            }


            if (tableUpdateDTO.Capacity <= 0)
            {
                return UnprocessableEntity("Capacity can't be 0 or less.");
            }

            if (tableUpdateDTO.TableNumber < 0)
            {
                return UnprocessableEntity("Table number can't be negative.");
            }

            table.TableNumber = tableUpdateDTO.TableNumber;
            table.Capacity = tableUpdateDTO.Capacity;

            _context.Entry(table).State = EntityState.Modified;

            await _context.SaveChangesAsync();
            
            return NoContent();
        }

        // DELETE: api/restaurants/{restaurantId}/tables/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTable(int restaurantId, int id)
        {
            var restaurantExists = await _context.Restaurants.AnyAsync(r => r.RestaurantId == restaurantId);
            if (!restaurantExists)
            {
                return NotFound($"Restaurant with ID {restaurantId} not found.");
            }

            var table = await _context.Tables.FirstOrDefaultAsync(t => t.TableId == id && t.RestaurantId == restaurantId);
            if (table == null)
            {
                return NotFound($"Table with ID {id} not found for restaurant with ID {restaurantId}.");
            }

            _context.Tables.Remove(table);
            await _context.SaveChangesAsync();

            return NoContent();
        }      
    }
}
