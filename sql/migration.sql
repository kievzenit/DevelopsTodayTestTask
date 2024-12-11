CREATE PROCEDURE dbo.uspSetupDatabase
AS
BEGIN
    IF NOT EXISTS (SELECT 1 FROM sys.databases WHERE name = 'CabDb')
    BEGIN 
        CREATE DATABASE CabDb;
    END
    
    IF NOT EXISTS (SELECT 1 FROM sys.sql_logins WHERE name = 'cabUser')
    BEGIN
        CREATE LOGIN cabUser WITH PASSWORD = 'Some_password123', DEFAULT_DATABASE = CabDb;
        CREATE USER cabUser FOR LOGIN cabUser;
        GRANT ALL ON CabData TO cabUser;
    END
END
GO

EXECUTE dbo.uspSetupDatabase
GO

DROP PROCEDURE dbo.uspSetupDatabase
GO

USE CabDb
GO

CREATE PROCEDURE dbo.uspSetupTable
AS
BEGIN
    CREATE TABLE dbo.CabData (
        TpepPickupDateTime DATETIME2 NOT NULL,
        TpepDropoffDateTime DATETIME2 NOT NULL,
        PassengerCount INT NOT NULL,
        TripDistance FLOAT NOT NULL,
        StoreAndFwdFlag VARCHAR(3) NOT NULL,
        PULocationId INT NOT NULL,
        DOLocationId INT NOT NULL,
        FareAmount MONEY NOT NULL,
        TipAmount MONEY NOT NULL);
    
    CREATE NONCLUSTERED INDEX IDX_CabData_PULocationIdWithTipAmount ON dbo.CabData(PULocationId) INCLUDE (TipAmount);
    CREATE INDEX IDX_CabData_LongestTripDistance ON dbo.CabData(TripDistance);
    CREATE INDEX IDX_CabData_LongestTimeSpent ON dbo.CabData(TpepPickupDateTime, TpepDropoffDateTime);
    CREATE INDEX IDX_CabData_PULocationId ON dbo.CabData(PULocationId);
END
GO

EXECUTE dbo.uspSetupTable
GO

DROP PROCEDURE dbo.uspSetupTable
GO
