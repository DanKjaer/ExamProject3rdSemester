using Dapper;
using infrastructure.datamodels;
using Npgsql;

namespace infrastructure.Repositories;

public class UserRepository
{
    private NpgsqlDataSource _dataSource;

    public UserRepository(NpgsqlDataSource dataSource)
    {
        _dataSource = dataSource;
    }

    public IEnumerable<Users> GetUsers()
    {
        var sqlGetUsers = "SELECT * FROM animaldb.users";
        //var userTypes = GetUserTypes();
        using (var conn = _dataSource.OpenConnection())
        {
             return conn.Query<Users>(sqlGetUsers);
        }

        /*
        foreach (var user in result)
        {
            if (user.UserType == null)
            {
                continue;
            }
            if (user.UserType.UserTypeID == 1)
            {
                user.UserType = userTypes.ToList()[0];
            }
            else if (user.UserType.UserTypeID == 2)
            {
                user.UserType = userTypes.ToList()[1];
            }
            else
            {
                user.UserType = userTypes.ToList()[3];
            }
        }

        return result;*/
    }

    public Users GetUserById(int id)
    {
        var sql = 
            "SELECT * FROM animaldb.users " +
            "WHERE userID = @id";
        
        using (var conn = _dataSource.OpenConnection())
        {
            return conn.QueryFirst<Users>(sql);
        }
    }

    /*public IEnumerable<UserType> GetUserTypes()
    {
        var sqlGetUserTypes = "SELECT * FROM animal.usertypes";

        using (var conn = _dataSource.OpenConnection())
        {
            return conn.Query<UserType>(sqlGetUserTypes);
        }
    }*/
    
    
}