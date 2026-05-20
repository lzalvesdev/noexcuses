CREATE DATABASE noexcusesfit


BEGIN TRY
	BEGIN TRANSACTION;

	CREATE TABLE UserAccount (
		Id 			UNIQUEIDENTIFIER PRIMARY KEY,
		FirstName 	NVARCHAR(50) NOT NULL,
		Email 		NVARCHAR(100) NOT NULL,
		Password 	NVARCHAR(255) NOT NULL,

		CONSTRAINT CK_UserAccount_Email CHECK (Email LIKE '%@%')
	);
		CREATE INDEX IX_UserAccount_Email ON UserAccount (Email);
		CREATE INDEX IX_UserAccount_DataForCoach ON UserAccount (Id) INCLUDE (FirstName, Email);


	CREATE TABLE Role (
	    Id   			INT PRIMARY KEY,
	    Description 	NVARCHAR(50) NOT NULL UNIQUE
	);


	CREATE TABLE UserRole (
	    UserAccountId 	UNIQUEIDENTIFIER NOT NULL,
	    RoleId        	INT NOT NULL,

	    PRIMARY KEY (UserAccountId, RoleId),

	    CONSTRAINT FK_UserRole_UserAccountId FOREIGN KEY (UserAccountId) REFERENCES UserAccount(Id) ON DELETE CASCADE,
	    CONSTRAINT FK_UserRole_RoleId        FOREIGN KEY (RoleId) REFERENCES Role(Id)
	);


	CREATE TABLE Coach (
		Id 				UNIQUEIDENTIFIER PRIMARY KEY,
		UserAccountId 	UNIQUEIDENTIFIER NOT NULL UNIQUE,

		CONSTRAINT FK_Coach_UserAccountId FOREIGN KEY (UserAccountId) REFERENCES UserAccount(Id)
	);


	CREATE TABLE Athlete (
		Id 				UNIQUEIDENTIFIER PRIMARY KEY,
		UserAccountId 	UNIQUEIDENTIFIER NOT NULL UNIQUE,
		CoachId 		UNIQUEIDENTIFIER NOT NULL,

		CONSTRAINT FK_Athlete_UserAccountId FOREIGN KEY (UserAccountId) REFERENCES UserAccount(Id),
		CONSTRAINT FK_Athlete_CoachId FOREIGN KEY (CoachId) REFERENCES Coach(Id) ON DELETE CASCADE
	);
		CREATE INDEX IX_Athlete_CoachId ON Athlete (CoachId);


	CREATE TABLE Speciality (
		Id 			INT IDENTITY(1,1) PRIMARY KEY,
		Description NVARCHAR(100) NOT NULL,
		CreatedAt 	DATETIME2 NOT NULL
	);
		CREATE INDEX IX_Speciality_CreatedAt ON Speciality (CreatedAt);
		CREATE INDEX IX_Speciality_Description ON Speciality (Description);


	CREATE TABLE RefreshToken (
	    Id            UNIQUEIDENTIFIER PRIMARY KEY,
	    UserAccountId UNIQUEIDENTIFIER NOT NULL,
	    Token         NVARCHAR(255)    NOT NULL UNIQUE,
	    ExpiresAt     DATETIME2        NOT NULL,
	    CreatedAt     DATETIME2        NOT NULL,
	    RevokedAt     DATETIME2        NULL,

	    CONSTRAINT FK_RefreshToken_UserAccountId FOREIGN KEY (UserAccountId) REFERENCES UserAccount(Id) ON DELETE CASCADE
	);

	CREATE TABLE CoachSpeciality (
		SpecialityId 	INT NOT NULL,
		CoachId 		UNIQUEIDENTIFIER NOT NULL,

		PRIMARY KEY (SpecialityId, CoachId),
		CONSTRAINT FK_CoachSpeciality_SpecialityId FOREIGN KEY (SpecialityId) REFERENCES Speciality(Id) ON DELETE CASCADE,
		CONSTRAINT FK_CoachSpeciality_CoachId FOREIGN KEY (CoachId) REFERENCES Coach(Id) ON DELETE CASCADE
	);

   	COMMIT TRANSACTION;

END TRY
BEGIN CATCH
	ROLLBACK TRANSACTION;

END CATCH;



INSERT INTO Role (Id, Description)
VALUES
(1, 'Coach'),
(2, 'Athlete'),
(3, 'User'),
(4, 'Manager'),
(99, 'Admin');
