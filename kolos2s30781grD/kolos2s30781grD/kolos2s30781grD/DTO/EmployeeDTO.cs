using System.ComponentModel.DataAnnotations;

namespace kolos2s30781grD.DTO;

public class EmployeeDTO
{
    public required int EmployeeId { get; set; }
    [Length(2,100)]
    public string FirstName { get; set; }
    [Length(2,100)]
    public string LastName { get; set; }
    [Length(1,22)]
    public string EmployeeNumber { get; set; }
}