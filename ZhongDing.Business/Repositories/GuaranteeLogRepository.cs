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
    public class GuaranteeLogRepository : BaseRepository<GuaranteeLog>, IGuaranteeLogRepository
    {
        public IList<Domain.UIObjects.UIGuaranteeLog> GetUIList(Domain.UISearchObjects.UISearchGuaranteeReceipt uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {
            IList<UIGuaranteeLog> uiEntities = new List<UIGuaranteeLog>();
            int total = 0;

            IQueryable<GuaranteeLog> query = null;

            List<Expression<Func<GuaranteeLog, bool>>> whereFuncs = new List<Expression<Func<GuaranteeLog, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.GuaranteeReceiptID.BiggerThanZero())
                {
                    whereFuncs.Add(x => x.GuaranteeReceiptID == uiSearchObj.GuaranteeReceiptID);
                }
            }

            query = GetList(pageIndex, pageSize, whereFuncs, out total);

            if (query != null)
            {
                uiEntities = (from q in query
                              join cbu in DB.Users on q.Guaranteeby equals cbu.UserID into tempCBU
                              from tcbu in tempCBU.DefaultIfEmpty()
                              select new UIGuaranteeLog()
                              {
                                  ID = q.ID,
                                  ClientSaleApplicationID = q.ClientSaleApplicationID,
                                  OrderCode = q.ClientSaleApplication.SalesOrderApplication.OrderCode,
                                  OrderDate = q.ClientSaleApplication.SalesOrderApplication.OrderDate,
                                  GuaranteeAmount = q.GuaranteeAmount,
                                  GuaranteeExpirationDate = q.GuaranteeExpirationDate,
                                  CreatedBy = tcbu == null ? string.Empty : tcbu.FullName,
                                  GuaranteeReceiptDate = q.GuaranteeReceiptDate,
                                  

                              }).ToList();
            }

            totalRecords = total;

            return uiEntities;
        }
    }
}
