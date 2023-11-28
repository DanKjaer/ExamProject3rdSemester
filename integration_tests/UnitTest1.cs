
using Dapper;
using FluentAssertions;
using FluentAssertions.Execution;
using infrastructure.datamodels;
using Newtonsoft.Json;

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
    public async Task GetOneAnimalSpecies()
    {
        Helper.TriggerRebuild();
  
            var animalSpecies = new AnimalSpecies
            {
                SpeciesName = "Species",
                SpeciesDescription = "stop",
                SpeciesPicture = "cheese.com",
            };
            
            var sql = $@" 
            INSERT INTO AnimalDB.AnimalSpecies (speciesName, speciesDescription, speciesPicture)
            VALUES (@speciesName, @speciesDescription, @speciesPicture)";
            using (var conn = Helper.DataSource.OpenConnection())
            {
                conn.Execute(sql, animalSpecies);
            }

        HttpResponseMessage response;
        try
        {
            response = await _httpClient.GetAsync("http://localhost:5000/api/animalspecies/1");
        }
        catch (HttpRequestException e)
        {
            throw new Exception("stupid");
        }

        var content = await response.Content.ReadAsStringAsync();
        Console.WriteLine(content);
        Console.WriteLine(response);
        AnimalSpecies species;
        species = JsonConvert.DeserializeObject<AnimalSpecies>(content);


        using (new AssertionScope())
        {
            species.SpeciesDescription.Should().BeEquivalentTo(animalSpecies.SpeciesDescription);
            species.SpeciesName.Should().BeEquivalentTo(animalSpecies.SpeciesName);
            species.SpeciesPicture.Should().BeEquivalentTo(animalSpecies.SpeciesPicture);
        }

    }
    
    
    
    
}