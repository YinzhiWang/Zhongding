using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ZhongDing.Business.IRepositories;
using ZhongDing.Common;
using ZhongDing.Common.Enums;
using ZhongDing.Domain.Models;
using ZhongDing.Domain.UIObjects;
using ZhongDing.Domain.UISearchObjects;

namespace ZhongDing.Business.Repositories
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        public IList<UIProduct> GetUIList(UISearchProduct uiSearchObj = null)
        {
            IList<UIProduct> uiEntities = new List<UIProduct>();

            IQueryable<Product> query = null;

            List<Expression<Func<Product, bool>>> whereFuncs = new List<Expression<Func<Product, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.ID > 0)
                    whereFuncs.Add(x => x.ID.Equals(uiSearchObj.ID));

                if (uiSearchObj.CompanyID > 0)
                    whereFuncs.Add(x => x.CompanyID == uiSearchObj.CompanyID);

                if (!string.IsNullOrEmpty(uiSearchObj.ProductCode))
                    whereFuncs.Add(x => x.ProductCode.Contains(uiSearchObj.ProductCode));

                if (!string.IsNullOrEmpty(uiSearchObj.ProductName))
                    whereFuncs.Add(x => x.ProductName.Contains(uiSearchObj.ProductName));
            }

            query = GetList(whereFuncs);

            if (query != null)
            {
                uiEntities = (from q in query
                              join c in DB.ProductCategory on q.CategoryID equals c.ID into tempC
                              from tc in tempC.DefaultIfEmpty()
                              join s in DB.Supplier on q.SupplierID equals s.ID into tempS
                              from ts in tempS.DefaultIfEmpty()
                              join d in DB.Department on q.DepartmentID equals d.ID into tempD
                              from td in tempD.DefaultIfEmpty()
                              select new UIProduct()
                              {
                                  ID = q.ID,
                                  ProductCode = q.ProductCode,
                                  ProductName = q.ProductName,
                                  ProductCategoryName = tc == null ? string.Empty : tc.CategoryName,
                                  SupplierName = ts == null ? string.Empty : ts.SupplierName,
                                  DepartmentName = td == null ? string.Empty : td.DepartmentName

                              }).ToList();
            }

            return uiEntities;
        }

        public IList<UIProduct> GetUIList(UISearchProduct uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {
            IList<UIProduct> uiEntities = new List<UIProduct>();

            int total = 0;

            IQueryable<Product> query = null;

            List<Expression<Func<Product, bool>>> whereFuncs = new List<Expression<Func<Product, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.ID > 0)
                    whereFuncs.Add(x => x.ID.Equals(uiSearchObj.ID));

                if (uiSearchObj.CompanyID > 0)
                    whereFuncs.Add(x => x.CompanyID == uiSearchObj.CompanyID);

                if (!string.IsNullOrEmpty(uiSearchObj.ProductCode))
                    whereFuncs.Add(x => x.ProductCode.Contains(uiSearchObj.ProductCode));

                if (!string.IsNullOrEmpty(uiSearchObj.ProductName))
                    whereFuncs.Add(x => x.ProductName.Contains(uiSearchObj.ProductName));
            }

            query = GetList(pageIndex, pageSize, whereFuncs, out total);

            if (query != null)
            {
                uiEntities = (from q in query
                              join c in DB.ProductCategory on q.CategoryID equals c.ID into tempC
                              from tc in tempC.DefaultIfEmpty()
                              join s in DB.Supplier on q.SupplierID equals s.ID into tempS
                              from ts in tempS.DefaultIfEmpty()
                              join d in DB.Department on q.DepartmentID equals d.ID into tempD
                              from td in tempD.DefaultIfEmpty()
                              select new UIProduct()
                              {
                                  ID = q.ID,
                                  ProductCode = q.ProductCode,
                                  ProductName = q.ProductName,
                                  ProductCategoryName = tc == null ? string.Empty : tc.CategoryName,
                                  SupplierName = ts == null ? string.Empty : ts.SupplierName,
                                  DepartmentName = td == null ? string.Empty : td.DepartmentName

                              }).ToList();
            }

            totalRecords = total;

            return uiEntities;
        }

        public int? GetMaxEntityID()
        {
            if (this.DB.Product.Count() > 0)
                return this.DB.Product.Max(x => x.ID);
            else
                return null;
        }

        public IList<UIDropdownItem> GetDropdownItems(UISearchDropdownItem uiSearchObj = null)
        {
            IList<UIDropdownItem> uiDropdownItems = new List<UIDropdownItem>();

            List<Expression<Func<Product, bool>>> whereFuncs = new List<Expression<Func<Product, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.IncludeItemValues != null
                    && uiSearchObj.IncludeItemValues.Count > 0)
                    whereFuncs.Add(x => uiSearchObj.IncludeItemValues.Contains(x.ID));

                if (uiSearchObj.ExcludeItemValues != null
                    && uiSearchObj.ExcludeItemValues.Count > 0)
                    whereFuncs.Add(x => !uiSearchObj.ExcludeItemValues.Contains(x.ID));

                if (!string.IsNullOrEmpty(uiSearchObj.ItemText))
                    whereFuncs.Add(x => x.ProductName.Contains(uiSearchObj.ItemText));

                if (uiSearchObj.Extension != null)
                {
                    if (uiSearchObj.Extension.DepartmentID > 0)
                    {
                        var department = DB.Department.Where(x => x.ID == uiSearchObj.Extension.DepartmentID).FirstOrDefault();

                        if (department != null)
                        {
                            //部门性质是基药，可以看到类型是基药和混合的货品
                            //部门性质是招商，只能看到该部门对应的货品；
                            switch (department.DepartmentTypeID)
                            {
                                case (int)EDepartmentType.BaseMedicine:
                                    var productCategoryIDs = new List<int>();
                                    productCategoryIDs.Add((int)EProductCategory.BaseMedicine);
                                    productCategoryIDs.Add((int)EProductCategory.MixedMedicine);

                                    uiSearchObj.Extension.ProductCategoryIDs = productCategoryIDs;
                                    break;

                                case (int)EDepartmentType.BusinessMedicine:
                                    whereFuncs.Add(x => x.DepartmentID == uiSearchObj.Extension.DepartmentID);
                                    break;
                            }
                        }
                    }

                    if (uiSearchObj.Extension.ProductCategoryID > 0)
                        whereFuncs.Add(x => x.CategoryID == uiSearchObj.Extension.ProductCategoryID);

                    if (uiSearchObj.Extension.ProductCategoryIDs != null
                        && uiSearchObj.Extension.ProductCategoryIDs.Count > 0)
                        whereFuncs.Add(x => uiSearchObj.Extension.ProductCategoryIDs
                            .Contains(x.CategoryID.HasValue ? x.CategoryID.Value : GlobalConst.INVALID_INT));

                    if (uiSearchObj.Extension.SupplierID > 0)
                        whereFuncs.Add(x => x.SupplierID == uiSearchObj.Extension.SupplierID);

                    if (uiSearchObj.Extension.CompanyID > 0)
                        whereFuncs.Add(x => x.CompanyID == uiSearchObj.Extension.CompanyID);
                }
            }

            var query = GetList(whereFuncs);

            if (query != null)
            {
                uiDropdownItems = (from q in query
                                   join s in DB.Supplier on q.SupplierID equals s.ID into tempS
                                   from ts in tempS.DefaultIfEmpty()
                                   select new UIDropdownItem
                                   {
                                       ItemValue = q.ID,
                                       ItemText = q.ProductName,
                                       Extension = new { FactoryName = ts == null ? string.Empty : ts.FactoryName }
                                   }).ToList();
            }

            return uiDropdownItems;
        }
    }
}
