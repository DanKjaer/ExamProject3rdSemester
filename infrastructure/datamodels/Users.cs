using System.ComponentModel.DataAnnotations;

namespace infrastructure.datamodels;

public class Users
{
    public int? UserID { get; set; }
    
    public required string UserEmail { get; set; }
    
    public required string UserName { get; set; }
    
    public required string PhoneNumber { get; set; }
    
    public required int UserType { get; set; }
    
    public bool Disabled { get; set; }
    
    public DateOnly DisabledDate { get; set; }
}