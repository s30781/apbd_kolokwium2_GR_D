using kolos2s30781grD.DTO;

namespace kolos2s30781grD.Services;

public interface IDatabaseService
{
    Task<BookingResponse?> GetBookingAsync(int bookingId);
    Task<(bool Success, string? Message, BookingResponse? Booking)> CreateBookingAsync(BookingRequest request);
}