-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetClientSaleAppReport]
	-- Add the parameters for the stored procedure here
    @pageSize INT = 10 ,
    @pageIndex INT = 0 ,
    @beginDate DATETIME = NULL ,
    @endDate DATETIME = NULL ,
    @clientUserId INT = NULL ,
    @clientCompanyId INT = NULL ,
    @productId INT = NULL ,
    @totalRecord INT = 0 OUTPUT
AS
    BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
        SET NOCOUNT ON;
    -- Insert statements for procedure here
        DECLARE @startRecord INT ,
            @endRecord INT
        SET @startRecord = @pageIndex * @pageSize + 1
        SET @endRecord = @startRecord + @pageSize - 1 
                           
        SELECT  *
        FROM    ( SELECT    ROW_NUMBER() OVER ( ORDER BY dbo.SalesOrderAppDetail.ID ) AS rowId ,
                            dbo.SalesOrderAppDetail.ID ,
                            dbo.SalesOrderApplication.OrderDate ,
                            dbo.SalesOrderApplication.OrderCode ,
                            dbo.ClientUser.ClientName ,
                            dbo.ClientCompany.Name AS ClientCompanyName ,
                            dbo.Product.ProductName ,
                            dbo.Product.ProductCode ,
                            dbo.ProductCategory.CategoryName ,
                            dbo.ProductSpecification.Specification ,
                            dbo.UnitOfMeasurement.UnitName ,
                            dbo.SalesOrderAppDetail.Count ,
                            dbo.SalesOrderAppDetail.SalesPrice ,
                            dbo.SalesOrderAppDetail.TotalSalesAmount
                  FROM      dbo.SalesOrderAppDetail
                            JOIN dbo.SalesOrderApplication ON dbo.SalesOrderApplication.ID = dbo.SalesOrderAppDetail.SalesOrderApplicationID
                            JOIN dbo.ClientSaleApplication ON dbo.ClientSaleApplication.SalesOrderApplicationID = dbo.SalesOrderApplication.ID
                            JOIN dbo.ClientUser ON dbo.ClientUser.ID = dbo.ClientSaleApplication.ClientUserID
                            JOIN dbo.ClientCompany ON dbo.ClientCompany.ID = dbo.ClientSaleApplication.ClientCompanyID
                            JOIN dbo.Product ON dbo.SalesOrderAppDetail.ProductID = dbo.Product.ID
                            JOIN dbo.ProductCategory ON dbo.Product.CategoryID = dbo.ProductCategory.ID
                            JOIN dbo.ProductSpecification ON dbo.SalesOrderAppDetail.ProductSpecificationID = dbo.ProductSpecification.ID
                            JOIN dbo.UnitOfMeasurement ON dbo.ProductSpecification.UnitOfMeasurementID = dbo.UnitOfMeasurement.ID
                  WHERE     dbo.SalesOrderAppDetail.IsDeleted = 0
                            AND ( @beginDate IS NULL
                                  OR dbo.SalesOrderApplication.OrderDate >= @beginDate
                                )
                            AND ( @endDate IS NULL
                                  OR dbo.SalesOrderApplication.OrderDate < @endDate
                                )
                            AND ( @clientUserId IS NULL
                                  OR dbo.ClientSaleApplication.ClientUserID = @clientUserId
                                )
                            AND ( @clientCompanyId IS NULL
                                  OR dbo.ClientSaleApplication.ClientCompanyID = @clientCompanyId
                                )
                            AND ( @productId IS NULL
                                  OR dbo.SalesOrderAppDetail.ProductID = @productId
                                )
                ) AS temp
        WHERE   temp.rowId BETWEEN @startRecord AND @endRecord
		

        SET @totalRecord = ( SELECT COUNT(dbo.SalesOrderAppDetail.ID)
                             FROM   dbo.SalesOrderAppDetail
                                    JOIN dbo.SalesOrderApplication ON dbo.SalesOrderApplication.ID = dbo.SalesOrderAppDetail.SalesOrderApplicationID
                                    JOIN dbo.ClientSaleApplication ON dbo.ClientSaleApplication.SalesOrderApplicationID = dbo.SalesOrderApplication.ID
                                    JOIN dbo.ClientUser ON dbo.ClientUser.ID = dbo.ClientSaleApplication.ClientUserID
                                    JOIN dbo.ClientCompany ON dbo.ClientCompany.ID = dbo.ClientSaleApplication.ClientCompanyID
                                    JOIN dbo.Product ON dbo.SalesOrderAppDetail.ProductID = dbo.Product.ID
                                    JOIN dbo.ProductCategory ON dbo.Product.CategoryID = dbo.ProductCategory.ID
                                    JOIN dbo.ProductSpecification ON dbo.SalesOrderAppDetail.ProductSpecificationID = dbo.ProductSpecification.ID
                                    JOIN dbo.UnitOfMeasurement ON dbo.ProductSpecification.UnitOfMeasurementID = dbo.UnitOfMeasurement.ID
                             WHERE  dbo.SalesOrderAppDetail.IsDeleted = 0
                                    AND ( @beginDate IS NULL
                                          OR dbo.SalesOrderApplication.OrderDate >= @beginDate
                                        )
                                    AND ( @endDate IS NULL
                                          OR dbo.SalesOrderApplication.OrderDate < @endDate
                                        )
                                    AND ( @clientUserId IS NULL
                                          OR dbo.ClientSaleApplication.ClientUserID = @clientUserId
                                        )
                                    AND ( @clientCompanyId IS NULL
                                          OR dbo.ClientSaleApplication.ClientCompanyID = @clientCompanyId
                                        )
                                    AND ( @productId IS NULL
                                          OR dbo.SalesOrderAppDetail.ProductID = @productId
                                        )
                           )
    END