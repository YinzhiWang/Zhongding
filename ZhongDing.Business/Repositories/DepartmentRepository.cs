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
    public class DepartmentRepository : BaseRepository<Department>, IDepartmentRepository
    {

        public IList<UIDepartment> GetUIList(UISearchDepartment uiSearchObj = null)
        {
            IList<UIDepartment> uiEntities = new List<UIDepartment>();

            IQueryable<Department> query = null;

            List<Expression<Func<Department, bool>>> whereFuncs = new List<Expression<Func<Department, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.ID > 0)
                    whereFuncs.Add(x => x.ID.Equals(uiSearchObj.ID));

                if (!string.IsNullOrEmpty(uiSearchObj.DepartmentName))
                    whereFuncs.Add(x => x.DepartmentName.Contains(uiSearchObj.DepartmentName));

                if (uiSearchObj.DepartmentTypeID > 0)
                    whereFuncs.Add(x => x.DepartmentTypeID == uiSearchObj.DepartmentTypeID);
            }

            if (query != null)
            {
                uiEntities = (from q in query
                              join du in DB.Users on q.DirectorUserID equals du.UserID into tempDU
                              from tdu in tempDU.DefaultIfEmpty()
                              join dd in DB.DeptDistrict on q.DeptDistrictID equals dd.ID into tempDD
                              from tdd in tempDD.DefaultIfEmpty()
                              select new UIDepartment()
                              {
                                  ID = q.ID,
                                  DepartmentName = q.DepartmentName,
                                  DepartmentType = q.DepartmentTypeID == (int)EDepartmentType.BaseMedicine
                                  ? GlobalConst.DepartmentTypes.BASE_MEDICINE
                                  : (q.DepartmentTypeID == (int)EDepartmentType.BusinessMedicine
                                        ? GlobalConst.DepartmentTypes.BUSINESS_MEDICINE
                                        : GlobalConst.DepartmentTypes.OTHER),
                                  DeptDistrict = tdd == null ? string.Empty : tdd.DistrictName,
                                  DirectorUserName = tdu == null ? string.Empty : tdu.FullName
                              }).ToList();
            }

            return uiEntities;
        }

        public IList<UIDepartment> GetUIList(UISearchDepartment uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {
            IList<UIDepartment> uiEntities = new List<UIDepartment>();

            int total = 0;

            IQueryable<Department> query = null;

            List<Expression<Func<Department, bool>>> whereFuncs = new List<Expression<Func<Department, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.ID > 0)
                    whereFuncs.Add(x => x.ID.Equals(uiSearchObj.ID));

                if (!string.IsNullOrEmpty(uiSearchObj.DepartmentName))
                    whereFuncs.Add(x => x.DepartmentName.Contains(uiSearchObj.DepartmentName));

                if (uiSearchObj.DepartmentTypeID > 0)
                    whereFuncs.Add(x => x.DepartmentTypeID == uiSearchObj.DepartmentTypeID);
            }

            query = GetList(pageIndex, pageSize, whereFuncs, out total);

            if (query != null)
            {
                uiEntities = (from q in query
                              join du in DB.Users on q.DirectorUserID equals du.UserID into tempDU
                              from tdu in tempDU.DefaultIfEmpty()
                              join dd in DB.DeptDistrict on q.DeptDistrictID equals dd.ID into tempDD
                              from tdd in tempDD.DefaultIfEmpty()
                              select new UIDepartment()
                              {
                                  ID = q.ID,
                                  DepartmentName = q.DepartmentName,
                                  DepartmentType = q.DepartmentTypeID == (int)EDepartmentType.BaseMedicine
                                  ? GlobalConst.DepartmentTypes.BASE_MEDICINE 
                                  : (q.DepartmentTypeID == (int)EDepartmentType.BusinessMedicine 
                                        ? GlobalConst.DepartmentTypes.BUSINESS_MEDICINE 
                                        : GlobalConst.DepartmentTypes.OTHER),
                                  DeptDistrict = tdd == null ? string.Empty : tdd.DistrictName,
                                  DirectorUserName = tdu == null ? string.Empty : tdu.FullName
                              }).ToList();
            }

            totalRecords = total;

            return uiEntities;
        }

        public IList<UIDropdownItem> GetDropdownItems(UISearchDropdownItem uiSearchObj = null)
        {
            IList<UIDropdownItem> uiDropdownItems = new List<UIDropdownItem>();

            List<Expression<Func<Department, bool>>> whereFuncs = new List<Expression<Func<Department, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.IncludeItemValues != null
                    && uiSearchObj.IncludeItemValues.Count > 0)
                    whereFuncs.Add(x => uiSearchObj.IncludeItemValues.Contains(x.ID));

                if (!string.IsNullOrEmpty(uiSearchObj.ItemText))
                    whereFuncs.Add(x => x.DepartmentName.Contains(uiSearchObj.ItemText));

                if (uiSearchObj.Extension != null && uiSearchObj.Extension.CurrentUserID > 0)
                    whereFuncs.Add(x => x.Users1.Any(y => y.UserID == uiSearchObj.Extension.CurrentUserID));
            }

            uiDropdownItems = GetList(whereFuncs).Select(x => new UIDropdownItem()
            {
                ItemValue = x.ID,
                ItemText = x.DepartmentName
            }).ToList();

            return uiDropdownItems;
        }

        public bool IsDeptRelatedWithProduct(int departmentID, int productID)
        {
            bool isRelated = false;

            //是否在部门产品线里含有该产品
            bool isDeptMarketProduct = DB.Users.Any(x => x.IsDeleted == false
                && x.DepartmentID == departmentID
                && x.DeptMarketDivision.Any(y =>
                    y.DeptMarketProduct.Any(z => z.IsDeleted == false && z.ProductID == productID)));

            //是否在部门考核产品含有该产品
            bool isDeptProductEvaluation = DB.Department.Any(x => x.IsDeleted == false && x.ID == departmentID
                && x.DeptProductEvaluation.Any(y => y.IsDeleted == false && y.ProductID == productID));

            if (isDeptMarketProduct || isDeptProductEvaluation)
                isRelated = true;

            return isRelated;
        }
    }
}
