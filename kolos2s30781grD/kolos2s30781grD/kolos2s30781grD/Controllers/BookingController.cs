using kolos2s30781grD.DTO;
using kolos2s30781grD.Services;
using Microsoft.AspNetCore.Mvc;

namespace kolos2s30781grD.Controllers;

[ApiController]
[Route("api/bookings")]
public class BookingController : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<IActionResult> GetBooking([FromRoute] int id, [FromServices] IDatabaseService _db)
    {
        var booking = await _db.GetBookingAsync(id);
        if (booking is null)
            return NotFound($"Booking with ID {id} not found");

        return Ok(booking);
    }

    [HttpPost]
    public async Task<IActionResult> CreateBooking([FromBody] BookingRequest request, [FromServices] IDatabaseService _db)
    {
        var result = await _db.CreateBookingAsync(request);

        if (!result.Success)
            return BadRequest(result.Message);

        return Created($"api/bookings/{request.BookingId}", result.Booking);
    }
}