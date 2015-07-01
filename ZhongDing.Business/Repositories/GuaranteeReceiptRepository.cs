using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ZhongDing.Business.IRepositories;
using ZhongDing.Domain.Models;
using ZhongDing.Domain.UIObjects;

namespace ZhongDing.Business.Repositories
{
    public class GuaranteeReceiptRepository : BaseRepository<GuaranteeReceipt>, IGuaranteeReceiptRepository
    {
        public IList<Domain.UIObjects.UIGuaranteeReceipt> GetUIList(Domain.UISearchObjects.UISearchGuaranteeReceipt uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {
            IList<UIGuaranteeReceipt> uiEntities = new List<UIGuaranteeReceipt>();
            int total = 0;

            IQueryable<GuaranteeReceipt> query = null;

            List<Expression<Func<GuaranteeReceipt, bool>>> whereFuncs = new List<Expression<Func<GuaranteeReceipt, bool>>>();

            if (uiSearchObj != null)
            {

                if (uiSearchObj.BeginDate.HasValue)
                    whereFuncs.Add(x => x.ReceiptDate >= uiSearchObj.BeginDate);

                if (uiSearchObj.EndDate.HasValue)
                {
                    uiSearchObj.EndDate = uiSearchObj.EndDate.Value.AddDays(1);
                    whereFuncs.Add(x => x.ReceiptDate < uiSearchObj.EndDate);
                }
            }

            query = GetList(pageIndex, pageSize, whereFuncs, out total);

            if (query != null)
            {
                uiEntities = (from q in query
                              select new UIGuaranteeReceipt()
                              {
                                  ID = q.ID,
                                  OrderCodesIDValueIntString = q.GuaranteeLog.Where(x => x.IsDeleted == false).Select(x => new IDValueIntString()
                                  {
                                      ID = x.ClientSaleApplicationID,
                                      Value = x.ClientSaleApplication.SalesOrderApplication.OrderCode
                                  }),
                                  ReceiptAmount = q.ReceiptAmount,
                                  ReceiptDate = q.ReceiptDate
                              }).ToList();
                StringBuilder sb = new StringBuilder();
                foreach (var entity in uiEntities)
                {
                    sb.Clear();
                    foreach (var code in entity.OrderCodesIDValueIntString)
                    {
                        sb.AppendLine("<a target='_blank' href='ClientSaleAppMaintenance.aspx?EntityID=" + code.ID + "'>" + code.Value + "</a>;");
                    }
                    entity.OrderCodesHtml = sb.ToString();
                }
            }

            totalRecords = total;

            return uiEntities;
        }
    }
}
