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
RETURNING AnimalSpecies.SpeciesID as {nameof(AnimalSpecies.SpeciesID)},
    speciesName as {nameof(AnimalSpecies.SpeciesName)},
    speciesDescription as {nameof(AnimalSpecies.SpeciesDescription)},
    speciesPicture as {nameof(AnimalSpecies.SpeciesPicture)};
";
        using (var conn = _dataSource.OpenConnection())
        {
            return conn.QueryFirst<AnimalSpecies>(sql, new { speciesName, speciesDescription, speciesPicture });
        }
    }

    public AnimalSpecies UpdateSpecies(string speciesName, string speciesDescription, string? speciesPicture)
    {
        var sql = $@"
UPDATE AnimalDB.AnimalSpecies SET speciesName = @speciesName, speciesDescription = @speciesDescription, speciesPicture = @speciesPicture
WHERE speciesID = @speciesID
RETURNING AnimalSpecies.SpeciesID as {nameof(AnimalSpecies.SpeciesID)},
    speciesName as {nameof(AnimalSpecies.SpeciesName)},
    speciesDescription as {nameof(AnimalSpecies.SpeciesDescription)},
    speciesPicture as {nameof(AnimalSpecies.SpeciesPicture)};
";
        using (var conn = _dataSource.OpenConnection())
        {
            return conn.QueryFirst<AnimalSpecies>(sql, new { speciesName, speciesDescription, speciesPicture});
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
    speciesID as {nameof(AnimalSpecies.SpeciesID)},
    speciesName as {nameof(AnimalSpecies.SpeciesName)},
    speciesDescription as {nameof(AnimalSpecies.SpeciesDescription)},
    speciesPicture as {nameof(AnimalSpecies.SpeciesPicture)} 
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
    speciesID as {{nameof(AnimalSpecies.SpeciesID)}},                  
    speciesName as {{nameof(AnimalSpecies.SpeciesName)}},              
    speciesPicture as {{nameof(AnimalSpecies.SpeciesPicture)}}
FROM AnimalDB.AnimalSpecies;              
";
        using (var conn = _dataSource.OpenConnection())
        {
            return conn.Query<AnimalSpeciesFeed>(sql);
        }
    }
}