using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ZhongDing.Business.IRepositories;
using ZhongDing.Common.Enums;
using ZhongDing.Domain.Models;
using ZhongDing.Domain.UIObjects;
using ZhongDing.Common.Extension;

namespace ZhongDing.Business.Repositories
{
    public class ClientCautionMoneyRepository : BaseRepository<ClientCautionMoney>, IClientCautionMoneyRepository
    {
        public IList<Domain.UIObjects.UIClientCautionMoney> GetUIList(Domain.UISearchObjects.UISearchClientCautionMoney uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {
            List<UIClientCautionMoney> uiEntitys = new List<UIClientCautionMoney>();
            int total = 0;

            IQueryable<ClientCautionMoney> query = null;

            List<Expression<Func<ClientCautionMoney, bool>>> whereFuncs = new List<Expression<Func<ClientCautionMoney, bool>>>();

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
                if (uiSearchObj.BeginDate.HasValue)
                {
                    whereFuncs.Add(x => x.EndDate >= uiSearchObj.BeginDate);
                }
                if (uiSearchObj.EndDate.HasValue)
                {
                    whereFuncs.Add(x => x.EndDate <= uiSearchObj.EndDate);
                }
                if (uiSearchObj.DepartmentID.BiggerThanZero())
                {
                    whereFuncs.Add(x => x.DepartmentID == uiSearchObj.DepartmentID);
                }

            }

            query = GetList(whereFuncs);

            if (query != null)
            {
                var queryResult = from q in query
                                  join cautionMoneyType in DB.CautionMoneyType on q.CautionMoneyTypeID equals cautionMoneyType.ID
                                  join product in DB.Product on q.ProductID equals product.ID
                                  join productSpecification in DB.ProductSpecification on q.ProductSpecificationID equals productSpecification.ID
                                  join clientUser in DB.ClientUser on q.ClientUserID equals clientUser.ID
                                  join department in DB.Department on q.DepartmentID equals department.ID
                                  join user in DB.Users on q.CreatedBy equals user.UserID
                                  join workflowStatus in DB.WorkflowStatus on q.WorkflowStatusID equals workflowStatus.ID
                                  select new UIClientCautionMoney()
                                  {
                                      ID = q.ID,
                                      CautionMoneyTypeName = cautionMoneyType.Name,
                                      EndDate = q.EndDate,
                                      PaymentCautionMoney = DB.ApplicationPayment.Any(x =>
                                          x.ApplicationID == q.ID &&
                                          x.WorkflowID == (int)EWorkflow.ClientCautionMoney &&
                                          x.PaymentTypeID == (int)EPaymentType.Income &&
                                          x.PaymentStatusID == (int)EPaymentStatus.Paid &&
                                          x.IsDeleted == false) ?
                                          DB.ApplicationPayment.Where(x =>
                                          x.ApplicationID == q.ID &&
                                          x.WorkflowID == (int)EWorkflow.ClientCautionMoney &&
                                          x.PaymentTypeID == (int)EPaymentType.Income &&
                                          x.PaymentStatusID == (int)EPaymentStatus.Paid &&
                                          x.IsDeleted == false).Sum(x => x.Amount) : 0,
                                      ProductName = product.ProductName,
                                      Remark = q.Remark,
                                      IsStop = q.IsStop,
                                      ProductSpecification = productSpecification.Specification,
                                      WorkflowStatus = workflowStatus.StatusName,
                                      WorkflowStatusID = q.WorkflowStatusID,
                                      CreatedByUserID = q.CreatedBy.Value,
                                      CreatedByUserName = user.FullName,
                                      ClientName = clientUser.ClientName,
                                      DepartmentName = department.DepartmentName,
                                      ReturnCautionMoney = (from ccra in DB.ClientCautionMoneyReturnApplication.Where(x => x.ClientCautionMoneyID == q.ID)
                                                            join ap in DB.ApplicationPayment.Where(x =>
                                                            x.WorkflowID == (int)EWorkflow.ClientCautionMoneyReturnApply &&
                                                            x.PaymentTypeID == (int)EPaymentType.Expend &&
                                                            x.PaymentStatusID == (int)EPaymentStatus.Paid &&
                                                            x.IsDeleted == false) on ccra.ID equals ap.ApplicationID
                                                            select ap.Amount).Any() ?
                                                            (from ccra in DB.ClientCautionMoneyReturnApplication.Where(x => x.ClientCautionMoneyID == q.ID)
                                                             join ap in DB.ApplicationPayment.Where(x =>
                                                             x.WorkflowID == (int)EWorkflow.ClientCautionMoneyReturnApply &&
                                                             x.PaymentTypeID == (int)EPaymentType.Expend &&
                                                             x.PaymentStatusID == (int)EPaymentStatus.Paid &&
                                                             x.IsDeleted == false) on ccra.ID equals ap.ApplicationID
                                                             select ap.Amount).Sum() : 0
                                  };

                if (uiSearchObj.ClientName.HasValue())
                {
                    queryResult = queryResult.Where(x => x.ClientName.Contains(uiSearchObj.ClientName));
                }
                if (uiSearchObj.ProductName.HasValue())
                {
                    queryResult = queryResult.Where(x => x.ProductName.Contains(uiSearchObj.ProductName));
                }
                total = queryResult.Count();

                uiEntitys = queryResult.OrderByDescending(x => x.ID)
                    .Skip(pageSize * pageIndex).Take(pageSize).ToList();
            }
            uiEntitys.ForEach(x =>
            {
                x.NotReturnCautionMoney = (x.PaymentCautionMoney ?? 0) - (x.ReturnCautionMoney ?? 0);
            });
            totalRecords = total;
            return uiEntitys;
        }


        public UIClientCautionMoney GetUIClientCautionMoneyByID(int id)
        {
            int totalRecords = 0;
            var list = GetUIList(new Domain.UISearchObjects.UISearchClientCautionMoney() { ID = id }, 0, 1, out totalRecords);
            if (totalRecords > 0)
            {
                return list.FirstOrDefault();
            }
            return null;
        }
    }
}
