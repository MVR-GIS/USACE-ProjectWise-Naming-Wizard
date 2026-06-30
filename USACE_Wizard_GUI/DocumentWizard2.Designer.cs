namespace USACE_Wizard_GUI
{
    partial class DocumentWizard2
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


        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DocumentWizard2));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.documentName21 = new USACE_Wizard_GUI.Controls.DocumentName2();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.department1 = new USACE_Wizard_GUI.Controls.Department();
            this.keywords1 = new USACE_Wizard_GUI.Controls.Keywords();
            this.description1 = new USACE_Wizard_GUI.Controls.Description();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changeDestinationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.autoAppendForDuplicateNamesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.filenameMatchesDocumentNameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.useFileLastModifiedDateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.useSameNameForAllFilesAppendToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.useOriginalDescriptionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.validateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.filterDocumentTypesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.filterTypeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripComboFilterType = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.coPToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripComboCoP = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.documentTypeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripComboBox3 = new System.Windows.Forms.ToolStripComboBox();
            this.testToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.comboProduct = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBoxDocType = new System.Windows.Forms.ComboBox();
            this.comboBoxCoPs = new System.Windows.Forms.ComboBox();
            this.comboBoxType = new System.Windows.Forms.ComboBox();
            this.multiFileList1 = new USACE_Wizard_GUI.Controls.MultiFileList();
            this.buttonsAndStatus1 = new USACE_Wizard_GUI.Controls.ButtonsAndStatus();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(0, 109);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(855, 238);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.documentName21);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(847, 212);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Name";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // documentName21
            // 
            this.documentName21.Data = null;
            this.documentName21.Dock = System.Windows.Forms.DockStyle.Fill;
            this.documentName21.Location = new System.Drawing.Point(3, 3);
            this.documentName21.Name = "documentName21";
            this.documentName21.Size = new System.Drawing.Size(841, 206);
            this.documentName21.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.department1);
            this.tabPage2.Controls.Add(this.keywords1);
            this.tabPage2.Controls.Add(this.description1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(847, 212);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Attributes";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // department1
            // 
            this.department1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.department1.Data = null;
            this.department1.Location = new System.Drawing.Point(8, 163);
            this.department1.MaximumSize = new System.Drawing.Size(0, 25);
            this.department1.MinimumSize = new System.Drawing.Size(263, 25);
            this.department1.Name = "department1";
            this.department1.Size = new System.Drawing.Size(263, 25);
            this.department1.TabIndex = 2;
            // 
            // keywords1
            // 
            this.keywords1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.keywords1.Data = null;
            this.keywords1.Location = new System.Drawing.Point(503, 6);
            this.keywords1.Name = "keywords1";
            this.keywords1.Size = new System.Drawing.Size(336, 204);
            this.keywords1.TabIndex = 1;
            // 
            // description1
            // 
            this.description1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.description1.Data = null;
            this.description1.Location = new System.Drawing.Point(8, 6);
            this.description1.MinimumSize = new System.Drawing.Size(218, 52);
            this.description1.Name = "description1";
            this.description1.Size = new System.Drawing.Size(489, 133);
            this.description1.TabIndex = 0;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.optionsToolStripMenuItem,
            this.filterDocumentTypesToolStripMenuItem,
            this.testToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(855, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addFileToolStripMenuItem,
            this.changeDestinationToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // addFileToolStripMenuItem
            // 
            this.addFileToolStripMenuItem.Name = "addFileToolStripMenuItem";
            this.addFileToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
            this.addFileToolStripMenuItem.Text = "Add File";
            // 
            // changeDestinationToolStripMenuItem
            // 
            this.changeDestinationToolStripMenuItem.Name = "changeDestinationToolStripMenuItem";
            this.changeDestinationToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
            this.changeDestinationToolStripMenuItem.Text = "Change Destination";
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.autoAppendForDuplicateNamesToolStripMenuItem,
            this.filenameMatchesDocumentNameToolStripMenuItem,
            this.useFileLastModifiedDateToolStripMenuItem,
            this.useSameNameForAllFilesAppendToolStripMenuItem,
            this.useOriginalDescriptionToolStripMenuItem,
            this.validateToolStripMenuItem});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.optionsToolStripMenuItem.Text = "Options";
            // 
            // autoAppendForDuplicateNamesToolStripMenuItem
            // 
            this.autoAppendForDuplicateNamesToolStripMenuItem.Checked = true;
            this.autoAppendForDuplicateNamesToolStripMenuItem.CheckOnClick = true;
            this.autoAppendForDuplicateNamesToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.autoAppendForDuplicateNamesToolStripMenuItem.Name = "autoAppendForDuplicateNamesToolStripMenuItem";
            this.autoAppendForDuplicateNamesToolStripMenuItem.Size = new System.Drawing.Size(278, 22);
            this.autoAppendForDuplicateNamesToolStripMenuItem.Text = "Auto Append # For Duplicate Names";
            // 
            // filenameMatchesDocumentNameToolStripMenuItem
            // 
            this.filenameMatchesDocumentNameToolStripMenuItem.Checked = true;
            this.filenameMatchesDocumentNameToolStripMenuItem.CheckOnClick = true;
            this.filenameMatchesDocumentNameToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.filenameMatchesDocumentNameToolStripMenuItem.Name = "filenameMatchesDocumentNameToolStripMenuItem";
            this.filenameMatchesDocumentNameToolStripMenuItem.Size = new System.Drawing.Size(278, 22);
            this.filenameMatchesDocumentNameToolStripMenuItem.Text = "Filename Matches Document Name";
            // 
            // useFileLastModifiedDateToolStripMenuItem
            // 
            this.useFileLastModifiedDateToolStripMenuItem.CheckOnClick = true;
            this.useFileLastModifiedDateToolStripMenuItem.Name = "useFileLastModifiedDateToolStripMenuItem";
            this.useFileLastModifiedDateToolStripMenuItem.Size = new System.Drawing.Size(278, 22);
            this.useFileLastModifiedDateToolStripMenuItem.Text = "Use File Last Modified Date";
            // 
            // useSameNameForAllFilesAppendToolStripMenuItem
            // 
            this.useSameNameForAllFilesAppendToolStripMenuItem.CheckOnClick = true;
            this.useSameNameForAllFilesAppendToolStripMenuItem.Name = "useSameNameForAllFilesAppendToolStripMenuItem";
            this.useSameNameForAllFilesAppendToolStripMenuItem.Size = new System.Drawing.Size(278, 22);
            this.useSameNameForAllFilesAppendToolStripMenuItem.Text = "Use Same Name For All Files Append #";
            // 
            // useOriginalDescriptionToolStripMenuItem
            // 
            this.useOriginalDescriptionToolStripMenuItem.CheckOnClick = true;
            this.useOriginalDescriptionToolStripMenuItem.Name = "useOriginalDescriptionToolStripMenuItem";
            this.useOriginalDescriptionToolStripMenuItem.Size = new System.Drawing.Size(278, 22);
            this.useOriginalDescriptionToolStripMenuItem.Text = "Use Original Description";
            // 
            // validateToolStripMenuItem
            // 
            this.validateToolStripMenuItem.Name = "validateToolStripMenuItem";
            this.validateToolStripMenuItem.Size = new System.Drawing.Size(278, 22);
            this.validateToolStripMenuItem.Text = "Validate";
            this.validateToolStripMenuItem.Click += new System.EventHandler(this.validateToolStripMenuItem_Click);
            // 
            // filterDocumentTypesToolStripMenuItem
            // 
            this.filterDocumentTypesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.filterTypeToolStripMenuItem,
            this.toolStripComboFilterType,
            this.toolStripSeparator1,
            this.coPToolStripMenuItem,
            this.toolStripComboCoP,
            this.toolStripSeparator2,
            this.documentTypeToolStripMenuItem,
            this.toolStripComboBox3});
            this.filterDocumentTypesToolStripMenuItem.Name = "filterDocumentTypesToolStripMenuItem";
            this.filterDocumentTypesToolStripMenuItem.Size = new System.Drawing.Size(136, 20);
            this.filterDocumentTypesToolStripMenuItem.Text = "Filter Document Types";
            this.filterDocumentTypesToolStripMenuItem.Visible = false;
            // 
            // filterTypeToolStripMenuItem
            // 
            this.filterTypeToolStripMenuItem.Name = "filterTypeToolStripMenuItem";
            this.filterTypeToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.filterTypeToolStripMenuItem.Text = "Filter Type";
            // 
            // toolStripComboFilterType
            // 
            this.toolStripComboFilterType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.toolStripComboFilterType.Items.AddRange(new object[] {
            "CoP",
            "Folder"});
            this.toolStripComboFilterType.Name = "toolStripComboFilterType";
            this.toolStripComboFilterType.Size = new System.Drawing.Size(121, 23);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(178, 6);
            // 
            // coPToolStripMenuItem
            // 
            this.coPToolStripMenuItem.Name = "coPToolStripMenuItem";
            this.coPToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.coPToolStripMenuItem.Text = "CoP";
            // 
            // toolStripComboCoP
            // 
            this.toolStripComboCoP.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.toolStripComboCoP.Name = "toolStripComboCoP";
            this.toolStripComboCoP.Size = new System.Drawing.Size(121, 23);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(178, 6);
            // 
            // documentTypeToolStripMenuItem
            // 
            this.documentTypeToolStripMenuItem.Name = "documentTypeToolStripMenuItem";
            this.documentTypeToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.documentTypeToolStripMenuItem.Text = "Document Type";
            // 
            // toolStripComboBox3
            // 
            this.toolStripComboBox3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.toolStripComboBox3.Name = "toolStripComboBox3";
            this.toolStripComboBox3.Size = new System.Drawing.Size(121, 23);
            // 
            // testToolStripMenuItem
            // 
            this.testToolStripMenuItem.Name = "testToolStripMenuItem";
            this.testToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.testToolStripMenuItem.Text = "Test";
            this.testToolStripMenuItem.Visible = false;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2});
            this.statusStrip1.Location = new System.Drawing.Point(0, 540);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(855, 22);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(162, 17);
            this.toolStripStatusLabel1.Text = "Document Type Filter:  {0}, {1}";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(678, 17);
            this.toolStripStatusLabel2.Spring = true;
            this.toolStripStatusLabel2.Text = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1, 363);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Destination";
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Enabled = false;
            this.textBox1.Location = new System.Drawing.Point(67, 360);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(784, 20);
            this.textBox1.TabIndex = 8;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.comboProduct);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.comboBoxDocType);
            this.groupBox1.Controls.Add(this.comboBoxCoPs);
            this.groupBox1.Controls.Add(this.comboBoxType);
            this.groupBox1.Location = new System.Drawing.Point(4, 28);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(847, 75);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Filter";
            // 
            // comboProduct
            // 
            this.comboProduct.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboProduct.FormattingEnabled = true;
            this.comboProduct.Location = new System.Drawing.Point(3, 36);
            this.comboProduct.Name = "comboProduct";
            this.comboProduct.Size = new System.Drawing.Size(306, 21);
            this.comboProduct.Sorted = true;
            this.comboProduct.TabIndex = 6;
            this.comboProduct.SelectedIndexChanged += new System.EventHandler(this.comboProduct_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(565, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(83, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Document Type";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(315, 19);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Discipline";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Product";
            // 
            // comboBoxDocType
            // 
            this.comboBoxDocType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxDocType.FormattingEnabled = true;
            this.comboBoxDocType.Location = new System.Drawing.Point(557, 36);
            this.comboBoxDocType.Name = "comboBoxDocType";
            this.comboBoxDocType.Size = new System.Drawing.Size(282, 21);
            this.comboBoxDocType.Sorted = true;
            this.comboBoxDocType.TabIndex = 2;
            // 
            // comboBoxCoPs
            // 
            this.comboBoxCoPs.FormattingEnabled = true;
            this.comboBoxCoPs.Location = new System.Drawing.Point(315, 36);
            this.comboBoxCoPs.Name = "comboBoxCoPs";
            this.comboBoxCoPs.Size = new System.Drawing.Size(236, 21);
            this.comboBoxCoPs.TabIndex = 1;
            this.comboBoxCoPs.SelectedIndexChanged += new System.EventHandler(this.comboBoxCoPs_SelectedIndexChanged_1);
            this.comboBoxCoPs.TextChanged += new System.EventHandler(this.comboBoxCoPs_TextChanged);
            // 
            // comboBoxType
            // 
            this.comboBoxType.Enabled = false;
            this.comboBoxType.FormattingEnabled = true;
            this.comboBoxType.Items.AddRange(new object[] {
            "CoP",
            "Folder"});
            this.comboBoxType.Location = new System.Drawing.Point(3, 36);
            this.comboBoxType.Name = "comboBoxType";
            this.comboBoxType.Size = new System.Drawing.Size(10, 21);
            this.comboBoxType.TabIndex = 0;
            this.comboBoxType.Visible = false;
            // 
            // multiFileList1
            // 
            this.multiFileList1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.multiFileList1.Data = null;
            this.multiFileList1.Location = new System.Drawing.Point(4, 385);
            this.multiFileList1.Name = "multiFileList1";
            this.multiFileList1.Size = new System.Drawing.Size(847, 103);
            this.multiFileList1.TabIndex = 6;
            this.multiFileList1.Text = "multiFileList1";
            // 
            // buttonsAndStatus1
            // 
            this.buttonsAndStatus1.ActionButtonText = "Create";
            this.buttonsAndStatus1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonsAndStatus1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.buttonsAndStatus1.Location = new System.Drawing.Point(0, 494);
            this.buttonsAndStatus1.Name = "buttonsAndStatus1";
            this.buttonsAndStatus1.Size = new System.Drawing.Size(855, 46);
            this.buttonsAndStatus1.TabIndex = 5;
            // 
            // DocumentWizard2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(855, 562);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.multiFileList1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.buttonsAndStatus1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.tabControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(871, 601);
            this.Name = "DocumentWizard2";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Document Wizard";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem filterDocumentTypesToolStripMenuItem;
        private System.Windows.Forms.ToolStripComboBox toolStripComboFilterType;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem filenameMatchesDocumentNameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem autoAppendForDuplicateNamesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem useOriginalDescriptionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem useSameNameForAllFilesAppendToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private Controls.ButtonsAndStatus buttonsAndStatus1;
        private Controls.DocumentName2 documentName21;
        private System.Windows.Forms.ToolStripMenuItem filterTypeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem coPToolStripMenuItem;
        private System.Windows.Forms.ToolStripComboBox toolStripComboCoP;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem documentTypeToolStripMenuItem;
        private System.Windows.Forms.ToolStripComboBox toolStripComboBox3;
        private Controls.MultiFileList multiFileList1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripMenuItem validateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem useFileLastModifiedDateToolStripMenuItem;
        private Controls.Description description1;
        private Controls.Keywords keywords1;
        private Controls.Department department1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem changeDestinationToolStripMenuItem;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.ToolStripMenuItem testToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBoxDocType;
        private System.Windows.Forms.ComboBox comboBoxCoPs;
        private System.Windows.Forms.ComboBox comboBoxType;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboProduct;
    }

}