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

EXEC SP_User_AddUser 'quentin.geerts@bstorm.be', 'Test1234=', 'Geerts', 'Quentin'
EXEC SP_User_AddUser 'thierry.morre@cognitic.be', 'Test1234?'
EXEC SP_User_AddUser 'michael.person@cognitic.be', 'Test1234!'
EXEC SP_User_AddUser 'samuel.legrain@cognitic.be', 'Test1234=', 'Legrain', 'Samuel'

GO

-- Gestion des Todos

EXEC SP_Todo_AddTodo 'Aller travailler', 'Oui, faut bien gagner sa croutte', 1
EXEC SP_Todo_AddTodo 'Faire la lessive', 'Plus rien à se mettre', 1
EXEC SP_Todo_AddTodo 'Changer la litière', 'Ca commence à sentir', 2
EXEC SP_Todo_AddTodo 'Nettoyage du marai', 'Farquad a envoyé ses troupes', 1

GO