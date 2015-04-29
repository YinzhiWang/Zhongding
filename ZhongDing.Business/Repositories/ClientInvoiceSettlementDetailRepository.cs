using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ZhongDing.Business.IRepositories;
using ZhongDing.Common;
using ZhongDing.Common.Enums;
using ZhongDing.Domain.Models;
using ZhongDing.Domain.UIObjects;
using ZhongDing.Domain.UISearchObjects;

namespace ZhongDing.Business.Repositories
{
    public class ClientInvoiceSettlementDetailRepository : BaseRepository<ClientInvoiceSettlementDetail>, IClientInvoiceSettlementDetailRepository
    {
        public IList<UIClientInvoiceSettlementDetail> GetUIList(UISearchClientInvoiceSettlementDetail uiSearchObj)
        {
            IList<UIClientInvoiceSettlementDetail> uiEntities = new List<UIClientInvoiceSettlementDetail>();

            if (uiSearchObj.EndDate.HasValue)
                uiSearchObj.EndDate = uiSearchObj.EndDate.Value.AddDays(1);

            IQueryable<UIClientInvoiceSettlementDetail> query1 = (from cisd in DB.ClientInvoiceSettlementDetail
                                                                  join cc in DB.ClientCompany on cisd.ClientCompanyID equals cc.ID
                                                                  where cisd.IsDeleted == false
                                                                  && cisd.ClientInvoiceSettlementID == uiSearchObj.ClientInvoiceSettlementID
                                                                  && cisd.ClientCompanyID == uiSearchObj.ClientCompanyID
                                                                  && (!uiSearchObj.BeginDate.HasValue || cisd.InvoiceDate >= uiSearchObj.BeginDate)
                                                                  && (!uiSearchObj.EndDate.HasValue || cisd.InvoiceDate < uiSearchObj.EndDate)
                                                                  select new UIClientInvoiceSettlementDetail()
                                                                  {
                                                                      ID = cisd.ID,
                                                                      ClientCompanyID = cisd.ClientCompanyID,
                                                                      ClientInvoiceID = cisd.ClientInvoiceID,
                                                                      ClientCompanyName = cc.Name,
                                                                      InvoiceDate = cisd.InvoiceDate,
                                                                      InvoiceNumber = cisd.InvoiceNumber,
                                                                      TotalInvoiceAmount = cisd.TotalInvoiceAmount,
                                                                      ClientTaxHighRatio = cisd.ClientTaxHighRatio,
                                                                      ClientTaxLowRatio = cisd.ClientTaxLowRatio,
                                                                      ClientTaxDeductionRatio = cisd.ClientTaxDeductionRatio,
                                                                      HighRatioAmount = cisd.HighRatioAmount,
                                                                      LowRatioAmount = cisd.LowRatioAmount,
                                                                      DeductionRatioAmount = cisd.DeductionRatioAmount,
                                                                      IsContainDeductionInvoice = cisd.DeductionRatioAmount.HasValue ? true : false,
                                                                      PayAmount = cisd.PayAmount,
                                                                      IsChecked = true,
                                                                      CreatedOn = cisd.CreatedOn
                                                                  });


            if (uiSearchObj.OnlyIncludeChecked)
            {
                uiEntities = query1.OrderByDescending(x => x.CreatedOn).ToList();
                return uiEntities;
            }

            IQueryable<UIClientInvoiceSettlementDetail> query2 = (from ci in DB.ClientInvoice
                                                                  join c in DB.Company on ci.CompanyID equals c.ID
                                                                  join cc in DB.ClientCompany on ci.ClientCompanyID equals cc.ID
                                                                  where ci.IsDeleted == false && ci.IsSettled != true
                                                                  && ci.CompanyID == uiSearchObj.CompanyID
                                                                  && ci.ClientCompanyID == uiSearchObj.ClientCompanyID
                                                                  && ci.SaleOrderTypeID == (int)ESaleOrderType.AttractBusinessMode
                                                                  && (!uiSearchObj.BeginDate.HasValue || ci.InvoiceDate >= uiSearchObj.BeginDate)
                                                                  && (!uiSearchObj.EndDate.HasValue || ci.InvoiceDate < uiSearchObj.EndDate)
                                                                  select new UIClientInvoiceSettlementDetail
                                                                  {
                                                                      ID = 0,
                                                                      ClientCompanyID = ci.ClientCompanyID,
                                                                      ClientInvoiceID = ci.ID,
                                                                      ClientCompanyName = cc.Name,
                                                                      InvoiceDate = ci.InvoiceDate,
                                                                      InvoiceNumber = ci.InvoiceNumber,
                                                                      TotalInvoiceAmount = ci.Amount,
                                                                      ClientTaxHighRatio = c.ClientTaxHighRatio,
                                                                      ClientTaxLowRatio = c.ClientTaxLowRatio,
                                                                      ClientTaxDeductionRatio = c.EnableTaxDeduction == true
                                                                            ? c.ClientTaxDeductionRatio : null,
                                                                      HighRatioAmount = ci.ClientInvoiceDetail
                                                                      .Where(x => x.InvoiceTypeID == (int)EInvoiceType.HighRatio)
                                                                      .Sum(x => x.Amount),
                                                                      LowRatioAmount = ci.ClientInvoiceDetail
                                                                      .Where(x => x.InvoiceTypeID == (int)EInvoiceType.LowRatio)
                                                                      .Sum(x => x.Amount),
                                                                      DeductionRatioAmount = ci.ClientInvoiceDetail
                                                                      .Where(x => x.InvoiceTypeID == (int)EInvoiceType.DeductionRatio)
                                                                      .Sum(x => x.Amount),
                                                                      IsContainDeductionInvoice = ci.ClientInvoiceDetail
                                                                      .Any(x => x.InvoiceTypeID == (int)EInvoiceType.DeductionRatio),
                                                                      PayAmount = 0,
                                                                      IsChecked = false,
                                                                      CreatedOn = ci.CreatedOn
                                                                  });


            var query = query1.Union(query2);

            uiEntities = query.OrderByDescending(x => x.CreatedOn).ToList();

            foreach (var uiEntity in uiEntities.Where(x => x.IsChecked == false))
            {
                if (!uiEntity.ClientTaxDeductionRatio.HasValue)
                    uiEntity.DeductionRatioAmount = null;

                uiEntity.PayAmount = ((uiEntity.HighRatioAmount ?? 0) * (uiEntity.ClientTaxHighRatio ?? 0))
                    + ((uiEntity.LowRatioAmount ?? 0) * (uiEntity.ClientTaxLowRatio ?? 0))
                    + ((uiEntity.DeductionRatioAmount ?? 0) * (uiEntity.ClientTaxDeductionRatio ?? 0));
            }

            return uiEntities;
        }

        public IList<UIClientInvoiceSettlementDetail> GetNeedSettleUIList(UISearchClientInvoiceSettlementDetail uiSearchObj)
        {
            IList<UIClientInvoiceSettlementDetail> uiEntities = new List<UIClientInvoiceSettlementDetail>();

            //未结算的发票
            var query = DB.ClientInvoice.Where(x => x.IsDeleted == false && x.IsSettled != true);

            var whereFuncs = new List<Expression<Func<ClientInvoice, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.ID > 0)
                    whereFuncs.Add(x => x.ID.Equals(uiSearchObj.ID));

                if (uiSearchObj.CompanyID > 0)
                    whereFuncs.Add(x => x.CompanyID == uiSearchObj.CompanyID);

                if (uiSearchObj.ClientCompanyID > 0)
                    whereFuncs.Add(x => x.ClientCompanyID == uiSearchObj.ClientCompanyID);

                if (uiSearchObj.BeginDate.HasValue)
                    whereFuncs.Add(x => x.InvoiceDate >= uiSearchObj.BeginDate);

                if (uiSearchObj.EndDate.HasValue)
                {
                    uiSearchObj.EndDate = uiSearchObj.EndDate.Value.AddDays(1);
                    whereFuncs.Add(x => x.InvoiceDate < uiSearchObj.EndDate);
                }
            }

            foreach (var func in whereFuncs)
            {
                query = query.Where(func);
            }

            uiEntities = (from q in query.OrderByDescending(x => x.InvoiceDate)
                          join c in DB.Company on q.CompanyID equals c.ID
                          join cc in DB.ClientCompany on q.ClientCompanyID equals cc.ID

                          select new UIClientInvoiceSettlementDetail
                          {
                              ClientCompanyID = q.ClientCompanyID,
                              ClientInvoiceID = q.ID,
                              ClientCompanyName = cc.Name,
                              InvoiceDate = q.InvoiceDate,
                              InvoiceNumber = q.InvoiceNumber,
                              TotalInvoiceAmount = q.Amount,
                              HighRatioAmount = q.ClientInvoiceDetail
                              .Where(x => x.InvoiceTypeID == (int)EInvoiceType.HighRatio)
                              .Sum(x => x.Amount * c.ClientTaxHighRatio ?? 0),
                              LowRatioAmount = q.ClientInvoiceDetail
                              .Where(x => x.InvoiceTypeID == (int)EInvoiceType.LowRatio)
                              .Sum(x => x.Amount * c.ClientTaxLowRatio ?? 0),
                              DeductionRatioAmount = q.ClientInvoiceDetail
                              .Where(x => x.InvoiceTypeID == (int)EInvoiceType.DeductionRatio)
                              .Sum(x => x.Amount * c.ClientTaxDeductionRatio ?? 0),
                          }).ToList();

            foreach (var uiEntity in uiEntities)
            {
                uiEntity.PayAmount = uiEntity.HighRatioAmount ?? 0 + uiEntity.LowRatioAmount ?? 0 + uiEntity.DeductionRatioAmount ?? 0;
            }

            return uiEntities;
        }
    }
}
