﻿using System;
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
    public class ProcureOrderApplicationImportFileLogRepository : BaseRepository<ProcureOrderApplicationImportFileLog>, IProcureOrderApplicationImportFileLogRepository
    {
        public IList<UIProcureOrderApplicationImportFileLog> GetUIList(Domain.UISearchObjects.UISearchDCImportFileLog uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {
            IList<UIProcureOrderApplicationImportFileLog> uiEntities = new List<UIProcureOrderApplicationImportFileLog>();

            int total = 0;

            IQueryable<ProcureOrderApplicationImportFileLog> query = null;

            var whereFuncs = new List<Expression<Func<ProcureOrderApplicationImportFileLog, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.ID > 0)
                    whereFuncs.Add(x => x.ImportFileLogID.Equals(uiSearchObj.ID));

                if (uiSearchObj.ImportDataTypeID > 0)
                    whereFuncs.Add(x => x.ImportFileLog.ImportDataTypeID == uiSearchObj.ImportDataTypeID);

                if (uiSearchObj.ImportStatusID > 0)
                    whereFuncs.Add(x => x.ImportFileLog.ImportStatusID == uiSearchObj.ImportStatusID);

                if (uiSearchObj.BeginDate.HasValue)
                    whereFuncs.Add(x => x.ImportFileLog.ImportBeginDate >= uiSearchObj.BeginDate);

                if (uiSearchObj.EndDate.HasValue)
                {
                    uiSearchObj.EndDate = uiSearchObj.EndDate.Value.AddDays(1);
                    whereFuncs.Add(x => x.ImportFileLog.ImportBeginDate < uiSearchObj.EndDate);
                }
            }

            query = GetList(pageIndex, pageSize, whereFuncs, GlobalConst.OrderByExpression.CREATEDON_DESC, out total);

            if (query != null)
            {
                uiEntities = (from q in query
                              join ifl in DB.ImportFileLog on q.ImportFileLogID equals ifl.ID
                              join iss in DB.ImportStatus on ifl.ImportStatusID equals iss.ID
                              select new UIProcureOrderApplicationImportFileLog()
                              {
                                  ID = q.ID,
                                  ImportBeginDate = ifl.ImportBeginDate,
                                  ImportEndDate = ifl.ImportEndDate,
                                  ImportStatusID = ifl.ImportStatusID,
                                  ImportStatus = iss.StatusName,
                                  ImportFileLogID = q.ImportFileLogID
                              }).ToList();
            }

            totalRecords = total;

            return uiEntities;
        }
    }
}
