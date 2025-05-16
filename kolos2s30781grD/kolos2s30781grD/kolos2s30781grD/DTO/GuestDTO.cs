using System.ComponentModel.DataAnnotations;

namespace kolos2s30781grD.DTO;

public class GuestDTO
{
    public required int GuestId { get; set; }
    [Length(2,100)]
    public string FirstName { get; set; }
    [Length(2,100)]
    public string LastName { get; set; }
    
    public DateTime DateOfBirth { get; set; }
}