CREATE TABLE [dbo].[DeptDistrict] (
    [ID]               INT           IDENTITY (1, 1) NOT NULL,
    [DepartmentTypeID] INT           NOT NULL,
    [DistrictName]     NVARCHAR (50) NULL,
    CONSTRAINT [PK_DeptDistrict] PRIMARY KEY CLUSTERED ([ID] ASC)
);

