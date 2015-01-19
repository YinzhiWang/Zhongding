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
    public class ClientInfoProductSettingRepository : BaseRepository<ClientInfoProductSetting>, IClientInfoProductSettingRepository
    {
        public IList<UIClientInfoProductSetting> GetUIList(UISearchClientInfoProductSetting uiSearchObj = null)
        {
            IList<UIClientInfoProductSetting> uiEntities = new List<UIClientInfoProductSetting>();

            IQueryable<ClientInfoProductSetting> query = null;

            List<Expression<Func<ClientInfoProductSetting, bool>>> whereFuncs = new List<Expression<Func<ClientInfoProductSetting, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.ID > 0)
                    whereFuncs.Add(x => x.ID.Equals(uiSearchObj.ID));

                if (uiSearchObj.ClientInfoID > 0)
                    whereFuncs.Add(x => x.ClientInfoID.Equals(uiSearchObj.ClientInfoID));

                if (uiSearchObj.ClientUserID > 0)
                    whereFuncs.Add(x => x.ClientInfo.ClientUserID == uiSearchObj.ClientUserID);

                if (uiSearchObj.ClientCompanyID > 0)
                    whereFuncs.Add(x => x.ClientInfo.ClientCompanyID == uiSearchObj.ClientCompanyID);
            }

            query = GetList(whereFuncs);

            if (query != null)
            {
                uiEntities = (from q in query
                              join ci in DB.ClientInfo on q.ClientInfoID equals ci.ID
                              join cu in DB.ClientUser on ci.ClientUserID equals cu.ID
                              join cc in DB.ClientCompany on ci.ClientCompanyID equals cc.ID
                              join p in DB.Product on q.ProductID equals p.ID
                              join ps in DB.ProductSpecification on q.ProductSpecificationID equals ps.ID
                              join s in DB.Supplier on p.SupplierID equals s.ID into tempS
                              from ts in tempS.DefaultIfEmpty()
                              select new UIClientInfoProductSetting()
                              {
                                  ID = q.ID,
                                  ClientName = cu.ClientName,
                                  ClientCompany = cc.Name,
                                  ProductName = p.ProductName,
                                  ProductSpecification = ps.Specification,
                                  FactoryName = ts == null ? string.Empty : ts.FactoryName,
                                  HighPrice = q.HighPrice,
                                  BasicPrice = q.BasicPrice,
                                  UseFlowData = q.UseFlowData
                              }).ToList();
            }

            return uiEntities;
        }

        public IList<UIClientInfoProductSetting> GetUIList(UISearchClientInfoProductSetting uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {
            IList<UIClientInfoProductSetting> uiEntities = new List<UIClientInfoProductSetting>();

            int total = 0;

            IQueryable<ClientInfoProductSetting> query = null;

            List<Expression<Func<ClientInfoProductSetting, bool>>> whereFuncs = new List<Expression<Func<ClientInfoProductSetting, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.ID > 0)
                    whereFuncs.Add(x => x.ID.Equals(uiSearchObj.ID));

                if (uiSearchObj.ClientInfoID > 0)
                    whereFuncs.Add(x => x.ClientInfoID.Equals(uiSearchObj.ClientInfoID));

                if (uiSearchObj.ClientUserID > 0)
                    whereFuncs.Add(x => x.ClientInfo.ClientUserID == uiSearchObj.ClientUserID);

                if (uiSearchObj.ClientCompanyID > 0)
                    whereFuncs.Add(x => x.ClientInfo.ClientCompanyID == uiSearchObj.ClientCompanyID);
            }

            query = GetList(pageIndex, pageSize, whereFuncs, out total);

            if (query != null)
            {
                uiEntities = (from q in query
                              join ci in DB.ClientInfo on q.ClientInfoID equals ci.ID
                              join cu in DB.ClientUser on ci.ClientUserID equals cu.ID
                              join cc in DB.ClientCompany on ci.ClientCompanyID equals cc.ID
                              join p in DB.Product on q.ProductID equals p.ID
                              join ps in DB.ProductSpecification on q.ProductSpecificationID equals ps.ID
                              join s in DB.Supplier on p.SupplierID equals s.ID into tempS
                              from ts in tempS.DefaultIfEmpty()
                              select new UIClientInfoProductSetting()
                              {
                                  ID = q.ID,
                                  ClientName = cu.ClientName,
                                  ClientCompany = cc.Name,
                                  ProductName = p.ProductName,
                                  ProductSpecification = ps.Specification,
                                  FactoryName = ts == null ? string.Empty : ts.FactoryName,
                                  HighPrice = q.HighPrice,
                                  BasicPrice = q.BasicPrice,
                                  UseFlowData = q.UseFlowData
                              }).ToList();
            }

            totalRecords = total;

            return uiEntities;
        }
    }
}
