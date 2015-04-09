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
    public class ClientInvoiceRepository : BaseRepository<ClientInvoice>, IClientInvoiceRepository
    {
        public IList<Domain.UIObjects.UIClientInvoice> GetUIList(Domain.UISearchObjects.UISearchClientInvoice uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {
            List<UIClientInvoice> uiWarehouses = new List<UIClientInvoice>();
            int total = 0;

            IQueryable<ClientInvoice> query = null;

            List<Expression<Func<ClientInvoice, bool>>> whereFuncs = new List<Expression<Func<ClientInvoice, bool>>>();

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
            }

            query = GetList(pageIndex, pageSize, whereFuncs, out total);

            if (query != null)
            {
                uiWarehouses = (from q in query
                                join clientInvoiceDetail in DB.ClientInvoiceDetail on q.ID equals clientInvoiceDetail.ClientInvoiceID
                                join stockOutDetail in DB.StockOutDetail on clientInvoiceDetail.StockOutDetailID equals stockOutDetail.ID
                                join stockOut in DB.StockOut on stockOutDetail.StockOutID equals stockOut.ID
                                join product in DB.Product on stockOutDetail.ProductID equals product.ID
                                join company in DB.Company on q.CompanyID equals company.ID
                                join clientCompany in DB.ClientCompany on q.ClientCompanyID equals clientCompany.ID
                                select new UIClientInvoice()
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
                                     ClientCompanyName = clientCompany.Name,
                                     ClientInvoiceDetailTaxAmount = clientInvoiceDetail.Amount,

                                 }).ToList();
            }

            totalRecords = total;
            uiWarehouses.ForEach(x =>
            {
                x.ClientInvoiceDetailQty = (int)(x.ClientInvoiceDetailTaxAmount / x.SalesPrice);
                x.ClientInvoiceDetailAmount = x.SalesPrice * x.ClientInvoiceDetailQty;

            });
            return uiWarehouses;
        }
    }
}
