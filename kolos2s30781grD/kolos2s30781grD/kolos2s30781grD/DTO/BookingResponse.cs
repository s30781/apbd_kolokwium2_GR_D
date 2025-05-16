namespace kolos2s30781grD.DTO;

public class BookingResponse
{
    public DateTime Date { get; set; }
    public GuestDTO Guest { get; set; }
    public EmployeeDTO Employee { get; set; }
    public List<AttractionDTO> Attractions { get; set; }
}