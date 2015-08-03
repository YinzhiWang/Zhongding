namespace ZhongDing.WinServiceTest
{
    partial class Form1
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnCalculateInventory = new System.Windows.Forms.Button();
            this.btnImportData = new System.Windows.Forms.Button();
            this.btnClientSettleBonus = new System.Windows.Forms.Button();
            this.btn计算银行卡余额 = new System.Windows.Forms.Button();
            this.btn权限判断测试 = new System.Windows.Forms.Button();
            this.btn现金流计算 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnCalculateInventory
            // 
            this.btnCalculateInventory.Location = new System.Drawing.Point(12, 30);
            this.btnCalculateInventory.Name = "btnCalculateInventory";
            this.btnCalculateInventory.Size = new System.Drawing.Size(152, 25);
            this.btnCalculateInventory.TabIndex = 0;
            this.btnCalculateInventory.Text = "计算每日库存";
            this.btnCalculateInventory.UseVisualStyleBackColor = true;
            this.btnCalculateInventory.Click += new System.EventHandler(this.btnCalculateInventory_Click);
            // 
            // btnImportData
            // 
            this.btnImportData.Location = new System.Drawing.Point(12, 74);
            this.btnImportData.Name = "btnImportData";
            this.btnImportData.Size = new System.Drawing.Size(152, 25);
            this.btnImportData.TabIndex = 1;
            this.btnImportData.Text = "导入数据";
            this.btnImportData.UseVisualStyleBackColor = true;
            this.btnImportData.Click += new System.EventHandler(this.btnImportData_Click);
            // 
            // btnClientSettleBonus
            // 
            this.btnClientSettleBonus.Location = new System.Drawing.Point(12, 115);
            this.btnClientSettleBonus.Name = "btnClientSettleBonus";
            this.btnClientSettleBonus.Size = new System.Drawing.Size(152, 25);
            this.btnClientSettleBonus.TabIndex = 1;
            this.btnClientSettleBonus.Text = "大包客户提出结算";
            this.btnClientSettleBonus.UseVisualStyleBackColor = true;
            this.btnClientSettleBonus.Click += new System.EventHandler(this.btnClientSettleBonus_Click);
            // 
            // btn计算银行卡余额
            // 
            this.btn计算银行卡余额.Location = new System.Drawing.Point(12, 161);
            this.btn计算银行卡余额.Name = "btn计算银行卡余额";
            this.btn计算银行卡余额.Size = new System.Drawing.Size(152, 23);
            this.btn计算银行卡余额.TabIndex = 2;
            this.btn计算银行卡余额.Text = "计算银行卡余额";
            this.btn计算银行卡余额.UseVisualStyleBackColor = true;
            this.btn计算银行卡余额.Click += new System.EventHandler(this.btn计算银行卡余额_Click);
            // 
            // btn权限判断测试
            // 
            this.btn权限判断测试.Location = new System.Drawing.Point(12, 207);
            this.btn权限判断测试.Name = "btn权限判断测试";
            this.btn权限判断测试.Size = new System.Drawing.Size(152, 23);
            this.btn权限判断测试.TabIndex = 3;
            this.btn权限判断测试.Text = "权限判断测试";
            this.btn权限判断测试.UseVisualStyleBackColor = true;
            this.btn权限判断测试.Click += new System.EventHandler(this.btn权限判断测试_Click);
            // 
            // btn现金流计算
            // 
            this.btn现金流计算.Location = new System.Drawing.Point(12, 253);
            this.btn现金流计算.Name = "btn现金流计算";
            this.btn现金流计算.Size = new System.Drawing.Size(152, 23);
            this.btn现金流计算.TabIndex = 4;
            this.btn现金流计算.Text = "现金流计算";
            this.btn现金流计算.UseVisualStyleBackColor = true;
            this.btn现金流计算.Click += new System.EventHandler(this.btn现金流计算_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(439, 297);
            this.Controls.Add(this.btn现金流计算);
            this.Controls.Add(this.btn权限判断测试);
            this.Controls.Add(this.btn计算银行卡余额);
            this.Controls.Add(this.btnClientSettleBonus);
            this.Controls.Add(this.btnImportData);
            this.Controls.Add(this.btnCalculateInventory);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCalculateInventory;
        private System.Windows.Forms.Button btnImportData;
        private System.Windows.Forms.Button btnClientSettleBonus;
        private System.Windows.Forms.Button btn计算银行卡余额;
        private System.Windows.Forms.Button btn权限判断测试;
        private System.Windows.Forms.Button btn现金流计算;
    }
}

