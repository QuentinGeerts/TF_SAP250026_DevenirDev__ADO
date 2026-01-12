CREATE PROCEDURE [dbo].[SP_Student_AddStudent]
	@Lastname VARCHAR(50),
	@Firstname VARCHAR(50),
	@BirthDate DATETIME2(7),
	@YearResult INT = NULL,
	@SectionId INT
AS
BEGIN

	IF NOT EXISTS (SELECT 1 FROM [dbo].[Section] WHERE [Id] = @SectionId)
		RAISERROR('La section n''existe pas.', 16, 1)

	INSERT INTO [dbo].[Student] (LastName, FirstName, BirthDate, YearResult, SectionId)
	VALUES (@Lastname, @Firstname, @BirthDate, @YearResult, @SectionId)

END
