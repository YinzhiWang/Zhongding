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
    public class DBContractRepository : BaseRepository<DBContract>, IDBContractRepository
    {
        //public IList<UIDBContract> GetUIList(UISearchDBContract uiSearchObj = null)
        //{
        //    IList<UIDBContract> uiEntities = new List<UIDBContract>();

        //    IQueryable<DBContract> query = null;

        //    List<Expression<Func<DBContract, bool>>> whereFuncs = new List<Expression<Func<DBContract, bool>>>();

        //    if (uiSearchObj != null)
        //    {
        //        if (uiSearchObj.ID > 0)
        //            whereFuncs.Add(x => x.ID.Equals(uiSearchObj.ID));

        //        if (!string.IsNullOrEmpty(uiSearchObj.ContractCode))
        //            whereFuncs.Add(x => x.ContractCode.Contains(uiSearchObj.ContractCode));

        //        if (uiSearchObj.ClientUserID > 0)
        //            whereFuncs.Add(x => x.ClientUserID == uiSearchObj.ClientUserID);

        //        if (!string.IsNullOrEmpty(uiSearchObj.ProductName))
        //            whereFuncs.Add(x => x.Product != null && x.Product.ProductName.Contains(uiSearchObj.ProductName));

        //        if (uiSearchObj.DepartmentID > 0)
        //            whereFuncs.Add(x => x.DepartmentID == uiSearchObj.DepartmentID);
        //    }

        //    query = GetList(whereFuncs);

        //    if (query != null)
        //    {
        //        uiEntities = (from q in query
        //                      join cu in DB.ClientUser on q.ClientUserID equals cu.ID
        //                      join p in DB.Product on q.ProductID equals p.ID
        //                      join ps in DB.ProductSpecification on q.ProductSpecificationID equals ps.ID into tempPS
        //                      from tps in tempPS.DefaultIfEmpty()
        //                      join dp in DB.Department on q.DepartmentID equals dp.ID into tempDP
        //                      from tdp in tempDP.DefaultIfEmpty()
        //                      join icu in DB.Users on q.InChargeUserID equals icu.UserID into tempICU
        //                      from ticu in tempICU.DefaultIfEmpty()
        //                      select new UIDBContract()
        //                      {
        //                          ID = q.ID,
        //                          ContractCode = q.ContractCode,
        //                          ClientUserName = cu.ClientName,
        //                          ProductName = p.ProductName,
        //                          ProductSpecification = tps == null ? string.Empty : tps.Specification,
        //                          DepartmentName = tdp == null ? string.Empty : tdp.DepartmentName,
        //                          IsTempContract = q.IsTempContract,
        //                          ContractExpDate = q.ContractExpDate,
        //                          PromotionExpense = q.PromotionExpense,
        //                          InChargeUser = ticu == null ? string.Empty : ticu.FullName,
        //                          HospitalIDs = q.DBContractHospital.Where(x => x.IsDeleted == false).Select(x => x.HospitalID)
        //                      }).ToList();

        //        foreach (var uiEntity in uiEntities)
        //        {
        //            uiEntity.HospitalNames = string.Join(", ", DB.Hospital.Where(x => uiEntity.HospitalIDs.Contains(x.ID)).Select(x => x.HospitalName).ToList());
        //        }
        //    }

        //    return uiEntities;
        //}

        public IList<UIDBContract> GetUIList(UISearchDBContract uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {
            IList<UIDBContract> uiEntities = new List<UIDBContract>();

            int total = 0;

            IQueryable<DBContract> query = null;

            List<Expression<Func<DBContract, bool>>> whereFuncs = new List<Expression<Func<DBContract, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.ID > 0)
                    whereFuncs.Add(x => x.ID.Equals(uiSearchObj.ID));

                if (!string.IsNullOrEmpty(uiSearchObj.ContractCode))
                    whereFuncs.Add(x => x.ContractCode.Contains(uiSearchObj.ContractCode));

                if (uiSearchObj.ClientUserID > 0)
                    whereFuncs.Add(x => x.ClientUserID == uiSearchObj.ClientUserID);

                if (!string.IsNullOrEmpty(uiSearchObj.ProductName))
                    whereFuncs.Add(x => x.Product != null && x.Product.ProductName.Contains(uiSearchObj.ProductName));

                if (uiSearchObj.DepartmentID > 0)
                    whereFuncs.Add(x => x.DepartmentID == uiSearchObj.DepartmentID);
            }

            query = GetList(pageIndex, pageSize, whereFuncs, out total);

            if (query != null)
            {
                uiEntities = (from q in query
                              join cu in DB.ClientUser on q.ClientUserID equals cu.ID
                              join p in DB.Product on q.ProductID equals p.ID
                              join ps in DB.ProductSpecification on q.ProductSpecificationID equals ps.ID into tempPS
                              from tps in tempPS.DefaultIfEmpty()
                              join dp in DB.Department on q.DepartmentID equals dp.ID into tempDP
                              from tdp in tempDP.DefaultIfEmpty()
                              join icu in DB.Users on q.InChargeUserID equals icu.UserID into tempICU
                              from ticu in tempICU.DefaultIfEmpty()
                              select new UIDBContract()
                              {
                                  ID = q.ID,
                                  ContractCode = q.ContractCode,
                                  ClientUserName = cu.ClientName,
                                  ProductName = p.ProductName,
                                  ProductSpecification = tps == null ? string.Empty : tps.Specification,
                                  DepartmentName = tdp == null ? string.Empty : tdp.DepartmentName,
                                  IsTempContract = q.IsTempContract,
                                  ContractExpDate = q.ContractExpDate,
                                  PromotionExpense = q.PromotionExpense,
                                  InChargeUser = ticu == null ? string.Empty : ticu.FullName,
                                  HospitalCodeIDs = q.DBContractHospital.Where(x => x.IsDeleted == false).Select(x => x.HospitalCodeID)
                              }).ToList();

                foreach (var uiEntity in uiEntities)
                {
                    uiEntity.HospitalNames = string.Join(", ", DB.Hospital.Where(x => uiEntity.HospitalCodeIDs.Contains(x.HospitalCodeID.Value) && x.IsDeleted == false).Select(x => x.HospitalName).ToList());
                }
            }

            totalRecords = total;

            return uiEntities;
        }

        public int? GetMaxEntityID()
        {
            if (this.DB.DBContract.Count() > 0)
                return this.DB.DBContract.Max(x => x.ID);
            else
                return null;
        }
    }
}
