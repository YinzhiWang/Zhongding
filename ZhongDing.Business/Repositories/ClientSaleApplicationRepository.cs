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
    public class ClientSaleApplicationRepository : BaseRepository<ClientSaleApplication>, IClientSaleApplicationRepository
    {
        public IList<UIClientSaleApplication> GetUIList(UISearchClientSaleApplication uiSearchObj = null)
        {
            IList<UIClientSaleApplication> uiEntities = new List<UIClientSaleApplication>();

            IQueryable<ClientSaleApplication> query = null;

            List<Expression<Func<ClientSaleApplication, bool>>> whereFuncs = new List<Expression<Func<ClientSaleApplication, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.ID > 0)
                    whereFuncs.Add(x => x.ID.Equals(uiSearchObj.ID));

                if (uiSearchObj.ClientUserID > 0)
                    whereFuncs.Add(x => x.ClientUserID == uiSearchObj.ClientUserID);

                if (uiSearchObj.ClientCompanyID > 0)
                    whereFuncs.Add(x => x.ClientCompanyID == uiSearchObj.ClientCompanyID);

                if (uiSearchObj.WorkflowStatusID > 0)
                    whereFuncs.Add(x => x.WorkflowStatusID == uiSearchObj.WorkflowStatusID);

                if (uiSearchObj.IncludeWorkflowStatusIDs != null
                    && uiSearchObj.IncludeWorkflowStatusIDs.Count() > 0)
                    whereFuncs.Add(x => uiSearchObj.IncludeWorkflowStatusIDs.Contains(x.WorkflowStatusID));

                if (uiSearchObj.BeginDate.HasValue)
                    whereFuncs.Add(x => x.CreatedOn >= uiSearchObj.BeginDate);

                if (uiSearchObj.EndDate.HasValue)
                {
                    uiSearchObj.EndDate = uiSearchObj.EndDate.Value.AddDays(1);
                    whereFuncs.Add(x => x.CreatedOn < uiSearchObj.EndDate);
                }

            }

            query = GetList(whereFuncs);

            if (query != null)
            {
                uiEntities = (from q in query
                              join soa in DB.SalesOrderApplication on q.SalesOrderApplicationID equals soa.ID
                              join sot in DB.SaleOrderType on soa.SaleOrderTypeID equals sot.ID
                              join cu in DB.ClientUser on q.ClientUserID equals cu.ID
                              join cc in DB.ClientCompany on q.ClientCompanyID equals cc.ID
                              join ws in DB.WorkflowStatus on q.WorkflowStatusID equals ws.ID
                              join cbu in DB.Users on q.CreatedBy equals cbu.UserID into tempCBU
                              from tcbu in tempCBU.DefaultIfEmpty()
                              orderby q.CreatedOn descending
                              select new UIClientSaleApplication()
                              {
                                  ID = q.ID,
                                  OrderCode = soa.OrderCode,
                                  OrderDate = soa.OrderDate,
                                  SaleOrderTypeName = sot.TypeName,
                                  ClientUserName = cu.ClientName,
                                  ClientCompanyName = cc.Name,
                                  IsGuaranteed = q.IsGuaranteed,
                                  IsReceiptedGuaranteeAmount = q.GuaranteeLog.Any(x => x.IsDeleted == false && x.IsReceipted == true),
                                  IsStop = soa.IsStop,
                                  WorkflowStatusID = q.WorkflowStatusID,
                                  WorkflowStatus = ws.StatusName,
                                  CreatedByUserID = q.CreatedBy,
                                  CreatedBy = tcbu == null ? string.Empty : tcbu.FullName
                              }).ToList();

                foreach (var uiEntity in uiEntities.Where(x => x.IsGuaranteed == true))
                {
                    if (uiEntity.IsReceiptedGuaranteeAmount)
                        uiEntity.IconUrlOfGuarantee = GlobalConst.Icons.ICON_GUARANTEE_RECEIPTED;
                    else
                        uiEntity.IconUrlOfGuarantee = GlobalConst.Icons.ICON_GUARANTEE_NOT_RECEIPTED;
                }
            }

            return uiEntities;
        }

        public IList<UIClientSaleApplication> GetUIList(UISearchClientSaleApplication uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {
            IList<UIClientSaleApplication> uiEntities = new List<UIClientSaleApplication>();

            int total = 0;

            IQueryable<ClientSaleApplication> query = null;

            List<Expression<Func<ClientSaleApplication, bool>>> whereFuncs = new List<Expression<Func<ClientSaleApplication, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.ID > 0)
                    whereFuncs.Add(x => x.ID.Equals(uiSearchObj.ID));

                if (uiSearchObj.ClientUserID > 0)
                    whereFuncs.Add(x => x.ClientUserID == uiSearchObj.ClientUserID);

                if (uiSearchObj.ClientCompanyID > 0)
                    whereFuncs.Add(x => x.ClientCompanyID == uiSearchObj.ClientCompanyID);

                if (uiSearchObj.WorkflowStatusID > 0)
                    whereFuncs.Add(x => x.WorkflowStatusID == uiSearchObj.WorkflowStatusID);

                if (uiSearchObj.IncludeWorkflowStatusIDs != null
                    && uiSearchObj.IncludeWorkflowStatusIDs.Count() > 0)
                    whereFuncs.Add(x => uiSearchObj.IncludeWorkflowStatusIDs.Contains(x.WorkflowStatusID));

                if (uiSearchObj.BeginDate.HasValue)
                    whereFuncs.Add(x => x.CreatedOn >= uiSearchObj.BeginDate);

                if (uiSearchObj.EndDate.HasValue)
                {
                    uiSearchObj.EndDate = uiSearchObj.EndDate.Value.AddDays(1);
                    whereFuncs.Add(x => x.CreatedOn < uiSearchObj.EndDate);
                }
                if (uiSearchObj.IsImport.HasValue)
                {
                    whereFuncs.Add(x => x.SalesOrderApplication.IsImport == uiSearchObj.IsImport);
                }
            }

            query = GetList(pageIndex, pageSize, whereFuncs, GlobalConst.OrderByExpression.CREATEDON_DESC, out total);

            if (query != null)
            {
                uiEntities = (from q in query
                              join soa in DB.SalesOrderApplication on q.SalesOrderApplicationID equals soa.ID
                              join sot in DB.SaleOrderType on soa.SaleOrderTypeID equals sot.ID
                              join cu in DB.ClientUser on q.ClientUserID equals cu.ID
                              join cc in DB.ClientCompany on q.ClientCompanyID equals cc.ID
                              join ws in DB.WorkflowStatus on q.WorkflowStatusID equals ws.ID
                              join cbu in DB.Users on q.CreatedBy equals cbu.UserID into tempCBU
                              from tcbu in tempCBU.DefaultIfEmpty()
                              select new UIClientSaleApplication()
                              {
                                  ID = q.ID,
                                  OrderCode = soa.OrderCode,
                                  OrderDate = soa.OrderDate,
                                  SaleOrderTypeName = sot.TypeName,
                                  ClientUserName = cu.ClientName,
                                  ClientCompanyName = cc.Name,
                                  IsGuaranteed = q.IsGuaranteed,
                                  IsReceiptedGuaranteeAmount = q.GuaranteeLog.Any(x => x.IsDeleted == false && x.IsReceipted == true),
                                  IsStop = soa.IsStop,
                                  WorkflowStatusID = q.WorkflowStatusID,
                                  WorkflowStatus = ws.StatusName,
                                  CreatedByUserID = q.CreatedBy,
                                  CreatedBy = tcbu == null ? string.Empty : tcbu.FullName
                              }).ToList();

                foreach (var uiEntity in uiEntities.Where(x => x.IsGuaranteed == true))
                {
                    if (uiEntity.IsReceiptedGuaranteeAmount)
                        uiEntity.IconUrlOfGuarantee = GlobalConst.Icons.ICON_GUARANTEE_RECEIPTED;
                    else
                        uiEntity.IconUrlOfGuarantee = GlobalConst.Icons.ICON_GUARANTEE_NOT_RECEIPTED;
                }
            }

            totalRecords = total;

            return uiEntities;
        }

        public int? GetMaxEntityID()
        {
            if (this.DB.ClientSaleApplication.Count() > 0)
                return this.DB.ClientSaleApplication.Max(x => x.ID);
            else return null;
        }

        public IList<UIClientSaleAppPayment> GetPayments(UISearchApplicationPayment uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {
            IList<UIClientSaleAppPayment> uiEntities = new List<UIClientSaleAppPayment>();

            int total = 0;

            var query = ((from ap in DB.ApplicationPayment
                          where ap.IsDeleted == false && ap.WorkflowID == uiSearchObj.WorkflowID
                          && ap.ApplicationID == uiSearchObj.ApplicationID
                          select new UIClientSaleAppPayment
                          {
                              ID = ap.ID,
                              PaymentMethodID = (int)EPaymentMethod.BankTransfer,
                              PaymentMethod = GlobalConst.PaymentMethods.BANK_TRANSFER,
                              Amount = ap.Amount,
                              Fee = ap.Fee,
                              PayDate = ap.PayDate,
                              Comment = ap.Comment
                          })
                //以后扩充客户余额部分
                //.Concat(
                //from an in DB.ApplicationNote
                //where an.WorkflowID == uiSearchObj.WorkflowID
                //&& an.ApplicationID == uiSearchObj.ApplicationID
                //select new UIClientSaleAppPayment { }
                //)
                         );

            if (query != null)
            {
                total = query.Count();

                uiEntities = query.OrderByDescending(x => x.PayDate)
                    .Skip(pageSize * pageIndex).Take(pageSize).ToList();
            }

            totalRecords = total;

            return uiEntities;
        }
    }
}
