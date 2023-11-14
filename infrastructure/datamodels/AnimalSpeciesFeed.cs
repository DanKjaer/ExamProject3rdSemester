namespace infrastructure.datamodels;

public class AnimalSpeciesFeed
{
    public int SpeciesID { get; set; }
    
    public required string SpeciesName { get; set; }
    
    public string? SpeciesPicture { get; set; }
}