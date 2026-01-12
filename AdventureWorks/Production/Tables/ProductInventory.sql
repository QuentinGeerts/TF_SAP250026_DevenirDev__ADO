CREATE TABLE [Production].[ProductInventory] (
    [ProductID]    INT              NOT NULL,
    [LocationID]   SMALLINT         NOT NULL,
    [Shelf]        NVARCHAR (10)    NOT NULL,
    [Bin]          TINYINT          NOT NULL,
    [Quantity]     SMALLINT         CONSTRAINT [DF_ProductInventory_Quantity] DEFAULT ((0)) NOT NULL,
    [rowguid]      UNIQUEIDENTIFIER CONSTRAINT [DF_ProductInventory_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [ModifiedDate] DATETIME         CONSTRAINT [DF_ProductInventory_ModifiedDate] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_ProductInventory_ProductID_LocationID] PRIMARY KEY CLUSTERED ([ProductID] ASC, [LocationID] ASC),
    CONSTRAINT [CK_ProductInventory_Bin] CHECK ([Bin]>=(0) AND [Bin]<=(100)),
    CONSTRAINT [CK_ProductInventory_Shelf] CHECK ([Shelf] like '[A-Za-z]' OR [Shelf]='N/A'),
    CONSTRAINT [FK_ProductInventory_Location_LocationID] FOREIGN KEY ([LocationID]) REFERENCES [Production].[Location] ([LocationID]),
    CONSTRAINT [FK_ProductInventory_Product_ProductID] FOREIGN KEY ([ProductID]) REFERENCES [Production].[Product] ([ProductID])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Product inventory information.', @level0type = N'SCHEMA', @level0name = N'Production', @level1type = N'TABLE', @level1name = N'ProductInventory';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Product identification number. Foreign key to Product.ProductID.', @level0type = N'SCHEMA', @level0name = N'Production', @level1type = N'TABLE', @level1name = N'ProductInventory', @level2type = N'COLUMN', @level2name = N'ProductID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Inventory location identification number. Foreign key to Location.LocationID. ', @level0type = N'SCHEMA', @level0name = N'Production', @level1type = N'TABLE', @level1name = N'ProductInventory', @level2type = N'COLUMN', @level2name = N'LocationID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Storage compartment within an inventory location.', @level0type = N'SCHEMA', @level0name = N'Production', @level1type = N'TABLE', @level1name = N'ProductInventory', @level2type = N'COLUMN', @level2name = N'Shelf';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Storage container on a shelf in an inventory location.', @level0type = N'SCHEMA', @level0name = N'Production', @level1type = N'TABLE', @level1name = N'ProductInventory', @level2type = N'COLUMN', @level2name = N'Bin';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Quantity of products in the inventory location.', @level0type = N'SCHEMA', @level0name = N'Production', @level1type = N'TABLE', @level1name = N'ProductInventory', @level2type = N'COLUMN', @level2name = N'Quantity';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'ROWGUIDCOL number uniquely identifying the record. Used to support a merge replication sample.', @level0type = N'SCHEMA', @level0name = N'Production', @level1type = N'TABLE', @level1name = N'ProductInventory', @level2type = N'COLUMN', @level2name = N'rowguid';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Date and time the record was last updated.', @level0type = N'SCHEMA', @level0name = N'Production', @level1type = N'TABLE', @level1name = N'ProductInventory', @level2type = N'COLUMN', @level2name = N'ModifiedDate';

