
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetStockOutDetailReport]
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
        FROM    ( SELECT    ROW_NUMBER() OVER ( ORDER BY dbo.StockOutDetail.ID ) AS rowId ,
                            dbo.StockOutDetail.ID ,
                            dbo.StockOut.OutDate ,
                            dbo.StockOut.Code  AS StockOutCode,
                            dbo.SalesOrderApplication.OrderCode AS SalesOrderApplicationOrderCode ,
                            dbo.ClientUser.ClientName ,
                            dbo.ClientCompany.Name AS ClientCompanyName,
                            dbo.Warehouse.Name  AS WarehouseName,
                            dbo.ProductCategory.CategoryName ,
                            dbo.Product.ProductCode ,
                            dbo.Product.ProductName ,
                            dbo.ProductSpecification.Specification ,
                            dbo.UnitOfMeasurement.UnitName ,
                            dbo.StockOutDetail.OutQty ,
                            NumberOfPackages = ( CASE WHEN dbo.ProductSpecification.NumberInLargePackage > 0
                                                      THEN dbo.StockOutDetail.OutQty
                                                           / ( dbo.ProductSpecification.NumberInLargePackage )
                                                      ELSE 1
                                                 END ) ,
                            dbo.StockOutDetail.BatchNumber ,
                            dbo.StockOutDetail.ExpirationDate ,
                            dbo.StockOutDetail.SalesPrice ,
                            dbo.StockOutDetail.TotalSalesAmount
                  FROM      dbo.StockOutDetail
                            JOIN dbo.StockOut ON dbo.StockOut.ID = dbo.StockOutDetail.StockOutID
                            JOIN dbo.SalesOrderApplication ON dbo.SalesOrderApplication.ID = dbo.StockOutDetail.SalesOrderApplicationID
                            JOIN dbo.ClientUser ON dbo.ClientUser.ID = dbo.StockOut.ClientUserID
                            JOIN dbo.ClientCompany ON dbo.ClientCompany.ID = dbo.StockOut.ClientCompanyID
                            JOIN dbo.Warehouse ON dbo.Warehouse.ID = dbo.StockOutDetail.WarehouseID
                            JOIN dbo.Product ON dbo.Product.ID = dbo.StockOutDetail.ProductID
                            JOIN dbo.ProductCategory ON dbo.Product.CategoryID = dbo.ProductCategory.ID
                            JOIN dbo.ProductSpecification ON dbo.StockOutDetail.ProductSpecificationID = dbo.ProductSpecification.ID
                            JOIN dbo.UnitOfMeasurement ON dbo.ProductSpecification.UnitOfMeasurementID = dbo.UnitOfMeasurement.ID
                  WHERE     dbo.StockOutDetail.IsDeleted = 0
                            AND ( @beginDate IS NULL
                                  OR dbo.StockOut.OutDate >= @beginDate
                                )
                            AND ( @endDate IS NULL
                                  OR dbo.StockOut.OutDate < @endDate
                                )
                            AND ( @clientUserId IS NULL
                                  OR dbo.StockOut.ClientUserID = @clientUserId
                                )
                            AND ( @clientCompanyId IS NULL
                                  OR dbo.StockOut.ClientCompanyID = @clientCompanyId
                                )
                            AND ( @productId IS NULL
                                  OR dbo.StockOutDetail.ProductID = @productId
                                )
                ) AS temp
        WHERE   temp.rowId BETWEEN @startRecord AND @endRecord
		

        SET @totalRecord = ( SELECT COUNT(dbo.StockOutDetail.ID)
                             FROM   dbo.StockOutDetail
                                    JOIN dbo.StockOut ON dbo.StockOut.ID = dbo.StockOutDetail.StockOutID
                                    JOIN dbo.SalesOrderApplication ON dbo.SalesOrderApplication.ID = dbo.StockOutDetail.SalesOrderApplicationID
                                    JOIN dbo.ClientUser ON dbo.ClientUser.ID = dbo.StockOut.ClientUserID
                                    JOIN dbo.ClientCompany ON dbo.ClientCompany.ID = dbo.StockOut.ClientCompanyID
                                    JOIN dbo.Warehouse ON dbo.Warehouse.ID = dbo.StockOutDetail.WarehouseID
                                    JOIN dbo.Product ON dbo.Product.ID = dbo.StockOutDetail.ProductID
                                    JOIN dbo.ProductCategory ON dbo.Product.CategoryID = dbo.ProductCategory.ID
                                    JOIN dbo.ProductSpecification ON dbo.StockOutDetail.ProductSpecificationID = dbo.ProductSpecification.ID
                                    JOIN dbo.UnitOfMeasurement ON dbo.ProductSpecification.UnitOfMeasurementID = dbo.UnitOfMeasurement.ID
                             WHERE  dbo.StockOutDetail.IsDeleted = 0
                                    AND ( @beginDate IS NULL
                                          OR dbo.StockOut.OutDate >= @beginDate
                                        )
                                    AND ( @endDate IS NULL
                                          OR dbo.StockOut.OutDate < @endDate
                                        )
                                    AND ( @clientUserId IS NULL
                                          OR dbo.StockOut.ClientUserID = @clientUserId
                                        )
                                    AND ( @clientCompanyId IS NULL
                                          OR dbo.StockOut.ClientCompanyID = @clientCompanyId
                                        )
                                    AND ( @productId IS NULL
                                          OR dbo.StockOutDetail.ProductID = @productId
                                        )
                           )
    END