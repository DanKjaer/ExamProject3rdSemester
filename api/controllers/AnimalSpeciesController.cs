using infrastructure.datamodels;
using Microsoft.AspNetCore.Mvc;
using service.Services;

namespace api.Controllers;

[ApiController]
public class AnimalSpeciesController
{

    private readonly AnimalSpeciesService _animalSpeciesService;
    
    public AnimalSpeciesController(AnimalSpeciesService animalSpeciesService)
    {
        _animalSpeciesService = animalSpeciesService;
    }

    [HttpGet]
    [Route("/api/animalspeciesfeed")]
    public IEnumerable<AnimalSpeciesFeed> GetAnimalSpeciesFeed()
    {
        return _animalSpeciesService.GetSpeciesForFeed();
    }

    [HttpGet]
    [Route("/api/animalspecies/{id}")]
    public AnimalSpecies GetAnimalSpecies(int id)
    {
        return _animalSpeciesService.GetSpeciesById(id);
    }
    
    [HttpPost]
    [Route("/api/animalspecies")]
    public AnimalSpecies CreateAnimalSpecies([FromBody] AnimalSpecies animalSpecies)
    {
        return _animalSpeciesService.CreateSpecies(animalSpecies);
    }

    [HttpPut]
    [Route("/api/animalspecies")]
    public AnimalSpecies UpdateAnimalSpecies([FromBody] AnimalSpecies animalSpecies)
    {
        return _animalSpeciesService.UpdateSpecies(animalSpecies);
    }

    [HttpDelete]
    [Route("/api/animalspecies/{id}")]
    public object DeleteAnimalSpecies(int id)
    {
        if (_animalSpeciesService.DeleteSpecies(id)) return new { message = "Animal species successfully deleted from system" };
        
        return new { message = "Failed deleting the animal species from the system" };
    }

    [HttpPost]
    [Route("/api/sendEmail")]
    public async Task<object> SendEmail([FromBody] SendEmailDTO emailDto)
    {
        AnimalSpecies animalSpecies = _animalSpeciesService.GetSpeciesById(emailDto.AnimalSpeciesId);
        try
        {
            MailService.SendEmail(emailDto.ToName, animalSpecies, emailDto.ToAddress);
            return new {message = "Email has been sent"};
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new {message = "Failed to send email, please try again or contact an administrator"};
        }
    }
}