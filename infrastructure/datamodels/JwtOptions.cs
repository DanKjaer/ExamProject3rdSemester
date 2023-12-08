namespace infrastructure.datamodels;

public class JwtOptions
{
    public required byte[] Secret { get; init; }
    public required TimeSpan Lifetime { get; init; }
    public string? Address { get; set; }

    private JwtOptions()
    {
    }

    public static JwtOptions create(byte[] secret, TimeSpan lifetime)
    {
        return new JwtOptions
        {
            Secret = secret,
            Lifetime = lifetime
        };
    }
}