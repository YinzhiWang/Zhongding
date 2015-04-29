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
    public class SupplierCautionMoneyRepository : BaseRepository<SupplierCautionMoney>, ISupplierCautionMoneyRepository
    {
        public IList<Domain.UIObjects.UISupplierCautionMoney> GetSupplierCautionMoneyApplyUIList(Domain.UISearchObjects.UISearchSupplierCautionMoney uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {
            IList<UISupplierCautionMoney> uiEntitys = new List<UISupplierCautionMoney>();
            int total = 0;

            IQueryable<SupplierCautionMoney> query = null;

            List<Expression<Func<SupplierCautionMoney, bool>>> whereFuncs = new List<Expression<Func<SupplierCautionMoney, bool>>>();

            if (uiSearchObj != null)
            {

            }

            query = GetList(pageIndex, pageSize, whereFuncs, out total);

            if (query != null)
            {
                uiEntitys = (from q in query
                             join cautionMoneyType in DB.CautionMoneyType on q.CautionMoneyTypeID equals cautionMoneyType.ID
                             join supplier in DB.Supplier on q.SupplierID equals supplier.ID
                             join product in DB.Product on q.ProductID equals product.ID
                             join user in DB.Users on q.CreatedBy equals user.UserID
                             select new UISupplierCautionMoney()
                             {
                                 ID = q.ID,
                                 ApplyDate = q.ApplyDate,
                                 CautionMoneyTypeName = cautionMoneyType.Name,
                                 EndDate = q.EndDate,
                                 PaymentCautionMoney = 0,
                                 ProductName = product.ProductName,
                                 Remark = q.Remark,
                                 IsStop = q.IsStop,



                                 CreatedByUserName = user.UserName
                             }).ToList();
            }

            totalRecords = total;
            return uiEntitys;
        }
    }
}
