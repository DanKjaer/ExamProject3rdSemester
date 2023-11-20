namespace infrastructure.datamodels;

public class Animals
{
    public int? AnimalID { get; set; }
    
    public required string AnimalName { get; set; }
    
    public DateTime AnimalBirthday { get; set; }
    
    public bool AnimalGender { get; set; }

    public bool AnimalDead { get; set; }
    
    public string? AnimalPicture { get; set; }
    
    public float AnimalWeight { get; set; }

    public required int SpeciesID { get; set; }
}