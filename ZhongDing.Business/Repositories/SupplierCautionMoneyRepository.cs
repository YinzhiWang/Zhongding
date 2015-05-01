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
using ZhongDing.Common.Extension;

namespace ZhongDing.Business.Repositories
{
    public class SupplierCautionMoneyRepository : BaseRepository<SupplierCautionMoney>, ISupplierCautionMoneyRepository
    {
        public IList<Domain.UIObjects.UISupplierCautionMoney> GetUIList(Domain.UISearchObjects.UISearchSupplierCautionMoney uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {
            IList<UISupplierCautionMoney> uiEntitys = new List<UISupplierCautionMoney>();
            int total = 0;

            IQueryable<SupplierCautionMoney> query = null;

            List<Expression<Func<SupplierCautionMoney, bool>>> whereFuncs = new List<Expression<Func<SupplierCautionMoney, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.ID > 0)
                {
                    whereFuncs.Add(x => x.ID == uiSearchObj.ID);
                }
                if (uiSearchObj.WorkflowStatusID > 0)
                {
                    whereFuncs.Add(x => x.WorkflowStatusID == uiSearchObj.WorkflowStatusID);
                }
                if (uiSearchObj.WorkflowStatusIDs != null && uiSearchObj.WorkflowStatusIDs.Length > 0)
                {
                    whereFuncs.Add(x => uiSearchObj.WorkflowStatusIDs.Contains(x.WorkflowStatusID));
                }
                if (uiSearchObj.SupplierName.IsNotNullOrEmpty())
                {
                    whereFuncs.Add(x => x.Supplier.SupplierName.Contains(uiSearchObj.SupplierName));
                }
                if (uiSearchObj.ProductName.IsNotNullOrEmpty())
                {
                    whereFuncs.Add(x => x.Product.ProductName.Contains(uiSearchObj.ProductName));
                }
            }

            query = GetList(pageIndex, pageSize, whereFuncs, out total);

            if (query != null)
            {
                if (!uiSearchObj.NeedStatistics)
                {
                    uiEntitys = (from q in query
                                 join cautionMoneyType in DB.CautionMoneyType on q.CautionMoneyTypeID equals cautionMoneyType.ID
                                 join supplier in DB.Supplier on q.SupplierID equals supplier.ID
                                 join product in DB.Product on q.ProductID equals product.ID
                                 join productSpecification in DB.ProductSpecification on q.ProductSpecificationID equals productSpecification.ID
                                 join user in DB.Users on q.CreatedBy equals user.UserID
                                 join workflowStatus in DB.WorkflowStatus on q.WorkflowStatusID equals workflowStatus.ID
                                 select new UISupplierCautionMoney()
                                 {
                                     ID = q.ID,
                                     ApplyDate = q.ApplyDate,
                                     CautionMoneyTypeName = cautionMoneyType.Name,
                                     EndDate = q.EndDate,
                                     PaymentCautionMoney = q.PaymentCautionMoney,
                                     ProductName = product.ProductName,
                                     Remark = q.Remark,
                                     IsStop = q.IsStop,
                                     SupplierName = supplier.SupplierName,
                                     ProductSpecification = productSpecification.Specification,
                                     StatusName = workflowStatus.StatusName,
                                     CreatedByUserID = q.CreatedBy.Value,
                                     CreatedByUserName = user.UserName
                                 }).ToList();
                }
                else
                {
                    uiEntitys = (from q in query
                                 join cautionMoneyType in DB.CautionMoneyType on q.CautionMoneyTypeID equals cautionMoneyType.ID
                                 join supplier in DB.Supplier on q.SupplierID equals supplier.ID
                                 join product in DB.Product on q.ProductID equals product.ID
                                 join productSpecification in DB.ProductSpecification on q.ProductSpecificationID equals productSpecification.ID
                                 join user in DB.Users on q.CreatedBy equals user.UserID
                                 join workflowStatus in DB.WorkflowStatus on q.WorkflowStatusID equals workflowStatus.ID
                                 select new UISupplierCautionMoney()
                                 {
                                     ID = q.ID,
                                     ApplyDate = q.ApplyDate,
                                     CautionMoneyTypeName = cautionMoneyType.Name,
                                     EndDate = q.EndDate,
                                     PaymentCautionMoney = q.PaymentCautionMoney,
                                     ProductName = product.ProductName,
                                     Remark = q.Remark,
                                     IsStop = q.IsStop,
                                     SupplierName = supplier.SupplierName,
                                     ProductSpecification = productSpecification.Specification,
                                     StatusName = workflowStatus.StatusName,
                                     CreatedByUserID = q.CreatedBy.Value,
                                     CreatedByUserName = user.UserName,
                                     //已返款总额
                                     RefundedAmount = (from sra in DB.ApplicationPayment.Where(x => x.IsDeleted == false && x.WorkflowID == (int)EWorkflow.SupplierCautionMoneyApply
                                                           && x.ApplicationID == q.ID && x.PaymentTypeID == (int)EPaymentType.Income)
                                                       select new { sra.Amount, sra.Fee }).Any() ? (from sra in DB.ApplicationPayment.Where(x => x.IsDeleted == false && x.WorkflowID == (int)EWorkflow.SupplierCautionMoneyApply
                                                             && x.ApplicationID == q.ID && x.PaymentTypeID == (int)EPaymentType.Income)
                                                                                                    select new { sra.Amount, sra.Fee })
                                                            .Sum(x => (x.Amount.HasValue ? x.Amount.Value : 0)
                                                                + (x.Fee.HasValue ? x.Fee.Value : 0)) : 0,
                                     //已抵扣总额
                                     DeductedAmount = (from supplierCautionMoneyDeduction in DB.SupplierCautionMoneyDeduction.Where(x => x.IsDeleted == false && x.SupplierCautionMoneyID == q.ID)
                                                       select new { supplierCautionMoneyDeduction.Amount }).Any() ? (from supplierCautionMoneyDeduction in DB.SupplierCautionMoneyDeduction.Where(x => x.IsDeleted == false && x.SupplierCautionMoneyID == q.ID)
                                                                                                                     select new { supplierCautionMoneyDeduction.Amount })
                                                                                                                     .Sum(x => x.Amount) : 0

                                 }).ToList();
                    foreach (var item in uiEntitys)
                    {
                        item.TakeBackCautionMoney = item.RefundedAmount + item.DeductedAmount;
                    }

                }


            }

            totalRecords = total;
            return uiEntitys;
        }

        public IList<UISupplierCautionMoneyAppPayment> GetPayments(UISearchApplicationPayment uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {
            IList<UISupplierCautionMoneyAppPayment> uiEntities = new List<UISupplierCautionMoneyAppPayment>();

            int total = 0;

            var query = ((from ap in DB.ApplicationPayment
                          where ap.IsDeleted == false && ap.WorkflowID == uiSearchObj.WorkflowID
                          && ap.ApplicationID == uiSearchObj.ApplicationID
                          select new UISupplierCautionMoneyAppPayment
                          {
                              ID = ap.ID,
                              PaymentMethodID = (int)EPaymentMethod.BankTransfer,
                              PaymentMethod = GlobalConst.PaymentMethods.BANK_TRANSFER,
                              Amount = ap.Amount,
                              Fee = ap.Fee,
                              PayDate = ap.PayDate,
                              Comment = ap.Comment,
                              FromAccount = ap.FromAccount,
                              ToAccount = ap.ToAccount
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
