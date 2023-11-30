
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
        Helper.TriggerRebuild();
    }

    [Test]
    public async Task GetOneAnimalSpecies()
    {
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

    public void MakeMeASpecies()
    {
        var animalSpecies = new AnimalSpecies()
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
        
    }

    [Test]
    public async Task GetAnimals()
    {
        MakeMeASpecies();
        
        var animals = new Animals()
        {
            SpeciesID = 1,
            AnimalName = "Animal",
            AnimalBirthday = new DateTime(03, 12, 10),
            AnimalDead = false,
            AnimalPicture = "https://media.wired.com/photos/593261cab8eb31692072f129/master/pass/85120553.jpg",
            AnimalWeight = 24,
            AnimalGender = true,
        };
        
        var sql = $@" 
            INSERT INTO AnimalDB.Animals (SpeciesID, AnimalName, AnimalBirthday, AnimalGender, AnimalDead, AnimalPicture, AnimalWeight)
            VALUES (@SpeciesID, @AnimalName, @AnimalBirthday, @AnimalGender, @AnimalDead, @AnimalPicture, @AnimalWeight)";
        using (var conn = Helper.DataSource.OpenConnection())
        {
            conn.Execute(sql, animals);
        }
        
        HttpResponseMessage response;
        try
        {
            response = await _httpClient.GetAsync("http://localhost:5000/api/animal/1");
        }
        catch (HttpRequestException e)
        {
            throw new Exception("stupid part 2");
        }
        
        var content = await response.Content.ReadAsStringAsync();
        Console.WriteLine(animals.SpeciesID);
        Animals animal;
        animal = JsonConvert.DeserializeObject<Animals>(content);


        using (new AssertionScope())
        {
            animal.AnimalGender.Should().Be(animals.AnimalGender);
            animal.AnimalDead.Should().Be(animals.AnimalDead);
            animal.AnimalWeight.Should().Be(animals.AnimalWeight);
            animal.AnimalName.Should().BeEquivalentTo(animals.AnimalName);
            animal.AnimalPicture.Should().BeEquivalentTo(animals.AnimalPicture);
            animal.SpeciesID.Should().Be(animals.SpeciesID);
        }
        
    }

    [Test]
    public async Task GetSpeciesFeed()
    {
        var expected = new List<object>();
        for (var i = 1; i < 10; i++)
        {
            var animalSpecies = new AnimalSpecies()
            {
                SpeciesName = "Species" + i,
                SpeciesPicture = "cheese.com",
                SpeciesDescription = "bobby han døde"
                
            };
            var sql = $@" 
            INSERT INTO AnimalDB.AnimalSpecies (speciesName, speciesDescription, speciesPicture)
            VALUES (@speciesName, @speciesDescription, @speciesPicture)";
            using (var conn = Helper.DataSource.OpenConnection())
            {
                conn.Execute(sql, animalSpecies);
            }
            
            var animalSpeciesFeed = new AnimalSpeciesFeed()
            {
                SpeciesName = animalSpecies.SpeciesName,
                SpeciesPicture = animalSpecies.SpeciesPicture,
            };
            expected.Add(animalSpeciesFeed);
        }
        
        HttpResponseMessage response;
        try
        {
            response = await _httpClient.GetAsync("http://localhost:5000/api/animalspeciesfeed");
        }
        catch (HttpRequestException e)
        {
            throw new Exception("stupid part 3");
        }

        var content = await response.Content.ReadAsStringAsync();
        IEnumerable<AnimalSpeciesFeed> speciesFeed;
        speciesFeed = JsonConvert.DeserializeObject<IEnumerable<AnimalSpeciesFeed>>(content);
        
        using (new AssertionScope())
        {
            foreach (var species in speciesFeed)
            {
                species.SpeciesName.Should()
                species.SpeciesPicture.Should().NotBeNull();
            }
        }
    }
    
    
    [Test]
    public async Task GetAnimalFeed()
    {
        MakeMeASpecies();
        
        
        
    }
    
    
    
}