namespace USACE_Wizard_GUI
{
    partial class UsaceDcwGui2
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UsaceDcwGui2));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.filterDocumentTypeBy1 = new USACE_Wizard_GUI.Controls.FilterDocumentTypeBy();
            this.destination1 = new USACE_Wizard_GUI.Controls.Destination();
            this.documentName1 = new USACE_Wizard_GUI.Controls.DocumentName();
            this.buttonsAndStatus1 = new USACE_Wizard_GUI.Controls.ButtonsAndStatus();
            this.attributes1 = new USACE_Wizard_GUI.Controls.Attributes();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 247F));
            this.tableLayoutPanel1.Controls.Add(this.filterDocumentTypeBy1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.destination1, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.documentName1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.buttonsAndStatus1, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.attributes1, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 118F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 290F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(981, 530);
            this.tableLayoutPanel1.TabIndex = 4;
            // 
            // filterDocumentTypeBy1
            // 
            this.filterDocumentTypeBy1.CoP = "";
            this.filterDocumentTypeBy1.CoPDataTable = null;
            this.filterDocumentTypeBy1.CoPDisplayMember = null;
            this.filterDocumentTypeBy1.CoPValueMember = null;
            this.filterDocumentTypeBy1.DocFilterDataTable = null;
            this.filterDocumentTypeBy1.DocFilterDisplayMember = null;
            this.filterDocumentTypeBy1.DocFilterValueMember = null;
            this.filterDocumentTypeBy1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.filterDocumentTypeBy1.DocumentType = "";
            this.filterDocumentTypeBy1.Filter = USACE_Wizard_GUI.Controls.FilterDocumentTypeBy.FilterType.All;
            this.filterDocumentTypeBy1.Location = new System.Drawing.Point(3, 3);
            this.filterDocumentTypeBy1.Name = "filterDocumentTypeBy1";
            this.filterDocumentTypeBy1.Padding = new System.Windows.Forms.Padding(5);
            this.filterDocumentTypeBy1.ProjectID = 0;
            this.filterDocumentTypeBy1.Size = new System.Drawing.Size(728, 112);
            this.filterDocumentTypeBy1.TabIndex = 2;
            this.filterDocumentTypeBy1.DocumentTypeChanged += new System.EventHandler(this.filterDocumentTypeBy1_DocumentTypeChanged);
            // 
            // destination1
            // 
            this.destination1.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.destination1, 2);
            this.destination1.DestinationPath = "";
            this.destination1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.destination1.Location = new System.Drawing.Point(3, 411);
            this.destination1.Name = "destination1";
            this.destination1.Padding = new System.Windows.Forms.Padding(5);
            this.destination1.ReadOnly = false;
            this.destination1.Size = new System.Drawing.Size(975, 64);
            this.destination1.TabIndex = 1;
            this.destination1.DestinationPathChanged += new System.EventHandler(this.destination1_DestinationPathChanged);
            // 
            // documentName1
            // 
            this.documentName1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.documentName1.DocTypeDataView = null;
            this.documentName1.HighlightDocumentFilename = false;
            this.documentName1.HighlightDocumentName = false;
            this.documentName1.HighlightDocumentType = false;
            this.documentName1.HighlightProjectID = false;
            this.documentName1.Location = new System.Drawing.Point(3, 121);
            this.documentName1.Name = "documentName1";
            this.documentName1.NewFileData = null;
            this.documentName1.ProjectProperties = null;
            this.documentName1.Size = new System.Drawing.Size(728, 284);
            this.documentName1.TabIndex = 3;
            this.documentName1.NameChanged += new System.EventHandler(this.documentName1_NameChanged);
            // 
            // buttonsAndStatus1
            // 
            this.buttonsAndStatus1.ActionButtonText = "Create";
            this.buttonsAndStatus1.AutoSize = true;
            this.buttonsAndStatus1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.SetColumnSpan(this.buttonsAndStatus1, 2);
            this.buttonsAndStatus1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonsAndStatus1.Location = new System.Drawing.Point(3, 481);
            this.buttonsAndStatus1.MaximumSize = new System.Drawing.Size(1000, 46);
            this.buttonsAndStatus1.MinimumSize = new System.Drawing.Size(0, 46);
            this.buttonsAndStatus1.Name = "buttonsAndStatus1";
            this.buttonsAndStatus1.Size = new System.Drawing.Size(975, 46);
            this.buttonsAndStatus1.TabIndex = 4;
            this.buttonsAndStatus1.CreateClicked += new System.EventHandler(this.buttonsAndStatus1_CreateClicked);
            // 
            // attributes1
            // 
            this.attributes1.AutoSize = true;
            this.attributes1.DepartmentNo = 0;
            this.attributes1.Description = "";
            this.attributes1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.attributes1.Keywords = "";
            this.attributes1.Location = new System.Drawing.Point(737, 3);
            this.attributes1.Name = "attributes1";
            this.tableLayoutPanel1.SetRowSpan(this.attributes1, 2);
            this.attributes1.Size = new System.Drawing.Size(241, 402);
            this.attributes1.TabIndex = 5;
            // 
            // UsaceDcwGui2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(981, 530);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UsaceDcwGui2";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "USACE Document Creation Wizard";
            this.TopMost = true;
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private Controls.Destination destination1;
        private Controls.FilterDocumentTypeBy filterDocumentTypeBy1;
        private Controls.DocumentName documentName1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private Controls.ButtonsAndStatus buttonsAndStatus1;
        private Controls.Attributes attributes1;
    }
}