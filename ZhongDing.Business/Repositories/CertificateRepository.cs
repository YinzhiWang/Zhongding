using System;
using System.Collections.Generic;
using System.Linq;
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
    public class CertificateRepository : BaseRepository<Certificate>, ICertificateRepository
    {
        public IList<UICertificate> GetUIList(UISearchCertificate uiSearchObj)
        {
            IList<UICertificate> uiCertificates = new List<UICertificate>();

            if (uiSearchObj != null)
            {
                IQueryable<TempCertificate> query = null;

                if (uiSearchObj.OwnerTypeID > 0)
                {
                    EOwnerType ownerType = (EOwnerType)uiSearchObj.OwnerTypeID;

                    switch (ownerType)
                    {
                        case EOwnerType.Company:
                            break;

                        case EOwnerType.Producer:
                        case EOwnerType.Supplier:
                            query = DB.SupplierCertificate
                                .Where(x => x.IsDeleted == false && x.SupplierID == uiSearchObj.OwnerEntityID
                                    && x.Certificate != null && x.Certificate.OwnerTypeID == uiSearchObj.OwnerTypeID)
                                .Select(x => new TempCertificate { OwnerEntityID = x.ID, CertificateID = x.CertificateID });
                            break;

                        case EOwnerType.Client:
                            query = DB.ClientCompanyCertificate
                                .Where(x => x.IsDeleted == false && x.ClientCompanyID == uiSearchObj.OwnerEntityID)
                                .Select(x => new TempCertificate { OwnerEntityID = x.ID, CertificateID = x.CertificateID });
                            break;

                        case EOwnerType.Product:
                            query = DB.ProductCertificate
                                .Where(x => x.IsDeleted == false && x.ProductID == uiSearchObj.OwnerEntityID)
                                .Select(x => new TempCertificate { OwnerEntityID = x.ID, CertificateID = x.CertificateID });
                            break;
                    }
                }

                if (query != null)
                {
                    uiCertificates = (from q in query
                                      join c in DB.Certificate on q.CertificateID equals c.ID
                                      join ct in DB.CertificateType on c.CertificateTypeID equals ct.ID
                                      where c.IsDeleted == false && c.OwnerTypeID == uiSearchObj.OwnerTypeID
                                      select new UICertificate()
                                      {
                                          CertificateID = q.CertificateID.Value,
                                          OwnerEntityID = q.OwnerEntityID,
                                          OwnerTypeID = c.OwnerTypeID,
                                          CertificateTypeName = ct.CertificateType1,
                                          IsGotten = c.IsGotten,
                                          GottenDescription = (c.IsGotten.HasValue && c.IsGotten.Value) ? GlobalConst.GOTTEN_DESC_HAVE : GlobalConst.GOTTEN_DESC_NONHAVE,
                                          EffectiveFrom = c.EffectiveFrom,
                                          EffectiveTo = c.EffectiveTo,
                                          IsNeedAlert = c.IsNeedAlert,
                                          AlertBeforeDays = c.AlertBeforeDays,
                                          Comment = c.Comment
                                      }).ToList();

                    foreach (var uiCertificate in uiCertificates)
                    {
                        if (uiCertificate.EffectiveFrom.HasValue
                            && uiCertificate.EffectiveTo.HasValue)
                        {
                            uiCertificate.EffectiveDateDescription = uiCertificate.EffectiveFrom.ToString("yyyy/MM/dd")
                                + " - " + uiCertificate.EffectiveTo.ToString("yyyy/MM/dd");
                        }
                    }
                }
            }


            return uiCertificates;
        }

        public IList<UICertificate> GetUIList(UISearchCertificate uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {
            IList<UICertificate> uiCertificates = new List<UICertificate>();

            int total = 0;

            IQueryable<TempCertificate> query = null;

            if (uiSearchObj != null)
            {
                if (uiSearchObj.OwnerTypeID > 0)
                {
                    EOwnerType ownerType = (EOwnerType)uiSearchObj.OwnerTypeID;

                    switch (ownerType)
                    {
                        case EOwnerType.Company:
                            break;

                        case EOwnerType.Producer:
                        case EOwnerType.Supplier:
                            query = DB.SupplierCertificate
                                .Where(x => x.IsDeleted == false && x.SupplierID == uiSearchObj.OwnerEntityID
                                    && x.Certificate != null && x.Certificate.OwnerTypeID == uiSearchObj.OwnerTypeID)
                                .Select(x => new TempCertificate { OwnerEntityID = x.ID, CertificateID = x.CertificateID });
                            break;

                        case EOwnerType.Client:
                            query = DB.ClientCompanyCertificate
                                .Where(x => x.IsDeleted == false && x.ClientCompanyID == uiSearchObj.OwnerEntityID)
                                .Select(x => new TempCertificate { OwnerEntityID = x.ID, CertificateID = x.CertificateID });
                            break;

                        case EOwnerType.Product:
                            query = DB.ProductCertificate
                                .Where(x => x.IsDeleted == false && x.ProductID == uiSearchObj.OwnerEntityID)
                                .Select(x => new TempCertificate { OwnerEntityID = x.ID, CertificateID = x.CertificateID });
                            break;
                    }
                }
            }

            if (query != null)
            {
                total = query.Count();

                uiCertificates = (from q in query
                                  join c in DB.Certificate on q.CertificateID equals c.ID
                                  join ct in DB.CertificateType on c.CertificateTypeID equals ct.ID
                                  where c.IsDeleted == false && c.OwnerTypeID == uiSearchObj.OwnerTypeID
                                  select new UICertificate()
                                  {
                                      CertificateID = q.CertificateID.Value,
                                      OwnerEntityID = q.OwnerEntityID,
                                      OwnerTypeID = c.OwnerTypeID,
                                      CertificateTypeName = ct.CertificateType1,
                                      IsGotten = c.IsGotten,
                                      GottenDescription = (c.IsGotten.HasValue && c.IsGotten.Value) ? GlobalConst.GOTTEN_DESC_HAVE : GlobalConst.GOTTEN_DESC_NONHAVE,
                                      EffectiveFrom = c.EffectiveFrom,
                                      EffectiveTo = c.EffectiveTo,
                                      IsNeedAlert = c.IsNeedAlert,
                                      AlertBeforeDays = c.AlertBeforeDays,
                                      Comment = c.Comment
                                  }).AsQueryable().OrderBy(x => x.CertificateID).Skip(pageSize * pageIndex).Take(pageSize).ToList();

                foreach (var uiCertificate in uiCertificates)
                {
                    if (uiCertificate.EffectiveFrom.HasValue
                        && uiCertificate.EffectiveTo.HasValue)
                    {
                        uiCertificate.EffectiveDateDescription = uiCertificate.EffectiveFrom.ToString("yyyy/MM/dd")
                            + " - " + uiCertificate.EffectiveTo.ToString("yyyy/MM/dd");
                    }
                }
            }

            totalRecords = total;

            return uiCertificates;
        }
    }

    /// <summary>
    /// 类：临时Certificate
    /// </summary>
    public class TempCertificate
    {
        public int OwnerEntityID { get; set; }

        public int? CertificateID { get; set; }
    }
}
