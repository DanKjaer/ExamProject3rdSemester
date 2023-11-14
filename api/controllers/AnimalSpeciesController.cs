using infrastructure.datamodels;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[ApiController]
public class AnimalSpeciesController
{

    //private readonly AnimalSpeciesService _animalSpeciesService;
    
    public AnimalSpeciesController()
    {
        // _animalSpeciesService = animalSpeciesService;
    }

    [HttpGet]
    [Route("/api/animalspeciesfeed")]
    public IEnumerable<AnimalSpeciesFeed> GetAnimalSpeciesFeed()
    {
        throw new NotImplementedException();
    }

    [HttpGet]
    [Route("/api/animalspecies/{id}")]
    public AnimalSpecies GetAnimalSpecies(int id)
    {
        throw new NotImplementedException();
    }
    
    [HttpPost]
    [Route("/api/animalspecies")]
    public AnimalSpecies CreateAnimalSpecies([FromBody] AnimalSpecies animalSpecies)
    {
        throw new NotImplementedException();
    }

    [HttpPut]
    [Route("/api/animalspecies/{id}")]
    public AnimalSpecies UpdateAnimalSpecies([FromBody] AnimalSpecies animalSpecies)
    {
        throw new NotImplementedException();
    }

    [HttpDelete]
    [Route("/api/animalspecies/{id}")]
    public object DeleteAnimalSpecies(int id)
    {
        throw new NotImplementedException();
        /*
         * if (service.DeleteAnimal) return new {message = "good"
         * else return new {message = "bad"
         */
    }
}