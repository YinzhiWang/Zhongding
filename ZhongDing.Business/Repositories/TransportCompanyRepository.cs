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
    public class TransportCompanyRepository : BaseRepository<TransportCompany>, ITransportCompanyRepository
    {
         
        public IList<UITransportCompany> GetUIList(UISearchTransportCompany uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {
            IList<UITransportCompany> uiWarehouses = new List<UITransportCompany>();
            int total = 0;

            IQueryable<TransportCompany> query = null;

            List<Expression<Func<TransportCompany, bool>>> whereFuncs = new List<Expression<Func<TransportCompany, bool>>>();

            if (uiSearchObj != null)
            {
                if (!string.IsNullOrEmpty(uiSearchObj.CompanyName))
                    whereFuncs.Add(x => x.CompanyName.Contains(uiSearchObj.CompanyName));
            }

            query = GetList(pageIndex, pageSize, whereFuncs, out total);

            if (query != null)
            {
                uiWarehouses = (from q in query
                                select new UITransportCompany()
                                {
                                    ID = q.ID,
                                    CompanyAddress = q.CompanyAddress,
                                    CompanyName = q.CompanyName,
                                    Driver = q.Driver,
                                    DriverTelephone = q.DriverTelephone,
                                    Telephone = q.Telephone,
                                    Remark = q.Remark
                                }).ToList();
            }

            totalRecords = total;

            return uiWarehouses;
        }


    }
}
