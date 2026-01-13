CREATE PROCEDURE [dbo].[SP_User_AddUser]
	@Email NVARCHAR(100),
	@Password NVARCHAR(255),
	@Lastname NVARCHAR(50) = NULL,
	@Firstname NVARCHAR(50) = NULL,
	@UserId INT OUTPUT
AS
BEGIN

	SET NOCOUNT ON

	BEGIN TRY

		IF EXISTS (SELECT 1 FROM [dbo].[User] WHERE [Email] = @Email)
		BEGIN
			RAISERROR('Email already exists.', 16, 13)
		END

		INSERT INTO [dbo].[User] (Email, Password, Lastname, Firstname)
		VALUES (@Email, HASHBYTES('SHA2_256', @Password), @Lastname, @Firstname)

		SELECT @UserId = @@IDENTITY FROM [dbo].[User];

	END TRY

	BEGIN CATCH
		THROW;
	END CATCH

	SET NOCOUNT OFF

END
