namespace HaoProgram
{
    partial class PayMoney
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PayMoney));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.PaymentView = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CHEQUE = new System.Windows.Forms.TextBox();
            this.PayBy = new System.Windows.Forms.ComboBox();
            this.Paid = new System.Windows.Forms.NumericUpDown();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.ExitBtn = new System.Windows.Forms.Button();
            this.PayBtn = new System.Windows.Forms.Button();
            this.DeleteBtn = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PaymentView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Paid)).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 55.67227F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 44.32773F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 92F));
            this.tableLayoutPanel1.Controls.Add(this.PaymentView, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.CHEQUE, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.PayBy, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.Paid, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.DeleteBtn, 2, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(566, 563);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // PaymentView
            // 
            this.PaymentView.AllowUserToAddRows = false;
            this.PaymentView.AllowUserToDeleteRows = false;
            this.PaymentView.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.PaymentView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.PaymentView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2});
            this.tableLayoutPanel1.SetColumnSpan(this.PaymentView, 3);
            this.PaymentView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PaymentView.Location = new System.Drawing.Point(3, 3);
            this.PaymentView.MultiSelect = false;
            this.PaymentView.Name = "PaymentView";
            this.PaymentView.ReadOnly = true;
            this.PaymentView.RowHeadersVisible = false;
            this.PaymentView.RowTemplate.Height = 24;
            this.PaymentView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.PaymentView.Size = new System.Drawing.Size(560, 331);
            this.PaymentView.TabIndex = 15;
            this.PaymentView.SelectionChanged += new System.EventHandler(this.PaymentView_SelectionChanged);
            // 
            // Column1
            // 
            this.Column1.HeaderText = "";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Width = 200;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "Payment";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.Width = 200;
            // 
            // CHEQUE
            // 
            this.CHEQUE.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.CHEQUE.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F);
            this.CHEQUE.Location = new System.Drawing.Point(266, 351);
            this.CHEQUE.Name = "CHEQUE";
            this.CHEQUE.ReadOnly = true;
            this.CHEQUE.Size = new System.Drawing.Size(203, 28);
            this.CHEQUE.TabIndex = 19;
            // 
            // PayBy
            // 
            this.PayBy.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.PayBy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.PayBy.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F);
            this.PayBy.FormattingEnabled = true;
            this.PayBy.Items.AddRange(new object[] {
            "CASH",
            "CHEQUE"});
            this.PayBy.Location = new System.Drawing.Point(86, 349);
            this.PayBy.Name = "PayBy";
            this.PayBy.Size = new System.Drawing.Size(174, 30);
            this.PayBy.TabIndex = 20;
            this.PayBy.SelectedIndexChanged += new System.EventHandler(this.PayBy_SelectedIndexChanged);
            // 
            // Paid
            // 
            this.Paid.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.tableLayoutPanel1.SetColumnSpan(this.Paid, 3);
            this.Paid.DecimalPlaces = 2;
            this.Paid.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F);
            this.Paid.Location = new System.Drawing.Point(113, 407);
            this.Paid.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.Paid.Minimum = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.Paid.Name = "Paid";
            this.Paid.Size = new System.Drawing.Size(340, 28);
            this.Paid.TabIndex = 21;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel1.SetColumnSpan(this.tableLayoutPanel2, 3);
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.ExitBtn, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.PayBtn, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 452);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(560, 108);
            this.tableLayoutPanel2.TabIndex = 22;
            // 
            // ExitBtn
            // 
            this.ExitBtn.AccessibleDescription = "";
            this.ExitBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ExitBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ExitBtn.Image = ((System.Drawing.Image)(resources.GetObject("ExitBtn.Image")));
            this.ExitBtn.Location = new System.Drawing.Point(283, 3);
            this.ExitBtn.Name = "ExitBtn";
            this.ExitBtn.Size = new System.Drawing.Size(274, 102);
            this.ExitBtn.TabIndex = 20;
            this.ExitBtn.Text = "Done";
            this.ExitBtn.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.ExitBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.ExitBtn.UseVisualStyleBackColor = true;
            this.ExitBtn.Click += new System.EventHandler(this.ExitBtn_Click);
            // 
            // PayBtn
            // 
            this.PayBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PayBtn.Image = ((System.Drawing.Image)(resources.GetObject("PayBtn.Image")));
            this.PayBtn.Location = new System.Drawing.Point(3, 3);
            this.PayBtn.Name = "PayBtn";
            this.PayBtn.Size = new System.Drawing.Size(274, 102);
            this.PayBtn.TabIndex = 19;
            this.PayBtn.Text = "Pay";
            this.PayBtn.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.PayBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.PayBtn.UseVisualStyleBackColor = true;
            this.PayBtn.Click += new System.EventHandler(this.PayBtn_Click);
            // 
            // DeleteBtn
            // 
            this.DeleteBtn.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.DeleteBtn.Image = ((System.Drawing.Image)(resources.GetObject("DeleteBtn.Image")));
            this.DeleteBtn.Location = new System.Drawing.Point(476, 347);
            this.DeleteBtn.Name = "DeleteBtn";
            this.DeleteBtn.Size = new System.Drawing.Size(36, 36);
            this.DeleteBtn.TabIndex = 23;
            this.DeleteBtn.UseVisualStyleBackColor = true;
            this.DeleteBtn.Click += new System.EventHandler(this.DeleteBtn_Click);
            // 
            // PayMoney
            // 
            this.AcceptButton = this.PayBtn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.ExitBtn;
            this.ClientSize = new System.Drawing.Size(566, 563);
            this.Controls.Add(this.tableLayoutPanel1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "PayMoney";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PayMoney";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PaymentView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Paid)).EndInit();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.DataGridView PaymentView;
        private System.Windows.Forms.TextBox CHEQUE;
        private System.Windows.Forms.ComboBox PayBy;
        private System.Windows.Forms.NumericUpDown Paid;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button ExitBtn;
        private System.Windows.Forms.Button PayBtn;
        private System.Windows.Forms.Button DeleteBtn;
    }
}