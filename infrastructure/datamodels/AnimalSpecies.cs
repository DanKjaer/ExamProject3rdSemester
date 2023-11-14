namespace infrastructure.datamodels;

public class AnimalSpecies
{
    public int? SpeciesID { get; set; }
    
    public required string SpeciesName { get; set; }
    
    public required string SpeciesDescription { get; set; }
    
    public string? SpeciesPicture { get; set; }
}