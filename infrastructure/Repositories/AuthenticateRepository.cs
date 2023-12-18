using Dapper;
using infrastructure.datamodels;
using Npgsql;

namespace infrastructure.Repositories;

public class AuthenticateRepository
{
    private NpgsqlDataSource _dataSource;
    public AuthenticateRepository(NpgsqlDataSource dataSource)
    {
        _dataSource = dataSource;
    }

    public PasswordHash GetUserByEmail(string userEmail)
    {
        const string sql = "SELECT password.UserID, password.PasswordHashed, password.PasswordSalt " +
                           "FROM animaldb.password " +
                           "JOIN animaldb.users ON password.UserID = users.UserID " +
                           "WHERE animaldb.users.UserEmail = @userEmail;";
        using (var conn = _dataSource.OpenConnection())
        {
            return conn.QueryFirst<PasswordHash>(sql, new {userEmail});
        }
    }
}