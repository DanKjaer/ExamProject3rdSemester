﻿using infrastructure.datamodels;
using Microsoft.AspNetCore.Mvc;
using service.Services;

namespace api.Controllers;

[ApiController]
public class AnimalController
{
    private readonly AnimalService _animalService;
    
    public AnimalController(AnimalService animalService)
    {
        _animalService = animalService;
    }

    [HttpGet]
    [Route("/api/animalfeed")]
    public IEnumerable<AnimalFeed> GetAnimalFeed()
    {
        return _animalService.GetAnimalsForFeed();
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
        if (_animalService.DeleteAnimal(id)) return new { message = "Animal species successfully deleted from system" };
        
        return new { message = "Failed deleting the animal species from the system" };
    }

    [HttpPost]
    [Route("/api/animalnotes/{id}")]
    public AnimalNote CreateAnimalNote(int id, [FromBody] AnimalNote animalNote)
    {
        return _animalService.CreateAnimalNote(id, animalNote);
    }
}