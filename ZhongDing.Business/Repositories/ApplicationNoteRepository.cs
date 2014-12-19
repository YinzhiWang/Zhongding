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
    public class ApplicationNoteRepository : BaseRepository<ApplicationNote>, IApplicationNoteRepository
    {
        public IList<UIApplicationNote> GetUIList(UISearchApplicationNote uiSearchObj = null)
        {
            IList<UIApplicationNote> uiEntities = new List<UIApplicationNote>();

            IQueryable<ApplicationNote> query = null;

            List<Expression<Func<ApplicationNote, bool>>> whereFuncs = new List<Expression<Func<ApplicationNote, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.ID > 0)
                    whereFuncs.Add(x => x.ID.Equals(uiSearchObj.ID));

                if (uiSearchObj.ApplicationID > 0)
                    whereFuncs.Add(x => x.ApplicationID == uiSearchObj.ApplicationID);

                if (uiSearchObj.WorkflowID > 0)
                    whereFuncs.Add(x => x.WorkflowID.Equals(uiSearchObj.WorkflowID));

                if (uiSearchObj.WorkflowStepID > 0)
                    whereFuncs.Add(x => x.WorkflowStepID.Equals(uiSearchObj.WorkflowStepID));
            }

            query = GetList(whereFuncs);

            if (query != null)
            {
                uiEntities = (from q in query
                              join cu in this.DB.Users on q.CreatedBy equals cu.UserID into tempCU
                              from tcu in tempCU.DefaultIfEmpty()
                              orderby q.CreatedOn descending
                              select new UIApplicationNote()
                              {
                                  ID = q.ID,
                                  CreatedOn = q.CreatedOn,
                                  CreatedBy = tcu == null ? string.Empty : tcu.FullName,
                                  Note = q.Note
                              }).ToList();
            }

            return uiEntities;
        }

        public IList<UIApplicationNote> GetUIList(UISearchApplicationNote uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {
            IList<UIApplicationNote> uiEntities = new List<UIApplicationNote>();

            int total = 0;

            IQueryable<ApplicationNote> query = null;

            List<Expression<Func<ApplicationNote, bool>>> whereFuncs = new List<Expression<Func<ApplicationNote, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.ID > 0)
                    whereFuncs.Add(x => x.ID.Equals(uiSearchObj.ID));

                if (uiSearchObj.ApplicationID > 0)
                    whereFuncs.Add(x => x.ApplicationID == uiSearchObj.ApplicationID);

                if (uiSearchObj.WorkflowID > 0)
                    whereFuncs.Add(x => x.WorkflowID.Equals(uiSearchObj.WorkflowID));

                if (uiSearchObj.WorkflowStepID > 0)
                    whereFuncs.Add(x => x.WorkflowStepID.Equals(uiSearchObj.WorkflowStepID));
            }

            query = GetList(pageIndex, pageSize, whereFuncs, out total);

            if (query != null)
            {
                uiEntities = (from q in query
                              join cu in this.DB.Users on q.CreatedBy equals cu.UserID into tempCU
                              from tcu in tempCU.DefaultIfEmpty()
                              orderby q.CreatedOn descending
                              select new UIApplicationNote()
                              {
                                  ID = q.ID,
                                  CreatedOn = q.CreatedOn,
                                  CreatedBy = tcu == null ? string.Empty : tcu.FullName,
                                  Note = q.Note
                              }).ToList();
            }

            totalRecords = total;

            return uiEntities;
        }
    }
}
