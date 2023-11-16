using infrastructure.datamodels;
using infrastructure.Repositories;

namespace service.Services;

public class AnimalService
{
    private readonly AnimalsRepository _animalsRepository;

    public AnimalService(AnimalsRepository animalsRepository)
    {
        _animalsRepository = animalsRepository;
    }

    public Animals CreateAnimal(Animals animalModel)
    {
        return _animalsRepository.CreateAnimals(animalModel);
    }

    public Animals UpdateAnimal(Animals animalModel)
    {
        return _animalsRepository.UpdateAnimals(animalModel);
    }

    public bool DeleteAnimal(int animalId)
    {
        return _animalsRepository.DeleteAnimals(animalId);
    }

    public Animals GetAnimalById(int animalId)
    {
        return _animalsRepository.GetAnimalById(animalId);
    }

    public IEnumerable<AnimalFeed> GetAnimalsForFeed()
    {
        return _animalsRepository.GetAnimalsForFeed();
    }

    public AnimalNote CreateAnimalNote(AnimalNote animalNote)
    {
        return _animalsRepository.CreateNote(animalNote);
    }

    public Boolean DeleteAnimalNote(int id)
    {
        return _animalsRepository.DeleteNote(id);
    }

    public IEnumerable<AnimalNote> GetAnimalNotes(int id)
    {
        return _animalsRepository.GetAnimalNotes(id);
    }

    public AnimalNote UpdateAnimalNote(AnimalNote animalNote)
    {
        return _animalsRepository.UpdateAnimalNote(animalNote);
    }
}