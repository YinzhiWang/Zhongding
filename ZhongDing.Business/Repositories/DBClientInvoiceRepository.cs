using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ZhongDing.Business.IRepositories;
using ZhongDing.Domain.Models;
using ZhongDing.Domain.UIObjects;
using ZhongDing.Common.Extension;

namespace ZhongDing.Business.Repositories
{
    public class DBClientInvoiceRepository : BaseRepository<DBClientInvoice>, IDBClientInvoiceRepository
    {
        public IList<Domain.UIObjects.UIDBClientInvoice> GetUIList(Domain.UISearchObjects.UISearchDBClientInvoice uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {
            List<UIDBClientInvoice> uiWarehouses = new List<UIDBClientInvoice>();
            int total = 0;

            IQueryable<DBClientInvoice> query = null;

            List<Expression<Func<DBClientInvoice, bool>>> whereFuncs = new List<Expression<Func<DBClientInvoice, bool>>>();

            if (uiSearchObj != null)
            {

                if (uiSearchObj.BeginDate.HasValue)
                    whereFuncs.Add(x => x.InvoiceDate >= uiSearchObj.BeginDate);

                if (uiSearchObj.EndDate.HasValue)
                {
                    uiSearchObj.EndDate = uiSearchObj.EndDate.Value.AddDays(1);
                    whereFuncs.Add(x => x.InvoiceDate < uiSearchObj.EndDate);
                }

                if (uiSearchObj.InvoiceNumber.IsNotNullOrEmpty())
                {
                    whereFuncs.Add(x => x.InvoiceNumber.Contains(uiSearchObj.InvoiceNumber));
                }
                if (uiSearchObj.CompanyID > 0)
                {
                    whereFuncs.Add(x => x.CompanyID == uiSearchObj.CompanyID);
                }
                if (uiSearchObj.DistributionCompanyID.BiggerThanZero())
                {
                    whereFuncs.Add(x => x.DistributionCompanyID == uiSearchObj.DistributionCompanyID);
                }
            }

            query = GetList(pageIndex, pageSize, whereFuncs, out total);

            if (query != null)
            {
                uiWarehouses = (from q in query
                                join dBClientInvoiceDetail in DB.DBClientInvoiceDetail on q.ID equals dBClientInvoiceDetail.DBClientInvoiceID
                                join stockOutDetail in DB.StockOutDetail on dBClientInvoiceDetail.StockOutDetailID equals stockOutDetail.ID
                                join stockOut in DB.StockOut on stockOutDetail.StockOutID equals stockOut.ID
                                join product in DB.Product on stockOutDetail.ProductID equals product.ID
                                join company in DB.Company on q.CompanyID equals company.ID
                                join distributionCompany in DB.DistributionCompany on q.DistributionCompanyID equals distributionCompany.ID
                                select new UIDBClientInvoice()
                                 {
                                     ID = q.ID,
                                     Amount = q.Amount,
                                     CompanyID = q.CompanyID,
                                     CreatedOn = q.CreatedOn,
                                     InvoiceDate = q.InvoiceDate,
                                     InvoiceNumber = q.InvoiceNumber,
                                     CompanyName = company.CompanyName,
                                     StockOutCode = stockOut.Code,
                                     StockOutOutDate = stockOut.OutDate,
                                     ProductName = product.ProductName,
                                     ProductID = product.ID,
                                     OutQty = stockOutDetail.OutQty,
                                     SalesPrice = stockOutDetail.SalesPrice,
                                     TotalSalesAmount = stockOutDetail.TotalSalesAmount,
                                     TaxQty = stockOutDetail.TaxQty,
                                     DistributionCompanyName = distributionCompany.Name,
                                     DBClientInvoiceDetailTaxAmount = dBClientInvoiceDetail.Amount,
                                     StockOutDetailSalesAmount = stockOutDetail.TotalSalesAmount,
                                     SaleOrderType = q.SaleOrderType.TypeName
                                 }).ToList();
            }

            totalRecords = total;
            uiWarehouses.ForEach(x =>
            {
                x.DBClientInvoiceDetailQty = (int)(x.DBClientInvoiceDetailTaxAmount / x.SalesPrice);
                x.DBClientInvoiceDetailAmount = x.SalesPrice * x.DBClientInvoiceDetailQty;

            });
            return uiWarehouses;
        }

        
    }
}
