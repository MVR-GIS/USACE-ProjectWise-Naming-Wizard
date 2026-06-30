namespace USACE_Wizard_GUI.Controls
{
    partial class FilterDocumentTypeBy
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
            this.components = new System.ComponentModel.Container();
            this.groupBoxFilterDocTypeBy = new System.Windows.Forms.GroupBox();
            this.radioButtonCoP = new System.Windows.Forms.RadioButton();
            this.radioButtonFolder = new System.Windows.Forms.RadioButton();
            this.radioButtonAll = new System.Windows.Forms.RadioButton();
            this.comboBoxCoP = new System.Windows.Forms.ComboBox();
            this.toolTipFilterDocTypeBy = new System.Windows.Forms.ToolTip(this.components);
            this.groupBoxDocumentTypes = new System.Windows.Forms.GroupBox();
            this.comboBoxDocTypes = new System.Windows.Forms.ComboBox();
            this.groupBoxFilterDocTypeBy.SuspendLayout();
            this.groupBoxDocumentTypes.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxFilterDocTypeBy
            // 
            this.groupBoxFilterDocTypeBy.Controls.Add(this.radioButtonCoP);
            this.groupBoxFilterDocTypeBy.Controls.Add(this.radioButtonFolder);
            this.groupBoxFilterDocTypeBy.Controls.Add(this.radioButtonAll);
            this.groupBoxFilterDocTypeBy.Controls.Add(this.comboBoxCoP);
            this.groupBoxFilterDocTypeBy.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxFilterDocTypeBy.Location = new System.Drawing.Point(3, 3);
            this.groupBoxFilterDocTypeBy.Name = "groupBoxFilterDocTypeBy";
            this.groupBoxFilterDocTypeBy.Padding = new System.Windows.Forms.Padding(0);
            this.groupBoxFilterDocTypeBy.Size = new System.Drawing.Size(212, 85);
            this.groupBoxFilterDocTypeBy.TabIndex = 0;
            this.groupBoxFilterDocTypeBy.TabStop = false;
            this.groupBoxFilterDocTypeBy.Text = "Filter Document Type By";
            // 
            // radioButtonCoP
            // 
            this.radioButtonCoP.AutoSize = true;
            this.radioButtonCoP.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButtonCoP.Location = new System.Drawing.Point(3, 22);
            this.radioButtonCoP.Margin = new System.Windows.Forms.Padding(0);
            this.radioButtonCoP.Name = "radioButtonCoP";
            this.radioButtonCoP.Size = new System.Drawing.Size(45, 17);
            this.radioButtonCoP.TabIndex = 0;
            this.radioButtonCoP.TabStop = true;
            this.radioButtonCoP.Text = "CoP";
            this.toolTipFilterDocTypeBy.SetToolTip(this.radioButtonCoP, "Filter document types by Community of Practice (CoP)");
            this.radioButtonCoP.UseVisualStyleBackColor = true;
            this.radioButtonCoP.CheckedChanged += new System.EventHandler(this.radioButtonCoP_CheckedChanged);
            this.radioButtonCoP.Click += new System.EventHandler(this.FilterChanged);
            // 
            // radioButtonFolder
            // 
            this.radioButtonFolder.AutoSize = true;
            this.radioButtonFolder.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButtonFolder.Location = new System.Drawing.Point(3, 43);
            this.radioButtonFolder.Margin = new System.Windows.Forms.Padding(0);
            this.radioButtonFolder.Name = "radioButtonFolder";
            this.radioButtonFolder.Size = new System.Drawing.Size(54, 17);
            this.radioButtonFolder.TabIndex = 1;
            this.radioButtonFolder.TabStop = true;
            this.radioButtonFolder.Text = "Folder";
            this.toolTipFilterDocTypeBy.SetToolTip(this.radioButtonFolder, "Filter document type by the current folder");
            this.radioButtonFolder.UseVisualStyleBackColor = true;
            this.radioButtonFolder.Click += new System.EventHandler(this.FilterChanged);
            // 
            // radioButtonAll
            // 
            this.radioButtonAll.AutoSize = true;
            this.radioButtonAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButtonAll.Location = new System.Drawing.Point(3, 64);
            this.radioButtonAll.Margin = new System.Windows.Forms.Padding(0);
            this.radioButtonAll.Name = "radioButtonAll";
            this.radioButtonAll.Size = new System.Drawing.Size(66, 17);
            this.radioButtonAll.TabIndex = 2;
            this.radioButtonAll.TabStop = true;
            this.radioButtonAll.Text = "Show All";
            this.toolTipFilterDocTypeBy.SetToolTip(this.radioButtonAll, "Show all document types");
            this.radioButtonAll.UseVisualStyleBackColor = true;
            this.radioButtonAll.Click += new System.EventHandler(this.FilterChanged);
            // 
            // comboBoxCoP
            // 
            this.comboBoxCoP.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxCoP.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxCoP.Enabled = false;
            this.comboBoxCoP.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxCoP.FormattingEnabled = true;
            this.comboBoxCoP.Location = new System.Drawing.Point(67, 22);
            this.comboBoxCoP.Margin = new System.Windows.Forms.Padding(0);
            this.comboBoxCoP.Name = "comboBoxCoP";
            this.comboBoxCoP.Size = new System.Drawing.Size(141, 21);
            this.comboBoxCoP.TabIndex = 3;
            this.comboBoxCoP.SelectedValueChanged += new System.EventHandler(this.FilterChanged);
            // 
            // toolTipFilterDocTypeBy
            // 
            this.toolTipFilterDocTypeBy.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            // 
            // groupBoxDocumentTypes
            // 
            this.groupBoxDocumentTypes.Controls.Add(this.comboBoxDocTypes);
            this.groupBoxDocumentTypes.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxDocumentTypes.Location = new System.Drawing.Point(221, 3);
            this.groupBoxDocumentTypes.Name = "groupBoxDocumentTypes";
            this.groupBoxDocumentTypes.Padding = new System.Windows.Forms.Padding(0);
            this.groupBoxDocumentTypes.Size = new System.Drawing.Size(187, 85);
            this.groupBoxDocumentTypes.TabIndex = 1;
            this.groupBoxDocumentTypes.TabStop = false;
            this.groupBoxDocumentTypes.Text = "Document Types";
            // 
            // comboBoxDocTypes
            // 
            this.comboBoxDocTypes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxDocTypes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxDocTypes.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxDocTypes.FormattingEnabled = true;
            this.comboBoxDocTypes.Location = new System.Drawing.Point(3, 22);
            this.comboBoxDocTypes.Name = "comboBoxDocTypes";
            this.comboBoxDocTypes.Size = new System.Drawing.Size(181, 21);
            this.comboBoxDocTypes.TabIndex = 0;
            this.comboBoxDocTypes.SelectedValueChanged += new System.EventHandler(this.comboBoxDocTypes_SelectedValueChanged);
            // 
            // FilterDocumentTypeBy
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxDocumentTypes);
            this.Controls.Add(this.groupBoxFilterDocTypeBy);
            this.MaximumSize = new System.Drawing.Size(0, 91);
            this.MinimumSize = new System.Drawing.Size(364, 91);
            this.Name = "FilterDocumentTypeBy";
            this.Size = new System.Drawing.Size(417, 91);
            this.groupBoxFilterDocTypeBy.ResumeLayout(false);
            this.groupBoxFilterDocTypeBy.PerformLayout();
            this.groupBoxDocumentTypes.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxFilterDocTypeBy;
        private System.Windows.Forms.RadioButton radioButtonCoP;
        private System.Windows.Forms.RadioButton radioButtonFolder;
        private System.Windows.Forms.RadioButton radioButtonAll;
        private System.Windows.Forms.ComboBox comboBoxCoP;
        private System.Windows.Forms.ToolTip toolTipFilterDocTypeBy;
        private System.Windows.Forms.GroupBox groupBoxDocumentTypes;
        private System.Windows.Forms.ComboBox comboBoxDocTypes;
    }
}
