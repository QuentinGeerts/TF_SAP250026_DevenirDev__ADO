CREATE TABLE [Sales].[SalesTerritory] (
    [TerritoryID]       INT              IDENTITY (1, 1) NOT NULL,
    [Name]              [dbo].[Name]     NOT NULL,
    [CountryRegionCode] NVARCHAR (3)     NOT NULL,
    [Group]             NVARCHAR (50)    NOT NULL,
    [SalesYTD]          MONEY            CONSTRAINT [DF_SalesTerritory_SalesYTD] DEFAULT ((0.00)) NOT NULL,
    [SalesLastYear]     MONEY            CONSTRAINT [DF_SalesTerritory_SalesLastYear] DEFAULT ((0.00)) NOT NULL,
    [CostYTD]           MONEY            CONSTRAINT [DF_SalesTerritory_CostYTD] DEFAULT ((0.00)) NOT NULL,
    [CostLastYear]      MONEY            CONSTRAINT [DF_SalesTerritory_CostLastYear] DEFAULT ((0.00)) NOT NULL,
    [rowguid]           UNIQUEIDENTIFIER CONSTRAINT [DF_SalesTerritory_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [ModifiedDate]      DATETIME         CONSTRAINT [DF_SalesTerritory_ModifiedDate] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_SalesTerritory_TerritoryID] PRIMARY KEY CLUSTERED ([TerritoryID] ASC),
    CONSTRAINT [CK_SalesTerritory_CostLastYear] CHECK ([CostLastYear]>=(0.00)),
    CONSTRAINT [CK_SalesTerritory_CostYTD] CHECK ([CostYTD]>=(0.00)),
    CONSTRAINT [CK_SalesTerritory_SalesLastYear] CHECK ([SalesLastYear]>=(0.00)),
    CONSTRAINT [CK_SalesTerritory_SalesYTD] CHECK ([SalesYTD]>=(0.00)),
    CONSTRAINT [FK_SalesTerritory_CountryRegion_CountryRegionCode] FOREIGN KEY ([CountryRegionCode]) REFERENCES [Person].[CountryRegion] ([CountryRegionCode])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [AK_SalesTerritory_Name]
    ON [Sales].[SalesTerritory]([Name] ASC);


GO
CREATE UNIQUE NONCLUSTERED INDEX [AK_SalesTerritory_rowguid]
    ON [Sales].[SalesTerritory]([rowguid] ASC);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Sales territory lookup table.', @level0type = N'SCHEMA', @level0name = N'Sales', @level1type = N'TABLE', @level1name = N'SalesTerritory';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Primary key for SalesTerritory records.', @level0type = N'SCHEMA', @level0name = N'Sales', @level1type = N'TABLE', @level1name = N'SalesTerritory', @level2type = N'COLUMN', @level2name = N'TerritoryID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Sales territory description', @level0type = N'SCHEMA', @level0name = N'Sales', @level1type = N'TABLE', @level1name = N'SalesTerritory', @level2type = N'COLUMN', @level2name = N'Name';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'ISO standard country or region code. Foreign key to CountryRegion.CountryRegionCode. ', @level0type = N'SCHEMA', @level0name = N'Sales', @level1type = N'TABLE', @level1name = N'SalesTerritory', @level2type = N'COLUMN', @level2name = N'CountryRegionCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Geographic area to which the sales territory belong.', @level0type = N'SCHEMA', @level0name = N'Sales', @level1type = N'TABLE', @level1name = N'SalesTerritory', @level2type = N'COLUMN', @level2name = N'Group';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Sales in the territory year to date.', @level0type = N'SCHEMA', @level0name = N'Sales', @level1type = N'TABLE', @level1name = N'SalesTerritory', @level2type = N'COLUMN', @level2name = N'SalesYTD';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Sales in the territory the previous year.', @level0type = N'SCHEMA', @level0name = N'Sales', @level1type = N'TABLE', @level1name = N'SalesTerritory', @level2type = N'COLUMN', @level2name = N'SalesLastYear';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Business costs in the territory year to date.', @level0type = N'SCHEMA', @level0name = N'Sales', @level1type = N'TABLE', @level1name = N'SalesTerritory', @level2type = N'COLUMN', @level2name = N'CostYTD';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Business costs in the territory the previous year.', @level0type = N'SCHEMA', @level0name = N'Sales', @level1type = N'TABLE', @level1name = N'SalesTerritory', @level2type = N'COLUMN', @level2name = N'CostLastYear';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'ROWGUIDCOL number uniquely identifying the record. Used to support a merge replication sample.', @level0type = N'SCHEMA', @level0name = N'Sales', @level1type = N'TABLE', @level1name = N'SalesTerritory', @level2type = N'COLUMN', @level2name = N'rowguid';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Date and time the record was last updated.', @level0type = N'SCHEMA', @level0name = N'Sales', @level1type = N'TABLE', @level1name = N'SalesTerritory', @level2type = N'COLUMN', @level2name = N'ModifiedDate';

