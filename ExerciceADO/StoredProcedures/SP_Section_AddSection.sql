CREATE PROCEDURE [dbo].[SP_Section_AddSection]
	@SectionId INT,
	@SectionName VARCHAR(50)
AS
BEGIN

	IF EXISTS (SELECT 1 FROM [dbo].[Section] WHERE [Id] = @SectionId)
		RAISERROR('La section existe déjà.', 16, 1)

	INSERT INTO [dbo].[Section]
	VALUES (@SectionId, @SectionName)

END