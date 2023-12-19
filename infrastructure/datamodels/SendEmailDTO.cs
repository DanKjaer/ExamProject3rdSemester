namespace infrastructure.datamodels;

public class SendEmailDTO
{
    public required int AnimalSpeciesId { get; set; }
    public required string ToAddress { get; set; }
    public required string ToName { get; set; }
}