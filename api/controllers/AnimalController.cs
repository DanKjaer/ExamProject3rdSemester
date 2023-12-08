using api.filters;
using infrastructure.datamodels;
using Microsoft.AspNetCore.Mvc;
using service.Services;

namespace api.Controllers;

[RequireAuthentication]
[ApiController]
public class AnimalController
{
    private readonly AnimalService _animalService;
    
    public AnimalController(AnimalService animalService)
    {
        _animalService = animalService;
    }

    [HttpGet]
    [Route("/api/animalfeed/{id}")]
    public IEnumerable<AnimalFeed> GetAnimalFeed(int id)
    {
        return _animalService.GetAnimalsForFeed(id);
    }

    [HttpGet]
    [Route("/api/animal/{id}")]
    public Animals GetAnimal(int id)
    {
        return _animalService.GetAnimalById(id);
    }
    
    [HttpPost]
    [Route("/api/animal")]
    public Animals CreateAnimal([FromBody] Animals animal)
    {
        return _animalService.CreateAnimal(animal);
    }

    [HttpPut]
    [Route("/api/animal")]
    public Animals UpdateAnimal([FromBody] Animals animal)
    {
        return _animalService.UpdateAnimal(animal);
    }

    [HttpDelete]
    [Route("/api/animal/{id}")]
    public object DeleteAnimal(int id)
    {
        if (_animalService.DeleteAnimal(id)) return new { message = "Animal successfully deleted from system" };
        
        return new { message = "Failed deleting the animal from the system" };
    }

    [HttpPost]
    [Route("/api/animalnote/")]
    public AnimalNote CreateAnimalNote([FromBody] AnimalNote animalNote)
    {
        return _animalService.CreateAnimalNote(animalNote);
    }

    [HttpDelete]
    [Route("/api/animalnote/{id}")]
    public object DeleteAnimalNote(int id)
    {
        if (_animalService.DeleteAnimalNote(id)) return new { message = "Animal note successfully deleted from system" };
        
        return new { message = "Failed deleting the animal note from the system" };
    }

    [HttpGet]
    [Route("/api/animalnote/{id}")]
    public IEnumerable<AnimalNote> GetAnimalNotes(int id)
    {
        return _animalService.GetAnimalNotes(id);
    }

    [HttpPut]
    [Route("/api/animalnote/{animalId}")]
    public AnimalNote UpdateAnimalNote([FromBody] AnimalNote animalNote, int animalId)
    {
        animalNote.AnimalID = animalId;
        return _animalService.UpdateAnimalNote(animalNote);
    }
}