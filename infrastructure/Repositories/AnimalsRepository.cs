using System.Runtime.InteropServices.JavaScript;
using Dapper;
using infrastructure.datamodels;
using Npgsql;

namespace infrastructure.Repositories;

public class AnimalsRepository
{
    private NpgsqlDataSource _dataSource;
    
    public AnimalsRepository(NpgsqlDataSource dataSource)
    {
        _dataSource = dataSource;
    }
    
    
    public Animals CreateAnimals(Animals animals)
    {
        var sql = $@"
INSERT INTO AnimalDB.Animals (animalName, animalBirthday, animalGender, animalDead, animalPicture, animalWeight)
VALUES (@animalName, @animalBirthday, @animalGender, @animalDead, @animalPicture, @animalWeight)
RETURNING Animals.AnimalID,
    animalName, animalBirthday, animalGender, animalDead, animalPicture, animalWeight;
";
        using (var conn = _dataSource.OpenConnection())
        {
            return conn.QueryFirst<Animals>(sql, new { animals.AnimalName, animals.AnimalBirthday, animals.AnimalGender, animals.AnimalDead, animals.AnimalPicture, animals.AnimalWeight });
        }
    }
    
    public Animals UpdateAnimals(Animals animals)
    {
        var sql = $@"
UPDATE AnimalDB.Animals SET animalName = @animalName, animalBirthday = @animalBirthday, animalGender = @animalGender, animalDead = @animalDead, animalPicture = @animalPicture, animalWeight = @animalWeight
WHERE animalID = @animalID 
RETURNING Animals.AnimalID,
    animalName, animalBirthday, animalGender, animalDead, animalPicture, animalWeight;
";
        using (var conn = _dataSource.OpenConnection())
        {
            return conn.QueryFirst<Animals>(sql, new {animals.AnimalName, animals.AnimalBirthday, animals.AnimalGender, animals.AnimalDead, animals.AnimalPicture, animals.AnimalWeight});
        }
    }
    
    public bool DeleteAnimals(int animalID)
    {
        var sql = @"DELETE FROM AnimalDB.Animals WHERE animalID = @animalID;";
        using (var conn = _dataSource.OpenConnection())
        {
            return conn.Execute(sql, new { animalID }) == 1;
        }
    }
    
    public Animals GetAnimalById(int animalID)
    {
        var sql = @$"
SELECT 
    animalName, animalBirthday, animalGender, animalDead, animalPicture, animalWeight
FROM AnimalDB.Animals WHERE animalID = @animalID;
";
        using (var conn = _dataSource.OpenConnection())
        {
            return conn.QueryFirst<Animals>(sql, new {animalID});
        }
    }
    
    public IEnumerable<AnimalFeed> GetAnimalsForFeed()
    {
        string sql = @$"
            SELECT                                                               
                animalName, animalBirthday, animalGender, animalDead, animalPicture, animalWeight
            FROM AnimalDB.Animals;              
            ";
        using (var conn = _dataSource.OpenConnection())
        {
            return conn.Query<AnimalFeed>(sql);
        }
    }
    
}