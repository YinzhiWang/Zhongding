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
    public class ReimbursementTypeRepository : BaseRepository<ReimbursementType>, IReimbursementTypeRepository
    {
        public IList<Domain.UIObjects.UIReimbursementType> GetUIList(Domain.UISearchObjects.UISearchReimbursementType uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {
            IList<UIReimbursementType> uiWarehouses = new List<UIReimbursementType>();

            int total = 0;

            IQueryable<ReimbursementType> query = null;

            List<Expression<Func<ReimbursementType, bool>>> whereFuncs = new List<Expression<Func<ReimbursementType, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.ID > 0)
                    whereFuncs.Add(x => x.ID.Equals(uiSearchObj.ID));

                if (uiSearchObj.Name.HasValue())
                    whereFuncs.Add(x => x.Name.Contains(uiSearchObj.Name));

                if (uiSearchObj.ParentID.BiggerThanZero())
                    whereFuncs.Add(x => x.ParentID == uiSearchObj.ParentID);

                if (uiSearchObj.OnlyParent)
                    whereFuncs.Add(x => x.ParentID == null);


            }

            query = GetList(pageIndex, pageSize, whereFuncs, out total);

            if (query != null)
            {
                uiWarehouses = (from q in query
                                select new UIReimbursementType()
                                {
                                    ID = q.ID,
                                    Name = q.Name,
                                    Comment = q.Comment,
                                }).ToList();
            }

            totalRecords = total;

            return uiWarehouses;
        }
    }
}
