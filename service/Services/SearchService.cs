using infrastructure.Repositories;

namespace service.Services;

public class SearchService
{

    private readonly SearchRepository _searchRepository;
    private readonly AnimalsRepository _animalsRepository;

    public SearchService(SearchRepository searchRepository, AnimalsRepository animalsRepository)
    {
        _searchRepository = searchRepository;
        _animalsRepository = animalsRepository;
    }
    
    
    public Dictionary<string, IEnumerable<object>>? Search(string searchTerm)
    {
        var animalspecies = _searchRepository.SearchAnimalSpecies(searchTerm);
        var animals = _searchRepository.SearchAnimals(searchTerm);
        var result = new Dictionary<string, IEnumerable<object>>();
        
        if ((animals == null || !animals.Any()) && animalspecies.ToList().Count == 1)
        {
            var animalsBasedOnSpecies = _animalsRepository.GetAnimalsForFeed(animalspecies.ToList()[0].SpeciesID);
            
            result.Add("AnimalSpecies", animalspecies);
            result.Add("Animals",animalsBasedOnSpecies);
            
            return result;
        }

        if (!animalspecies.Any())
        {
            result.Add("Animals",animals);
            return result;
        }
        
        //This should never happen, since we have check in frontend. But just incase.
        return null;
    }
}