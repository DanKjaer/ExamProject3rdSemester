using Microsoft.AspNetCore.Mvc;
using service.Services;

namespace api.Controllers;

[ApiController]
public class SearchController
{
    private readonly SearchService _searchService;

    public SearchController(SearchService searchService)
    {
        _searchService = searchService;
    }

    [HttpGet]
    [Route("/api/search")]
    public Dictionary<string, IEnumerable<object>> Search([FromQuery] string searchTerm)
    {
        return _searchService.Search(searchTerm);
    }
}