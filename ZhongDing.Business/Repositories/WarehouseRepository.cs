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
    public class WarehouseRepository : BaseRepository<Warehouse>, IWarehouseRepository
    {
        public IList<UIWarehouse> GetUIList(UISearchWarehouse uiSearchObj = null)
        {
            IList<UIWarehouse> uiWarehouses = new List<UIWarehouse>();

            IQueryable<Warehouse> query = null;

            List<Expression<Func<Warehouse, bool>>> whereFuncs = new List<Expression<Func<Warehouse, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.ID > 0)
                    whereFuncs.Add(x => x.ID.Equals(uiSearchObj.ID));

                if (uiSearchObj.CompanyID > 0)
                    whereFuncs.Add(x => x.CompanyID == uiSearchObj.CompanyID);

                if (uiSearchObj.SaleTypeID > 0)
                    whereFuncs.Add(x => x.SaleTypeID == uiSearchObj.SaleTypeID);

                if (!string.IsNullOrEmpty(uiSearchObj.WarehouseCode))
                    whereFuncs.Add(x => x.WarehouseCode.Contains(uiSearchObj.WarehouseCode));

                if (!string.IsNullOrEmpty(uiSearchObj.Name))
                    whereFuncs.Add(x => x.Name.Contains(uiSearchObj.Name));
            }

            query = GetList(whereFuncs);

            if (query != null)
            {
                uiWarehouses = (from q in query
                                join st in DB.SaleType on q.SaleTypeID equals st.ID into tempST
                                from tst in tempST.DefaultIfEmpty()
                                select new UIWarehouse()
                                {
                                    ID = q.ID,
                                    WarehouseCode = q.WarehouseCode,
                                    Name = q.Name,
                                    //Comment = q.Comment,
                                    SaleTypeName = tst == null ? string.Empty : tst.SaleType1
                                }).ToList();
            }

            return uiWarehouses;
        }

        public IList<UIWarehouse> GetUIList(UISearchWarehouse uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {
            IList<UIWarehouse> uiWarehouses = new List<UIWarehouse>();

            int total = 0;

            IQueryable<Warehouse> query = null;

            List<Expression<Func<Warehouse, bool>>> whereFuncs = new List<Expression<Func<Warehouse, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.ID > 0)
                    whereFuncs.Add(x => x.ID.Equals(uiSearchObj.ID));

                if (uiSearchObj.CompanyID > 0)
                    whereFuncs.Add(x => x.CompanyID == uiSearchObj.CompanyID);

                if (uiSearchObj.SaleTypeID > 0)
                    whereFuncs.Add(x => x.SaleTypeID == uiSearchObj.SaleTypeID);

                if (!string.IsNullOrEmpty(uiSearchObj.WarehouseCode))
                    whereFuncs.Add(x => x.WarehouseCode.Contains(uiSearchObj.WarehouseCode));

                if (!string.IsNullOrEmpty(uiSearchObj.Name))
                    whereFuncs.Add(x => x.Name.Contains(uiSearchObj.Name));
            }

            query = GetList(pageIndex, pageSize, whereFuncs, out total);

            if (query != null)
            {
                uiWarehouses = (from q in query
                                join st in DB.SaleType on q.SaleTypeID equals st.ID into tempST
                                from tst in tempST.DefaultIfEmpty()
                                select new UIWarehouse()
                                {
                                    ID = q.ID,
                                    WarehouseCode = q.WarehouseCode,
                                    Name = q.Name,
                                    //Comment = q.Comment,
                                    SaleTypeName = tst == null ? string.Empty : tst.SaleType1
                                }).ToList();
            }

            totalRecords = total;

            return uiWarehouses;
        }

        public int? GetMaxEntityID()
        {
            if (this.DB.Warehouse.Count() > 0)
                return this.DB.Warehouse.Max(x => x.ID);
            else return null;
        }

        public IList<UIDropdownItem> GetDropdownItems(UISearchDropdownItem uiSearchObj = null)
        {
            IList<UIDropdownItem> uiDropdownItems = new List<UIDropdownItem>();

            List<Expression<Func<Warehouse, bool>>> whereFuncs = new List<Expression<Func<Warehouse, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.IncludeItemValues != null
                    && uiSearchObj.IncludeItemValues.Count > 0)
                    whereFuncs.Add(x => uiSearchObj.IncludeItemValues.Contains(x.ID));

                if (!string.IsNullOrEmpty(uiSearchObj.ItemText))
                    whereFuncs.Add(x => x.Name.Contains(uiSearchObj.ItemText));

                if (uiSearchObj.Extension != null)
                {
                    if (uiSearchObj.Extension.CompanyID > 0)
                        whereFuncs.Add(x => x.CompanyID.Equals(uiSearchObj.Extension.CompanyID));

                    if (uiSearchObj.Extension.SaleTypeID > 0)
                        whereFuncs.Add(x => x.SaleTypeID == uiSearchObj.Extension.SaleTypeID);
                }
            }

            uiDropdownItems = GetList(whereFuncs).Select(x => new UIDropdownItem()
            {
                ItemValue = x.ID,
                ItemText = x.Name
            }).ToList();

            return uiDropdownItems;
        }
    }
}
