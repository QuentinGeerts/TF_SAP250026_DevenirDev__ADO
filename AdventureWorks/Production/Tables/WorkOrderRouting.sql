CREATE TABLE [Production].[WorkOrderRouting] (
    [WorkOrderID]        INT            NOT NULL,
    [ProductID]          INT            NOT NULL,
    [OperationSequence]  SMALLINT       NOT NULL,
    [LocationID]         SMALLINT       NOT NULL,
    [ScheduledStartDate] DATETIME       NOT NULL,
    [ScheduledEndDate]   DATETIME       NOT NULL,
    [ActualStartDate]    DATETIME       NULL,
    [ActualEndDate]      DATETIME       NULL,
    [ActualResourceHrs]  DECIMAL (9, 4) NULL,
    [PlannedCost]        MONEY          NOT NULL,
    [ActualCost]         MONEY          NULL,
    [ModifiedDate]       DATETIME       CONSTRAINT [DF_WorkOrderRouting_ModifiedDate] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_WorkOrderRouting_WorkOrderID_ProductID_OperationSequence] PRIMARY KEY CLUSTERED ([WorkOrderID] ASC, [ProductID] ASC, [OperationSequence] ASC),
    CONSTRAINT [CK_WorkOrderRouting_ActualCost] CHECK ([ActualCost]>(0.00)),
    CONSTRAINT [CK_WorkOrderRouting_ActualEndDate] CHECK ([ActualEndDate]>=[ActualStartDate] OR [ActualEndDate] IS NULL OR [ActualStartDate] IS NULL),
    CONSTRAINT [CK_WorkOrderRouting_ActualResourceHrs] CHECK ([ActualResourceHrs]>=(0.0000)),
    CONSTRAINT [CK_WorkOrderRouting_PlannedCost] CHECK ([PlannedCost]>(0.00)),
    CONSTRAINT [CK_WorkOrderRouting_ScheduledEndDate] CHECK ([ScheduledEndDate]>=[ScheduledStartDate]),
    CONSTRAINT [FK_WorkOrderRouting_Location_LocationID] FOREIGN KEY ([LocationID]) REFERENCES [Production].[Location] ([LocationID]),
    CONSTRAINT [FK_WorkOrderRouting_WorkOrder_WorkOrderID] FOREIGN KEY ([WorkOrderID]) REFERENCES [Production].[WorkOrder] ([WorkOrderID])
);


GO
CREATE NONCLUSTERED INDEX [IX_WorkOrderRouting_ProductID]
    ON [Production].[WorkOrderRouting]([ProductID] ASC);

