
using Dapper;
using infrastructure.datamodels;

namespace integration_tests;

public class Tests
{
    private HttpClient _httpClient;

    [SetUp]
    public void Setup()
    {
        _httpClient = new HttpClient();
    }

    [Test]
    public async Task GetAllAnimalSpecies()
    {
        Helper.TriggerRebuild();
        var expected = new List<object>();
        for (var i = 1; i < 10; i++)
        {
            var animalSpecies = new AnimalSpecies
            {
                SpeciesID = 3,
                SpeciesName = "name",
                SpeciesDescription = "stop",
                SpeciesPicture = "cheese.com",
            };
            expected.Add(animalSpecies);
            var sql = $@" 
            INSERT INTO AnimalDB.AnimalSpecies (speciesName, speciesDescription, speciesPicture)
            VALUES (@speciesName, @speciesDescription, @speciesPicture)";
            using (var conn = Helper.DataSource.OpenConnection())
            {
                conn.Execute(sql, animalSpecies);
            }
        }

        HttpResponseMessage response;
        try
        {
            response = await _httpClient.GetAsync("http://localhost:5000/api/books");
        }
        catch (HttpRequestException e)
        {
            throw new Exception("stupid");
        }

    }
}