CREATE PROCEDURE [dbo].[SP_Todo_AddTodo]
	@Title NVARCHAR(100),
	@Description NVARCHAR(MAX) = NULL,
	@UserId INT
AS
BEGIN 
	SET NOCOUNT ON
	BEGIN TRY
		IF NOT EXISTS (SELECT 1 FROM [dbo].[V_User] WHERE [Id] = @UserId)
		BEGIN
			RAISERROR('User doesn''t exists.', 16, 10)
		END

		INSERT INTO [dbo].[Todo] (Title, Description, Status, UserId)
		VALUES (@Title, @Description, 'To do', @UserId)
	END TRY

	BEGIN CATCH
		THROW;
	END CATCH

	SET NOCOUNT OFF
END
