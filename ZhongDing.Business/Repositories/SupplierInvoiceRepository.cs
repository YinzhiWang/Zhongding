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
    public class SupplierInvoiceRepository : BaseRepository<SupplierInvoice>, ISupplierInvoiceRepository
    {
        public IList<UISupplierInvoice> GetUIList(Domain.UISearchObjects.UISearchSupplierInvoice uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {
            IList<UISupplierInvoice> uiEntities = new List<UISupplierInvoice>();
            int total = 0;

            IQueryable<SupplierInvoice> query = null;

            List<Expression<Func<SupplierInvoice, bool>>> whereFuncs = new List<Expression<Func<SupplierInvoice, bool>>>();

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

                if (uiSearchObj.SupplierID.BiggerThanZero())
                {
                    whereFuncs.Add(x => x.SupplierID == uiSearchObj.SupplierID);
                }

                if (uiSearchObj.SupplierID.BiggerThanZero())
                {
                    whereFuncs.Add(x => x.CompanyID == uiSearchObj.CompanyID);
                }


            }

            query = GetList(pageIndex, pageSize, whereFuncs, out total);

            if (query != null)
            {
                if (!uiSearchObj.IsGroupByProduct)
                {
                    uiEntities = (from q in query
                                    join supplierInvoiceDetail in DB.SupplierInvoiceDetail on q.ID equals supplierInvoiceDetail.SupplierInvoiceID
                                    join procureOrderAppDetail in DB.ProcureOrderAppDetail on supplierInvoiceDetail.ProcureOrderAppDetailID equals procureOrderAppDetail.ID
                                    join procureOrderApplication in DB.ProcureOrderApplication on procureOrderAppDetail.ProcureOrderApplicationID equals procureOrderApplication.ID
                                    join product in DB.Product on procureOrderAppDetail.ProductID equals product.ID
                                    join company in DB.Company on q.CompanyID equals company.ID
                                    join supplier in DB.Supplier on q.SupplierID equals supplier.ID
                                    select new UISupplierInvoice()
                                    {
                                        ID = q.ID,
                                        SupplierInvoiceDetailID = supplierInvoiceDetail.ID,
                                        Amount = q.Amount,
                                        CompanyID = q.CompanyID,
                                        CreatedOn = q.CreatedOn,
                                        InvoiceDate = q.InvoiceDate,
                                        InvoiceNumber = q.InvoiceNumber,
                                        SupplierName = supplier.SupplierName,
                                        CompanyName = company.CompanyName,
                                        ProcureOrderApplicationOrderCode = procureOrderApplication.OrderCode,
                                        ProcureOrderApplicationOrderDate = procureOrderApplication.OrderDate,
                                        ProductName = product.ProductName,
                                        ProductID = product.ID,
                                        ProcureCount = procureOrderAppDetail.ProcureCount,
                                        TotalAmount = procureOrderAppDetail.TotalAmount,
                                        TaxAmount = supplierInvoiceDetail.Amount
                                    }).ToList();
                }
                else
                {

                    uiEntities = (from q in query
                                    join supplierInvoiceDetail in DB.SupplierInvoiceDetail on q.ID equals supplierInvoiceDetail.SupplierInvoiceID
                                    join procureOrderAppDetail in DB.ProcureOrderAppDetail on supplierInvoiceDetail.ProcureOrderAppDetailID equals procureOrderAppDetail.ID
                                    join procureOrderApplication in DB.ProcureOrderApplication on procureOrderAppDetail.ProcureOrderApplicationID equals procureOrderApplication.ID
                                    join product in DB.Product on procureOrderAppDetail.ProductID equals product.ID
                                    join company in DB.Company on q.CompanyID equals company.ID
                                    join supplier in DB.Supplier on q.SupplierID equals supplier.ID
                                    select new UISupplierInvoice()
                                    {
                                        ID = q.ID,
                                        //SupplierInvoiceDetailID = supplierInvoiceDetail.ID,
                                        Amount = q.Amount,
                                        CompanyID = q.CompanyID,
                                        //CreatedOn = q.CreatedOn,
                                        InvoiceDate = q.InvoiceDate,
                                        InvoiceNumber = q.InvoiceNumber,
                                        SupplierName = supplier.SupplierName,
                                        CompanyName = company.CompanyName,
                                        //ProcureOrderApplicationOrderCode = procureOrderApplication.OrderCode,
                                        //ProcureOrderApplicationOrderDate = procureOrderApplication.OrderDate,
                                        ProductName = product.ProductName,
                                        ProductID = product.ID,
                                        ProcureCount = procureOrderAppDetail.ProcureCount,
                                        TotalAmount = procureOrderAppDetail.TotalAmount,
                                        TaxAmount = supplierInvoiceDetail.Amount
                                    }).GroupBy(x => new { x.ProductID, x.ID, x.CompanyID, x.CompanyName, x.InvoiceDate, x.InvoiceNumber, x.SupplierName, x.ProductName }).Select(
                                    x => new UISupplierInvoice()
                                    {
                                        ID = x.Key.ID,
                                        Amount = x.Any() ? x.Sum(m => m.Amount) : 0,
                                        CompanyID = x.Key.CompanyID,
                                        InvoiceDate = x.Key.InvoiceDate,
                                        InvoiceNumber = x.Key.InvoiceNumber,
                                        SupplierName = x.Key.SupplierName,
                                        CompanyName = x.Key.CompanyName,
                                        ProductName = x.Key.ProductName,
                                        ProductID = x.Key.ProductID,
                                        ProcureCount = x.Any() ? x.Sum(m => m.ProcureCount) : 0,
                                        TotalAmount = x.Any() ? x.Sum(m => m.TotalAmount) : 0,
                                        TaxAmount = x.Any() ? x.Sum(m => m.TaxAmount) : 0,
                                    }).ToList();
                }
            }

            totalRecords = total;

            return uiEntities;
        }
    }
}
