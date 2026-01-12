CREATE TABLE [dbo].[User]
(
	[Id] INT,
	[Email] NVARCHAR(100) NOT NULL, 
    [Password] NVARCHAR(255) NOT NULL,
    [Lastname] NVARCHAR(50) NULL, 
    [Firstname] NVARCHAR(50) NULL, 
    [CreatedAt] DATETIME2 NULL DEFAULT GETDATE(), 
    [UpdatedAt] DATETIME2 NULL DEFAULT GETDATE(), 
    [IsActive] BIT NULL DEFAULT 1, 

    CONSTRAINT PK_User PRIMARY KEY (Id), 
    CONSTRAINT [CK_User_Email] CHECK (Email LIKE '%__@%__.%__')
)

GO

CREATE TRIGGER [dbo].[TR_User_UpdatedAt]
    ON [dbo].[User]
    AFTER UPDATE
    AS
    BEGIN
        SET NoCount ON
        UPDATE [dbo].[User]
        SET [UpdatedAt] = GETDATE()
        WHERE [Id] = (SELECT [Id] FROM inserted)
    END
GO

-- Déclencheur qui remplace le comportement par défaut d'une suppression
-- Par la désactivation de celui-ci (Soft-delete)
CREATE TRIGGER [dbo].[TR_User_IsActive]
    ON [dbo].[User]
    INSTEAD OF DELETE
    AS
    BEGIN
        SET NoCount ON
        UPDATE [dbo].[User]
        SET [IsActive] = 0
        WHERE [Id] = (SELECT [Id] FROM deleted)
    END