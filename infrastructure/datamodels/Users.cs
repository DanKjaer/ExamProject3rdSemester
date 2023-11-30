using System.ComponentModel.DataAnnotations;

namespace infrastructure.datamodels;

public class Users
{
    public int? UserID { get; set; }
    
    public required string UserEmail { get; set; }
    
    public required string UserName { get; set; }
    
    public required string PhoneNumber { get; set; }
    
    public required UserType UserType { get; set; }
    
    public bool Disabled { get; set; }
    
    public DateTime? ToBeDisabledDate { get; set; }
}

public enum UserType
{
    Manager,
    Dyrepasser,
    Elev,
}