using Dapper;
using infrastructure.datamodels;
using Npgsql;

namespace infrastructure.Repositories;

public class SearchRepository
{
    private readonly NpgsqlDataSource _dataSource;

    public SearchRepository(NpgsqlDataSource dataSource)
    {
        _dataSource = dataSource;
    }

    public IEnumerable<AnimalSpeciesFeed> SearchAnimalSpecies(string searchTerm)
    {
        string sql = "SELECT SpeciesID, SpeciesName " +
                     "FROM AnimalDB.AnimalSpecies " +
                     "WHERE SpeciesName ILIKE '%' || @searchTerm || '%';";
        using (var conn = _dataSource.OpenConnection())
        {
            return conn.Query<AnimalSpeciesFeed>(sql, new {searchTerm});
        }
    }

    public IEnumerable<AnimalFeed>? SearchAnimals(string searchTerm)
    {
        string sql = "SELECT AnimalID, AnimalName " +
                     "FROM AnimalDB.Animals " +
                     "WHERE AnimalName ILIKE '%' || @searchTerm || '%';";
        using (var conn = _dataSource.OpenConnection())
        {
            return conn.Query<AnimalFeed>(sql, new {searchTerm});
        }
    }
}