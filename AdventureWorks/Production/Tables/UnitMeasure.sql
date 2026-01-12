CREATE TABLE [Production].[UnitMeasure] (
    [UnitMeasureCode] NCHAR (3)    NOT NULL,
    [Name]            [dbo].[Name] NOT NULL,
    [ModifiedDate]    DATETIME     CONSTRAINT [DF_UnitMeasure_ModifiedDate] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_UnitMeasure_UnitMeasureCode] PRIMARY KEY CLUSTERED ([UnitMeasureCode] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [AK_UnitMeasure_Name]
    ON [Production].[UnitMeasure]([Name] ASC);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Unit of measure lookup table.', @level0type = N'SCHEMA', @level0name = N'Production', @level1type = N'TABLE', @level1name = N'UnitMeasure';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Primary key.', @level0type = N'SCHEMA', @level0name = N'Production', @level1type = N'TABLE', @level1name = N'UnitMeasure', @level2type = N'COLUMN', @level2name = N'UnitMeasureCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Unit of measure description.', @level0type = N'SCHEMA', @level0name = N'Production', @level1type = N'TABLE', @level1name = N'UnitMeasure', @level2type = N'COLUMN', @level2name = N'Name';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Date and time the record was last updated.', @level0type = N'SCHEMA', @level0name = N'Production', @level1type = N'TABLE', @level1name = N'UnitMeasure', @level2type = N'COLUMN', @level2name = N'ModifiedDate';

