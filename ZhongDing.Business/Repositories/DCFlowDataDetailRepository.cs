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
    public class DCFlowDataDetailRepository : BaseRepository<DCFlowDataDetail>, IDCFlowDataDetailRepository
    {
        public IList<UIDCFlowDataDetail> GetUIList(UISearchDCFlowDataDetail uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {
            IList<UIDCFlowDataDetail> uiEntities = new List<UIDCFlowDataDetail>();

            int total = 0;

            IQueryable<DCFlowDataDetail> query = null;

            var whereFuncs = new List<Expression<Func<DCFlowDataDetail, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.ID > 0)
                    whereFuncs.Add(x => x.ID.Equals(uiSearchObj.ID));

                if (uiSearchObj.DCFlowDataID > 0)
                    whereFuncs.Add(x => x.DCFlowDataID == uiSearchObj.DCFlowDataID);
            }

            query = GetList(pageIndex, pageSize, whereFuncs, GlobalConst.OrderByExpression.CREATEDON_DESC, out total);

            if (query != null)
            {
                uiEntities = (from q in query
                              select new UIDCFlowDataDetail()
                              {
                                  ID = q.ID,
                                  ContractCode = q.ContractCode,
                                  ClientUserName = q.ClientUserName,
                                  InChargeUserFullName = q.InChargeUserFullName,
                                  HospitalName = q.HospitalName,
                                  UnitName = q.UnitName,
                                  SaleQty = q.SaleQty,
                                  IsTempContract = q.IsTempContract,
                                  Comment = q.Comment
                              }).ToList();
            }

            totalRecords = total;

            return uiEntities;
        }
    }
}
