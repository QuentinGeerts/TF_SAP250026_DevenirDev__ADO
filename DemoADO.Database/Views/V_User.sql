CREATE VIEW [dbo].[V_User]
	AS 
	SELECT [Id], [Email], [Lastname], [Firstname] 
	FROM [User]
	WHERE [IsActive] = 1
