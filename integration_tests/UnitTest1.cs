
using System.Net.Http.Headers;
using System.Net.Http.Json;
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
        string jwtToken = Environment.GetEnvironmentVariable("JWT")!;
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
        Console.WriteLine(_httpClient.DefaultRequestHeaders.Authorization);
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
        
        response = await _httpClient.GetAsync("http://localhost:5000/api/animalspecies/1");

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

    public AnimalSpecies MakeMeASpecies()
    {
        var animalSpecies = new AnimalSpecies()
        {
            SpeciesName = "Species",
            SpeciesDescription = "stop",
            SpeciesPicture = "https://upload.wikimedia.org/wikipedia/commons/6/6f/Animal_diversity_b.png",
        };

        var sql = $@" 
            INSERT INTO AnimalDB.AnimalSpecies (speciesName, speciesDescription, speciesPicture)
            VALUES (@speciesName, @speciesDescription, @speciesPicture) 
            RETURNING *";
        using (var conn = Helper.DataSource.OpenConnection())
        {
             return conn.QueryFirst<AnimalSpecies>(sql, animalSpecies);
        }

    }

    public Animals MakeMeAnAnimal(int id)
    {
        var animal = new Animals()
        {
            SpeciesID = 1,
            AnimalName = "Animal" + id,
            AnimalBirthday = new DateTime(03, 12, 10),
            AnimalDead = false,
            AnimalPicture = "https://media.wired.com/photos/593261cab8eb31692072f129/master/pass/85120553.jpg",
            AnimalWeight = 24,
            AnimalGender = true,
        };
        var sql = $@" 
            INSERT INTO AnimalDB.Animals (SpeciesID, AnimalName, AnimalBirthday, AnimalGender, AnimalDead, AnimalPicture, AnimalWeight)
            VALUES (@SpeciesID, @AnimalName, @AnimalBirthday, @AnimalGender, @AnimalDead, @AnimalPicture, @AnimalWeight) 
            RETURNING *";
        using (var conn = Helper.DataSource.OpenConnection())
        {
            return conn.QueryFirst<Animals>(sql, animal);
        }
    }
    

    [Test]
    public async Task GetAnimal()
    {
        MakeMeASpecies();
        var animal = MakeMeAnAnimal(1);

        HttpResponseMessage response;
        response = await _httpClient.GetAsync("http://localhost:5000/api/animal/1");

        var content = await response.Content.ReadAsStringAsync();
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
        
        var expected = new List<AnimalFeed>();
        for (var i = 1; i < 10; i++)
        {
            var animal = MakeMeAnAnimal(2);
            var animalFeed = new AnimalFeed()
            {
                AnimalName = animal.AnimalName
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
        var animal = new Animals
        {
            AnimalName = "Alfred",
            SpeciesID = 1
        };
        
        HttpResponseMessage response;
        response = await _httpClient.PostAsJsonAsync("http://localhost:5000/api/animal", animal);

        var content = await response.Content.ReadAsStringAsync();
        Animals animals;
        animals = JsonConvert.DeserializeObject<Animals>(content);
        
        using (new AssertionScope())
        {
            response.IsSuccessStatusCode.Should().BeTrue();
            animals.AnimalName.Should().BeEquivalentTo(animal.AnimalName);
            animals.SpeciesID.Should().Be(animal.SpeciesID);
        }
    }

    [Test]
    public async Task UpdateAnimal()
    {
        MakeMeASpecies();
        var animal = MakeMeAnAnimal(4);
        
        HttpResponseMessage response;
        animal.AnimalName = "sur søren";
        response = await _httpClient.PutAsJsonAsync("http://localhost:5000/api/animal", animal);

        var content = await response.Content.ReadAsStringAsync();
        Animals animals;
        animals = JsonConvert.DeserializeObject<Animals>(content);
        
        using (new AssertionScope())
        {
            response.IsSuccessStatusCode.Should().BeTrue();
            animals.AnimalName.Should().BeEquivalentTo(animal.AnimalName);
        }
    }

    [Test]
    public async Task UpdateAnimalSpecies()
    {
        var species = MakeMeASpecies();
        
        HttpResponseMessage response;
        species.SpeciesName = "sur søren species";
        response = await _httpClient.PutAsJsonAsync("http://localhost:5000/api/animalspecies", species);
        
        var content = await response.Content.ReadAsStringAsync();
        AnimalSpecies animalSpecies;
        animalSpecies = JsonConvert.DeserializeObject<AnimalSpecies>(content);
        
        using (new AssertionScope())
        {
            response.IsSuccessStatusCode.Should().BeTrue();
            animalSpecies.SpeciesName.Should().BeEquivalentTo(species.SpeciesName);
        }
    }

    [Test]
    public async Task CreateAnimalNote()
    {
        MakeMeASpecies();
        MakeMeAnAnimal(1);

        var animalNote = new AnimalNote
        {
            AnimalID = 1,
            NoteText = "kalder på hjælp fra testene...."
        };
        
        HttpResponseMessage response;
        response = await _httpClient.PostAsJsonAsync("http://localhost:5000/api/animalnote", animalNote);

        var content = await response.Content.ReadAsStringAsync();
        AnimalNote animalNotes;
        animalNotes = JsonConvert.DeserializeObject<AnimalNote>(content);
        
        using (new AssertionScope())
        {
            response.IsSuccessStatusCode.Should().BeTrue();
            animalNotes.NoteText.Should().Be(animalNote.NoteText);
        }
    }
    
    [Test]
    public async Task GetAnimalNotes()
    {
        MakeMeASpecies();
        MakeMeAnAnimal(1);
        
        var expected = new List<AnimalNote>();
        for (var i = 1; i < 10; i++)
        {
            var animalNote = new AnimalNote()
            {
                AnimalID = 1,
                NoteText = "kalder på hjælp fra testene...." + i
            };
            var sql = $@" 
            INSERT INTO AnimalDB.AnimalNote (AnimalID, NoteDate, NoteText)
            VALUES (@AnimalID, @NoteDate, @NoteText)
            RETURNING *;";
            using (var conn = Helper.DataSource.OpenConnection())
            {
                expected.Add(conn.QueryFirst<AnimalNote>(sql, animalNote));
            }
        }
        
        HttpResponseMessage response;
        response = await _httpClient.GetAsync("http://localhost:5000/api/animalnote/1");

        var content = await response.Content.ReadAsStringAsync();
        
        IEnumerable<AnimalNote> animalNotes;
        animalNotes = JsonConvert.DeserializeObject<IEnumerable<AnimalNote>>(content);

        using (new AssertionScope())
        {
            for (var i = 0; i < expected.Count; i++)
            {
                animalNotes.ToList()[1].NoteText.Should().BeEquivalentTo(expected.ToList()[1].NoteText);
            }
        }
    }

    [Test]
    public async Task CreateUser()
    {
        var user = new Users
        {
            UserEmail = "bobby@bobsen.dk",
            UserName = "hennyy",
            PhoneNumber = "23457823",
            UserType = UserType.Dyrepasser
        };

        HttpResponseMessage response;
        response = await _httpClient.PostAsJsonAsync("http://localhost:5000/api/users?password=1234", user);
        
        var content = await response.Content.ReadAsStringAsync();
        Users users;
        users = JsonConvert.DeserializeObject<Users>(content);
        
        using (new AssertionScope())
        {
            response.IsSuccessStatusCode.Should().BeTrue();
            users.UserName.Should().BeEquivalentTo(user.UserName);
            users.UserEmail.Should().BeEquivalentTo(user.UserEmail);
        }
    }

    [Test]
    public async Task DisabledUser()
    {
        var user = new Users
        {
            UserEmail = "bobby@bobsen.dk",
            UserName = "hennyy",
            PhoneNumber = "23457823",
            UserType = UserType.Dyrepasser
        };
        
        var sql = "INSERT INTO animaldb.users" +
                  "(UserName, UserEmail, PhoneNumber, Usertype, ToBeDisabledDate)" +
                  "VALUES (@UserName, @UserEmail, @PhoneNumber, @UserType, @ToBeDisabledDate) " +
                  "RETURNING *;";

        using (var conn = Helper.DataSource.OpenConnection())
        {
            user = conn.QueryFirst<Users>(sql, new {user.UserName, user.UserEmail, user.PhoneNumber, user.UserType, user.ToBeDisabledDate});
        }

        HttpResponseMessage response;
        user.Disabled = true;
        response = await _httpClient.PutAsJsonAsync("http://localhost:5000/api/users", user);
        
        var content = await response.Content.ReadAsStringAsync();
        Users users;
        users = JsonConvert.DeserializeObject<Users>(content);
        
        using (new AssertionScope())
        {
            response.IsSuccessStatusCode.Should().BeTrue();
            users.Disabled.Should().BeTrue();
        }
    }
}