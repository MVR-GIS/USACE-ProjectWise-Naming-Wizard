namespace USACE_Wizard_GUI
{
    partial class UsaceDcwGui
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UsaceDcwGui));
            this.btnCreate = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblDescription = new System.Windows.Forms.Label();
            this.txtBoxDocumentDescription = new System.Windows.Forms.TextBox();
            this.lblDocumentName = new System.Windows.Forms.Label();
            this.txtBoxDocName = new System.Windows.Forms.TextBox();
            this.comboBoxProgram = new System.Windows.Forms.ComboBox();
            this.lblProgram = new System.Windows.Forms.Label();
            this.gbFilterBy = new System.Windows.Forms.GroupBox();
            this.rbAll = new System.Windows.Forms.RadioButton();
            this.rbFolderName = new System.Windows.Forms.RadioButton();
            this.comboBoxCop = new System.Windows.Forms.ComboBox();
            this.rbCoP = new System.Windows.Forms.RadioButton();
            this.comboBoxDocFilter = new System.Windows.Forms.ComboBox();
            this.lblDocFilter = new System.Windows.Forms.Label();
            this.dtPickerDate = new System.Windows.Forms.DateTimePicker();
            this.lblDate = new System.Windows.Forms.Label();
            this.txtBoxProjectNumber = new System.Windows.Forms.TextBox();
            this.lblProjectNumber = new System.Windows.Forms.Label();
            this.comboBoxDocType = new System.Windows.Forms.ComboBox();
            this.lblDocType = new System.Windows.Forms.Label();
            this.txtBoxUserAdded = new System.Windows.Forms.TextBox();
            this.lblUserAdded = new System.Windows.Forms.Label();
            this.txtBoxDocTypeDescription = new System.Windows.Forms.TextBox();
            this.lblDocTypeDescription = new System.Windows.Forms.Label();
            this.bntSelectNewDest = new System.Windows.Forms.Button();
            this.txtBoxDestPath = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkBoxMatchNewFileName = new System.Windows.Forms.CheckBox();
            this.txtBoxKeyword = new System.Windows.Forms.TextBox();
            this.lblKeyword = new System.Windows.Forms.Label();
            this.comboBoxDisciplineAttribute = new System.Windows.Forms.ComboBox();
            this.lblDisciplineAttribute = new System.Windows.Forms.Label();
            this.comboBoxStatus = new System.Windows.Forms.ComboBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblNewFileNameValue = new System.Windows.Forms.Label();
            this.lblNewFileNameLabel = new System.Windows.Forms.Label();
            this.lblFileDest = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.checkBoxUseDocFilter = new System.Windows.Forms.CheckBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.gbFilterBy.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCreate
            // 
            this.btnCreate.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnCreate.Enabled = false;
            this.btnCreate.Location = new System.Drawing.Point(787, 310);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(99, 52);
            this.btnCreate.TabIndex = 1;
            this.btnCreate.Text = "Create";
            this.btnCreate.UseVisualStyleBackColor = true;
            this.btnCreate.Click += new System.EventHandler(this.BtnCreateClick);
            this.btnCreate.MouseHover += new System.EventHandler(this.btnCreate_MouseHover);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(892, 310);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(73, 52);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancelClick);
            // 
            // lblDescription
            // 
            this.lblDescription.AutoSize = true;
            this.lblDescription.Location = new System.Drawing.Point(6, 87);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(60, 13);
            this.lblDescription.TabIndex = 6;
            this.lblDescription.Text = "Description";
            // 
            // txtBoxDocumentDescription
            // 
            this.txtBoxDocumentDescription.Location = new System.Drawing.Point(9, 106);
            this.txtBoxDocumentDescription.Multiline = true;
            this.txtBoxDocumentDescription.Name = "txtBoxDocumentDescription";
            this.txtBoxDocumentDescription.Size = new System.Drawing.Size(233, 68);
            this.txtBoxDocumentDescription.TabIndex = 7;
            // 
            // lblDocumentName
            // 
            this.lblDocumentName.AutoSize = true;
            this.lblDocumentName.Location = new System.Drawing.Point(6, 43);
            this.lblDocumentName.Name = "lblDocumentName";
            this.lblDocumentName.Size = new System.Drawing.Size(92, 13);
            this.lblDocumentName.TabIndex = 9;
            this.lblDocumentName.Text = "Original File Name";
            // 
            // txtBoxDocName
            // 
            this.txtBoxDocName.Location = new System.Drawing.Point(9, 61);
            this.txtBoxDocName.Name = "txtBoxDocName";
            this.txtBoxDocName.Size = new System.Drawing.Size(233, 20);
            this.txtBoxDocName.TabIndex = 8;
            this.txtBoxDocName.TextChanged += new System.EventHandler(this.txtBoxDocName_TextChanged);
            // 
            // comboBoxProgram
            // 
            this.comboBoxProgram.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxProgram.FormattingEnabled = true;
            this.comboBoxProgram.Location = new System.Drawing.Point(173, 358);
            this.comboBoxProgram.Name = "comboBoxProgram";
            this.comboBoxProgram.Size = new System.Drawing.Size(142, 21);
            this.comboBoxProgram.TabIndex = 10;
            this.comboBoxProgram.Visible = false;
            // 
            // lblProgram
            // 
            this.lblProgram.AutoSize = true;
            this.lblProgram.Location = new System.Drawing.Point(174, 339);
            this.lblProgram.Name = "lblProgram";
            this.lblProgram.Size = new System.Drawing.Size(46, 13);
            this.lblProgram.TabIndex = 11;
            this.lblProgram.Text = "Program";
            this.lblProgram.Visible = false;
            // 
            // gbFilterBy
            // 
            this.gbFilterBy.Controls.Add(this.rbAll);
            this.gbFilterBy.Controls.Add(this.rbFolderName);
            this.gbFilterBy.Controls.Add(this.comboBoxCop);
            this.gbFilterBy.Controls.Add(this.rbCoP);
            this.gbFilterBy.Location = new System.Drawing.Point(160, 9);
            this.gbFilterBy.Name = "gbFilterBy";
            this.gbFilterBy.Size = new System.Drawing.Size(332, 101);
            this.gbFilterBy.TabIndex = 12;
            this.gbFilterBy.TabStop = false;
            this.gbFilterBy.Text = "Filter Document Type By";
            // 
            // rbAll
            // 
            this.rbAll.AutoSize = true;
            this.rbAll.Location = new System.Drawing.Point(20, 74);
            this.rbAll.Name = "rbAll";
            this.rbAll.Size = new System.Drawing.Size(90, 17);
            this.rbAll.TabIndex = 3;
            this.rbAll.Text = "All (Unfiltered)";
            this.rbAll.UseVisualStyleBackColor = true;
            this.rbAll.CheckedChanged += new System.EventHandler(this.RadioButtonsCheckedChanged);
            // 
            // rbFolderName
            // 
            this.rbFolderName.AutoSize = true;
            this.rbFolderName.Enabled = false;
            this.rbFolderName.Location = new System.Drawing.Point(20, 51);
            this.rbFolderName.Name = "rbFolderName";
            this.rbFolderName.Size = new System.Drawing.Size(85, 17);
            this.rbFolderName.TabIndex = 2;
            this.rbFolderName.Text = "Folder Name";
            this.rbFolderName.UseVisualStyleBackColor = true;
            this.rbFolderName.CheckedChanged += new System.EventHandler(this.RadioButtonsCheckedChanged);
            // 
            // comboBoxCop
            // 
            this.comboBoxCop.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxCop.FormattingEnabled = true;
            this.comboBoxCop.Location = new System.Drawing.Point(107, 25);
            this.comboBoxCop.Name = "comboBoxCop";
            this.comboBoxCop.Size = new System.Drawing.Size(215, 21);
            this.comboBoxCop.TabIndex = 1;
            // 
            // rbCoP
            // 
            this.rbCoP.AutoSize = true;
            this.rbCoP.Checked = true;
            this.rbCoP.Location = new System.Drawing.Point(20, 28);
            this.rbCoP.Name = "rbCoP";
            this.rbCoP.Size = new System.Drawing.Size(45, 17);
            this.rbCoP.TabIndex = 0;
            this.rbCoP.TabStop = true;
            this.rbCoP.Text = "CoP";
            this.rbCoP.UseVisualStyleBackColor = true;
            this.rbCoP.CheckedChanged += new System.EventHandler(this.RadioButtonsCheckedChanged);
            // 
            // comboBoxDocFilter
            // 
            this.comboBoxDocFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxDocFilter.Enabled = false;
            this.comboBoxDocFilter.FormattingEnabled = true;
            this.comboBoxDocFilter.Location = new System.Drawing.Point(269, 136);
            this.comboBoxDocFilter.Name = "comboBoxDocFilter";
            this.comboBoxDocFilter.Size = new System.Drawing.Size(213, 21);
            this.comboBoxDocFilter.TabIndex = 35;
            this.comboBoxDocFilter.SelectedIndexChanged += new System.EventHandler(this.comboBoxDocFilter_SelectedIndexChanged);
            // 
            // lblDocFilter
            // 
            this.lblDocFilter.AutoSize = true;
            this.lblDocFilter.Location = new System.Drawing.Point(179, 140);
            this.lblDocFilter.Name = "lblDocFilter";
            this.lblDocFilter.Size = new System.Drawing.Size(81, 13);
            this.lblDocFilter.TabIndex = 34;
            this.lblDocFilter.Text = "Document Filter";
            // 
            // dtPickerDate
            // 
            this.dtPickerDate.CustomFormat = "yyyy-MM-dd";
            this.dtPickerDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtPickerDate.Location = new System.Drawing.Point(381, 188);
            this.dtPickerDate.Name = "dtPickerDate";
            this.dtPickerDate.Size = new System.Drawing.Size(128, 20);
            this.dtPickerDate.TabIndex = 13;
            this.dtPickerDate.ValueChanged += new System.EventHandler(this.DtPickerDateValueChanged);
            // 
            // lblDate
            // 
            this.lblDate.AutoSize = true;
            this.lblDate.Location = new System.Drawing.Point(381, 170);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(37, 13);
            this.lblDate.TabIndex = 14;
            this.lblDate.Text = "Date *";
            // 
            // txtBoxProjectNumber
            // 
            this.txtBoxProjectNumber.Enabled = false;
            this.txtBoxProjectNumber.Location = new System.Drawing.Point(12, 187);
            this.txtBoxProjectNumber.Name = "txtBoxProjectNumber";
            this.txtBoxProjectNumber.Size = new System.Drawing.Size(108, 20);
            this.txtBoxProjectNumber.TabIndex = 15;
            // 
            // lblProjectNumber
            // 
            this.lblProjectNumber.AutoSize = true;
            this.lblProjectNumber.Location = new System.Drawing.Point(12, 170);
            this.lblProjectNumber.Name = "lblProjectNumber";
            this.lblProjectNumber.Size = new System.Drawing.Size(87, 13);
            this.lblProjectNumber.TabIndex = 16;
            this.lblProjectNumber.Text = "Project Number *";
            // 
            // comboBoxDocType
            // 
            this.comboBoxDocType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxDocType.FormattingEnabled = true;
            this.comboBoxDocType.Location = new System.Drawing.Point(130, 187);
            this.comboBoxDocType.Name = "comboBoxDocType";
            this.comboBoxDocType.Size = new System.Drawing.Size(245, 21);
            this.comboBoxDocType.TabIndex = 17;
            // 
            // lblDocType
            // 
            this.lblDocType.AutoSize = true;
            this.lblDocType.Location = new System.Drawing.Point(130, 170);
            this.lblDocType.Name = "lblDocType";
            this.lblDocType.Size = new System.Drawing.Size(90, 13);
            this.lblDocType.TabIndex = 18;
            this.lblDocType.Text = "Document Type *";
            // 
            // txtBoxUserAdded
            // 
            this.txtBoxUserAdded.Location = new System.Drawing.Point(514, 188);
            this.txtBoxUserAdded.MaxLength = 25;
            this.txtBoxUserAdded.Name = "txtBoxUserAdded";
            this.txtBoxUserAdded.Size = new System.Drawing.Size(196, 20);
            this.txtBoxUserAdded.TabIndex = 19;
            this.txtBoxUserAdded.TextChanged += new System.EventHandler(this.txtBoxUserAdded_TextChanged);
            this.txtBoxUserAdded.Leave += new System.EventHandler(this.TxtBoxUserAddedLeave);
            // 
            // lblUserAdded
            // 
            this.lblUserAdded.AutoSize = true;
            this.lblUserAdded.Location = new System.Drawing.Point(514, 170);
            this.lblUserAdded.Name = "lblUserAdded";
            this.lblUserAdded.Size = new System.Drawing.Size(63, 13);
            this.lblUserAdded.TabIndex = 20;
            this.lblUserAdded.Text = "User Added";
            // 
            // txtBoxDocTypeDescription
            // 
            this.txtBoxDocTypeDescription.Enabled = false;
            this.txtBoxDocTypeDescription.Location = new System.Drawing.Point(133, 246);
            this.txtBoxDocTypeDescription.Multiline = true;
            this.txtBoxDocTypeDescription.Name = "txtBoxDocTypeDescription";
            this.txtBoxDocTypeDescription.Size = new System.Drawing.Size(447, 57);
            this.txtBoxDocTypeDescription.TabIndex = 21;
            // 
            // lblDocTypeDescription
            // 
            this.lblDocTypeDescription.AutoSize = true;
            this.lblDocTypeDescription.Location = new System.Drawing.Point(130, 228);
            this.lblDocTypeDescription.Name = "lblDocTypeDescription";
            this.lblDocTypeDescription.Size = new System.Drawing.Size(184, 13);
            this.lblDocTypeDescription.TabIndex = 22;
            this.lblDocTypeDescription.Text = "Selected Document Type Description";
            // 
            // bntSelectNewDest
            // 
            this.bntSelectNewDest.Location = new System.Drawing.Point(569, 106);
            this.bntSelectNewDest.Name = "bntSelectNewDest";
            this.bntSelectNewDest.Size = new System.Drawing.Size(142, 42);
            this.bntSelectNewDest.TabIndex = 24;
            this.bntSelectNewDest.Text = "Select New Destination";
            this.bntSelectNewDest.UseVisualStyleBackColor = true;
            this.bntSelectNewDest.Click += new System.EventHandler(this.BtnSelectNewDestClick);
            // 
            // txtBoxDestPath
            // 
            this.txtBoxDestPath.Enabled = false;
            this.txtBoxDestPath.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxDestPath.Location = new System.Drawing.Point(498, 26);
            this.txtBoxDestPath.Multiline = true;
            this.txtBoxDestPath.Name = "txtBoxDestPath";
            this.txtBoxDestPath.Size = new System.Drawing.Size(213, 74);
            this.txtBoxDestPath.TabIndex = 26;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkBoxMatchNewFileName);
            this.groupBox1.Controls.Add(this.txtBoxKeyword);
            this.groupBox1.Controls.Add(this.lblKeyword);
            this.groupBox1.Controls.Add(this.lblDocumentName);
            this.groupBox1.Controls.Add(this.lblDescription);
            this.groupBox1.Controls.Add(this.txtBoxDocumentDescription);
            this.groupBox1.Controls.Add(this.txtBoxDocName);
            this.groupBox1.Location = new System.Drawing.Point(717, 9);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(248, 277);
            this.groupBox1.TabIndex = 27;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Attributes";
            // 
            // checkBoxMatchNewFileName
            // 
            this.checkBoxMatchNewFileName.AutoSize = true;
            this.checkBoxMatchNewFileName.Location = new System.Drawing.Point(9, 20);
            this.checkBoxMatchNewFileName.Name = "checkBoxMatchNewFileName";
            this.checkBoxMatchNewFileName.Size = new System.Drawing.Size(131, 17);
            this.checkBoxMatchNewFileName.TabIndex = 16;
            this.checkBoxMatchNewFileName.Text = "Match New File Name";
            this.checkBoxMatchNewFileName.UseVisualStyleBackColor = true;
            this.checkBoxMatchNewFileName.CheckedChanged += new System.EventHandler(this.checkBoxMatchNewFileName_CheckedChanged);
            // 
            // txtBoxKeyword
            // 
            this.txtBoxKeyword.Location = new System.Drawing.Point(9, 196);
            this.txtBoxKeyword.Multiline = true;
            this.txtBoxKeyword.Name = "txtBoxKeyword";
            this.txtBoxKeyword.Size = new System.Drawing.Size(233, 66);
            this.txtBoxKeyword.TabIndex = 15;
            // 
            // lblKeyword
            // 
            this.lblKeyword.AutoSize = true;
            this.lblKeyword.Location = new System.Drawing.Point(6, 180);
            this.lblKeyword.Name = "lblKeyword";
            this.lblKeyword.Size = new System.Drawing.Size(48, 13);
            this.lblKeyword.TabIndex = 14;
            this.lblKeyword.Text = "Keyword";
            // 
            // comboBoxDisciplineAttribute
            // 
            this.comboBoxDisciplineAttribute.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxDisciplineAttribute.FormattingEnabled = true;
            this.comboBoxDisciplineAttribute.Location = new System.Drawing.Point(368, 369);
            this.comboBoxDisciplineAttribute.Name = "comboBoxDisciplineAttribute";
            this.comboBoxDisciplineAttribute.Size = new System.Drawing.Size(233, 21);
            this.comboBoxDisciplineAttribute.TabIndex = 13;
            this.comboBoxDisciplineAttribute.Visible = false;
            // 
            // lblDisciplineAttribute
            // 
            this.lblDisciplineAttribute.AutoSize = true;
            this.lblDisciplineAttribute.Location = new System.Drawing.Point(365, 353);
            this.lblDisciplineAttribute.Name = "lblDisciplineAttribute";
            this.lblDisciplineAttribute.Size = new System.Drawing.Size(52, 13);
            this.lblDisciplineAttribute.TabIndex = 12;
            this.lblDisciplineAttribute.Text = "Discipline";
            this.lblDisciplineAttribute.Visible = false;
            // 
            // comboBoxStatus
            // 
            this.comboBoxStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxStatus.FormattingEnabled = true;
            this.comboBoxStatus.Location = new System.Drawing.Point(368, 326);
            this.comboBoxStatus.Name = "comboBoxStatus";
            this.comboBoxStatus.Size = new System.Drawing.Size(233, 21);
            this.comboBoxStatus.TabIndex = 11;
            this.comboBoxStatus.Visible = false;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(365, 310);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(37, 13);
            this.lblStatus.TabIndex = 10;
            this.lblStatus.Text = "Status";
            this.lblStatus.Visible = false;
            // 
            // lblNewFileNameValue
            // 
            this.lblNewFileNameValue.AutoSize = true;
            this.lblNewFileNameValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNewFileNameValue.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lblNewFileNameValue.Location = new System.Drawing.Point(8, 366);
            this.lblNewFileNameValue.Name = "lblNewFileNameValue";
            this.lblNewFileNameValue.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblNewFileNameValue.Size = new System.Drawing.Size(69, 20);
            this.lblNewFileNameValue.TabIndex = 28;
            this.lblNewFileNameValue.Text = "123456";
            // 
            // lblNewFileNameLabel
            // 
            this.lblNewFileNameLabel.AutoSize = true;
            this.lblNewFileNameLabel.Location = new System.Drawing.Point(9, 349);
            this.lblNewFileNameLabel.Name = "lblNewFileNameLabel";
            this.lblNewFileNameLabel.Size = new System.Drawing.Size(79, 13);
            this.lblNewFileNameLabel.TabIndex = 29;
            this.lblNewFileNameLabel.Text = "New File Name";
            // 
            // lblFileDest
            // 
            this.lblFileDest.AutoSize = true;
            this.lblFileDest.Location = new System.Drawing.Point(495, 9);
            this.lblFileDest.Name = "lblFileDest";
            this.lblFileDest.Size = new System.Drawing.Size(60, 13);
            this.lblFileDest.TabIndex = 30;
            this.lblFileDest.Text = "Destination";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::USACE_Wizard_GUI.Properties.Resources.usace108x81;
            this.pictureBox1.InitialImage = null;
            this.pictureBox1.Location = new System.Drawing.Point(15, 26);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(108, 81);
            this.pictureBox1.TabIndex = 31;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(831, 289);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(130, 13);
            this.label1.TabIndex = 32;
            this.label1.Text = "* indicates mandatory field";
            // 
            // checkBoxUseDocFilter
            // 
            this.checkBoxUseDocFilter.AutoSize = true;
            this.checkBoxUseDocFilter.Location = new System.Drawing.Point(162, 117);
            this.checkBoxUseDocFilter.Name = "checkBoxUseDocFilter";
            this.checkBoxUseDocFilter.Size = new System.Drawing.Size(122, 17);
            this.checkBoxUseDocFilter.TabIndex = 36;
            this.checkBoxUseDocFilter.Text = "Use Document Filter";
            this.checkBoxUseDocFilter.UseVisualStyleBackColor = true;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 399);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.statusStrip1.Size = new System.Drawing.Size(977, 22);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 37;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
            // 
            // UsaceDcwGui
            // 
            this.ClientSize = new System.Drawing.Size(977, 421);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.checkBoxUseDocFilter);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBoxDocFilter);
            this.Controls.Add(this.comboBoxDisciplineAttribute);
            this.Controls.Add(this.lblDocFilter);
            this.Controls.Add(this.lblDisciplineAttribute);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.comboBoxStatus);
            this.Controls.Add(this.lblFileDest);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.lblNewFileNameLabel);
            this.Controls.Add(this.lblNewFileNameValue);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.txtBoxDestPath);
            this.Controls.Add(this.bntSelectNewDest);
            this.Controls.Add(this.lblDocTypeDescription);
            this.Controls.Add(this.txtBoxDocTypeDescription);
            this.Controls.Add(this.lblUserAdded);
            this.Controls.Add(this.txtBoxUserAdded);
            this.Controls.Add(this.lblDocType);
            this.Controls.Add(this.comboBoxDocType);
            this.Controls.Add(this.lblProjectNumber);
            this.Controls.Add(this.txtBoxProjectNumber);
            this.Controls.Add(this.lblDate);
            this.Controls.Add(this.dtPickerDate);
            this.Controls.Add(this.gbFilterBy);
            this.Controls.Add(this.lblProgram);
            this.Controls.Add(this.comboBoxProgram);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnCreate);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(993, 460);
            this.MinimumSize = new System.Drawing.Size(993, 460);
            this.Name = "UsaceDcwGui";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "USACE Document Creation Wizard";
            this.gbFilterBy.ResumeLayout(false);
            this.gbFilterBy.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCreate;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.TextBox txtBoxDocumentDescription;
        private System.Windows.Forms.Label lblDocumentName;
        private System.Windows.Forms.TextBox txtBoxDocName;
        private System.Windows.Forms.ComboBox comboBoxProgram;
        private System.Windows.Forms.Label lblProgram;
        private System.Windows.Forms.GroupBox gbFilterBy;
        private System.Windows.Forms.RadioButton rbAll;
        private System.Windows.Forms.RadioButton rbFolderName;
        private System.Windows.Forms.ComboBox comboBoxCop;
        private System.Windows.Forms.RadioButton rbCoP;
        private System.Windows.Forms.DateTimePicker dtPickerDate;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.TextBox txtBoxProjectNumber;
        private System.Windows.Forms.Label lblProjectNumber;
        private System.Windows.Forms.ComboBox comboBoxDocType;
        private System.Windows.Forms.Label lblDocType;
        private System.Windows.Forms.TextBox txtBoxUserAdded;
        private System.Windows.Forms.Label lblUserAdded;
        private System.Windows.Forms.TextBox txtBoxDocTypeDescription;
        private System.Windows.Forms.Label lblDocTypeDescription;
        private System.Windows.Forms.Button bntSelectNewDest;
        private System.Windows.Forms.TextBox txtBoxDestPath;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblNewFileNameValue;
        private System.Windows.Forms.Label lblNewFileNameLabel;
        private System.Windows.Forms.Label lblFileDest;
        private System.Windows.Forms.TextBox txtBoxKeyword;
        private System.Windows.Forms.Label lblKeyword;
        private System.Windows.Forms.ComboBox comboBoxDisciplineAttribute;
        private System.Windows.Forms.Label lblDisciplineAttribute;
        private System.Windows.Forms.ComboBox comboBoxStatus;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ComboBox comboBoxDocFilter;
        private System.Windows.Forms.Label lblDocFilter;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkBoxUseDocFilter;
        private System.Windows.Forms.CheckBox checkBoxMatchNewFileName;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
    }
}