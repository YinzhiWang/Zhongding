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
using ZhongDing.Domain.UISearchObjects;

namespace ZhongDing.Business.Repositories
{
    public class DCImportFileLogRepository : BaseRepository<DCImportFileLog>, IDCImportFileLogRepository
    {
        public IList<UIDCImportFileLog> GetUIList(UISearchDCImportFileLog uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {
            IList<UIDCImportFileLog> uiEntities = new List<UIDCImportFileLog>();

            int total = 0;

            IQueryable<DCImportFileLog> query = null;

            var whereFuncs = new List<Expression<Func<DCImportFileLog, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.ID > 0)
                    whereFuncs.Add(x => x.ImportFileLogID.Equals(uiSearchObj.ID));

                if (uiSearchObj.DistributionCompanyID > 0)
                    whereFuncs.Add(x => x.DistributionCompanyID == uiSearchObj.DistributionCompanyID);

                if (uiSearchObj.ImportDataTypeID > 0)
                    whereFuncs.Add(x => x.ImportFileLog.ImportDataTypeID == uiSearchObj.ImportDataTypeID);

                if (uiSearchObj.ImportStatusID > 0)
                    whereFuncs.Add(x => x.ImportFileLog.ImportStatusID == uiSearchObj.ImportStatusID);

                if (uiSearchObj.SettlementDate.HasValue)
                    whereFuncs.Add(x => x.SettlementDate == uiSearchObj.SettlementDate);

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
                              join dc in DB.DistributionCompany on q.DistributionCompanyID equals dc.ID
                              join ifl in DB.ImportFileLog on q.ImportFileLogID equals ifl.ID
                              join iss in DB.ImportStatus on ifl.ImportStatusID equals iss.ID
                              select new UIDCImportFileLog()
                              {
                                  ID = q.ImportFileLogID,
                                  SettlementDate = q.SettlementDate,
                                  DistributionCompanyName = dc.Name,
                                  ImportBeginDate = ifl.ImportBeginDate,
                                  ImportEndDate = ifl.ImportEndDate,
                                  ImportStatusID = ifl.ImportStatusID,
                                  ImportStatus = iss.StatusName
                              }).ToList();
            }

            totalRecords = total;

            return uiEntities;
        }
    }
}