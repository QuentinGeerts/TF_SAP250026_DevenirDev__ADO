CREATE TRIGGER [TR_Student_Active]
	ON [dbo].[Student]
	INSTEAD OF DELETE
	AS
	BEGIN
		SET NOCOUNT ON

		UPDATE [dbo].[Student]
		SET [Active] = 0
		WHERE [Id] = (SELECT [Id] FROM deleted)
	END
