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
            try
            {
                conn.Execute(RebuildScript);
            }
            catch (Exception e)
            {
                throw new Exception("couldn't rebuild database");
            }

        }
    }

    public static string RebuildScript = $@"
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

CREATE TABLE AnimalDB.UserTypes (
    UserTypeID SERIAL PRIMARY KEY,
    UserTypeName VARCHAR(50) NOT NULL
);

CREATE TABLE AnimalDB.Password (
    UserID INTEGER REFERENCES AnimalDB.Users(UserID),
    PasswordHashed TEXT,
    PasswordSalt TEXT
);

INSERT INTO AnimalDB.AnimalSpecies
VALUES ('Hest', 'Får ordnet hove kl 3 om natten', 'https://videnskab.dk/wp-content/uploads/2016/08/shutterstock-121352503.jpg');

INSERT INTO AnimalDB.AnimalSpecies
VALUES ('Flodhest', 'Overvåger hesten få ordnet hove kl 3 om natten', 'https://safaritanzania.dk/wp-content/uploads/2015/03/flodhesren.jpeg');

INSERT INTO AnimalDB.Animals
VALUES (1, 'Bobby', '12:04:2004', true, false, 'https://www.equistroem.dk/wp-content/uploads/2020/03/FC9540C9-B304-4EE3-8960-475DB1E387DD-2048x1070.jpg', '9420kg');

INSERT INTO AnimalDB.Animals
VALUES (1, 'Ursula', '10:10:2020', false, false, 'https://evidensia.dk/getmedia/cd6f9b67-bee2-4f01-b6aa-228deaf834a1/Overv%c3%a6gt-hos-hest', '320kg');

INSERT INTO AnimalDB.Animals
VALUES (2, 'Inga', '29:09:2016', false, false, 'https://encrypted-tbn2.gstatic.com/images?q=tbn:ANd9GcTum9aWYyQ_FcT9UvYPmmMq-gxawKM15xdc835F3d1fl_o0I0Qk', '94920kg');

INSERT INTO AnimalDB.Animals
VALUES (2, 'Finn', '08:02:2017', true, false, 'https://safaritanzania.dk/wp-content/uploads/2015/03/Flodhest11.jpeg', '3200kg');
";

}