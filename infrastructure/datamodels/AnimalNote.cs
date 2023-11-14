namespace infrastructure.datamodels;

public class AnimalNote
{
    public int NoteID { get; set; }
    
    public int AnimalID { get; set; }
    
    public DateOnly NoteDate { get; set; }
    
    public required string NoteText { get; set; }
}