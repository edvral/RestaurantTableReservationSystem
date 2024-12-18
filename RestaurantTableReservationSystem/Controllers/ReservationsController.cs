using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantTableReservationSystem.Data;
using RestaurantTableReservationSystem.Models;
using RestaurantTableReservationSystem.DTOs;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace RestaurantTableReservationSystem.Controllers
{
    [ApiController]
    [Route("api/tables/{tableId}/reservations")]
    public class ReservationsController : ControllerBase
    {
        private readonly RestaurantReservationContext _context;

        public ReservationsController(RestaurantReservationContext context)
        {
            _context = context;
        }

        // GET: api/tables/{tableId}/reservations
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Reservation>>> GetReservations(int tableId)
        {           
            var reservations = await _context.Reservations.Where(r => r.TableId == tableId).ToListAsync();
           
            if (!reservations.Any())
            {
                return NotFound($"No reservations found for table with ID {tableId}.");
            }
          
            return Ok(reservations);
        }

        // GET: api/tables/{tableId}/reservations/{id}
        [HttpGet("{id}")]
        [Authorize(Roles = "User,Admin")]
        public async Task<ActionResult<Reservation>> GetReservation(int tableId, int id)
        {
            var tableExists = await _context.Tables.AnyAsync(t => t.TableId == tableId);
            if (!tableExists)
            {
                return NotFound($"Table with ID {tableId} not found.");
            }

            var reservation = await _context.Reservations.FirstOrDefaultAsync(r => r.ReservationId == id && r.TableId == tableId);
            if (reservation == null)
            {
                return NotFound($"Reservation with ID {id} not found for table with ID {tableId}.");
            }

            if (User.IsInRole("User"))
            {
                var userId = int.Parse(User.FindFirstValue("userId"));
                if (reservation.UserId != userId)
                {
                    return Forbid();
                }
            }

            return Ok(reservation);
        }

        // POST: api/tables/{tableId}/reservations
        [HttpPost]
        [Authorize(Roles = "User,Admin")]
        public async Task<ActionResult<ReservationResponseDTO>> PostReservation(int tableId, ReservationDTO reservationCreateDTO)
        {
            var table = await _context.Tables.FindAsync(tableId);
            if (table == null)
            {
                return NotFound($"Table with ID {tableId} not found.");
            }
      
            if (reservationCreateDTO.NumberOfGuests < 1)
            {
                return UnprocessableEntity("Number of guests must be at least 1.");
            }

            if (reservationCreateDTO.NumberOfGuests > table.Capacity)
            {
                return UnprocessableEntity($"Number of guests cannot exceed the table's capacity of {table.Capacity}.");
            }

            var conflictingReservation = await _context.Reservations
                .Where(r => r.TableId == tableId)
                .Where(r => r.ReservationStart < reservationCreateDTO.ReservationEnd && reservationCreateDTO.ReservationStart < r.ReservationEnd)
                .FirstOrDefaultAsync();

            if (conflictingReservation != null)
            {
                return UnprocessableEntity("A reservation for this table already exists during the specified time.");
            }

            DateTime now = DateTime.Now;
            if (reservationCreateDTO.ReservationStart < now)
            {
                return UnprocessableEntity("Reservation start time cannot be in the past.");
            }

            if (reservationCreateDTO.ReservationEnd <= reservationCreateDTO.ReservationStart)
            {
                return UnprocessableEntity("Reservation end time must be after the start time.");
            }

            var reservation = new Reservation
            {
                TableId = tableId,
                UserId = int.Parse(User.FindFirstValue("userId")),
                GuestName = reservationCreateDTO.GuestName,
                GuestPhoneNumber = reservationCreateDTO.GuestPhoneNumber,
                ReservationStart = reservationCreateDTO.ReservationStart,
                ReservationEnd = reservationCreateDTO.ReservationEnd,
                NumberOfGuests = reservationCreateDTO.NumberOfGuests,
                SpecialRequests = reservationCreateDTO.SpecialRequests
            };

            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();

            var reservationResponseDTO = new ReservationResponseDTO
            {
                ReservationId = reservation.ReservationId,
                GuestName = reservation.GuestName,
                GuestPhoneNumber = reservation.GuestPhoneNumber,
                ReservationStart = reservation.ReservationStart,
                ReservationEnd = reservation.ReservationEnd,
                NumberOfGuests = reservation.NumberOfGuests,
                SpecialRequests = reservation.SpecialRequests
            };

            return CreatedAtAction("GetReservation", new { tableId = tableId, id = reservation.ReservationId }, reservationResponseDTO);
        }

        // PUT: api/tables/{tableId}/reservations/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> PutReservation(int tableId, int id, ReservationDTO reservationUpdateDTO)
        {
            var table = await _context.Tables.FindAsync(tableId);
            if (table == null)
            {
                return NotFound($"Table with ID {tableId} not found.");
            }

            var reservation = await _context.Reservations.FirstOrDefaultAsync(r => r.ReservationId == id && r.TableId == tableId);
            if (reservation == null)
            {
                return NotFound($"Reservation with ID {id} not found for table with ID {tableId}.");
            }

            if (reservationUpdateDTO.NumberOfGuests < 1)
            {
                return UnprocessableEntity("Number of guests must be at least 1.");
            }

            if (reservationUpdateDTO.NumberOfGuests > table.Capacity)
            {
                return UnprocessableEntity($"Number of guests cannot exceed the table's capacity of {table.Capacity}.");
            }

            var conflictingReservation = await _context.Reservations
                .Where(r => r.TableId == tableId)
                .Where(r => r.ReservationId != id)
                .Where(r => r.ReservationStart < reservationUpdateDTO.ReservationEnd && reservationUpdateDTO.ReservationStart < r.ReservationEnd)
                .FirstOrDefaultAsync();

            if (conflictingReservation != null)
            {
                return UnprocessableEntity("A reservation for this table already exists during the specified time.");
            }

            DateTime now = DateTime.Now;

            if (reservationUpdateDTO.ReservationStart < now)
            {
                return UnprocessableEntity("Reservation start time cannot be in the past.");
            }

            if (reservationUpdateDTO.ReservationEnd <= reservationUpdateDTO.ReservationStart)
            {
                return UnprocessableEntity("Reservation end time must be after the start time.");
            }

            if (User.IsInRole("User"))
            {
                var userId = int.Parse(User.FindFirstValue("userId"));
                if (reservation.UserId != userId)
                {
                    return Forbid();
                }
            }

            reservation.GuestName = reservationUpdateDTO.GuestName;
            reservation.GuestPhoneNumber = reservationUpdateDTO.GuestPhoneNumber;
            reservation.ReservationStart = reservationUpdateDTO.ReservationStart;
            reservation.ReservationEnd = reservationUpdateDTO.ReservationEnd;
            reservation.NumberOfGuests = reservationUpdateDTO.NumberOfGuests;
            reservation.SpecialRequests = reservationUpdateDTO.SpecialRequests;

            _context.Entry(reservation).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            var reservationResponseDTO = new ReservationResponseDTO
            {
                ReservationId = reservation.ReservationId,
                GuestName = reservation.GuestName,
                GuestPhoneNumber = reservation.GuestPhoneNumber,
                ReservationStart = reservation.ReservationStart,
                ReservationEnd = reservation.ReservationEnd,
                NumberOfGuests = reservation.NumberOfGuests,
                SpecialRequests = reservation.SpecialRequests
            };

            return NoContent();
        }

        // DELETE: api/tables/{tableId}/reservations/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> DeleteReservation(int tableId, int id)
        {
            var tableExists = await _context.Tables.AnyAsync(t => t.TableId == tableId);
            if (!tableExists)
            {
                return NotFound($"Table with ID {tableId} not found.");
            }

            var reservation = await _context.Reservations.FirstOrDefaultAsync(r => r.ReservationId == id && r.TableId == tableId);
            if (reservation == null)
            {
                return NotFound($"Reservation with ID {id} not found for table with ID {tableId}.");
            }

            if (User.IsInRole("User"))
            {
                var userId = int.Parse(User.FindFirstValue("userId"));
                if (reservation.UserId != userId)
                {
                    return Forbid();
                }
            }

            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();

            return NoContent();
        }      
    }
}
