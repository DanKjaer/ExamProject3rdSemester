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
    DisabledDate DATE,
    UserType INTEGER REFERENCES AnimalDB.UserTypes(UserTypeID)
);

CREATE TABLE AnimalDB.Password (
    UserID INTEGER REFERENCES AnimalDB.Users(UserID),
    PasswordHashed TEXT,
    PasswordSalt TEXT
);

CREATE TABLE AnimalDB.LastCheckUserDisable (
    LastCheckUserDisableID SERIAL PRIMARY KEY,
    LastCheck DATE
);