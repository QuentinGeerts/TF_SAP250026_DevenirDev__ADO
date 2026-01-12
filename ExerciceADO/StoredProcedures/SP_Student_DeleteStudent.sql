CREATE PROCEDURE [dbo].[SP_Student_DeleteStudent]
	@StudentId INT
AS
BEGIN
	IF NOT EXISTS (SELECT 1 FROM [dbo].[Student] WHERE [Id] = @StudentId)
		RAISERROR('L''étudiant est introuvable.', 16, 1)

	DELETE FROM [dbo].[Student] WHERE [Id] = @StudentId
END