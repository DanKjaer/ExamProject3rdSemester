using System.ComponentModel.DataAnnotations;

namespace infrastructure.datamodels;

public class Users
{
    public int? UserID { get; set; }
    
    public required EmailAddressAttribute UserEmail { get; set; }
    
    public required string UserName { get; set; }
    
    public required PhoneAttribute PhoneNumber { get; set; }
    
    public int UserType { get; set; }
    
    public bool Disabled { get; set; }
    
    public DateOnly DisabledDate { get; set; }
}