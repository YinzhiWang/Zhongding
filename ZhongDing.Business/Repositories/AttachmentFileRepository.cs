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
    public class AttachmentFileRepository : BaseRepository<AttachmentFile>, IAttachmentFileRepository
    {
        public IList<Domain.UIObjects.UIAttachmentFile> GetUIList(Domain.UISearchObjects.UISearchAttachmentFile uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {
            IList<UIAttachmentFile> uiEntites = new List<UIAttachmentFile>();
            int total = 0;

            IQueryable<AttachmentFile> query = null;

            List<Expression<Func<AttachmentFile, bool>>> whereFuncs = new List<Expression<Func<AttachmentFile, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.AttachmentHostTableID.BiggerThanZero())
                {
                    whereFuncs.Add(x => x.AttachmentHostTableID == uiSearchObj.AttachmentHostTableID);
                }
                if (uiSearchObj.AttachmentTypeID == uiSearchObj.AttachmentTypeID) ;
            }

            query = GetList(pageIndex, pageSize, whereFuncs, out total);

            if (query != null)
            {
                uiEntites = (from q in query
                             select new UIAttachmentFile()
                             {
                                 ID = q.ID,
                                 Comment = q.Comment,
                                 AttachmentHostTableID = q.AttachmentHostTableID,
                                 AttachmenTypeID = q.AttachmenTypeID,
                                 FileName = q.FileName,
                                 FilePath = q.FilePath
                             }).ToList();
            }

            totalRecords = total;

            return uiEntites;
        }






    }
}
