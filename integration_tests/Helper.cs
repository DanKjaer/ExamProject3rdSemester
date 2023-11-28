using Dapper;
using Newtonsoft.Json;
using Npgsql;

namespace integration_tests;

public static class Helper
{
    public static readonly Uri Uri;
    public static readonly string ProperlyFormattedConnectionString;
    public static readonly NpgsqlDataSource DataSource;


    static Helper()
    {
        string rawConnectionString;
        string envVarKeyName = "pgconn";

        rawConnectionString = Environment.GetEnvironmentVariable(envVarKeyName)!;
        Console.WriteLine(rawConnectionString);
        if (rawConnectionString == null)
        {
            throw new Exception("Empty pgconn");
        }

        try
        {
            Uri = new Uri(rawConnectionString);
            ProperlyFormattedConnectionString = string.Format(
                "Server={0};Database={1};User Id={2};Password={3};Port={4};Pooling=true;MaxPoolSize=3",
                Uri.Host,
                Uri.AbsolutePath.Trim('/'),
                Uri.UserInfo.Split(':')[0],
                Uri.UserInfo.Split(':')[1],
                Uri.Port > 0 ? Uri.Port : 5432);
            DataSource =
                new NpgsqlDataSourceBuilder(ProperlyFormattedConnectionString).Build();
            DataSource.OpenConnection().Close();
        }
        catch (Exception e)
        {
            throw new Exception("Connection string couldn't be used, fix ur shit");
        }
    }

    public static void TriggerRebuild()
    {
        using (var conn = DataSource.OpenConnection())
        {
                conn.Execute(RebuildScript);
        }
    }
    
    public static string RebuildScript = @"
DROP SCHEMA IF EXISTS AnimalDB CASCADE;
CREATE SCHEMA AnimalDB;

CREATE TABLE AnimalDB.AnimalSpecies (
    SpeciesID SERIAL PRIMARY KEY,
    SpeciesName VARCHAR(50) NOT NULL,
    SpeciesDescription TEXT,
    SpeciesPicture TEXT
);

CREATE TABLE AnimalDB.Animals (
    AnimalID SERIAL PRIMARY KEY,
    SpeciesID INTEGER REFERENCES AnimalDB.AnimalSpecies(SpeciesID),
    AnimalName VARCHAR(50) NOT NULL,
    AnimalBirthday DATE,
    AnimalGender BOOLEAN,
    AnimalDead BOOLEAN,
    AnimalPicture TEXT,
    AnimalWeight FLOAT
);

CREATE TABLE AnimalDB.AnimalNote (
    NoteID SERIAL PRIMARY KEY,
    AnimalID INTEGER REFERENCES AnimalDB.Animals(AnimalID),
    NoteDate DATE NOT NULL,
    NoteText TEXT
);

CREATE TABLE AnimalDB.UserTypes (
    UserTypeID SERIAL PRIMARY KEY,
    UserTypeName VARCHAR(50) NOT NULL
);

CREATE TABLE AnimalDB.Users (
    UserID SERIAL PRIMARY KEY,
    UserName VARCHAR(50) NOT NULL,
    UserEmail VARCHAR(50) NOT NULL,
    PhoneNumber VARCHAR(20),
    Disabled BOOLEAN,
    ToBeDisabledDate DATE,
    DisabledDate DATE,
    UserType INTEGER REFERENCES AnimalDB.UserTypes(UserTypeID)
);


CREATE TABLE AnimalDB.Password (
    UserID INTEGER REFERENCES AnimalDB.Users(UserID),
    PasswordHashed TEXT,
    PasswordSalt TEXT
);

INSERT INTO animaldb.animalspecies (SpeciesName) VALUES ('abe')";

}