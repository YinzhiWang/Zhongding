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
    public class ClientRefundAppDetailRepository : BaseRepository<ClientRefundAppDetail>, IClientRefundAppDetailRepository
    {
        public IList<UIClientRefundAppDetail> GetUIList(UISearchClientRefundAppDetail uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {
            IList<UIClientRefundAppDetail> uiEntities = new List<UIClientRefundAppDetail>();

            int total = 0;

            IQueryable<ClientRefundAppDetail> query = null;

            List<Expression<Func<ClientRefundAppDetail, bool>>> whereFuncs = new List<Expression<Func<ClientRefundAppDetail, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.ID > 0)
                    whereFuncs.Add(x => x.ID.Equals(uiSearchObj.ID));

                if (uiSearchObj.ClientRefundAppID > 0)
                    whereFuncs.Add(x => x.ClientRefundAppID == uiSearchObj.ClientRefundAppID);
            }

            query = GetList(pageIndex, pageSize, whereFuncs, out total);

            if (query != null)
            {
                uiEntities = (from q in query
                              join p in DB.Product on q.ProductID equals p.ID
                              join ps in DB.ProductSpecification on q.ProductSpecificationID equals ps.ID
                              join um in DB.UnitOfMeasurement on ps.UnitOfMeasurementID equals um.ID into tempUM
                              from tum in tempUM.DefaultIfEmpty()
                              select new UIClientRefundAppDetail()
                              {
                                  ID = q.ID,
                                  ProductName = p.ProductName,
                                  Specification = ps.Specification,
                                  UnitOfMeasurement = tum == null ? string.Empty : tum.UnitName,
                                  HighPrice = q.HighPrice,
                                  ActualSalePrice = q.ActualSalePrice,
                                  Count = q.Count,
                                  TotalSalesAmount = q.TotalSalesAmount,
                                  ClientTaxRatio = q.ClientTaxRatio,
                                  RefundAmount = q.RefundAmount
                              }).ToList();
            }

            totalRecords = total;

            return uiEntities;
        }
    }
}
