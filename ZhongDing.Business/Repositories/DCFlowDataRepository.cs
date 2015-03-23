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
    public class DCFlowDataRepository : BaseRepository<DCFlowData>, IDCFlowDataRepository
    {
        public IList<UIDCFlowData> GetUIList(UISearchDCFlowData uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {
            IList<UIDCFlowData> uiEntities = new List<UIDCFlowData>();

            int total = 0;

            IQueryable<DCFlowData> query = null;

            var whereFuncs = new List<Expression<Func<DCFlowData, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.ID > 0)
                    whereFuncs.Add(x => x.ID.Equals(uiSearchObj.ID));

                if (uiSearchObj.DistributionCompanyID > 0)
                    whereFuncs.Add(x => x.DistributionCompanyID == uiSearchObj.DistributionCompanyID);

                if (uiSearchObj.ProductID > 0)
                    whereFuncs.Add(x => x.ProductID == uiSearchObj.ProductID);

                if (uiSearchObj.BeginDate.HasValue)
                    whereFuncs.Add(x => x.SaleDate >= uiSearchObj.BeginDate);

                if (uiSearchObj.EndDate.HasValue)
                {
                    uiSearchObj.EndDate = uiSearchObj.EndDate.Value.AddDays(1);
                    whereFuncs.Add(x => x.SaleDate < uiSearchObj.EndDate);
                }
            }

            query = GetList(pageIndex, pageSize, whereFuncs, GlobalConst.OrderByExpression.CREATEDON_DESC, out total);

            if (query != null)
            {
                uiEntities = (from q in query
                              join dc in DB.DistributionCompany on q.DistributionCompanyID equals dc.ID
                              join p in DB.Product on q.ProductID equals p.ID
                              join ps in DB.ProductSpecification on q.ProductSpecificationID equals ps.ID
                              join s in DB.Supplier on p.SupplierID equals s.ID
                              join um in DB.UnitOfMeasurement on ps.UnitOfMeasurementID equals um.ID into tempUM
                              from tum in tempUM.DefaultIfEmpty()
                              select new UIDCFlowData()
                              {
                                  ID = q.ID,
                                  SaleDate = q.SaleDate,
                                  DistributionCompanyName = dc.Name,
                                  ProductCode = p.ProductCode,
                                  ProductName = p.ProductName,
                                  Specification = ps.Specification,
                                  UnitName = tum == null ? string.Empty : tum.UnitName,
                                  SaleQty = q.SaleQty,
                                  FactoryName = s.FactoryName,

                              }).ToList();
            }

            totalRecords = total;

            return uiEntities;
        }
    }
}
