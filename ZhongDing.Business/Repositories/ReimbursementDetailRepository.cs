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
using ZhongDing.Common;

namespace ZhongDing.Business.Repositories
{
    public class ReimbursementDetailRepository : BaseRepository<ReimbursementDetail>, IReimbursementDetailRepository
    {
        public IList<UIReimbursementDetail> GetUIList(Domain.UISearchObjects.UISearchReimbursementDetail uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {
            List<UIReimbursementDetail> uiEntitys = new List<UIReimbursementDetail>();
            int total = 0;

            List<Expression<Func<ReimbursementDetail, bool>>> whereFuncs = new List<Expression<Func<ReimbursementDetail, bool>>>();
            IQueryable<ReimbursementDetail> query = null;
          
            if (uiSearchObj != null)
            {
                if (uiSearchObj.ID > 0)
                {
                    whereFuncs.Add(x => x.ID == uiSearchObj.ID);
                }
                if (uiSearchObj.ReimbursementID.BiggerThanZero())
                {
                    whereFuncs.Add(x => x.ReimbursementID == uiSearchObj.ReimbursementID);
                }

            }
            query = GetList(pageIndex, pageSize, whereFuncs, GlobalConst.OrderByExpression.CREATEDON_DESC, out total);

            var queryResult = from q in query
                              join type in DB.ReimbursementType on q.ReimbursementTypeID equals type.ID
                              join typeChild in DB.ReimbursementType on q.ReimbursementTypeChildID equals typeChild.ID into tempTypeChildList
                              from tempTypeChild in tempTypeChildList.DefaultIfEmpty()
                              select new UIReimbursementDetail()
                              {
                                  ID = q.ID,
                                  ReimbursementType = type.Name,
                                  ReimbursementTypeChild = tempTypeChild != null ? tempTypeChild.Name : null,
                                  Amount = q.Amount,
                                  Comment = q.Comment,
                                  EndDate = q.EndDate,
                                  StartDate = q.StartDate,
                                  Quantity = q.Quantity

                              };

            total = query.Count();
            totalRecords = total;
            uiEntitys = queryResult.ToList();
            return uiEntitys;
        }
    }
}
