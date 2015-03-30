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
    public class ClientFlowDataRepository : BaseRepository<ClientFlowData>, IClientFlowDataRepository
    {
        public IList<UIClientFlowData> GetUIList(UISearchClientFlowData uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {
            IList<UIClientFlowData> uiEntities = new List<UIClientFlowData>();

            int total = 0;

            IQueryable<ClientFlowData> query = null;

            var whereFuncs = new List<Expression<Func<ClientFlowData, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.ID > 0)
                    whereFuncs.Add(x => x.ID.Equals(uiSearchObj.ID));

                if (uiSearchObj.ImportFileLogID > 0)
                    whereFuncs.Add(x => x.ImportFileLogID == uiSearchObj.ImportFileLogID);

                if (uiSearchObj.ClientUserID > 0)
                    whereFuncs.Add(x => x.ClientUserID == uiSearchObj.ClientUserID);

                if (uiSearchObj.ClientCompanyID > 0)
                    whereFuncs.Add(x => x.ClientCompanyID == uiSearchObj.ClientCompanyID);

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
                              join cu in DB.ClientUser on q.ClientUserID equals cu.ID
                              join cc in DB.ClientCompany on q.ClientCompanyID equals cc.ID
                              join p in DB.Product on q.ProductID equals p.ID
                              join ps in DB.ProductSpecification on q.ProductSpecificationID equals ps.ID
                              join s in DB.Supplier on p.SupplierID equals s.ID
                              join ht in DB.HospitalType on q.HospitalTypeID equals ht.ID into tempHT
                              from tht in tempHT.DefaultIfEmpty()
                              select new UIClientFlowData()
                              {
                                  ID = q.ID,
                                  SaleDate = q.SaleDate,
                                  ClientUserName = cu.ClientName,
                                  ClientCompanyName = cc.Name,
                                  ProductCode = p.ProductCode,
                                  ProductName = p.ProductName,
                                  Specification = ps.Specification,
                                  SaleQty = q.SaleQty,
                                  FactoryName = s.FactoryName,
                                  FlowTo = q.FlowTo,
                                  MarketName = q.MarketName,
                                  HospitalType = tht == null ? string.Empty : tht.TypeName
                              }).ToList();
            }

            totalRecords = total;

            return uiEntities;
        }
    }
}
