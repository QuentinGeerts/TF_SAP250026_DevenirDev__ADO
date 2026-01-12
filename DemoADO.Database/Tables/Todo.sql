CREATE TABLE [dbo].[Todo]
(
	[Id] INT IDENTITY,
    [Title] NVARCHAR(100) NOT NULL,
    [Description] NVARCHAR(MAX) NULL,
    [Status] NVARCHAR(20) NOT NULL,
    [CreatedAt] DATETIME2 NULL DEFAULT GETDATE(), 
    [UpdatedAt] DATETIME2 NULL DEFAULT GETDATE(), 
    [IsActive] BIT NULL DEFAULT 1, 

    [UserId] INT NOT NULL,

    CONSTRAINT PK_Todo PRIMARY KEY (Id), 
    CONSTRAINT [FK_Todo_User] FOREIGN KEY ([UserId]) 
        REFERENCES [User]([Id]),
)


GO

CREATE TRIGGER [dbo].[TR_Todo_UpdatedAt]
    ON [dbo].[Todo]
    AFTER UPDATE
    AS
    BEGIN
        SET NoCount ON
        UPDATE [dbo].[Todo]
        SET [UpdatedAt] = GETDATE()
        WHERE [Id] = (SELECT [Id] FROM inserted)
    END
GO

CREATE TRIGGER [dbo].[TR_Todo_IsActive]
    ON [dbo].[Todo]
    INSTEAD OF DELETE
    AS
    BEGIN
        SET NoCount ON
        UPDATE [dbo].[Todo]
        SET [IsActive] = 0
        WHERE [Id] = (SELECT [Id] FROM deleted)
    END