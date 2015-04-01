
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetProcureOrderApplicationPaymentReport]
	-- Add the parameters for the stored procedure here
    @pageSize INT = 10 ,
    @pageIndex INT = 0 ,
    @beginDate DATETIME = NULL ,
    @endDate DATETIME = NULL ,
    @supplierId INT = NULL ,
    @totalRecord INT = 0 OUTPUT
AS
    BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
        SET NOCOUNT ON;
    -- Insert statements for procedure here
        DECLARE @startRecord INT ,
            @endRecord INT ,
            @workflowID INT= 1--这个代表的是 采购订单
        SET @startRecord = @pageIndex * @pageSize + 1
        SET @endRecord = @startRecord + @pageSize - 1 
                           
        SELECT  *
        FROM    ( SELECT    ROW_NUMBER() OVER ( ORDER BY dbo.ApplicationPayment.ID ) AS rowId ,
                            dbo.ApplicationPayment.PayDate ,
                            dbo.ProcureOrderApplication.OrderCode ,
                            dbo.ApplicationPayment.Amount ,
                            dbo.ApplicationPayment.FromAccount ,
                            dbo.ApplicationPayment.ToAccount
                  FROM      dbo.ApplicationPayment
                            JOIN dbo.ProcureOrderApplication ON dbo.ApplicationPayment.ApplicationID = dbo.ProcureOrderApplication.ID
                  WHERE     dbo.ApplicationPayment.IsDeleted = 0
                            AND dbo.ApplicationPayment.WorkflowID = @workflowID
                            AND ( @beginDate IS NULL
                                  OR dbo.ApplicationPayment.PayDate >= @beginDate
                                )
                            AND ( @endDate IS NULL
                                  OR dbo.ApplicationPayment.PayDate < @endDate
                                )
                            AND ( @supplierId IS NULL
                                  OR dbo.ProcureOrderApplication.SupplierID = @supplierId
                                )
                ) AS temp
        WHERE   temp.rowId BETWEEN @startRecord AND @endRecord
		

        SET @totalRecord = ( SELECT COUNT(dbo.ApplicationPayment.ID)
                             FROM   dbo.ApplicationPayment
                                    JOIN dbo.ProcureOrderApplication ON dbo.ApplicationPayment.ApplicationID = dbo.ProcureOrderApplication.ID
                             WHERE  
							 dbo.ApplicationPayment.IsDeleted=0 AND
							dbo.ApplicationPayment.WorkflowID = @workflowID
                                    AND ( @beginDate IS NULL
                                          OR dbo.ApplicationPayment.PayDate >= @beginDate
                                        )
                                    AND ( @endDate IS NULL
                                          OR dbo.ApplicationPayment.PayDate < @endDate
                                        )
                                    AND ( @supplierId IS NULL
                                          OR dbo.ProcureOrderApplication.SupplierID = @supplierId
                                        )
                           )
    END