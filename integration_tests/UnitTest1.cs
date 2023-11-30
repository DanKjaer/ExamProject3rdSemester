
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
            SpeciesPicture = "https://upload.wikimedia.org/wikipedia/commons/6/6f/Animal_diversity_b.png",
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
            SpeciesPicture = "https://upload.wikimedia.org/wikipedia/commons/6/6f/Animal_diversity_b.png",
        };

        var sql = $@" 
            INSERT INTO AnimalDB.AnimalSpecies (speciesName, speciesDescription, speciesPicture)
            VALUES (@speciesName, @speciesDescription, @speciesPicture)";
        using (var conn = Helper.DataSource.OpenConnection())
        {
            conn.Execute(sql, animalSpecies);
        }

    }

    public Animals MakeMeAnAnimal()
    {
        var animal = new Animals()
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
            conn.Execute(sql, animal);
        }
        
        return animal;
    }
    

    [Test]
    public async Task GetAnimals()
    {
        MakeMeASpecies();
        var animal = MakeMeAnAnimal();

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
        Console.WriteLine(animal.SpeciesID);
        Animals animals;
        animals = JsonConvert.DeserializeObject<Animals>(content);


        using (new AssertionScope())
        {
            animals.AnimalGender.Should().Be(animal.AnimalGender);
            animals.AnimalDead.Should().Be(animal.AnimalDead);
            animals.AnimalWeight.Should().Be(animal.AnimalWeight);
            animals.AnimalName.Should().BeEquivalentTo(animal.AnimalName);
            animals.AnimalPicture.Should().BeEquivalentTo(animal.AnimalPicture);
            animals.SpeciesID.Should().Be(animal.SpeciesID);
        }

    }

    [Test]
    public async Task GetSpeciesFeed()
    {
        var expected = new List<AnimalSpeciesFeed>();
        for (var i = 1; i < 10; i++)
        {
            var animalSpecies = new AnimalSpecies()
            {
                SpeciesName = "Species" + i,
                SpeciesPicture = "https://upload.wikimedia.org/wikipedia/commons/6/6f/Animal_diversity_b.png",
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

        response = await _httpClient.GetAsync("http://localhost:5000/api/animalspeciesfeed");

        var content = await response.Content.ReadAsStringAsync();
        IEnumerable<AnimalSpeciesFeed> speciesFeed;
        speciesFeed = JsonConvert.DeserializeObject<IEnumerable<AnimalSpeciesFeed>>(content);

        using (new AssertionScope())
        {
            for (var i = 0; i < expected.Count; i++)
            {
                speciesFeed.ToList()[i].SpeciesName.Should().BeEquivalentTo(expected.ToList()[i].SpeciesName);
                speciesFeed.ToList()[i].SpeciesPicture.Should().BeEquivalentTo(expected.ToList()[i].SpeciesPicture);
            }
        }
    }

    [Test]
    public async Task GetAnimalFeed()
    {
        MakeMeASpecies();
        var animal = MakeMeAnAnimal();
        
        var expected = new List<AnimalFeed>();
        for (var i = 1; i < 10; i++)
        {
            var animalFeed = new AnimalFeed()
            {
                AnimalName = animal.AnimalName,
            };
            expected.Add(animalFeed);
        }

        HttpResponseMessage response;

        response = await _httpClient.GetAsync("http://localhost:5000/api/animalfeed/1");

        var content = await response.Content.ReadAsStringAsync();
        IEnumerable<AnimalFeed> animalsFeed;
        animalsFeed = JsonConvert.DeserializeObject<IEnumerable<AnimalFeed>>(content);

        using (new AssertionScope())
        {
            for (var i = 0; i < expected.Count; i++)
            {
                animalsFeed.ToList()[i].AnimalName.Should().BeEquivalentTo(expected.ToList()[i].AnimalName);
            }
        }
    }

    [Test]
    public async Task CreateAnimal()
    {
        MakeMeASpecies();
        var animal = MakeMeAnAnimal();

        HttpResponseMessage response;
        response = await _httpClient.GetAsync("http://localhost:5000/api/animal/1");

        var content = await response.Content.ReadAsStringAsync();
        Animals animals;
        animals = JsonConvert.DeserializeObject<Animals>(content);
        
        using (new AssertionScope())
        {
            response.IsSuccessStatusCode.Should().BeTrue();
            animals.AnimalGender.Should().Be(animal.AnimalGender);
            animals.AnimalDead.Should().Be(animal.AnimalDead);
            animals.AnimalWeight.Should().Be(animal.AnimalWeight);
            animals.AnimalName.Should().BeEquivalentTo(animal.AnimalName);
            animals.AnimalPicture.Should().BeEquivalentTo(animal.AnimalPicture);
            animals.SpeciesID.Should().Be(animal.SpeciesID);
        }
    }

    [Test]
    public async Task UpdateAnimal()
    {
        MakeMeASpecies();
        var animal = MakeMeAnAnimal();
    }
}