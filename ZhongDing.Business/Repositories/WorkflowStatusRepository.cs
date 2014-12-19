using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ZhongDing.Business.IRepositories;
using ZhongDing.Domain.Models;
using ZhongDing.Domain.UIObjects;
using ZhongDing.Domain.UISearchObjects;

namespace ZhongDing.Business.Repositories
{
    public class WorkflowStatusRepository : BaseRepository<WorkflowStatus>, IWorkflowStatusRepository
    {
        public IList<UIDropdownItem> GetDropdownItems(UISearchDropdownItem uiSearchObj = null)
        {
            IList<UIDropdownItem> uiDropdownItems = new List<UIDropdownItem>();

            List<Expression<Func<WorkflowStatus, bool>>> whereFuncs = new List<Expression<Func<WorkflowStatus, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.IncludeItemValues != null
                    && uiSearchObj.IncludeItemValues.Count > 0)
                    whereFuncs.Add(x => uiSearchObj.IncludeItemValues.Contains(x.ID));

                if (!string.IsNullOrEmpty(uiSearchObj.ItemText))
                    whereFuncs.Add(x => x.StatusName.Contains(uiSearchObj.ItemText));
            }

            uiDropdownItems = GetList(whereFuncs).Select(x => new UIDropdownItem()
            {
                ItemValue = x.ID,
                ItemText = x.StatusName
            }).ToList();

            return uiDropdownItems;
        }

        public IList<int> GetCanAccessUserIDsByID(int statusID)
        {
            IList<int> userIDs = new List<int>();

            userIDs = (from wss in DB.WorkflowStepStatus
                       join ws in DB.WorkflowStatus on wss.WorkflowStatusID equals ws.ID
                       join wsu in DB.WorkflowStepUser on wss.WorkflowStepID equals wsu.WorkflowStepID
                       where ws.IsDeleted == false
                       && wss.IsDeleted == false
                       && wsu.IsDeleted == false
                       && wss.WorkflowStatusID == statusID
                       select wsu.UserID).ToList();

            return userIDs;
        }

        public IList<int> GetCanAccessIDsByUserID(int userID)
        {
            IList<int> statusIDs = new List<int>();

            statusIDs = (from wss in DB.WorkflowStepStatus
                         join ws in DB.WorkflowStatus on wss.WorkflowStatusID equals ws.ID
                         join wsu in DB.WorkflowStepUser on wss.WorkflowStepID equals wsu.WorkflowStepID
                         where ws.IsDeleted == false
                         && wss.IsDeleted == false
                         && wsu.IsDeleted == false
                         && wsu.UserID == userID 
                         select wss.WorkflowStatusID).ToList();

            return statusIDs;
        }
    }
}
