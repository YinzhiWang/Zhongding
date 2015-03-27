using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZhongDing.Business.IRepositories;
using ZhongDing.Domain.Models;
using ZhongDing.Domain.UIObjects;

namespace ZhongDing.Business.Repositories
{
    public class CertificateTypeRepository : BaseRepository<CertificateType>, ICertificateTypeRepository
    {
        public IList<UICertificateType> GetUIList(int? ownerTypeID)
        {
            IList<UICertificateType> uiCertificateTypes = new List<UICertificateType>();

            uiCertificateTypes = (from ct in DB.CertificateType
                                  where ct.IsDeleted == false 
                                  && ct.OwnerTypeID == ownerTypeID
                                  select new UICertificateType()
                                  {
                                      ID = ct.ID,
                                      CertificateTypeName = ct.CertificateType1,
                                      OwnerTypeID = ct.OwnerTypeID
                                  }).ToList();

            return uiCertificateTypes;
        }
    }
}
