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
    public class DBContractHospitalRepository : BaseRepository<DBContractHospital>, IDBContractHospitalRepository
    {
        public IList<UIDBContractHospital> GetUIList(UISearchHospital uiSearchObj = null)
        {
            IList<UIDBContractHospital> uiEntities = new List<UIDBContractHospital>();

            IQueryable<DBContractHospital> query = null;

            List<Expression<Func<DBContractHospital, bool>>> whereFuncs = new List<Expression<Func<DBContractHospital, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.ID > 0)
                    whereFuncs.Add(x => x.ID.Equals(uiSearchObj.ID));

                if (uiSearchObj.DBContractID > 0)
                    whereFuncs.Add(x => x.DBContractID.Equals(uiSearchObj.DBContractID));

                if (!string.IsNullOrEmpty(uiSearchObj.HospitalName))
                    whereFuncs.Add(x => x.Hospital != null && x.Hospital.HospitalName.Contains(uiSearchObj.HospitalName));
            }

            query = GetList(whereFuncs);

            if (query != null)
            {
                uiEntities = (from q in query
                              join h in DB.Hospital on q.HospitalID equals h.ID
                              select new UIDBContractHospital()
                              {
                                  ID = q.ID,
                                  DBContractID = q.DBContractID,
                                  HospitalID = q.HospitalID,
                                  HospitalName = h.HospitalName
                              }).ToList();
            }

            return uiEntities;
        }

        public IList<UIDBContractHospital> GetUIList(UISearchHospital uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {
            IList<UIDBContractHospital> uiEntities = new List<UIDBContractHospital>();

            int total = 0;

            IQueryable<DBContractHospital> query = null;

            List<Expression<Func<DBContractHospital, bool>>> whereFuncs = new List<Expression<Func<DBContractHospital, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.ID > 0)
                    whereFuncs.Add(x => x.ID.Equals(uiSearchObj.ID));

                if (uiSearchObj.DBContractID > 0)
                    whereFuncs.Add(x => x.DBContractID.Equals(uiSearchObj.DBContractID));

                if (!string.IsNullOrEmpty(uiSearchObj.HospitalName))
                    whereFuncs.Add(x => x.Hospital != null && x.Hospital.HospitalName.Contains(uiSearchObj.HospitalName));
            }

            query = GetList(pageIndex, pageSize, whereFuncs, out total);

            if (query != null)
            {
                uiEntities = (from q in query
                              join h in DB.Hospital on q.HospitalID equals h.ID
                              select new UIDBContractHospital()
                              {
                                  ID = q.ID,
                                  DBContractID = q.DBContractID,
                                  HospitalID = q.HospitalID,
                                  HospitalName = h.HospitalName
                              }).ToList();
            }

            totalRecords = total;

            return uiEntities;
        }
    }
}
