﻿namespace ZhongDing.WinService
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
            ((System.ComponentModel.ISupportInitialize)(this.tmCalculateInventory)).BeginInit();
            // 
            // tmCalculateInventory
            // 
            this.tmCalculateInventory.Enabled = true;
            this.tmCalculateInventory.Elapsed += new System.Timers.ElapsedEventHandler(this.tmCalculateInventory_Elapsed);
            // 
            // ZhongDingService
            // 
            this.ServiceName = "ZhongDing MIS Service";
            ((System.ComponentModel.ISupportInitialize)(this.tmCalculateInventory)).EndInit();

        }

        #endregion

        private System.Timers.Timer tmCalculateInventory;
    }
}
