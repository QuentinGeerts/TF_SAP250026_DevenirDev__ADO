CREATE PROCEDURE [dbo].[SP_Student_UpdateStudent]
	@StudentId INT,
	@SectionId INT,
	@YearResult INT = NULL
AS
BEGIN
	IF NOT EXISTS (SELECT 1 FROM [dbo].[Student] WHERE [Id] = @StudentId)
		RAISERROR('L''étudiant est introuvable.', 16, 1)

	IF NOT EXISTS (SELECT 1 FROM [dbo].[Section] WHERE [Id] = @SectionId)
		RAISERROR('La section est introuvable.', 16, 1)

	IF @YearResult = NULL
		UPDATE [dbo].[Student] 
		SET [SectionId] = @SectionId 
		WHERE [Id] = @StudentId
	ELSE 
		UPDATE [dbo].[Student] 
		SET  [SectionId] = @SectionId, [YearResult] = @YearResult 
		WHERE [Id] = @StudentId
END
