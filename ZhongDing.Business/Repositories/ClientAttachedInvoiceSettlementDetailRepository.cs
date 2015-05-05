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


            var query = from cid in DB.ClientInvoiceDetail
                        join ci in DB.ClientInvoice on cid.ClientInvoiceID equals ci.ID
                        join c in DB.Company on ci.CompanyID equals c.ID
                        join cc in DB.ClientCompany on ci.ClientCompanyID equals cc.ID
                        join sod in DB.StockOutDetail on cid.StockOutDetailID equals sod.ID
                        join soa in DB.SalesOrderApplication on sod.SalesOrderApplicationID equals soa.ID
                        join soad in DB.SalesOrderAppDetail on sod.SalesOrderAppDetailID equals soad.ID
                        join p in DB.Product on sod.ProductID equals p.ID
                        join ps in DB.ProductSpecification on sod.ProductSpecificationID equals ps.ID
                        join caisd in DB.ClientAttachedInvoiceSettlementDetail.Where(x => x.IsDeleted == false
                            && x.ClientAttachedInvoiceSettlementID == uiSearchObj.ClientAttachedInvoiceSettlementID)
                            on new { ClientInvoiceDetailID = cid.ID, cid.StockOutDetailID }
                            equals new { caisd.ClientInvoiceDetailID, caisd.StockOutDetailID } into tempCAISD
                        from tcaisd in tempCAISD.DefaultIfEmpty()
                        where ci.IsDeleted == false && ci.IsSettled != true
                        && ci.CompanyID == uiSearchObj.CompanyID
                        && ci.ClientCompanyID == uiSearchObj.ClientCompanyID
                        && ci.SaleOrderTypeID == (int)ESaleOrderType.AttachedMode
                        && (!uiSearchObj.BeginDate.HasValue || ci.InvoiceDate >= uiSearchObj.BeginDate)
                        && (!uiSearchObj.EndDate.HasValue || ci.InvoiceDate < uiSearchObj.EndDate)
                        select new UIClientAttachedInvoiceSettlementDetail
                        {
                            ID = tcaisd == null ? 0 : tcaisd.ID,
                            ClientInvoiceDetailID = cid.ID,
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
                            SettledQty = (DB.ClientAttachedInvoiceSettlementDetail.Any(x => x.IsDeleted == false
                                && x.ClientInvoiceDetailID == cid.ID && x.StockOutDetailID == sod.ID)
                                ? DB.ClientAttachedInvoiceSettlementDetail.Where(x => x.IsDeleted == false
                                  && x.ClientInvoiceDetailID == cid.ID && x.StockOutDetailID == sod.ID)
                                  .Sum(x => x.SettlementQty ?? 0)
                                : 0),
                            SettlementQty = tcaisd == null ? (cid.Qty ?? 0) : (tcaisd.SettlementQty ?? 0),
                            SettlementAmount = tcaisd == null ? (soad.InvoicePrice ?? 0) * (cid.Qty ?? 0) : tcaisd.SettlementAmount,
                            InvoiceTypeID = cid.InvoiceTypeID,
                            InvoiceSettlementRatio = (cid.InvoiceTypeID == (int)EInvoiceType.HighRatio
                              ? c.ClientTaxHighRatio : (cid.InvoiceTypeID == (int)EInvoiceType.LowRatio
                                  ? c.ClientTaxLowRatio : ((cid.InvoiceTypeID == (int)EInvoiceType.DeductionRatio && c.EnableTaxDeduction == true)
                                      ? c.ClientTaxDeductionRatio : null))),
                            IsChecked = tcaisd == null ? false : true,
                            CreatedOn = tcaisd == null ? cid.CreatedOn : tcaisd.CreatedOn
                        };

            if (uiSearchObj.OnlyIncludeChecked)
                query = query.Where(x => x.IsChecked == true);

            uiEntities = query.OrderByDescending(x => x.CreatedOn).ToList();

            foreach (var item in uiEntities)
            {
                item.ToBeSettlementQty = item.InvoiceQty - item.SettledQty;
                //调整未结算的本次预结算数据
                if (item.IsChecked == false)
                    item.SettlementQty = item.ToBeSettlementQty;
            }

            return uiEntities;
        }
    }
}
