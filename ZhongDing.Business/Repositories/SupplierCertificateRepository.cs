using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ZhongDing.Business.IRepositories;
using ZhongDing.Common;
using ZhongDing.Domain.Models;
using ZhongDing.Domain.UIObjects;
using ZhongDing.Domain.UISearchObjects;

namespace ZhongDing.Business.Repositories
{
    public class SupplierCertificateRepository : BaseRepository<SupplierCertificate>, ISupplierCertificateRepository
    {
        public IList<UISupplierCertificate> GetUIList(UISearchCertificate uiSearchObj = null)
        {
            IList<UISupplierCertificate> uiCertificates = new List<UISupplierCertificate>();

            IQueryable<SupplierCertificate> query = null;

            List<Expression<Func<SupplierCertificate, bool>>> whereFuncs = new List<Expression<Func<SupplierCertificate, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.OwnerEntityID > 0)
                    whereFuncs.Add(x => x.SupplierID == uiSearchObj.OwnerEntityID);

                if (uiSearchObj.OwnerTypeID > 0)
                    whereFuncs.Add(x => x.Certificate != null && x.Certificate.OwnerTypeID == uiSearchObj.OwnerTypeID);
            }

            query = GetList(whereFuncs);

            if (query != null)
            {
                uiCertificates = (from q in query
                                  join c in DB.Certificate on q.CertificateID equals c.ID
                                  join ct in DB.CertificateType on c.CertificateTypeID equals ct.ID
                                  join cu in DB.Users on q.CreatedBy equals cu.UserID into tempCU
                                  from tcu in tempCU.DefaultIfEmpty()
                                  join mu in DB.Users on q.LastModifiedBy equals mu.UserID into tempMU
                                  from tmu in tempMU.DefaultIfEmpty()
                                  //orderby sc.CreatedOn descending
                                  select new UISupplierCertificate()
                                  {
                                      ID = q.ID,
                                      SupplierID = q.SupplierID,
                                      CertificateID = q.CertificateID,
                                      OwnerTypeID = c.OwnerTypeID,
                                      CertificateTypeName = ct.CertificateType1,
                                      IsGotten = c.IsGotten,
                                      GottenDescription = (c.IsGotten.HasValue && c.IsGotten.Value) ? GlobalConst.GOTTEN_DESC_HAVE : GlobalConst.GOTTEN_DESC_NONHAVE,
                                      EffectiveFrom = c.EffectiveFrom,
                                      EffectiveTo = c.EffectiveTo,
                                      IsNeedAlert = c.IsNeedAlert,
                                      AlertBeforeDays = c.AlertBeforeDays,
                                      Comment = c.Comment
                                  }
                                 ).ToList();

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

            return uiCertificates;
        }

        public IList<UISupplierCertificate> GetUIList(UISearchCertificate uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {
            IList<UISupplierCertificate> uiCertificates = new List<UISupplierCertificate>();

            int total = 0;

            IQueryable<SupplierCertificate> query = null;

            List<Expression<Func<SupplierCertificate, bool>>> whereFuncs = new List<Expression<Func<SupplierCertificate, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.OwnerEntityID > 0)
                    whereFuncs.Add(x => x.SupplierID == uiSearchObj.OwnerEntityID);

                if (uiSearchObj.OwnerTypeID > 0)
                    whereFuncs.Add(x => x.Certificate != null && x.Certificate.OwnerTypeID == uiSearchObj.OwnerTypeID);
            }

            query = GetList(pageIndex, pageSize, whereFuncs, out total);

            if (query != null)
            {
                uiCertificates = (from q in query
                                  join c in DB.Certificate on q.CertificateID equals c.ID
                                  join ct in DB.CertificateType on c.CertificateTypeID equals ct.ID
                                  join cu in DB.Users on q.CreatedBy equals cu.UserID into tempCU
                                  from tcu in tempCU.DefaultIfEmpty()
                                  join mu in DB.Users on q.LastModifiedBy equals mu.UserID into tempMU
                                  from tmu in tempMU.DefaultIfEmpty()
                                  //orderby sc.CreatedOn descending
                                  select new UISupplierCertificate()
                                  {
                                      ID = q.ID,
                                      SupplierID = q.SupplierID,
                                      CertificateID = q.CertificateID,
                                      OwnerTypeID = c.OwnerTypeID,
                                      CertificateTypeName = ct.CertificateType1,
                                      IsGotten = c.IsGotten,
                                      GottenDescription = (c.IsGotten.HasValue && c.IsGotten.Value) ? GlobalConst.GOTTEN_DESC_HAVE : GlobalConst.GOTTEN_DESC_NONHAVE,
                                      EffectiveFrom = c.EffectiveFrom,
                                      EffectiveTo = c.EffectiveTo,
                                      IsNeedAlert = c.IsNeedAlert,
                                      AlertBeforeDays = c.AlertBeforeDays,
                                      Comment = c.Comment
                                  }
                                 ).ToList();

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
}
