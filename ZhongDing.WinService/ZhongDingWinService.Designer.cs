namespace ZhongDing.WinService
{
    partial class ZhongDingWinService
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tmCalculateInventory = new System.Timers.Timer();
            this.tmImportDCFlowData = new System.Timers.Timer();
            this.tmSettleDBClientBonus = new System.Timers.Timer();
            ((System.ComponentModel.ISupportInitialize)(this.tmCalculateInventory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tmImportDCFlowData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tmSettleDBClientBonus)).BeginInit();
            // 
            // tmCalculateInventory
            // 
            this.tmCalculateInventory.Enabled = true;
            this.tmCalculateInventory.Elapsed += new System.Timers.ElapsedEventHandler(this.tmCalculateInventory_Elapsed);
            // 
            // tmImportDCFlowData
            // 
            this.tmImportDCFlowData.Enabled = true;
            this.tmImportDCFlowData.Elapsed += new System.Timers.ElapsedEventHandler(this.tmImportDCFlowData_Elapsed);
            // 
            // tmSettleDBClientBonus
            // 
            this.tmSettleDBClientBonus.Enabled = true;
            this.tmSettleDBClientBonus.Elapsed += new System.Timers.ElapsedEventHandler(this.tmSettleDBClientBonus_Elapsed);
            // 
            // ZhongDingWinService
            // 
            this.ServiceName = "ZhongDing MIS Service";
            ((System.ComponentModel.ISupportInitialize)(this.tmCalculateInventory)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tmImportDCFlowData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tmSettleDBClientBonus)).EndInit();

        }

        #endregion

        private System.Timers.Timer tmCalculateInventory;
        private System.Timers.Timer tmImportDCFlowData;
        private System.Timers.Timer tmSettleDBClientBonus;
    }
}
