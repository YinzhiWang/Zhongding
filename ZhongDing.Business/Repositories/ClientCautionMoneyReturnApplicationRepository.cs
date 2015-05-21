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
    public class ClientCautionMoneyReturnApplicationRepository : BaseRepository<ClientCautionMoneyReturnApplication>, IClientCautionMoneyReturnApplicationRepository
    {
        public IList<Domain.UIObjects.UIClientCautionMoneyReturnApplication> GetUIList(Domain.UISearchObjects.UISearchClientCautionMoneyReturnApplication uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {
            IList<UIClientCautionMoneyReturnApplication> uiEntitys = new List<UIClientCautionMoneyReturnApplication>();
            int total = 0;

            IQueryable<ClientCautionMoneyReturnApplication> query = null;

            List<Expression<Func<ClientCautionMoneyReturnApplication, bool>>> whereFuncs = new List<Expression<Func<ClientCautionMoneyReturnApplication, bool>>>();

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
                if (uiSearchObj.ClientCautionMoneyID.BiggerThanZero())
                {
                    whereFuncs.Add(x => x.ClientCautionMoneyID == uiSearchObj.ClientCautionMoneyID);
                }

            }

            query = GetList(pageIndex, pageSize, whereFuncs, out total);

            if (query != null)
            {
                var queryResult = from q in query
                                  join user in DB.Users on q.CreatedBy equals user.UserID
                                  join workflowStatus in DB.WorkflowStatus on q.WorkflowStatusID equals workflowStatus.ID
                                  select new UIClientCautionMoneyReturnApplication()
                                  {
                                      ID = q.ID,
                                      ApplyDate = q.ApplyDate,
                                      Amount = q.Amount,
                                      ClientCautionMoneyID = q.ClientCautionMoneyID,
                                      Reason = q.Reason,
                                      WorkflowStatus = workflowStatus.StatusName,
                                      WorkflowStatusID = q.WorkflowStatusID,
                                      CreatedByUserID = q.CreatedBy.Value,
                                      CreatedByUserName = user.FullName
                                  };
                total = queryResult.Count();
                uiEntitys = queryResult.ToList();
            }

            totalRecords = total;
            return uiEntitys;
        }

        public IList<Domain.UIObjects.UIClientCautionMoneyReturnApplication> GetUIListForClientCautionMoneyReturnApplyManagement(Domain.UISearchObjects.UISearchClientCautionMoneyReturnApplication uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {
            IList<UIClientCautionMoneyReturnApplication> uiEntitys = new List<UIClientCautionMoneyReturnApplication>();
            int total = 0;

            IQueryable<ClientCautionMoneyReturnApplication> query = null;

            List<Expression<Func<ClientCautionMoneyReturnApplication, bool>>> whereFuncs = new List<Expression<Func<ClientCautionMoneyReturnApplication, bool>>>();

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
                if (uiSearchObj.ClientCautionMoneyID.BiggerThanZero())
                {
                    whereFuncs.Add(x => x.ClientCautionMoneyID == uiSearchObj.ClientCautionMoneyID);
                }
                if (uiSearchObj.IncludeWorkflowStatusIDs != null)
                {
                    whereFuncs.Add(x => uiSearchObj.IncludeWorkflowStatusIDs.Contains(x.WorkflowStatusID));

                }

            }

            query = GetList(pageIndex, pageSize, whereFuncs, out total);

            if (query != null)
            {
                var queryResult = from q in query
                                  join clientCautionMoney in DB.ClientCautionMoney on q.ClientCautionMoneyID equals clientCautionMoney.ID
                                  join cautionMoneyType in DB.CautionMoneyType on clientCautionMoney.CautionMoneyTypeID equals cautionMoneyType.ID
                                  join product in DB.Product on clientCautionMoney.ProductID equals product.ID
                                  join productSpecification in DB.ProductSpecification on clientCautionMoney.ProductSpecificationID equals productSpecification.ID
                                  join clientUser in DB.ClientUser on clientCautionMoney.ClientUserID equals clientUser.ID
                                  join department in DB.Department on clientCautionMoney.DepartmentID equals department.ID

                                  join user in DB.Users on q.CreatedBy equals user.UserID
                                  join workflowStatus in DB.WorkflowStatus on q.WorkflowStatusID equals workflowStatus.ID
                                  select new UIClientCautionMoneyReturnApplication()
                                  {
                                      ID = q.ID,
                                      ApplyDate = q.ApplyDate,
                                      Amount = q.Amount,
                                      ClientCautionMoneyID = q.ClientCautionMoneyID,
                                      Reason = q.Reason,
                                      WorkflowStatus = workflowStatus.StatusName,
                                      WorkflowStatusID = q.WorkflowStatusID,
                                      CreatedByUserID = q.CreatedBy.Value,
                                      CreatedByUserName = user.FullName,

                                      CautionMoneyTypeName = cautionMoneyType.Name,
                                      EndDate = clientCautionMoney.EndDate,
                                      ProductName = product.ProductName,
                                      IsStop = q.IsStop,
                                      ProductSpecification = productSpecification.Specification,
                                      ClientName = clientUser.ClientName,
                                      DepartmentName = department.DepartmentName,
                                  };
                total = queryResult.Count();
                uiEntitys = queryResult.ToList();
            }

            totalRecords = total;
            return uiEntitys;
        }
    }
}
