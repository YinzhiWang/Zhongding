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

namespace ZhongDing.Business.Repositories
{
    public class ProcureOrderApplicationImportDataRepository : BaseRepository<ProcureOrderApplicationImportData>, IProcureOrderApplicationImportDataRepository
    {
        public IList<Domain.UIObjects.UIProcureOrderApplicationImportData> GetUIList(Domain.UISearchObjects.UISearchProcureOrderApplicationImportData uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {
            IList<UIProcureOrderApplicationImportData> uiEntities = new List<UIProcureOrderApplicationImportData>();

            int total = 0;

            //IQueryable<ProcureOrderApplicationImportData> query = null;

            //var whereFuncs = new List<Expression<Func<ProcureOrderApplicationImportData, bool>>>();

            //if (uiSearchObj != null)
            //{
            //    if (uiSearchObj.ID > 0)
            //        whereFuncs.Add(x => x.ID.Equals(uiSearchObj.ID));
            //}

            //query = GetList(pageIndex, pageSize, whereFuncs, GlobalConst.OrderByExpression.CREATEDON_DESC, out total);

            var query = from procureOrderApplicationImportFileLog in DB.ProcureOrderApplicationImportFileLog
                        join procureOrderApplicationImportData in DB.ProcureOrderApplicationImportData
                        on procureOrderApplicationImportFileLog.ID equals procureOrderApplicationImportData.ProcureOrderApplicationImportFileLogID
                        join procureOrderAppDetailImportData in DB.ProcureOrderAppDetailImportData
                        on procureOrderApplicationImportData.ID equals procureOrderAppDetailImportData.ProcureOrderApplicationImportDataID
                        where procureOrderApplicationImportFileLog.ImportFileLogID == uiSearchObj.ImportFileLogID
                        select new UIProcureOrderApplicationImportData()
                        {
                             
                        };

            totalRecords = total;

            return uiEntities;
        }
    }
}
