using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UISearchObjects
{
    public class UISearchAttachmentFile : UISearchBase
    {
        public int AttachmentTypeID { get; set; }

        public int? AttachmentHostTableID { get; set; }
    }
}
