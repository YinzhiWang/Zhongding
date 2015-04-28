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
                if (uiSearchObj.ClientCompanyID.BiggerThanZero())
                {
                    whereFuncs.Add(x => x.ClientCompanyID == uiSearchObj.ClientCompanyID);
                }
                if (uiSearchObj.CompanyID > 0)
                {
                    whereFuncs.Add(x => x.CompanyID == uiSearchObj.CompanyID);
                }
            }

            query = GetList(pageIndex, pageSize, whereFuncs, out total);

            if (query != null)
            {
                uiWarehouses = (from q in query
                                join cid in DB.ClientInvoiceDetail on q.ID equals cid.ClientInvoiceID
                                join sod in DB.StockOutDetail on cid.StockOutDetailID equals sod.ID
                                join so in DB.StockOut on sod.StockOutID equals so.ID
                                join p in DB.Product on sod.ProductID equals p.ID
                                join c in DB.Company on q.CompanyID equals c.ID
                                join cc in DB.ClientCompany on q.ClientCompanyID equals cc.ID
                                join cu in DB.ClientUser on so.ClientUserID equals cu.ID
                                select new UIClientInvoice()
                                 {
                                     ID = q.ID,
                                     CompanyID = q.CompanyID,
                                     InvoiceDate = q.InvoiceDate,
                                     SaleOrderType = q.SaleOrderType.TypeName,
                                     InvoiceNumber = q.InvoiceNumber,
                                     CompanyName = c.CompanyName,
                                     StockOutCode = so.Code,
                                     ProductName = p.ProductName,
                                     ClientCompanyName = cc.Name,
                                     ClientUserName = cu.ClientName,
                                     InvoiceQty = cid.Qty ?? 0,
                                     TotalSalesAmount = cid.StockOutDetail.SalesOrderApplication.SalesOrderAppDetail.Any(x => x.IsDeleted == false)
                                                        ? cid.StockOutDetail.SalesOrderApplication.SalesOrderAppDetail.Where(x => x.IsDeleted == false).Sum(x => x.TotalSalesAmount) : 0,
                                     InvoiceAmount = cid.Amount,
                                     TransportNumber = q.TransportNumber,
                                 }).ToList();
            }

            totalRecords = total;

            return uiWarehouses;
        }
    }
}
