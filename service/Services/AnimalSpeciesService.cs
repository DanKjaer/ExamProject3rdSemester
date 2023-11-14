using infrastructure.datamodels;
using infrastructure.Repositories;

namespace service.Services;

public class AnimalSpeciesService
{
    private readonly AnimalSpeciesRepository _animalSpeciesRepository;

    public AnimalSpeciesService(AnimalSpeciesRepository animalSpeciesRepository)
    {
        _animalSpeciesRepository = animalSpeciesRepository;
    }

    public AnimalSpecies CreateSpecies(AnimalSpecies animalSpeciesModel)
    {
        return _animalSpeciesRepository.CreateSpecies(
            speciesName: animalSpeciesModel.SpeciesName,
            speciesDescription: animalSpeciesModel.SpeciesDescription,
            speciesPicture: animalSpeciesModel.SpeciesPicture
        );
    }

    public AnimalSpecies UpdateSpecies(AnimalSpecies animalSpeciesModel)
    {
        return _animalSpeciesRepository.UpdateSpecies(
            speciesName: animalSpeciesModel.SpeciesName,
            speciesDescription: animalSpeciesModel.SpeciesDescription,
            speciesPicture: animalSpeciesModel.SpeciesPicture
        );
    }

    public bool DeleteSpecies(int speciesID)
    {
        return _animalSpeciesRepository.DeleteSpecies(speciesID);
    }

    public AnimalSpecies GetSpeciesById(int speciesID)
    {
        return _animalSpeciesRepository.GetSpeciesById(speciesID);
    }

    public IEnumerable<AnimalSpeciesFeed> GetSpeciesForFeed()
    {
        return _animalSpeciesRepository.GetSpeciesForFeed();
    }
}