using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UIObjects
{
    public class UIAttachmentFile : UIBase
    {
        public int AttachmentHostTableID { get; set; }
        public int AttachmenTypeID { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string Comment { get; set; }
    }
}
