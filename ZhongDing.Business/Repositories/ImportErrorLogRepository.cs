using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ZhongDing.Business.IRepositories;
using ZhongDing.Common;
using ZhongDing.Domain.Models;
using ZhongDing.Domain.UIObjects;
using ZhongDing.Domain.UISearchObjects;

namespace ZhongDing.Business.Repositories
{
    public class ImportErrorLogRepository : BaseRepository<ImportErrorLog>, IImportErrorLogRepository
    {
        public IList<UIImportErrorLog> GetUIList(UISearchImportErrorLog uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {
            IList<UIImportErrorLog> uiEntities = new List<UIImportErrorLog>();

            int total = 0;

            IQueryable<ImportErrorLog> query = null;

            var whereFuncs = new List<Expression<Func<ImportErrorLog, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.ID > 0)
                    whereFuncs.Add(x => x.ID.Equals(uiSearchObj.ID));

                if (uiSearchObj.ImportFileLogID > 0)
                    whereFuncs.Add(x => x.ImportFileLogID == uiSearchObj.ImportFileLogID);
            }

            query = GetList(pageIndex, pageSize, whereFuncs, GlobalConst.OrderByExpression.CREATEDON_DESC, out total);

            if (query != null)
            {
                uiEntities = (from q in query
                              select new UIImportErrorLog()
                              {
                                  ID = q.ID,
                                  ErrorRowIndex = q.ErrorRowIndex,
                                  ErrorRowData = q.ErrorRowData,
                                  ErrorMsg = q.ErrorMsg
                              }).ToList();

                foreach (var uiEntity in uiEntities)
                {
                    if (!string.IsNullOrEmpty(uiEntity.ErrorMsg))
                        uiEntity.AbbrErrorMsg = Utility.CutString(uiEntity.ErrorMsg, 135, true);
                }
            }

            totalRecords = total;

            return uiEntities;
        }
    }
}
