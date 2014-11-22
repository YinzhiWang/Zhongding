CREATE TABLE [dbo].[ProductDBPolicyPrice] (
    [ID]                     INT            IDENTITY (1, 1) NOT NULL,
    [ProductID]              INT            NOT NULL,
    [ProductSpecificationID] INT            NOT NULL,
    [BidPrice]               MONEY          NULL,
    [FeeRatio]               FLOAT (53)     NULL,
    [PreferredPrice]         MONEY          NULL,
    [PolicyPrice]            MONEY          NULL,
    [Comment]                NVARCHAR (255) NULL,
    [IsDeleted]              BIT            CONSTRAINT [DF_ProductDBPolicyPrice_IsDeleted] DEFAULT ((0)) NOT NULL,
    [CreatedOn]              DATETIME       CONSTRAINT [DF_ProductDBPolicyPrice_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]              INT            NULL,
    [LastModifiedOn]         DATETIME       NULL,
    [LastModifiedBy]         INT            NULL,
    CONSTRAINT [PK_ProductDBPolicyPrice] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_ProductDBPolicyPrice_Product] FOREIGN KEY ([ProductID]) REFERENCES [dbo].[Product] ([ID]),
    CONSTRAINT [FK_ProductDBPolicyPrice_ProductSpecification] FOREIGN KEY ([ProductSpecificationID]) REFERENCES [dbo].[ProductSpecification] ([ID])
);







