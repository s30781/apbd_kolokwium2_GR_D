using System.ComponentModel.DataAnnotations;

namespace kolos2s30781grD.DTO;

public class AttractionInBookingDTO
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; }
    [Range(1, int.MaxValue)]
    public int Amount { get; set; }
}