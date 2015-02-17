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
    public class SupplierRefundApplicationRepository : BaseRepository<SupplierRefundApplication>, ISupplierRefundApplicationRepository
    {
        public IList<UISupplierRefundApp> GetUIList(UISearchSupplierRefundApp uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {
            IList<UISupplierRefundApp> uiEntities = new List<UISupplierRefundApp>();

            int total = 0;

            var baseQuery = (from poa in DB.ProcureOrderAppDetail
                             join po in DB.ProcureOrderApplication on poa.ProcureOrderApplicationID equals po.ID
                             join wh in DB.Warehouse on poa.WarehouseID equals wh.ID
                             join s in DB.Supplier on po.SupplierID equals s.ID
                             join p in DB.Product on poa.ProductID equals p.ID
                             join ps in DB.ProductSpecification on poa.ProductSpecificationID equals ps.ID
                             where poa.IsDeleted == false && po.IsDeleted == false && po.IsStop == false
                             && po.WorkflowStatusID == (int)EWorkflowStatus.Paid && wh.SaleTypeID == (int)ESaleType.HighPrice
                             && (uiSearchObj.CompanyID == 0 || (uiSearchObj.CompanyID > 0 && s.CompanyID == uiSearchObj.CompanyID))
                             && (uiSearchObj.SupplierID == 0 || (uiSearchObj.SupplierID > 0 && s.ID == uiSearchObj.SupplierID))
                             && (uiSearchObj.ProductID == 0 || (uiSearchObj.ProductID > 0 && poa.ProductID == uiSearchObj.ProductID))
                             && (uiSearchObj.ProductSpecificationID == 0 || (uiSearchObj.ProductSpecificationID > 0 && poa.ProductSpecificationID == uiSearchObj.ProductSpecificationID))
                             && (uiSearchObj.WarehouseID == 0 || (uiSearchObj.WarehouseID > 0 && poa.WarehouseID == uiSearchObj.WarehouseID))
                             select new
                             {
                                 CompanyID = s.CompanyID.HasValue ? s.CompanyID.Value : GlobalConst.INVALID_INT,
                                 po.SupplierID,
                                 poa.ProductID,
                                 poa.ProductSpecificationID,
                                 poa.ProcureCount,
                                 poa.ProcurePrice,
                                 poa.TotalAmount,
                                 poa.CreatedOn,
                             });

            if (baseQuery != null)
            {
                var query = (from gd in
                                 ((from q in baseQuery
                                   join php in DB.ProductHighPrice on new { q.ProductID, q.ProductSpecificationID }
                                     equals new { php.ProductID, php.ProductSpecificationID } into tempPHP
                                   from tphp in tempPHP.DefaultIfEmpty()
                                   select new
                                   {
                                       q.CompanyID,
                                       q.SupplierID,
                                       q.ProductID,
                                       q.ProductSpecificationID,
                                       q.ProcureCount,
                                       q.TotalAmount,
                                       //应返款
                                       NeedRefundAmount = (((q.ProcurePrice - (tphp == null ? q.ProcurePrice : (tphp.ActualProcurePrice.HasValue ? tphp.ActualProcurePrice.Value : 0))) * q.ProcureCount)
                                       - (((q.ProcurePrice - (tphp == null ? q.ProcurePrice : (tphp.ActualProcurePrice.HasValue ? tphp.ActualProcurePrice.Value : 0)))
                                           * q.ProcureCount
                                           * (tphp == null ? 0 : (tphp.SupplierTaxRatio.HasValue ? tphp.SupplierTaxRatio.Value : 1))))),
                                   })
                                   .GroupBy(x => new { x.SupplierID, x.ProductID, x.ProductSpecificationID, x.CompanyID })
                                   .Select(g => new
                                   {
                                       GroupKey = g.Key,
                                       TotalCount = g.Sum(x => x.ProcureCount),
                                       TotalAmount = g.Sum(x => x.TotalAmount),
                                       //应返款总额
                                       NeedRefundAmount = g.Sum(x => x.NeedRefundAmount),
                                       //已返款总额
                                       RefundedAmount = (from sra in DB.SupplierRefundApplication
                                                         join ap in DB.ApplicationPayment.Where(x => x.IsDeleted == false && x.WorkflowID == (int)EWorkflow.SupplierRefunds)
                                                         on sra.ID equals ap.ApplicationID
                                                         where sra.IsDeleted == false && sra.CompanyID == g.Key.CompanyID && sra.WorkflowID == (int)EWorkflow.SupplierRefunds
                                                         && sra.SupplierID == g.Key.SupplierID && sra.ProductID == g.Key.ProductID
                                                         && sra.ProductSpecificationID == g.Key.ProductSpecificationID
                                                         select new { ap.Amount, ap.Fee })
                                                              .Sum(x => (x.Amount.HasValue ? x.Amount.Value : 0)
                                                                  + (x.Fee.HasValue ? x.Fee.Value : 0)),
                                       //已抵扣总额
                                       DeductedAmount = (from sd in DB.SupplierDeduction
                                                         join sra in DB.SupplierRefundApplication on sd.SupplierRefundAppID equals sra.ID
                                                         where sra.IsDeleted == false && sra.CompanyID == g.Key.CompanyID && sra.WorkflowID == (int)EWorkflow.SupplierRefunds
                                                         && sra.SupplierID == g.Key.SupplierID && sra.ProductID == g.Key.ProductID
                                                         && sra.ProductSpecificationID == g.Key.ProductSpecificationID
                                                         select new { sd.Amount })
                                                         .Sum(x => x.Amount)
                                   }))
                             join s in DB.Supplier on gd.GroupKey.SupplierID equals s.ID
                             join p in DB.Product on gd.GroupKey.ProductID equals p.ID
                             join ps in DB.ProductSpecification on gd.GroupKey.ProductSpecificationID equals ps.ID
                             join c in DB.Company on gd.GroupKey.CompanyID equals c.ID
                             join uom in DB.UnitOfMeasurement on ps.UnitOfMeasurementID equals uom.ID into tempUOM
                             from tuom in tempUOM.DefaultIfEmpty()
                             join sra in DB.SupplierRefundApplication.Where(x => x.WorkflowID == (int)EWorkflow.SupplierRefunds)
                             on new
                             {
                                 gd.GroupKey.SupplierID,
                                 gd.GroupKey.ProductID,
                                 gd.GroupKey.ProductSpecificationID,
                                 gd.GroupKey.CompanyID
                             }
                             equals new { sra.SupplierID, sra.ProductID, sra.ProductSpecificationID, sra.CompanyID } into tempSRA
                             from tsra in tempSRA.DefaultIfEmpty()
                             select new UISupplierRefundApp
                             {
                                 ID = tsra == null ? GlobalConst.INVALID_INT : tsra.ID,
                                 CompanyID = c.ID,
                                 SupplierID = gd.GroupKey.SupplierID,
                                 ProductID = gd.GroupKey.ProductID,
                                 ProductSpecificationID = gd.GroupKey.ProductSpecificationID,
                                 CompanyName = c.CompanyName,
                                 SupplierName = s.SupplierName,
                                 ProductName = p.ProductName,
                                 Specification = ps.Specification,
                                 UnitName = tuom == null ? string.Empty : tuom.UnitName,
                                 TotalCount = gd.TotalCount,
                                 TotalAmount = gd.TotalAmount,
                                 TotalNeedRefundAmount = gd.NeedRefundAmount,
                                 TotalRefundedAmount = (gd.RefundedAmount == null ? 0 : gd.RefundedAmount)
                                                        + (gd.DeductedAmount == null ? 0 : gd.DeductedAmount),
                                 TotalToBeRefundAmount = gd.NeedRefundAmount
                                                        - ((gd.RefundedAmount == null ? 0 : gd.RefundedAmount)
                                                            + (gd.DeductedAmount == null ? 0 : gd.DeductedAmount)),
                             });

                if (query != null)
                {
                    total = query.Count();

                    uiEntities = query.OrderByDescending(x => x.SupplierID)
                        .Skip(pageIndex * pageSize).Take(pageSize).ToList();
                }
            }

            totalRecords = total;

            return uiEntities;
        }

        public IList<UISupplierRefundAppDetail> GetDetails(UISearchSupplierRefundApp uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {
            IList<UISupplierRefundAppDetail> uiEntities = new List<UISupplierRefundAppDetail>();

            if (uiSearchObj.EndDate.HasValue)
                uiSearchObj.EndDate = uiSearchObj.EndDate.Value.AddDays(1);

            int total = 0;

            var query = (from poa in DB.ProcureOrderAppDetail
                         join po in DB.ProcureOrderApplication on poa.ProcureOrderApplicationID equals po.ID
                         join wh in DB.Warehouse on poa.WarehouseID equals wh.ID
                         join s in DB.Supplier on po.SupplierID equals s.ID
                         join p in DB.Product on poa.ProductID equals p.ID
                         join ps in DB.ProductSpecification on poa.ProductSpecificationID equals ps.ID
                         join uom in DB.UnitOfMeasurement on ps.UnitOfMeasurementID equals uom.ID into tempUOM
                         from tuom in tempUOM.DefaultIfEmpty()
                         join php in DB.ProductHighPrice on new { poa.ProductID, poa.ProductSpecificationID }
                                  equals new { php.ProductID, php.ProductSpecificationID } into tempPHP
                         from tphp in tempPHP.DefaultIfEmpty()
                         where poa.IsDeleted == false && po.IsDeleted == false && po.IsStop == false
                         && po.WorkflowStatusID == (int)EWorkflowStatus.Paid && wh.SaleTypeID == (int)ESaleType.HighPrice
                         && (!uiSearchObj.BeginDate.HasValue || (uiSearchObj.BeginDate.HasValue && po.OrderDate >= uiSearchObj.BeginDate))
                         && (!uiSearchObj.EndDate.HasValue || (uiSearchObj.EndDate.HasValue && po.OrderDate < uiSearchObj.EndDate))
                         && (uiSearchObj.CompanyID == 0 || (uiSearchObj.CompanyID > 0 && s.CompanyID == uiSearchObj.CompanyID))
                         && (uiSearchObj.SupplierID == 0 || (uiSearchObj.SupplierID > 0 && s.ID == uiSearchObj.SupplierID))
                         && (uiSearchObj.ProductID == 0 || (uiSearchObj.ProductID > 0 && poa.ProductID == uiSearchObj.ProductID))
                         && (uiSearchObj.ProductSpecificationID == 0 || (uiSearchObj.ProductSpecificationID > 0 && poa.ProductSpecificationID == uiSearchObj.ProductSpecificationID))
                         && (uiSearchObj.WarehouseID == 0 || (uiSearchObj.WarehouseID > 0 && poa.WarehouseID == uiSearchObj.WarehouseID))
                         select new UISupplierRefundAppDetail
                         {
                             ID = poa.ID,
                             SupplierName = s.SupplierName,
                             OrderDate = po.OrderDate,
                             OrderCode = po.OrderCode,
                             ProductName = p.ProductName,
                             Specification = ps.Specification,
                             UnitName = tuom == null ? string.Empty : tuom.UnitName,
                             SupplierTaxRatio = tphp == null ? null : tphp.SupplierTaxRatio,
                             ProcurePrice = poa.ProcurePrice,
                             ProcureCount = poa.ProcureCount,
                             TotalAmount = poa.TotalAmount,
                             TotalNeedRefundAmount = (((poa.ProcurePrice - (tphp == null ? poa.ProcurePrice : (tphp.ActualProcurePrice.HasValue ? tphp.ActualProcurePrice.Value : 0))) * poa.ProcureCount)
                                    - (((poa.ProcurePrice - (tphp == null ? poa.ProcurePrice : (tphp.ActualProcurePrice.HasValue ? tphp.ActualProcurePrice.Value : 0)))
                                        * poa.ProcureCount
                                        * (tphp == null ? 0 : (tphp.SupplierTaxRatio.HasValue ? tphp.SupplierTaxRatio.Value : 1))))),
                             CreatedOn = po.CreatedOn
                         });

            if (query != null)
            {
                total = query.Count();

                uiEntities = query.OrderByDescending(x => x.CreatedOn)
                .Skip(pageIndex * pageSize).Take(pageSize).ToList();
            }

            totalRecords = total;

            return uiEntities;
        }

        public IList<UISupplierRefundApp> GetTaskRefunds(UISearchSupplierRefundApp uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {
            IList<UISupplierRefundApp> uiEntities = new List<UISupplierRefundApp>();

            int total = 0;

            IQueryable<SupplierRefundApplication> query = null;

            var whereFuncs = new List<Expression<Func<SupplierRefundApplication, bool>>>();

            whereFuncs.Add(x => x.WorkflowID == (int)EWorkflow.SupplierTaskRefunds);

            if (uiSearchObj != null)
            {
                if (uiSearchObj.ID > 0)
                    whereFuncs.Add(x => x.ID.Equals(uiSearchObj.ID));

                if (uiSearchObj.CompanyID > 0)
                    whereFuncs.Add(x => x.CompanyID.Equals(uiSearchObj.CompanyID));

                if (uiSearchObj.SupplierID > 0)
                    whereFuncs.Add(x => x.SupplierID.Equals(uiSearchObj.SupplierID));

                if (uiSearchObj.ProductID > 0)
                    whereFuncs.Add(x => x.ProductID.Equals(uiSearchObj.ProductID));

                if (uiSearchObj.BeginDate.HasValue)
                    whereFuncs.Add(x => x.RefundDate >= uiSearchObj.BeginDate);

                if (uiSearchObj.EndDate.HasValue)
                {
                    uiSearchObj.EndDate = uiSearchObj.EndDate.Value.AddDays(1);
                    whereFuncs.Add(x => x.RefundDate < uiSearchObj.EndDate);
                }
            }

            query = GetList(pageIndex, pageSize, whereFuncs, GlobalConst.OrderByExpression.CREATEDON_DESC, out total);

            if (query != null)
            {
                uiEntities = (from q in query
                              join c in DB.Company on q.CompanyID equals c.ID
                              join s in DB.Supplier on q.SupplierID equals s.ID
                              join p in DB.Product on q.ProductID equals p.ID
                              join ps in DB.ProductSpecification on q.ProductSpecificationID equals ps.ID
                              join cbu in DB.Users on q.CreatedBy equals cbu.UserID into tempCBU
                              from tcbu in tempCBU.DefaultIfEmpty()
                              select new UISupplierRefundApp
                              {
                                  ID = q.ID,
                                  CompanyName = c.CompanyName,
                                  SupplierName = s.SupplierName,
                                  ProductName = p.ProductName,
                                  Specification = ps.Specification,
                                  BeginDate = q.BeginDate,
                                  EndDate = q.EndDate,
                                  PaymentMethod = q.PaymentMethodID.HasValue 
                                  ? (q.PaymentMethodID == (int)EPaymentMethod.BankTransfer 
                                        ? GlobalConst.PaymentMethods.BANK_TRANSFER 
                                        : (q.PaymentMethodID == (int)EPaymentMethod.Deduction 
                                            ? GlobalConst.PaymentMethods.DEDUCATION : string.Empty)) 
                                  : string.Empty,
                                  RefundAmount = q.RefundAmount ?? 0M,
                                  RefundDate = q.RefundDate,
                                  CreatedBy = tcbu == null ? string.Empty : tcbu.FullName,
                              }).ToList();


            }

            totalRecords = total;

            return uiEntities;

        }
    }
}
