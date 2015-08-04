using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ZhongDing.Business.IRepositories;
using ZhongDing.Domain.Models;
using ZhongDing.Domain.UIObjects;
using ZhongDing.Common.Extension;

namespace ZhongDing.Business.Repositories
{
    public class HospitalCodeRepository : BaseRepository<HospitalCode>, IHospitalCodeRepository
    {
        public IList<UIHospitalCode> GetUIList(Domain.UISearchObjects.UISearchHospitalCode uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {
            IList<UIHospitalCode> uiEntities = new List<UIHospitalCode>();

            int total = 0;

            IQueryable<HospitalCode> query = null;

            List<Expression<Func<HospitalCode, bool>>> whereFuncs = new List<Expression<Func<HospitalCode, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.ID > 0)
                    whereFuncs.Add(x => x.ID.Equals(uiSearchObj.ID));

                if (uiSearchObj.Name.IsNotNullOrEmpty())
                {
                    whereFuncs.Add(x => x.Hospital.Where(m => m.HospitalName.Contains(uiSearchObj.Name)).Any());
                }
                //if (uiSearchObj.ParentID.BiggerThanZero())
                //    whereFuncs.Add(x => x.ParentID == uiSearchObj.ParentID);

                //if (uiSearchObj.OnlyParent)
                //    whereFuncs.Add(x => x.ParentID == null);


            }

            query = GetList(pageIndex, pageSize, whereFuncs, out total);

            if (query != null)
            {
                uiEntities = (from q in query
                              select new UIHospitalCode()
                              {
                                  ID = q.ID,
                                  Code = q.Code,
                                  NameList = q.Hospital.Where(x => x.IsDeleted == false).Select(x => x.HospitalName),
                                  Comment = q.Comment
                              }).ToList();

                foreach (var entity in uiEntities)
                {
                    entity.Names = string.Join("; ", entity.NameList);
                }
            }

            totalRecords = total;

            return uiEntities;
        }
    }
}
