CREATE TABLE [dbo].[Student]
(
	[Id] INT CONSTRAINT PK_Student PRIMARY KEY IDENTITY, 
    [FirstName] VARCHAR(50) NOT NULL, 
    [LastName] VARCHAR(50) NOT NULL, 
    [BirthDate] DATETIME2 NOT NULL, 
    [YearResult] INT NULL, 
    [SectionId] INT NOT NULL, 
    [Active] BIT NULL DEFAULT 1, 

    CONSTRAINT [FK_Student_Section] FOREIGN KEY ([SectionId]) REFERENCES [Section]([Id]), 
    CONSTRAINT [CK_Student_YearResult] CHECK ([YearResult] BETWEEN 0 AND 20), 
    CONSTRAINT [CK_Student_BirthDate] CHECK ([BirthDate] >= '1930-01-01') 
)
