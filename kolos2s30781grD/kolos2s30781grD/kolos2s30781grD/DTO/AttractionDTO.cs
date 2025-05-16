using System.ComponentModel.DataAnnotations;

namespace kolos2s30781grD.DTO;

public class AttractionDTO
{
    public required int AttractionId { get; set; }
    [Length(2,100)]
    public string Name { get; set; }
    [Length(10,2)]
    public decimal Price { get; set; }
    public int Amount { get; set; }
}