
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetProcureOrderReport]
	-- Add the parameters for the stored procedure here
    @pageSize INT = 10 ,
    @pageIndex INT = 0 ,
    @beginDate DATETIME = NULL ,
    @endDate DATETIME = NULL ,
    @supplierId INT = NULL ,
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
        FROM    ( SELECT    ROW_NUMBER() OVER ( ORDER BY dbo.ProcureOrderAppDetail.ID ) AS rowId ,
                            dbo.ProcureOrderAppDetail.ID ,
                            dbo.ProcureOrderApplication.OrderDate ,
                            dbo.ProcureOrderApplication.OrderCode ,
                            dbo.Supplier.SupplierName ,
                            dbo.Warehouse.Name AS WarehouseName ,
                            dbo.Product.ProductName ,
                            dbo.Product.ProductCode ,
                            dbo.ProductCategory.CategoryName ,
                            dbo.ProductSpecification.Specification ,
                            dbo.UnitOfMeasurement.UnitName ,
                            dbo.ProcureOrderAppDetail.ProcureCount ,
                            dbo.ProcureOrderAppDetail.ProcurePrice ,
                            dbo.ProcureOrderAppDetail.TotalAmount ,
                            ISNULL(( SELECT SUM(dbo.StockInDetail.InQty)
                                     FROM   dbo.StockInDetail
                                            JOIN dbo.StockIn ON dbo.StockIn.ID = dbo.StockInDetail.StockInID
                                     WHERE  dbo.StockInDetail.IsDeleted = 0
                                            AND dbo.StockIn.IsDeleted = 0
                                            AND dbo.StockIn.WorkflowStatusID = 10
                                            AND dbo.StockInDetail.ProcureOrderAppDetailID = dbo.ProcureOrderAppDetail.ID
                                   ), 0) AS AlreadyInQty ,
                            ISNULL(( SELECT SUM(dbo.StockInDetail.ProcurePrice
                                                * dbo.StockInDetail.InQty)
                                     FROM   dbo.StockInDetail
                                     WHERE  dbo.StockInDetail.IsDeleted = 0
                                            AND dbo.StockInDetail.ProcureOrderAppDetailID = dbo.ProcureOrderAppDetail.ID
                                   ), 0) AS AlreadyInQtyProcurePrice ,
                            dbo.ProductSpecification.NumberInLargePackage ,
                            ProcureOrderApplication.IsStop AS ProcureOrderApplicationIsStop
                  FROM      dbo.ProcureOrderAppDetail
                            JOIN dbo.ProcureOrderApplication ON dbo.ProcureOrderAppDetail.ProcureOrderApplicationID = dbo.ProcureOrderApplication.ID
                            JOIN dbo.Supplier ON dbo.ProcureOrderApplication.SupplierID = dbo.Supplier.ID
                            JOIN dbo.Warehouse ON dbo.ProcureOrderAppDetail.WarehouseID = dbo.Warehouse.ID
                            JOIN dbo.Product ON dbo.ProcureOrderAppDetail.ProductID = dbo.Product.ID
                            JOIN dbo.ProductCategory ON dbo.Product.CategoryID = dbo.ProductCategory.ID
                            JOIN dbo.ProductSpecification ON dbo.ProcureOrderAppDetail.ProductSpecificationID = dbo.ProductSpecification.ID
                            JOIN dbo.UnitOfMeasurement ON dbo.ProductSpecification.UnitOfMeasurementID = dbo.UnitOfMeasurement.ID
                  WHERE     dbo.ProcureOrderAppDetail.IsDeleted = 0
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
                                  OR dbo.ProcureOrderAppDetail.ProductID = @productId
                                )
                            AND dbo.ProcureOrderApplication.WorkflowStatusID IN (
                            8, 14, 15 )
                ) AS temp
        WHERE   temp.rowId BETWEEN @startRecord AND @endRecord
		

        SET @totalRecord = ( SELECT COUNT(dbo.ProcureOrderAppDetail.ID)
                             FROM   dbo.ProcureOrderAppDetail
                                    JOIN dbo.ProcureOrderApplication ON dbo.ProcureOrderAppDetail.ProcureOrderApplicationID = dbo.ProcureOrderApplication.ID
                                    JOIN dbo.Supplier ON dbo.ProcureOrderApplication.SupplierID = dbo.Supplier.ID
                                    JOIN dbo.Warehouse ON dbo.ProcureOrderAppDetail.WarehouseID = dbo.Warehouse.ID
                                    JOIN dbo.Product ON dbo.ProcureOrderAppDetail.ProductID = dbo.Product.ID
                                    JOIN dbo.ProductCategory ON dbo.Product.CategoryID = dbo.ProductCategory.ID
                                    JOIN dbo.ProductSpecification ON dbo.ProcureOrderAppDetail.ProductSpecificationID = dbo.ProductSpecification.ID
                                    JOIN dbo.UnitOfMeasurement ON dbo.ProductSpecification.UnitOfMeasurementID = dbo.UnitOfMeasurement.ID
                             WHERE  dbo.ProcureOrderAppDetail.IsDeleted = 0
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
                                          OR dbo.ProcureOrderAppDetail.ProductID = @productId
                                        )
                                    AND dbo.ProcureOrderApplication.WorkflowStatusID IN (
                                    8, 14, 15 )
                           )
    END