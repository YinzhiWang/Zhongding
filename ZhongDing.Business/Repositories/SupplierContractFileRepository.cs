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
    public class SupplierContractFileRepository : BaseRepository<SupplierContractFile>, ISupplierContractFileRepository
    {
        public IList<UISupplierContractFile> GetUIList(UISearchSupplierContractFile uiSearchObj = null)
        {
            IList<UISupplierContractFile> uiContractFiles = new List<UISupplierContractFile>();

            IQueryable<SupplierContractFile> query = null;

            List<Expression<Func<SupplierContractFile, bool>>> whereFuncs = new List<Expression<Func<SupplierContractFile, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.ContractID > 0)
                    whereFuncs.Add(x => x.ContractID == uiSearchObj.ContractID);

                if (!string.IsNullOrEmpty(uiSearchObj.FileName))
                    whereFuncs.Add(x => x.FileName.Contains(uiSearchObj.FileName));
            }

            query = GetList(whereFuncs);

            if (query != null)
            {
                uiContractFiles = (from q in query
                                   select new UISupplierContractFile()
                                   {
                                       ID = q.ID,
                                       FileName = q.FileName,
                                       FilePath = q.FilePath,
                                       Comment = q.Comment
                                   }).ToList();
            }

            return uiContractFiles;
        }

        public IList<UISupplierContractFile> GetUIList(UISearchSupplierContractFile uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {
            IList<UISupplierContractFile> uiContractFiles = new List<UISupplierContractFile>();

            int total;

            IQueryable<SupplierContractFile> query = null;

            List<Expression<Func<SupplierContractFile, bool>>> whereFuncs = new List<Expression<Func<SupplierContractFile, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.ContractID > 0)
                    whereFuncs.Add(x => x.ContractID == uiSearchObj.ContractID);

                if (!string.IsNullOrEmpty(uiSearchObj.FileName))
                    whereFuncs.Add(x => x.FileName.Contains(uiSearchObj.FileName));
            }

            query = GetList(pageIndex, pageSize, whereFuncs, out total);

            if (query != null)
            {
                uiContractFiles = (from q in query
                                   select new UISupplierContractFile()
                                   {
                                       ID = q.ID,
                                       FileName = q.FileName,
                                       FilePath = q.FilePath,
                                       Comment = q.Comment
                                   }).ToList();
            }

            totalRecords = total;


            return uiContractFiles;
        }
    }
}
