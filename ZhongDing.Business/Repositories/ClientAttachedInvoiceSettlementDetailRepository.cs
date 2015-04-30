using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZhongDing.Business.IRepositories;
using ZhongDing.Common.Enums;
using ZhongDing.Domain.Models;
using ZhongDing.Domain.UIObjects;
using ZhongDing.Domain.UISearchObjects;

namespace ZhongDing.Business.Repositories
{
    public class ClientAttachedInvoiceSettlementDetailRepository : BaseRepository<ClientAttachedInvoiceSettlementDetail>, IClientAttachedInvoiceSettlementDetailRepository
    {
        public IList<UIClientAttachedInvoiceSettlementDetail> GetUIList(UISearchClientAttachedInvoiceSettlementDetail uiSearchObj)
        {
            IList<UIClientAttachedInvoiceSettlementDetail> uiEntities = new List<UIClientAttachedInvoiceSettlementDetail>();

            if (uiSearchObj.EndDate.HasValue)
                uiSearchObj.EndDate = uiSearchObj.EndDate.Value.AddDays(1);

            IQueryable<UIClientAttachedInvoiceSettlementDetail> query1 = (from caisd in DB.ClientAttachedInvoiceSettlementDetail
                                                                          join cid in DB.ClientInvoiceDetail on caisd.ClientInvoiceDetailID equals cid.ID
                                                                          join ci in DB.ClientInvoice on cid.ClientInvoiceID equals ci.ID
                                                                          join c in DB.Company on ci.CompanyID equals c.ID
                                                                          join sod in DB.StockOutDetail on caisd.StockOutDetailID equals sod.ID
                                                                          join soad in DB.SalesOrderAppDetail on sod.SalesOrderAppDetailID equals soad.ID
                                                                          join p in DB.Product on sod.ProductID equals p.ID
                                                                          join ps in DB.ProductSpecification on sod.ProductSpecificationID equals ps.ID
                                                                          where caisd.IsDeleted == false
                                                                          && caisd.ClientAttachedInvoiceSettlementID == uiSearchObj.ClientAttachedInvoiceSettlementID
                                                                          && (!uiSearchObj.BeginDate.HasValue || ci.InvoiceDate >= uiSearchObj.BeginDate)
                                                                          && (!uiSearchObj.EndDate.HasValue || ci.InvoiceDate < uiSearchObj.EndDate)
                                                                          select new UIClientAttachedInvoiceSettlementDetail()
                                                                          {
                                                                              ID = caisd.ID,
                                                                              ClientInvoiceDetailID = caisd.ClientInvoiceDetailID,
                                                                              StockOutDetailID = caisd.StockOutDetailID,
                                                                              ClientCompanyID = ci.ClientCompanyID,
                                                                              InvoiceDate = ci.InvoiceDate,
                                                                              InvoiceNumber = ci.InvoiceNumber,
                                                                              TotalInvoiceAmount = cid.Amount,
                                                                              ProductName = p.ProductName,
                                                                              Specification = ps.Specification,
                                                                              SalesPrice = soad.SalesPrice,
                                                                              InvoicePrice = soad.InvoicePrice ?? 0,
                                                                              InvoiceQty = cid.Qty ?? 0,
                                                                              SettledQty = (DB.ClientAttachedInvoiceSettlementDetail.Any(x => x.IsDeleted == false
                                                                                  && x.ClientInvoiceDetailID == caisd.ClientInvoiceDetailID)
                                                                                  ? DB.ClientAttachedInvoiceSettlementDetail.Where(x => x.IsDeleted == false
                                                                                  && x.ClientInvoiceDetailID == caisd.ClientInvoiceDetailID).Sum(x => x.SettlementQty ?? 0)
                                                                                  : 0),
                                                                              SettlementQty = caisd.SettlementQty ?? 0,
                                                                              InvoiceTypeID = cid.InvoiceTypeID,
                                                                              InvoiceSettlementRatio = (cid.InvoiceTypeID == (int)EInvoiceType.HighRatio
                                                                                ? c.ClientTaxHighRatio : (cid.InvoiceTypeID == (int)EInvoiceType.LowRatio
                                                                                    ? c.ClientTaxLowRatio : ((cid.InvoiceTypeID == (int)EInvoiceType.DeductionRatio && c.EnableTaxDeduction == true)
                                                                                        ? c.ClientTaxDeductionRatio : null))),
                                                                              IsChecked = true,
                                                                              CreatedOn = caisd.CreatedOn
                                                                          });


            if (uiSearchObj.OnlyIncludeChecked)
            {
                uiEntities = query1.OrderByDescending(x => x.CreatedOn).ToList();

                foreach (var item in uiEntities)
                {
                    item.ToBeSettlementQty = item.InvoiceQty - item.SettledQty;
                    //item.SalesAmount = item.SalesPrice * item.SettlementQty;
                    item.SettlementAmount = item.InvoicePrice * item.SettlementQty;
                }

                return uiEntities;
            }

            IQueryable<UIClientAttachedInvoiceSettlementDetail> query2 = (from cid in DB.ClientInvoiceDetail
                                                                          join ci in DB.ClientInvoice on cid.ClientInvoiceID equals ci.ID
                                                                          join c in DB.Company on ci.CompanyID equals c.ID
                                                                          join cc in DB.ClientCompany on ci.ClientCompanyID equals cc.ID
                                                                          join sod in DB.StockOutDetail on cid.StockOutDetailID equals sod.ID
                                                                          join soa in DB.SalesOrderApplication on sod.SalesOrderApplicationID equals soa.ID
                                                                          join soad in DB.SalesOrderAppDetail on sod.SalesOrderAppDetailID equals soad.ID
                                                                          join p in DB.Product on sod.ProductID equals p.ID
                                                                          join ps in DB.ProductSpecification on sod.ProductSpecificationID equals ps.ID
                                                                          where ci.IsDeleted == false && ci.IsSettled != true
                                                                          && ci.CompanyID == uiSearchObj.CompanyID
                                                                          && ci.ClientCompanyID == uiSearchObj.ClientCompanyID
                                                                          && ci.SaleOrderTypeID == (int)ESaleOrderType.AttachedMode
                                                                          && (!uiSearchObj.BeginDate.HasValue || ci.InvoiceDate >= uiSearchObj.BeginDate)
                                                                          && (!uiSearchObj.EndDate.HasValue || ci.InvoiceDate < uiSearchObj.EndDate)
                                                                          select new UIClientAttachedInvoiceSettlementDetail
                                                                  {
                                                                      ID = 0,
                                                                      ClientInvoiceDetailID = ci.ID,
                                                                      StockOutDetailID = cid.StockOutDetailID,
                                                                      ClientCompanyID = ci.ClientCompanyID,
                                                                      InvoiceDate = ci.InvoiceDate,
                                                                      InvoiceNumber = ci.InvoiceNumber,
                                                                      TotalInvoiceAmount = cid.Amount,
                                                                      ProductName = p.ProductName,
                                                                      Specification = ps.Specification,
                                                                      SalesPrice = soad.SalesPrice,
                                                                      InvoicePrice = soad.InvoicePrice ?? 0,
                                                                      InvoiceQty = cid.Qty ?? 0,
                                                                      SettledQty = 0,
                                                                      SettlementQty = cid.Qty ?? 0,
                                                                      InvoiceTypeID = cid.InvoiceTypeID,
                                                                      InvoiceSettlementRatio = (cid.InvoiceTypeID == (int)EInvoiceType.HighRatio
                                                                        ? c.ClientTaxHighRatio : (cid.InvoiceTypeID == (int)EInvoiceType.LowRatio
                                                                            ? c.ClientTaxLowRatio : ((cid.InvoiceTypeID == (int)EInvoiceType.DeductionRatio && c.EnableTaxDeduction == true)
                                                                                ? c.ClientTaxDeductionRatio : null))),
                                                                      IsChecked = false,
                                                                      CreatedOn = ci.CreatedOn
                                                                  });


            var query = query1.Union(query2);

            uiEntities = query.OrderByDescending(x => x.CreatedOn).ToList();

            foreach (var item in uiEntities.Where(x => x.IsChecked == false))
            {
                item.ToBeSettlementQty = item.InvoiceQty - item.SettledQty;
                //item.SalesAmount = item.SalesPrice * item.SettlementQty;
                item.SettlementAmount = item.InvoicePrice * item.SettlementQty;
            }

            return uiEntities;
        }
    }
}
