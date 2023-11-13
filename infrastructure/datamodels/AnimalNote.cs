namespace infrastructure.datamodels;

public class AnimalNote
{
    public int NoteId { get; set; }
    
    public int AnimalId { get; set; }
    
    public DateOnly NoteDate { get; set; }
    
    public required string NoteText { get; set; }
}