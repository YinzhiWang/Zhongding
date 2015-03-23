using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZhongDing.Business.IRepositories;
using ZhongDing.Business.Repositories;
using ZhongDing.Business.Repositories.Imports;
using ZhongDing.Common;
using ZhongDing.Common.Enums;

namespace ZhongDing.WinService.Lib
{
    /// <summary>
    /// 导入数据服务
    /// </summary>
    public class ImportDataService
    {
        public static void ProcessWork()
        {
            Utility.WriteTrace("导入数据，开始于:" + DateTime.Now);

            IImportDataRepository importDataRepository = new ImportDataRepository();

            importDataRepository.ImportData();

            Utility.WriteTrace("导入数据，结束于:" + DateTime.Now);
        }
    }
}
