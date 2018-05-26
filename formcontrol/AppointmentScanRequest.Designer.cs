namespace HaoProgram
{
    partial class AppointmentScanRequest
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AppointmentScanRequest));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.ScannerImage = new System.Windows.Forms.PictureBox();
            this.ImageChecker = new System.Windows.Forms.ListBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.DoneBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.AcceptBtn = new System.Windows.Forms.Button();
            this.LoadBtn = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ScannerImage)).BeginInit();
            this.tableLayoutPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 66.66666F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(588, 444);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel1.SetColumnSpan(this.tableLayoutPanel2, 2);
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 66.66666F));
            this.tableLayoutPanel2.Controls.Add(this.ScannerImage, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.ImageChecker, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 290F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(582, 290);
            this.tableLayoutPanel2.TabIndex = 8;
            // 
            // ScannerImage
            // 
            this.ScannerImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ScannerImage.Location = new System.Drawing.Point(197, 3);
            this.ScannerImage.Name = "ScannerImage";
            this.ScannerImage.Size = new System.Drawing.Size(382, 284);
            this.ScannerImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.ScannerImage.TabIndex = 1;
            this.ScannerImage.TabStop = false;
            this.ScannerImage.Click += new System.EventHandler(this.ScannerImage_Click);
            // 
            // ImageChecker
            // 
            this.ImageChecker.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ImageChecker.FormattingEnabled = true;
            this.ImageChecker.ItemHeight = 16;
            this.ImageChecker.Location = new System.Drawing.Point(3, 3);
            this.ImageChecker.Name = "ImageChecker";
            this.ImageChecker.Size = new System.Drawing.Size(188, 284);
            this.ImageChecker.TabIndex = 2;
            this.ImageChecker.SelectedIndexChanged += new System.EventHandler(this.ImageChecker_SelectedIndexChanged);
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 4;
            this.tableLayoutPanel1.SetColumnSpan(this.tableLayoutPanel3, 2);
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.00062F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.00062F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.00062F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 24.99813F));
            this.tableLayoutPanel3.Controls.Add(this.LoadBtn, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.DoneBtn, 3, 0);
            this.tableLayoutPanel3.Controls.Add(this.CancelBtn, 2, 0);
            this.tableLayoutPanel3.Controls.Add(this.AcceptBtn, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 299);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(582, 142);
            this.tableLayoutPanel3.TabIndex = 9;
            // 
            // DoneBtn
            // 
            this.DoneBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.DoneBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DoneBtn.Image = ((System.Drawing.Image)(resources.GetObject("DoneBtn.Image")));
            this.DoneBtn.Location = new System.Drawing.Point(438, 3);
            this.DoneBtn.Name = "DoneBtn";
            this.DoneBtn.Size = new System.Drawing.Size(141, 136);
            this.DoneBtn.TabIndex = 10;
            this.DoneBtn.Text = "Done";
            this.DoneBtn.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.DoneBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.DoneBtn.UseVisualStyleBackColor = true;
            this.DoneBtn.Click += new System.EventHandler(this.DoneBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CancelBtn.Image = ((System.Drawing.Image)(resources.GetObject("CancelBtn.Image")));
            this.CancelBtn.Location = new System.Drawing.Point(293, 3);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(139, 136);
            this.CancelBtn.TabIndex = 9;
            this.CancelBtn.Text = "Delete";
            this.CancelBtn.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.CancelBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // AcceptBtn
            // 
            this.AcceptBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AcceptBtn.Image = ((System.Drawing.Image)(resources.GetObject("AcceptBtn.Image")));
            this.AcceptBtn.Location = new System.Drawing.Point(3, 3);
            this.AcceptBtn.Name = "AcceptBtn";
            this.AcceptBtn.Size = new System.Drawing.Size(139, 136);
            this.AcceptBtn.TabIndex = 8;
            this.AcceptBtn.Text = "Scan";
            this.AcceptBtn.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.AcceptBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.AcceptBtn.UseVisualStyleBackColor = true;
            this.AcceptBtn.Click += new System.EventHandler(this.AcceptBtn_Click_1);
            // 
            // LoadBtn
            // 
            this.LoadBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LoadBtn.Image = ((System.Drawing.Image)(resources.GetObject("LoadBtn.Image")));
            this.LoadBtn.Location = new System.Drawing.Point(148, 3);
            this.LoadBtn.Name = "LoadBtn";
            this.LoadBtn.Size = new System.Drawing.Size(139, 136);
            this.LoadBtn.TabIndex = 11;
            this.LoadBtn.Text = "Add";
            this.LoadBtn.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.LoadBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.LoadBtn.UseVisualStyleBackColor = true;
            this.LoadBtn.Click += new System.EventHandler(this.LoadBtn_Click);
            // 
            // AppointmentScanRequest
            // 
            this.AcceptButton = this.AcceptBtn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.DoneBtn;
            this.ClientSize = new System.Drawing.Size(588, 444);
            this.Controls.Add(this.tableLayoutPanel1);
            this.DoubleBuffered = true;
            this.Name = "AppointmentScanRequest";
            this.Text = "New Appointment Scan";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ScannerImage)).EndInit();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.PictureBox ScannerImage;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Button DoneBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button AcceptBtn;
        private System.Windows.Forms.ListBox ImageChecker;
        private System.Windows.Forms.Button LoadBtn;
    }
}
