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
            INSERT INTO AnimalDB.Animals (SpeciesID, AnimalName, AnimalBirthday, AnimalGender, AnimalDead, AnimalPicture, AnimalWeight)
            VALUES (@SpeciesID, @AnimalName, @AnimalBirthday, @AnimalGender, @AnimalDead, @AnimalPicture, @AnimalWeight)
            RETURNING *;
            ";
        using (var conn = _dataSource.OpenConnection())
        {
            return conn.QueryFirst<Animals>(sql, new { SpeciesID = animals.SpeciesID, animals.AnimalName, animals.AnimalBirthday, animals.AnimalGender, animals.AnimalDead, animals.AnimalPicture, animals.AnimalWeight });
        }
    }
    
    public Animals UpdateAnimals(Animals animals)
    {
        var sql = $@"
            UPDATE AnimalDB.Animals SET SpeciesID = @SpeciesID, animalName = @animalName, animalBirthday = @animalBirthday, animalGender = @animalGender, animalDead = @animalDead, animalPicture = @animalPicture, animalWeight = @animalWeight
            WHERE AnimalID = @AnimalID 
            RETURNING *;
            ";
        using (var conn = _dataSource.OpenConnection())
        {
            return conn.QueryFirst<Animals>(sql, new { SpeciesID = animals.SpeciesID, animals.AnimalName, animals.AnimalBirthday, animals.AnimalGender, animals.AnimalDead, animals.AnimalPicture, animals.AnimalWeight, animals.AnimalID});
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
            SELECT *
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
            SELECT AnimalID, AnimalName
            FROM AnimalDB.Animals;              
            ";
        using (var conn = _dataSource.OpenConnection())
        {
            return conn.Query<AnimalFeed>(sql);
        }
    }
    
}