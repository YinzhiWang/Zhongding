
CREATE PROCEDURE GetCompanyList

AS
BEGIN

	SET NOCOUNT ON;

	SELECT c.ID,
           c.CompanyCode,
           c.CompanyName,
           c.ProviderTexRatio,
           c.ClientTaxHighRatio,
           c.ClientTaxLowRatio,
           c.EnableTaxDeduction,
           c.ClientTaxDeductionRatio,
           c.CreatedOn,
           cu.UserName      AS CreatedBy,
           c.LastModifiedOn AS LastModifiedBy,
           mu.UserName
    FROM   Company c
           LEFT JOIN Users cu
                  ON cu.UserID = c.CreatedBy
           LEFT JOIN Users mu
                  ON mu.UserID = c.LastModifiedBy
    WHERE  Isnull(c.IsDeleted, 0) = 0 
    
END