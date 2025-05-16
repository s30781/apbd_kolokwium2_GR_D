using System.ComponentModel.DataAnnotations;

namespace kolos2s30781grD.DTO;

public class BookingRequest
{
    public required int BookingId { get; set; }
    public int GuestId { get; set; }

    [Required]
    [StringLength(22)]
    public string EmployeeNumber { get; set; }

    public List<AttractionInBookingDTO> Attractions { get; set; }
}