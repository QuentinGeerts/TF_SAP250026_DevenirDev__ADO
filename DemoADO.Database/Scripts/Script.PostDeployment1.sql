/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

-- Gestion des users


SET IDENTITY_INSERT [dbo].[User] ON;

INSERT INTO [dbo].[User] (Id, Email, Password, Lastname, Firstname) VALUES
(1, 'quentin.geerts@bstorm.be', HASHBYTES('SHA2_256', 'Test1234='), 'Geerts', 'Quentin'),
(2, 'thierry.morre@cognitic.be', HASHBYTES('SHA2_256', 'Test1234?'), NULL, NULL),
(3, 'michael.person@cognitic.be', HASHBYTES('SHA2_256', 'Test1234!'), NULL, NULL),
(4, 'samuel.legrain@cognitic.be', HASHBYTES('SHA2_256', 'Test1234='), 'Legrain', 'Samuel')

SET IDENTITY_INSERT [dbo].[User] OFF;


GO

-- Gestion des Todos

EXEC SP_Todo_AddTodo 'Aller travailler', 'Oui, faut bien gagner sa croutte', 1
EXEC SP_Todo_AddTodo 'Faire la lessive', 'Plus rien à se mettre', 1
EXEC SP_Todo_AddTodo 'Changer la litière', 'Ca commence à sentir', 2
EXEC SP_Todo_AddTodo 'Nettoyage du marai', 'Farquad a envoyé ses troupes', 1

GO