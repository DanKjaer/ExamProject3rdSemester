using System.Runtime.InteropServices.JavaScript;
using Dapper;
using infrastructure.datamodels;
using Npgsql;

namespace infrastructure.Repositories;

public class UserRepository
{
    private NpgsqlDataSource _dataSource;
    /*private DateTime DisabledDate;*/
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
            "WHERE userID=@UserID";
        
        using (var conn = _dataSource.OpenConnection())
        {
            return conn.QueryFirst<Users>(sql, new {UserID = id});
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


    public void CreatePassword(string hash, string salt, int? userId)
    {
        var sql = "INSERT INTO animaldb.password " +
                  "(userid, passwordhashed,passwordsalt) " +
                  "VALUES (@userId, @hash, @salt)";
        using (var conn = _dataSource.OpenConnection())
        {
            conn.Query(sql, new {userId, hash, salt});
        }
    }

    public Users CreateUser(Users user)
    {
        var sql = "INSERT INTO animaldb.users" +
                  "(UserName, UserEmail, PhoneNumber, Usertype, ToBeDisabledDate)" +
                  "VALUES (@UserName, @UserEmail, @PhoneNumber, @UserType, @ToBeDisabledDate) " +
                  "RETURNING *;";

        using (var conn = _dataSource.OpenConnection())
        {
            return conn.QueryFirst<Users>(sql, new {user.UserName, user.UserEmail, user.PhoneNumber, user.UserType, user.ToBeDisabledDate,});
        }
    }

    public void UpdatePassword(string hash, string salt, int? userId)
    {
        const string sql = "UPDATE animaldb.password " +
                  "SET passwordhashed=@hash, passwordsalt=@salt " +
                  "WHERE userid=@userId;";

        using (var conn = _dataSource.OpenConnection())
        {
            conn.Query(sql, new {hash, salt, userId});
        }
    }

    public Users UpdateUser(Users user)
    {
        const string sql = "UPDATE animaldb.users " +
                  "SET username=@UserName, useremail=@UserEmail, phonenumber=@PhoneNumber, " +
                  "usertype=@UserType " +
                  "WHERE userid=@UserID " +
                  "RETURNING *;";

        using (var conn = _dataSource.OpenConnection())
        {
            return conn.QueryFirst<Users>(sql, new {user.UserName, user.UserEmail, user.PhoneNumber, user.UserType, user.UserID});
        }
    }

    public void SetToBeDisabledDate(Users user)
    {
        var sql = "UPDATE animaldb.users " +
                  "SET ToBeDisabledDate=@ToBeDisabledDate " +
                  "WHERE UserID=@UserID;";

        using (var conn = _dataSource.OpenConnection())
        {
            conn.Query(sql, new { user.ToBeDisabledDate, user.UserID });
        }
    }

    public void SetDisableUser(Users user)
    {
        var sql = "UPDATE animaldb.users " +
                  "SET Disabled=@Disabled, DisabledDate=@DisabledDate " +
                  "WHERE UserID=@UserID;";

        using (var conn = _dataSource.OpenConnection())
        {
            conn.Query(sql, new { user.Disabled, DisabledDate = DateTime.Now, user.UserID });
        }
    }

    public void CheckUsersToBeDeleted()
    {
        var sql = "DELETE FROM animaldb.users " +
                  "WHERE DisabledDate < CURRENT_DATE - INTERVAL '1 month';";

        using (var conn = _dataSource.OpenConnection())
        {
            conn.Query(sql);
        }
    }

    public void CheckUsersToBeDisabled()
    {
        var sql = "UPDATE animaldb.users " +
                  "SET Disabled=@Disabled " +
                  "WHERE ToBeDisabledDate <= CURRENT_DATE;";

        using (var conn = _dataSource.OpenConnection())
        {
            conn.Query(sql, true);
        }
    }
}