﻿using System;
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
    public class DeptProductRecordRepository : BaseRepository<DepartmentProductRecord>, IDeptProductRecordRepository
    {
        public IList<UIDeptProductRecord> GetUIList(UISearchDeptProductRecord uiSearchObj = null)
        {
            IList<UIDeptProductRecord> uiEntities = new List<UIDeptProductRecord>();

            IQueryable<DepartmentProductRecord> query = null;

            List<Expression<Func<DepartmentProductRecord, bool>>> whereFuncs = new List<Expression<Func<DepartmentProductRecord, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.ID > 0)
                    whereFuncs.Add(x => x.ID.Equals(uiSearchObj.ID));

                if (uiSearchObj.DepartmentID > 0)
                    whereFuncs.Add(x => x.DepartmentID.Equals(uiSearchObj.DepartmentID));

                if (uiSearchObj.ProductID > 0)
                    whereFuncs.Add(x => x.ProductID.Equals(uiSearchObj.ProductID));
            }

            query = GetList(whereFuncs);

            if (query != null)
            {
                uiEntities = (from q in query
                              join d in DB.Department on q.DepartmentID equals d.ID
                              join p in DB.Product on q.ProductID equals p.ID
                              select new UIDeptProductRecord()
                              {
                                  ID = q.ID,
                                  Year = q.Year,
                                  Task = q.Task,
                                  Actual = q.Actual
                              }).ToList();
            }

            return uiEntities;
        }

        public IList<UIDeptProductRecord> GetUIList(UISearchDeptProductRecord uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {
            IList<UIDeptProductRecord> uiEntities = new List<UIDeptProductRecord>();

            int total = 0;

            IQueryable<DepartmentProductRecord> query = null;

            List<Expression<Func<DepartmentProductRecord, bool>>> whereFuncs = new List<Expression<Func<DepartmentProductRecord, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.ID > 0)
                    whereFuncs.Add(x => x.ID.Equals(uiSearchObj.ID));

                if (uiSearchObj.DepartmentID > 0)
                    whereFuncs.Add(x => x.DepartmentID.Equals(uiSearchObj.DepartmentID));

                if (uiSearchObj.ProductID > 0)
                    whereFuncs.Add(x => x.ProductID.Equals(uiSearchObj.ProductID));
            }

            query = GetList(pageIndex, pageSize, whereFuncs, out total);

            if (query != null)
            {
                uiEntities = (from q in query
                              join d in DB.Department on q.DepartmentID equals d.ID
                              join p in DB.Product on q.ProductID equals p.ID
                              select new UIDeptProductRecord()
                              {
                                  ID = q.ID,
                                  Year = q.Year,
                                  Task = q.Task,
                                  Actual = q.Actual
                              }).ToList();
            }

            totalRecords = total;

            return uiEntities;
        }
    }
}
