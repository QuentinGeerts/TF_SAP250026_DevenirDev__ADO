CREATE TABLE [Sales].[PersonCreditCard] (
    [BusinessEntityID] INT      NOT NULL,
    [CreditCardID]     INT      NOT NULL,
    [ModifiedDate]     DATETIME CONSTRAINT [DF_PersonCreditCard_ModifiedDate] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_PersonCreditCard_BusinessEntityID_CreditCardID] PRIMARY KEY CLUSTERED ([BusinessEntityID] ASC, [CreditCardID] ASC),
    CONSTRAINT [FK_PersonCreditCard_CreditCard_CreditCardID] FOREIGN KEY ([CreditCardID]) REFERENCES [Sales].[CreditCard] ([CreditCardID]),
    CONSTRAINT [FK_PersonCreditCard_Person_BusinessEntityID] FOREIGN KEY ([BusinessEntityID]) REFERENCES [Person].[Person] ([BusinessEntityID])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Cross-reference table mapping people to their credit card information in the CreditCard table. ', @level0type = N'SCHEMA', @level0name = N'Sales', @level1type = N'TABLE', @level1name = N'PersonCreditCard';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Business entity identification number. Foreign key to Person.BusinessEntityID.', @level0type = N'SCHEMA', @level0name = N'Sales', @level1type = N'TABLE', @level1name = N'PersonCreditCard', @level2type = N'COLUMN', @level2name = N'BusinessEntityID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Credit card identification number. Foreign key to CreditCard.CreditCardID.', @level0type = N'SCHEMA', @level0name = N'Sales', @level1type = N'TABLE', @level1name = N'PersonCreditCard', @level2type = N'COLUMN', @level2name = N'CreditCardID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Date and time the record was last updated.', @level0type = N'SCHEMA', @level0name = N'Sales', @level1type = N'TABLE', @level1name = N'PersonCreditCard', @level2type = N'COLUMN', @level2name = N'ModifiedDate';

