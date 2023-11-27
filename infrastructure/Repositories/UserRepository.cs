using System.Runtime.InteropServices.JavaScript;
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
        using (var conn = _dataSource.OpenConnection())
        {
             return conn.Query<Users>(sqlGetUsers);
        }
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

    public void CreatePassword(string hash, string salt, int? userId)
    {
        var sql = "INSERT INTO animaldb.password " +
                  "(userid, passwordhashed,passwordsalt) " +
                  "VALUES (@userId, @hash, @salt)";
        using (var conn = _dataSource.OpenConnection())
        {
            conn.Execute(sql, new {userId, hash, salt});
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
            conn.Execute(sql, new {hash, salt, userId});
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
            conn.Execute(sql, new { user.ToBeDisabledDate, user.UserID });
        }
    }

    public void SetDisableUser(Users user)
    {
        var sql = "UPDATE animaldb.users " +
                  "SET Disabled=@Disabled, DisabledDate=@DisabledDate " +
                  "WHERE UserID=@UserID;";

        using (var conn = _dataSource.OpenConnection())
        {
            conn.Execute(sql, new { user.Disabled, DisabledDate = DateTime.Now, user.UserID });
        }
    }

    public void CheckUsersToBeDeleted()
    {
        var sqlDeleteFromPassword = "DELETE FROM animaldb.password WHERE userid IN (SELECT userid FROM animaldb.users WHERE DisabledDate < CURRENT_DATE - INTERVAL '1 month');";
        var sqlDeleteFromUsers = "DELETE FROM animaldb.users " +
                  "WHERE DisabledDate < CURRENT_DATE - INTERVAL '1 month';";

        using (var conn = _dataSource.OpenConnection())
        {
            conn.Execute(sqlDeleteFromPassword);
            conn.Execute(sqlDeleteFromUsers);
        }
    }

    public void CheckUsersToBeDisabled()
    {
        var sql = "UPDATE animaldb.users " +
                  "SET Disabled=@Disabled, DisabledDate=@DisabledDate, ToBeDisabledDate=null " +
                  "WHERE ToBeDisabledDate <= CURRENT_DATE;";

        using (var conn = _dataSource.OpenConnection())
        {
            conn.Execute(sql, new {Disabled = true, DisabledDate = DateTime.Now});
        }
    }
}