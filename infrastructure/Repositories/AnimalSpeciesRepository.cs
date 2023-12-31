using Dapper;
using infrastructure.datamodels;
using Npgsql;

namespace infrastructure.Repositories;

public class AnimalSpeciesRepository
{
    private NpgsqlDataSource _dataSource;

    public AnimalSpeciesRepository(NpgsqlDataSource dataSource)
    {
        _dataSource = dataSource;
    }

    public AnimalSpecies CreateSpecies(string speciesName, string speciesDescription, string? speciesPicture)
    {
        var sql = $@"
INSERT INTO AnimalDB.AnimalSpecies (speciesName, speciesDescription, speciesPicture)
VALUES (@speciesName, @speciesDescription, @speciesPicture)
RETURNING AnimalSpecies.SpeciesID,
    SpeciesName,
    SpeciesDescription,
    SpeciesPicture;
";
        using (var conn = _dataSource.OpenConnection())
        {
            return conn.QueryFirst<AnimalSpecies>(sql, new { speciesName, speciesDescription, speciesPicture });
        }
    }

    public AnimalSpecies UpdateSpecies(AnimalSpecies animalSpeciesModel)
    {
        var sql = $@"
UPDATE AnimalDB.AnimalSpecies SET speciesName = @speciesName, speciesDescription = @speciesDescription, speciesPicture = @speciesPicture
WHERE speciesID = @speciesID
RETURNING *;
";
        using (var conn = _dataSource.OpenConnection())
        {
            return conn.QueryFirst<AnimalSpecies>(sql, new { animalSpeciesModel.SpeciesName, animalSpeciesModel.SpeciesDescription, animalSpeciesModel.SpeciesPicture, animalSpeciesModel.SpeciesID});
        }
    }

    public bool DeleteSpecies(int speciesID)
    {
        var sql = @"DELETE FROM AnimalDB.AnimalSpecies WHERE speciesID = @speciesID;";
        using (var conn = _dataSource.OpenConnection())
        {
            return conn.Execute(sql, new { speciesID }) == 1;
        }
    }

    public AnimalSpecies GetSpeciesById(int speciesID)
    {
        var sql = @$"
SELECT 
    SpeciesID,
    SpeciesName,
    SpeciesDescription,
    SpeciesPicture 
FROM AnimalDB.AnimalSpecies WHERE speciesID = @speciesID;
";

        using (var conn = _dataSource.OpenConnection())
        {
            return conn.QueryFirst<AnimalSpecies>(sql, new {speciesID});
        }
    }

    public IEnumerable<AnimalSpeciesFeed> GetSpeciesForFeed()
    {
        string sql = @$"
            SELECT                                                               
                SpeciesID,                  
                SpeciesName,              
                SpeciesPicture
            FROM AnimalDB.AnimalSpecies;              
            ";
        using (var conn = _dataSource.OpenConnection())
        {
            return conn.Query<AnimalSpeciesFeed>(sql);
        }
    }
}