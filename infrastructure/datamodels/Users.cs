namespace infrastructure.datamodels;

public class Users
{
    public int UserId { get; set; }
    
    public required string UserEmail { get; set; }
    
    public required string UserName { get; set; }
    
    public int PhoneNumber { get; set; }
    
    public int UserType { get; set; }
    
    public bool Disabled { get; set; }
    
    public DateOnly DisabledDate { get; set; }
}