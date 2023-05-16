
namespace DE_Alignment
{
    partial class FrmCameraSetting
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmCameraSetting));
            this.lsbMessage = new System.Windows.Forms.ListBox();
            this.IDSBox1 = new iDSControl.iDSBox();
            this.btnSaveSetting = new System.Windows.Forms.Button();
            this.btnLOAD = new System.Windows.Forms.Button();
            this.picPattern = new System.Windows.Forms.PictureBox();
            this.btnCompare = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.btnCOnnect = new System.Windows.Forms.Button();
            this.txtName = new System.Windows.Forms.TextBox();
            this.labSetting = new System.Windows.Forms.Label();
            this.labRoot = new System.Windows.Forms.Label();
            this.txtRoot = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtSerialNum = new System.Windows.Forms.TextBox();
            this.txtPath = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.picPattern)).BeginInit();
            this.SuspendLayout();
            // 
            // lsbMessage
            // 
            this.lsbMessage.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lsbMessage.FormattingEnabled = true;
            this.lsbMessage.ItemHeight = 21;
            this.lsbMessage.Location = new System.Drawing.Point(430, 12);
            this.lsbMessage.Name = "lsbMessage";
            this.lsbMessage.Size = new System.Drawing.Size(592, 193);
            this.lsbMessage.TabIndex = 6;
            // 
            // IDSBox1
            // 
            this.IDSBox1.Location = new System.Drawing.Point(12, 217);
            this.IDSBox1.Name = "IDSBox1";
            this.IDSBox1.Size = new System.Drawing.Size(1880, 766);
            this.IDSBox1.TabIndex = 0;
            // 
            // btnSaveSetting
            // 
            this.btnSaveSetting.BackgroundImage = global::DE_Alignment.Properties.Resources.saveSetting;
            this.btnSaveSetting.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnSaveSetting.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnSaveSetting.Location = new System.Drawing.Point(1421, 5);
            this.btnSaveSetting.Name = "btnSaveSetting";
            this.btnSaveSetting.Size = new System.Drawing.Size(98, 96);
            this.btnSaveSetting.TabIndex = 7;
            this.btnSaveSetting.UseVisualStyleBackColor = true;
            this.btnSaveSetting.Click += new System.EventHandler(this.btnSaveSetting_Click);
            // 
            // btnLOAD
            // 
            this.btnLOAD.BackgroundImage = global::DE_Alignment.Properties.Resources.sample;
            this.btnLOAD.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnLOAD.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnLOAD.Location = new System.Drawing.Point(116, 12);
            this.btnLOAD.Name = "btnLOAD";
            this.btnLOAD.Size = new System.Drawing.Size(98, 96);
            this.btnLOAD.TabIndex = 5;
            this.btnLOAD.UseVisualStyleBackColor = true;
            this.btnLOAD.Click += new System.EventHandler(this.btnLOAD_Click);
            // 
            // picPattern
            // 
            this.picPattern.Location = new System.Drawing.Point(220, 12);
            this.picPattern.Name = "picPattern";
            this.picPattern.Size = new System.Drawing.Size(204, 199);
            this.picPattern.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picPattern.TabIndex = 3;
            this.picPattern.TabStop = false;
            // 
            // btnCompare
            // 
            this.btnCompare.BackgroundImage = global::DE_Alignment.Properties.Resources.compare;
            this.btnCompare.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnCompare.Enabled = false;
            this.btnCompare.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnCompare.Location = new System.Drawing.Point(116, 115);
            this.btnCompare.Name = "btnCompare";
            this.btnCompare.Size = new System.Drawing.Size(98, 96);
            this.btnCompare.TabIndex = 2;
            this.btnCompare.UseVisualStyleBackColor = true;
            this.btnCompare.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.BackgroundImage = global::DE_Alignment.Properties.Resources.setting;
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button1.Enabled = false;
            this.button1.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.button1.Location = new System.Drawing.Point(12, 115);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(98, 96);
            this.button1.TabIndex = 1;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnCOnnect
            // 
            this.btnCOnnect.BackgroundImage = global::DE_Alignment.Properties.Resources.CameraConnect;
            this.btnCOnnect.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnCOnnect.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnCOnnect.Location = new System.Drawing.Point(12, 12);
            this.btnCOnnect.Name = "btnCOnnect";
            this.btnCOnnect.Size = new System.Drawing.Size(98, 96);
            this.btnCOnnect.TabIndex = 0;
            this.btnCOnnect.UseVisualStyleBackColor = true;
            this.btnCOnnect.Click += new System.EventHandler(this.btnCOnnect_Click);
            // 
            // txtName
            // 
            this.txtName.Font = new System.Drawing.Font("微軟正黑體", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.txtName.Location = new System.Drawing.Point(1177, 51);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(238, 33);
            this.txtName.TabIndex = 8;
            // 
            // labSetting
            // 
            this.labSetting.AutoSize = true;
            this.labSetting.Font = new System.Drawing.Font("微軟正黑體", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.labSetting.Location = new System.Drawing.Point(1028, 58);
            this.labSetting.Name = "labSetting";
            this.labSetting.Size = new System.Drawing.Size(143, 26);
            this.labSetting.TabIndex = 9;
            this.labSetting.Text = "設定環境名稱:";
            // 
            // labRoot
            // 
            this.labRoot.AutoSize = true;
            this.labRoot.Font = new System.Drawing.Font("微軟正黑體", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.labRoot.Location = new System.Drawing.Point(1047, 12);
            this.labRoot.Name = "labRoot";
            this.labRoot.Size = new System.Drawing.Size(122, 26);
            this.labRoot.TabIndex = 10;
            this.labRoot.Text = "設定根目錄:";
            // 
            // txtRoot
            // 
            this.txtRoot.Font = new System.Drawing.Font("微軟正黑體", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.txtRoot.Location = new System.Drawing.Point(1177, 5);
            this.txtRoot.Name = "txtRoot";
            this.txtRoot.ReadOnly = true;
            this.txtRoot.Size = new System.Drawing.Size(238, 33);
            this.txtRoot.TabIndex = 11;
            this.txtRoot.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.txtRoot_MouseDoubleClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微軟正黑體", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label1.Location = new System.Drawing.Point(1068, 98);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 26);
            this.label1.TabIndex = 12;
            this.label1.Text = "相機序號:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微軟正黑體", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label2.Location = new System.Drawing.Point(1028, 143);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(143, 26);
            this.label2.TabIndex = 13;
            this.label2.Text = "相機參數路徑:";
            // 
            // txtSerialNum
            // 
            this.txtSerialNum.Font = new System.Drawing.Font("微軟正黑體", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.txtSerialNum.Location = new System.Drawing.Point(1177, 97);
            this.txtSerialNum.Name = "txtSerialNum";
            this.txtSerialNum.ReadOnly = true;
            this.txtSerialNum.Size = new System.Drawing.Size(238, 33);
            this.txtSerialNum.TabIndex = 14;
            // 
            // txtPath
            // 
            this.txtPath.Font = new System.Drawing.Font("微軟正黑體", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.txtPath.Location = new System.Drawing.Point(1177, 142);
            this.txtPath.Name = "txtPath";
            this.txtPath.ReadOnly = true;
            this.txtPath.Size = new System.Drawing.Size(238, 33);
            this.txtPath.TabIndex = 15;
            // 
            // FrmCameraSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1904, 1041);
            this.Controls.Add(this.txtPath);
            this.Controls.Add(this.txtSerialNum);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtRoot);
            this.Controls.Add(this.labRoot);
            this.Controls.Add(this.labSetting);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.btnSaveSetting);
            this.Controls.Add(this.lsbMessage);
            this.Controls.Add(this.btnLOAD);
            this.Controls.Add(this.picPattern);
            this.Controls.Add(this.btnCompare);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.IDSBox1);
            this.Controls.Add(this.btnCOnnect);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmCameraSetting";
            this.Text = "DE_Alignment";
            this.Load += new System.EventHandler(this.FrmMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picPattern)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private iDSControl.iDSBox IDSBox1;
        private System.Windows.Forms.Button btnCOnnect;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnCompare;
        private System.Windows.Forms.PictureBox picPattern;
        private System.Windows.Forms.Button btnLOAD;
        private System.Windows.Forms.ListBox lsbMessage;
        private System.Windows.Forms.Button btnSaveSetting;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label labSetting;
        private System.Windows.Forms.Label labRoot;
        private System.Windows.Forms.TextBox txtRoot;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtSerialNum;
        private System.Windows.Forms.TextBox txtPath;
    }
}

