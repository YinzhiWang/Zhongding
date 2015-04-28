

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetProcurePlanReport]
	-- Add the parameters for the stored procedure here
    @pageSize INT = 10 ,
    @pageIndex INT = 0 ,
    @warehouseId INT = NULL ,
    @productName NVARCHAR(255) = NULL ,
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
        FROM    ( SELECT    ROW_NUMBER() OVER ( ORDER BY Temp1.WarehouseID ) AS rowId ,
                            Temp1.WarehouseID ,
                            Temp1.WarehouseName ,
                            Temp1.ProductID ,
                            Temp1.ProductName ,
                            Temp1.ProductSpecificationID ,
                            Temp1.Specification ,
                            Temp1.UnitName ,
                            Temp1.NumberInLargePackage ,
                            SUM(Temp1.ToBeOutQty) AS TotalToBeOutQty
                  FROM      ( SELECT    dbo.SalesOrderAppDetail.WarehouseID ,
                                        dbo.Warehouse.Name AS WarehouseName ,
                                        ProductID = dbo.SalesOrderAppDetail.ProductID ,
                                        ProductName = dbo.Product.ProductName ,
                                        dbo.SalesOrderAppDetail.ProductSpecificationID ,
                                        Specification = dbo.ProductSpecification.Specification ,
                                        dbo.UnitOfMeasurement.UnitName ,
                                        dbo.ProductSpecification.NumberInLargePackage ,
                                        ( dbo.SalesOrderAppDetail.Count
                                          + ISNULL(dbo.SalesOrderAppDetail.GiftCount,
                                                   0)
                                          - ISNULL(( SELECT SUM(dbo.StockOutDetail.OutQty)
                                                     FROM   dbo.StockOutDetail
                                                     WHERE  dbo.StockOutDetail.IsDeleted = 0
                                                            AND dbo.StockOutDetail.SalesOrderAppDetailID = dbo.SalesOrderAppDetail.ID
                                                   ), 0) ) AS ToBeOutQty
                              FROM      dbo.SalesOrderAppDetail
                                        JOIN dbo.Warehouse ON dbo.Warehouse.ID = dbo.SalesOrderAppDetail.WarehouseID
                                        JOIN ( SELECT   *
                                               FROM     dbo.SalesOrderApplication
                                               WHERE    dbo.SalesOrderApplication.IsDeleted = 0
                                                        AND dbo.SalesOrderApplication.IsStop = 0
                                             ) SalesOrderApplicationTemp ON dbo.SalesOrderAppDetail.SalesOrderApplicationID = SalesOrderApplicationTemp.ID
                                        JOIN dbo.ClientSaleApplication ON dbo.ClientSaleApplication.SalesOrderApplicationID = SalesOrderApplicationTemp.ID
                                        JOIN dbo.Product ON dbo.SalesOrderAppDetail.ProductID = dbo.Product.ID
                                        JOIN dbo.ProductSpecification ON dbo.SalesOrderAppDetail.ProductSpecificationID = dbo.ProductSpecification.ID
                                        JOIN dbo.UnitOfMeasurement ON dbo.UnitOfMeasurement.ID = dbo.ProductSpecification.UnitOfMeasurementID
                              WHERE     dbo.SalesOrderAppDetail.IsDeleted = 0
                                        AND (dbo.ClientSaleApplication.WorkflowStatusID = 14 OR dbo.ClientSaleApplication.WorkflowStatusID=3)/*发货中*/
                            ) Temp1
                  WHERE     Temp1.ToBeOutQty > 0
                            AND ( @warehouseId IS NULL
                                  OR Temp1.WarehouseID = @warehouseId
                                )
                            AND ( @productName IS NULL
                                  OR Temp1.ProductName LIKE '%' + @productName
                                  + '%'
                                )
                  GROUP BY  Temp1.WarehouseID ,
                            Temp1.WarehouseName ,
                            Temp1.ProductID ,
                            Temp1.ProductName ,
                            Temp1.ProductSpecificationID ,
                            Temp1.Specification ,
                            Temp1.UnitName ,
                            Temp1.NumberInLargePackage
                ) AS temp
        WHERE   temp.rowId BETWEEN @startRecord AND @endRecord
		

        SET @totalRecord = ( SELECT COUNT(*)
                             FROM   ( SELECT    Temp1.WarehouseID
                                      FROM      ( SELECT    dbo.SalesOrderAppDetail.WarehouseID ,
                                                            dbo.Warehouse.Name AS WarehouseName ,
                                                            ProductID = dbo.SalesOrderAppDetail.ProductID ,
                                                            ProductName = dbo.Product.ProductName ,
                                                            dbo.SalesOrderAppDetail.ProductSpecificationID ,
                                                            Specification = dbo.ProductSpecification.Specification ,
                                                            dbo.UnitOfMeasurement.UnitName ,
                                                            dbo.ProductSpecification.NumberInLargePackage ,
                                                            ( dbo.SalesOrderAppDetail.Count
                                                              + ISNULL(dbo.SalesOrderAppDetail.GiftCount,
                                                              0)
                                                              - ISNULL(( SELECT
                                                              SUM(dbo.StockOutDetail.OutQty)
                                                              FROM
                                                              dbo.StockOutDetail
                                                              WHERE
                                                              dbo.StockOutDetail.IsDeleted = 0
                                                              AND dbo.StockOutDetail.SalesOrderAppDetailID = dbo.SalesOrderAppDetail.ID
                                                              ), 0) ) AS ToBeOutQty
                                                  FROM      dbo.SalesOrderAppDetail
                                                            JOIN dbo.Warehouse ON dbo.Warehouse.ID = dbo.SalesOrderAppDetail.WarehouseID
                                                            JOIN ( SELECT
                                                              *
                                                              FROM
                                                              dbo.SalesOrderApplication
                                                              WHERE
                                                              dbo.SalesOrderApplication.IsDeleted = 0
                                                              AND dbo.SalesOrderApplication.IsStop = 0
                                                              ) SalesOrderApplicationTemp ON dbo.SalesOrderAppDetail.SalesOrderApplicationID = SalesOrderApplicationTemp.ID
                                                            JOIN dbo.ClientSaleApplication ON dbo.ClientSaleApplication.SalesOrderApplicationID = SalesOrderApplicationTemp.ID
                                                            JOIN dbo.Product ON dbo.SalesOrderAppDetail.ProductID = dbo.Product.ID
                                                            JOIN dbo.ProductSpecification ON dbo.SalesOrderAppDetail.ProductSpecificationID = dbo.ProductSpecification.ID
                                                            JOIN dbo.UnitOfMeasurement ON dbo.UnitOfMeasurement.ID = dbo.ProductSpecification.UnitOfMeasurementID
                                                  WHERE     dbo.SalesOrderAppDetail.IsDeleted = 0
                                                            AND dbo.ClientSaleApplication.WorkflowStatusID = 14/*发货中*/
                                                ) Temp1
                                      WHERE     Temp1.ToBeOutQty > 0
                                                AND ( @warehouseId IS NULL
                                                      OR Temp1.WarehouseID = @warehouseId
                                                    )
                                                AND ( @productName IS NULL
                                                      OR Temp1.ProductName LIKE '%'
                                                      + @productName + '%'
                                                    )
                                      GROUP BY  Temp1.WarehouseID ,
                                                Temp1.WarehouseName ,
                                                Temp1.ProductID ,
                                                Temp1.ProductName ,
                                                Temp1.ProductSpecificationID ,
                                                Temp1.Specification ,
                                                Temp1.UnitName ,
                                                Temp1.NumberInLargePackage
                                    ) Temp2
                           )
    END