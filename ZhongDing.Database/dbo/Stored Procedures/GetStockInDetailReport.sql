

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetStockInDetailReport]
	-- Add the parameters for the stored procedure here
    @pageSize INT = 10 ,
    @pageIndex INT = 0 ,
    @beginDate DATETIME = NULL ,
    @endDate DATETIME = NULL ,
    @supplierId INT = NULL ,
    @productId INT = NULL ,
    @batchNumber NVARCHAR(256) = NULL ,
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
        FROM    ( SELECT    ROW_NUMBER() OVER ( ORDER BY dbo.StockInDetail.ID ) AS rowId ,
                            dbo.StockInDetail.ID ,
                            dbo.StockIn.EntryDate ,
                            dbo.StockIn.Code AS StockInCode ,
                            dbo.ProcureOrderApplication.OrderCode ,
                            dbo.Supplier.SupplierName ,
                            dbo.Supplier.FactoryName ,
                            dbo.Warehouse.Name AS WarehouseName ,
                            dbo.Product.ProductName ,
                            dbo.Product.ProductCode ,
                            dbo.ProductCategory.CategoryName ,
                            dbo.ProductSpecification.Specification ,
                            dbo.UnitOfMeasurement.UnitName ,
                            dbo.StockInDetail.ProcurePrice ,
                            dbo.StockInDetail.InQty ,
                            NumberOfPackages = ( CASE WHEN dbo.ProductSpecification.NumberInLargePackage > 0
                                                      THEN dbo.StockInDetail.InQty
                                                           / ( dbo.ProductSpecification.NumberInLargePackage )
                                                      ELSE 1
                                                 END ) ,
                            dbo.StockInDetail.BatchNumber ,
                            dbo.StockInDetail.ExpirationDate ,
                            TotalAmount = dbo.StockInDetail.InQty
                            * dbo.StockInDetail.ProcurePrice
                  FROM      dbo.StockInDetail
                            JOIN dbo.StockIn ON dbo.StockIn.ID = dbo.StockInDetail.StockInID
                            JOIN dbo.ProcureOrderApplication ON dbo.ProcureOrderApplication.ID = dbo.StockInDetail.ProcureOrderAppID
                            JOIN dbo.Supplier ON dbo.ProcureOrderApplication.SupplierID = dbo.Supplier.ID
                            JOIN dbo.Warehouse ON dbo.StockInDetail.WarehouseID = dbo.Warehouse.ID
                            JOIN dbo.Product ON dbo.StockInDetail.ProductID = dbo.Product.ID
                            JOIN dbo.ProductCategory ON dbo.Product.CategoryID = dbo.ProductCategory.ID
                            JOIN dbo.ProductSpecification ON dbo.StockInDetail.ProductSpecificationID = dbo.ProductSpecification.ID
                            JOIN dbo.UnitOfMeasurement ON dbo.ProductSpecification.UnitOfMeasurementID = dbo.UnitOfMeasurement.ID
                  WHERE     dbo.StockInDetail.IsDeleted = 0
                            AND ( @beginDate IS NULL
                                  OR dbo.ProcureOrderApplication.OrderDate >= @beginDate
                                )
                            AND ( @endDate IS NULL
                                  OR dbo.ProcureOrderApplication.OrderDate < @endDate
                                )
                            AND ( @supplierId IS NULL
                                  OR dbo.ProcureOrderApplication.SupplierID = @supplierId
                                )
                            AND ( @productId IS NULL
                                  OR dbo.StockInDetail.ProductID = @productId
                                )
                            AND ( @batchNumber IS NULL
                                  OR dbo.StockInDetail.BatchNumber LIKE '%'
                                  + @batchNumber + '%'
                                )
                ) AS temp
        WHERE   temp.rowId BETWEEN @startRecord AND @endRecord
		

        SET @totalRecord = ( SELECT COUNT(dbo.StockInDetail.ID)
                             FROM   dbo.StockInDetail
                                    JOIN dbo.StockIn ON dbo.StockIn.ID = dbo.StockInDetail.StockInID
                                    JOIN dbo.ProcureOrderApplication ON dbo.ProcureOrderApplication.ID = dbo.StockInDetail.ProcureOrderAppID
                                    JOIN dbo.Supplier ON dbo.ProcureOrderApplication.SupplierID = dbo.Supplier.ID
                                    JOIN dbo.Warehouse ON dbo.StockInDetail.WarehouseID = dbo.Warehouse.ID
                                    JOIN dbo.Product ON dbo.StockInDetail.ProductID = dbo.Product.ID
                                    JOIN dbo.ProductCategory ON dbo.Product.CategoryID = dbo.ProductCategory.ID
                                    JOIN dbo.ProductSpecification ON dbo.StockInDetail.ProductSpecificationID = dbo.ProductSpecification.ID
                                    JOIN dbo.UnitOfMeasurement ON dbo.ProductSpecification.UnitOfMeasurementID = dbo.UnitOfMeasurement.ID
                             WHERE  dbo.StockInDetail.IsDeleted = 0
                                    AND ( @beginDate IS NULL
                                          OR dbo.ProcureOrderApplication.OrderDate >= @beginDate
                                        )
                                    AND ( @endDate IS NULL
                                          OR dbo.ProcureOrderApplication.OrderDate < @endDate
                                        )
                                    AND ( @supplierId IS NULL
                                          OR dbo.ProcureOrderApplication.SupplierID = @supplierId
                                        )
                                    AND ( @productId IS NULL
                                          OR dbo.StockInDetail.ProductID = @productId
                                        )
                                    AND ( @batchNumber IS NULL
                                          OR dbo.StockInDetail.BatchNumber LIKE '%'
                                          + @batchNumber + '%'
                                        )
                           )
    END