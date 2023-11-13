namespace infrastructure.datamodels;

public class PasswordHash
{
    public int UserId { get; set; }
    
    public required string PasswordHashed { get; set; }
    
    public required string PasswordSalt { get; set; }
}