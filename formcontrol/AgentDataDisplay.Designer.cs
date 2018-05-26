namespace HaoProgram
{
    partial class AgentDataDisplay
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AgentDataDisplay));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.InputLayout = new System.Windows.Forms.TableLayoutPanel();
            this.AgentContact = new System.Windows.Forms.TextBox();
            this.AgentName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.AgentIC = new System.Windows.Forms.TextBox();
            this.AgentAddress = new System.Windows.Forms.RichTextBox();
            this.AgentComission = new System.Windows.Forms.ComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.AgentDOJ = new System.Windows.Forms.DateTimePicker();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.EditBtn = new System.Windows.Forms.Button();
            this.ExitBtn = new System.Windows.Forms.Button();
            this.AddBtn = new System.Windows.Forms.Button();
            this.DeleteBtn = new System.Windows.Forms.Button();
            this.SaveBtn = new System.Windows.Forms.Button();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.AgentDataDGV = new System.Windows.Forms.DataGridView();
            this.NameHeader = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NRIC = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Contact = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Address = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DateOfJoin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ComissionCategory = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.label7 = new System.Windows.Forms.Label();
            this.SortBy = new System.Windows.Forms.ComboBox();
            this.SortBox = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.InputLayout.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AgentDataDGV)).BeginInit();
            this.tableLayoutPanel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tableLayoutPanel3);
            this.splitContainer1.Size = new System.Drawing.Size(1080, 720);
            this.splitContainer1.SplitterDistance = 481;
            this.splitContainer1.TabIndex = 1;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer2.IsSplitterFixed = true;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.InputLayout);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.tableLayoutPanel2);
            this.splitContainer2.Size = new System.Drawing.Size(481, 720);
            this.splitContainer2.SplitterDistance = 586;
            this.splitContainer2.TabIndex = 1;
            // 
            // InputLayout
            // 
            this.InputLayout.AutoScroll = true;
            this.InputLayout.ColumnCount = 2;
            this.InputLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 21.2585F));
            this.InputLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 78.7415F));
            this.InputLayout.Controls.Add(this.AgentContact, 1, 2);
            this.InputLayout.Controls.Add(this.AgentName, 1, 0);
            this.InputLayout.Controls.Add(this.label1, 0, 0);
            this.InputLayout.Controls.Add(this.label2, 0, 1);
            this.InputLayout.Controls.Add(this.label5, 0, 4);
            this.InputLayout.Controls.Add(this.label3, 0, 2);
            this.InputLayout.Controls.Add(this.label4, 0, 3);
            this.InputLayout.Controls.Add(this.label6, 0, 5);
            this.InputLayout.Controls.Add(this.AgentIC, 1, 1);
            this.InputLayout.Controls.Add(this.AgentAddress, 1, 3);
            this.InputLayout.Controls.Add(this.AgentComission, 1, 5);
            this.InputLayout.Controls.Add(this.panel1, 1, 4);
            this.InputLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.InputLayout.Location = new System.Drawing.Point(0, 0);
            this.InputLayout.Name = "InputLayout";
            this.InputLayout.RowCount = 6;
            this.InputLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 13.3328F));
            this.InputLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 13.3328F));
            this.InputLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 13.3328F));
            this.InputLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 13.3328F));
            this.InputLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 13.3328F));
            this.InputLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 13.3328F));
            this.InputLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.InputLayout.Size = new System.Drawing.Size(481, 586);
            this.InputLayout.TabIndex = 9;
            // 
            // AgentContact
            // 
            this.AgentContact.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.AgentContact.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F);
            this.AgentContact.Location = new System.Drawing.Point(105, 228);
            this.AgentContact.Name = "AgentContact";
            this.AgentContact.ReadOnly = true;
            this.AgentContact.Size = new System.Drawing.Size(373, 28);
            this.AgentContact.TabIndex = 3;
            // 
            // AgentName
            // 
            this.AgentName.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.AgentName.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F);
            this.AgentName.Location = new System.Drawing.Point(105, 34);
            this.AgentName.Name = "AgentName";
            this.AgentName.ReadOnly = true;
            this.AgentName.Size = new System.Drawing.Size(373, 28);
            this.AgentName.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(54, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Name";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(59, 137);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 17);
            this.label2.TabIndex = 2;
            this.label2.Text = "NRIC";
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 428);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(87, 17);
            this.label5.TabIndex = 6;
            this.label5.Text = "Date Of Join";
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(43, 234);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 17);
            this.label3.TabIndex = 4;
            this.label3.Text = "Contact";
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(39, 331);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 17);
            this.label4.TabIndex = 5;
            this.label4.Text = "Address";
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(23, 518);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(76, 34);
            this.label6.TabIndex = 7;
            this.label6.Text = "Comission Category";
            // 
            // AgentIC
            // 
            this.AgentIC.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.AgentIC.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AgentIC.Location = new System.Drawing.Point(105, 131);
            this.AgentIC.Name = "AgentIC";
            this.AgentIC.ReadOnly = true;
            this.AgentIC.Size = new System.Drawing.Size(373, 28);
            this.AgentIC.TabIndex = 2;
            // 
            // AgentAddress
            // 
            this.AgentAddress.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.AgentAddress.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F);
            this.AgentAddress.Location = new System.Drawing.Point(105, 297);
            this.AgentAddress.Name = "AgentAddress";
            this.AgentAddress.ReadOnly = true;
            this.AgentAddress.Size = new System.Drawing.Size(373, 84);
            this.AgentAddress.TabIndex = 4;
            this.AgentAddress.Text = "";
            // 
            // AgentComission
            // 
            this.AgentComission.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.AgentComission.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.AgentComission.Enabled = false;
            this.AgentComission.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AgentComission.FormattingEnabled = true;
            this.AgentComission.Location = new System.Drawing.Point(105, 519);
            this.AgentComission.Name = "AgentComission";
            this.AgentComission.Size = new System.Drawing.Size(373, 30);
            this.AgentComission.TabIndex = 6;
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.AgentDOJ);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(105, 391);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(373, 91);
            this.panel1.TabIndex = 18;
            // 
            // AgentDOJ
            // 
            this.AgentDOJ.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.AgentDOJ.CustomFormat = "ddd . dd/MM/yyyy";
            this.AgentDOJ.Enabled = false;
            this.AgentDOJ.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.AgentDOJ.Location = new System.Drawing.Point(3, 36);
            this.AgentDOJ.Name = "AgentDOJ";
            this.AgentDOJ.Size = new System.Drawing.Size(370, 22);
            this.AgentDOJ.TabIndex = 5;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 5;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel2.Controls.Add(this.EditBtn, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.ExitBtn, 3, 0);
            this.tableLayoutPanel2.Controls.Add(this.AddBtn, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.DeleteBtn, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.SaveBtn, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(481, 130);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // EditBtn
            // 
            this.EditBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EditBtn.Image = ((System.Drawing.Image)(resources.GetObject("EditBtn.Image")));
            this.EditBtn.Location = new System.Drawing.Point(99, 3);
            this.EditBtn.Name = "EditBtn";
            this.EditBtn.Size = new System.Drawing.Size(90, 124);
            this.EditBtn.TabIndex = 12;
            this.EditBtn.Text = "Edit";
            this.EditBtn.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.EditBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.EditBtn.UseVisualStyleBackColor = true;
            this.EditBtn.Click += new System.EventHandler(this.EditBtn_Click);
            // 
            // ExitBtn
            // 
            this.ExitBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ExitBtn.Image = ((System.Drawing.Image)(resources.GetObject("ExitBtn.Image")));
            this.ExitBtn.Location = new System.Drawing.Point(387, 3);
            this.ExitBtn.Name = "ExitBtn";
            this.ExitBtn.Size = new System.Drawing.Size(91, 124);
            this.ExitBtn.TabIndex = 10;
            this.ExitBtn.Text = "Exit";
            this.ExitBtn.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.ExitBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.ExitBtn.UseVisualStyleBackColor = true;
            this.ExitBtn.Click += new System.EventHandler(this.ExitBtn_Click);
            // 
            // AddBtn
            // 
            this.AddBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AddBtn.Image = ((System.Drawing.Image)(resources.GetObject("AddBtn.Image")));
            this.AddBtn.Location = new System.Drawing.Point(3, 3);
            this.AddBtn.Name = "AddBtn";
            this.AddBtn.Size = new System.Drawing.Size(90, 124);
            this.AddBtn.TabIndex = 7;
            this.AddBtn.Text = "Add";
            this.AddBtn.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.AddBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.AddBtn.UseVisualStyleBackColor = true;
            this.AddBtn.Click += new System.EventHandler(this.AddBtn_Click);
            // 
            // DeleteBtn
            // 
            this.DeleteBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DeleteBtn.Image = ((System.Drawing.Image)(resources.GetObject("DeleteBtn.Image")));
            this.DeleteBtn.Location = new System.Drawing.Point(291, 3);
            this.DeleteBtn.Name = "DeleteBtn";
            this.DeleteBtn.Size = new System.Drawing.Size(90, 124);
            this.DeleteBtn.TabIndex = 9;
            this.DeleteBtn.Text = "Delete";
            this.DeleteBtn.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.DeleteBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.DeleteBtn.UseVisualStyleBackColor = true;
            this.DeleteBtn.Click += new System.EventHandler(this.DeleteBtn_Click);
            // 
            // SaveBtn
            // 
            this.SaveBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SaveBtn.Image = ((System.Drawing.Image)(resources.GetObject("SaveBtn.Image")));
            this.SaveBtn.Location = new System.Drawing.Point(195, 3);
            this.SaveBtn.Name = "SaveBtn";
            this.SaveBtn.Size = new System.Drawing.Size(90, 124);
            this.SaveBtn.TabIndex = 8;
            this.SaveBtn.Text = "Save";
            this.SaveBtn.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.SaveBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.SaveBtn.UseVisualStyleBackColor = true;
            this.SaveBtn.Click += new System.EventHandler(this.SaveBtn_Click);
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.AgentDataDGV, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel4, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 90F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(595, 720);
            this.tableLayoutPanel3.TabIndex = 2;
            // 
            // AgentDataDGV
            // 
            this.AgentDataDGV.AllowUserToAddRows = false;
            this.AgentDataDGV.AllowUserToDeleteRows = false;
            this.AgentDataDGV.AllowUserToOrderColumns = true;
            this.AgentDataDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.AgentDataDGV.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.NameHeader,
            this.NRIC,
            this.Contact,
            this.Address,
            this.DateOfJoin,
            this.ComissionCategory});
            this.AgentDataDGV.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AgentDataDGV.Location = new System.Drawing.Point(3, 75);
            this.AgentDataDGV.MultiSelect = false;
            this.AgentDataDGV.Name = "AgentDataDGV";
            this.AgentDataDGV.ReadOnly = true;
            this.AgentDataDGV.RowHeadersVisible = false;
            this.AgentDataDGV.RowTemplate.Height = 24;
            this.AgentDataDGV.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.AgentDataDGV.Size = new System.Drawing.Size(589, 642);
            this.AgentDataDGV.TabIndex = 0;
            this.AgentDataDGV.SelectionChanged += new System.EventHandler(this.DGVSelect);
            // 
            // NameHeader
            // 
            this.NameHeader.HeaderText = "Name";
            this.NameHeader.Name = "NameHeader";
            this.NameHeader.ReadOnly = true;
            this.NameHeader.Width = 140;
            // 
            // NRIC
            // 
            this.NRIC.HeaderText = "NRIC";
            this.NRIC.Name = "NRIC";
            this.NRIC.ReadOnly = true;
            // 
            // Contact
            // 
            this.Contact.HeaderText = "Contact";
            this.Contact.Name = "Contact";
            this.Contact.ReadOnly = true;
            this.Contact.Width = 140;
            // 
            // Address
            // 
            this.Address.HeaderText = "Address";
            this.Address.Name = "Address";
            this.Address.ReadOnly = true;
            // 
            // DateOfJoin
            // 
            this.DateOfJoin.HeaderText = "DateOfJoin";
            this.DateOfJoin.Name = "DateOfJoin";
            this.DateOfJoin.ReadOnly = true;
            this.DateOfJoin.Width = 140;
            // 
            // ComissionCategory
            // 
            this.ComissionCategory.HeaderText = "ComissionCategory";
            this.ComissionCategory.Name = "ComissionCategory";
            this.ComissionCategory.ReadOnly = true;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 3;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 55F));
            this.tableLayoutPanel4.Controls.Add(this.label7, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.SortBy, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.SortBox, 2, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(589, 66);
            this.tableLayoutPanel4.TabIndex = 1;
            // 
            // label7
            // 
            this.label7.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(31, 24);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(54, 17);
            this.label7.TabIndex = 0;
            this.label7.Text = "Sort By";
            // 
            // SortBy
            // 
            this.SortBy.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.SortBy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SortBy.FormattingEnabled = true;
            this.SortBy.Location = new System.Drawing.Point(91, 20);
            this.SortBy.Name = "SortBy";
            this.SortBy.Size = new System.Drawing.Size(150, 24);
            this.SortBy.TabIndex = 1;
            // 
            // SortBox
            // 
            this.SortBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.SortBox.Location = new System.Drawing.Point(267, 22);
            this.SortBox.Name = "SortBox";
            this.SortBox.Size = new System.Drawing.Size(225, 22);
            this.SortBox.TabIndex = 2;
            this.SortBox.TextChanged += new System.EventHandler(this.OnSortTyping);
            // 
            // AgentDataDisplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "AgentDataDisplay";
            this.Size = new System.Drawing.Size(1080, 720);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.InputLayout.ResumeLayout(false);
            this.InputLayout.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.AgentDataDGV)).EndInit();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.TableLayoutPanel InputLayout;
        private System.Windows.Forms.TextBox AgentContact;
        private System.Windows.Forms.TextBox AgentName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox AgentIC;
        private System.Windows.Forms.RichTextBox AgentAddress;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button ExitBtn;
        private System.Windows.Forms.Button AddBtn;
        private System.Windows.Forms.Button DeleteBtn;
        private System.Windows.Forms.Button SaveBtn;
        private System.Windows.Forms.DataGridView AgentDataDGV;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DateTimePicker AgentDOJ;
        private System.Windows.Forms.DataGridViewTextBoxColumn NameHeader;
        private System.Windows.Forms.DataGridViewTextBoxColumn NRIC;
        private System.Windows.Forms.DataGridViewTextBoxColumn Contact;
        private System.Windows.Forms.DataGridViewTextBoxColumn Address;
        private System.Windows.Forms.DataGridViewTextBoxColumn DateOfJoin;
        private System.Windows.Forms.DataGridViewTextBoxColumn ComissionCategory;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox AgentComission;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox SortBy;
        private System.Windows.Forms.TextBox SortBox;
        private System.Windows.Forms.Button EditBtn;
    }
}
