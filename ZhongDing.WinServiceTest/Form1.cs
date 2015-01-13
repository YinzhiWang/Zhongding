using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZhongDing.Common;
using ZhongDing.Common.Enums;
using ZhongDing.WinService;

namespace ZhongDing.WinServiceTest
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            //Initialize Logging Logger
            IConfigurationSource configurationSource = ConfigurationSourceFactory.Create();
            LogWriterFactory logWriterFactory = new LogWriterFactory(configurationSource);
            Microsoft.Practices.EnterpriseLibrary.Logging.Logger.SetLogWriter(logWriterFactory.Create());

            InitializeComponent();
        }

        private void btnCalculateInventory_Click(object sender, EventArgs e)
        {
            CalculateInventoryService.ProcessWork();
        }

    }
}
