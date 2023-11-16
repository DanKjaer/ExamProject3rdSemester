using infrastructure.datamodels;

namespace service.Services;

public class AnimalService
{
    private readonly AnimalRepository _animalRepository;

    public AnimalService(AnimalRepository animalRepository)
    {
        _animalRepository = animalRepository;
    }

    public Animals CreateAnimal(Animals animalModel)
    {
        return _animalRepository.CreateAnimal(animalModel);
    }

    public Animals UpdateAnimal(Animals animalModel)
    {
        return _animalRepository.UpdateAnimal(animalModel);
    }

    public bool DeleteAnimal(int animalId)
    {
        return _animalRepository.DeleteAnimal(animalId);
    }

    public Animals GetAnimalById(int animalId)
    {
        return _animalRepository.GetAnimalById(animalId);
    }

    public IEnumerable<AnimalFeed> GetAnimalsForFeed()
    {
        return _animalRepository.GetAnimalForFeed();
    }
}