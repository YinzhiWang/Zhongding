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
    public class SupplierCautionMoneyRepository : BaseRepository<SupplierCautionMoney>, ISupplierCautionMoneyRepository
    {
        public IList<Domain.UIObjects.UISupplierCautionMoney> GetSupplierCautionMoneyApplyUIList(Domain.UISearchObjects.UISearchSupplierCautionMoney uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {
            IList<UISupplierCautionMoney> uiEntitys = new List<UISupplierCautionMoney>();
            int total = 0;

            IQueryable<SupplierCautionMoney> query = null;

            List<Expression<Func<SupplierCautionMoney, bool>>> whereFuncs = new List<Expression<Func<SupplierCautionMoney, bool>>>();

            if (uiSearchObj != null)
            {

            }

            query = GetList(pageIndex, pageSize, whereFuncs, out total);

            if (query != null)
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
