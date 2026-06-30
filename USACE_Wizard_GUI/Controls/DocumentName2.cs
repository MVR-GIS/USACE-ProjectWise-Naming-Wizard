namespace USACE_Wizard_GUI.Controls
{
    using System;
    using System.Data;
    using System.Collections.Generic;
    using System.Windows.Forms;

    /// <summary>
    /// Defines the <see cref="DocumentName2" />
    /// </summary>
    public partial class DocumentName2 : UserControl
    {
        /// <summary>
        /// Defines the _data
        /// </summary>
        private WizardData _data;

        public bool dontRunHandler = false;
        /// <summary>
        /// Gets or sets the Data
        /// </summary>
        public WizardData Data
        {
            get
            {
                return _data;
            }
            set
            {
                _data = value;
                if (_data != null)
                {
                    _data.Files.SelectionChanged -= Files_SelectionChanged;
                    _data.NamingData.ProjectChanged -= NamingData_ProjectChanged;

                    LoadDocumentCodes();
                    //GTH 12/1/2022
                    //LoadProductCodes();
                    //END GTH
                  
                    _data.Files.SelectionChanged += Files_SelectionChanged;
                    _data.NamingData.ProjectChanged += NamingData_ProjectChanged;
                }
            }
        }

        private void NamingData_ProjectChanged(object sender, EventArgs e)
        {
            labelProjectID.Text = _data.NamingData.ProjectDesc;
            textBoxProjectID.Text = _data.NamingData.DestinationProjectCode;
        }

        private void Validation()
        {
            if (Data.Files.Selected.IsDocDateValid)
            {
                label3.BackColor = Control.DefaultBackColor;
                label3.ForeColor = Control.DefaultForeColor;
            }
            else
            {
                label3.BackColor = System.Drawing.Color.Red;
                label3.ForeColor = System.Drawing.Color.Yellow;
            }

            if (Data.Files.Selected.IsDocCodeValid)
            {
                labelDocumentCode.BackColor = Control.DefaultBackColor;
                labelDocumentCode.ForeColor = Control.DefaultForeColor;
            }
            else
            {
                labelDocumentCode.BackColor = System.Drawing.Color.Red;
                labelDocumentCode.ForeColor = System.Drawing.Color.Yellow;
            }

            if (Data.Files.Selected.IsProjCodeValid)
            {
                labelProjectID.BackColor = Control.DefaultBackColor;
                labelProjectID.ForeColor = Control.DefaultForeColor;
            }
            else
            {
                labelProjectID.BackColor = System.Drawing.Color.Red;
                labelProjectID.ForeColor = System.Drawing.Color.Yellow;
            }
        }



        /// <summary>
        /// The Selected_MatchNameAndFileNameChanged
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="FileInfoEventArgs"/></param>
        private void Selected_MatchNameAndFileNameChanged(object sender, FileInfoEventArgs e)
        {
            textBoxFileName.TextChanged -= TextBoxFileName_TextChanged;
            textBoxFileName.Enabled = !e.File.MatchNameAndFileName;
            textBoxFileName.ReadOnly = e.File.MatchNameAndFileName;
            if (Data != null && Data.Files.Selected != null)
            {
                textBoxFileName.Text = Data.Files.Selected.PWNewFilename;
            }
            textBoxFileName.TextChanged += TextBoxFileName_TextChanged;
        }

        /// <summary>
        /// The Filter_FilterAllUpdated
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="FilterAllEventArgs"/></param>
        private void Filter_FilterAllUpdated(object sender, FilterAllEventArgs e)
        {
            LoadDocumentCodes();
            //GTH 12/1/2022
            //LoadProductCodes();
            //END GTH
        }

        /// <summary>
        /// The LoadDocumentCodes
        /// </summary>
        public void LoadDocumentCodes()
        {
            if (Data.Files != null && Data.Files.Count > 0)
            {
                string prev = comboBoxDocumentCode.Text;

                BindingSource bs = new BindingSource { DataSource = Data.Files.Selected.Filter.FilteredDocumentCodes };
                comboBoxDocumentCode.DataSource = bs;

                if (comboBoxDocumentCode.Items.Contains(prev))
                {
                    comboBoxDocumentCode.Text = prev;
                }                
                else if (comboBoxDocumentCode.Items.Count > 0 )
                {
                    comboBoxDocumentCode.SelectedIndex = 0;
                }
                else
                {
                    MessageBox.Show("No document types found with selected options", "USACE DCW", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                                //GTH 4/5/2023 If there is only one document code, set that
                if (comboBoxDocumentCode.Items.Count == 2)
                {
                    comboBoxDocumentCode.SelectedIndex = 1;
                }
            }
        }

        /// <summary>
        /// The Files_SelectionChanged
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void Files_SelectionChanged(object sender, EventArgs e)
        {
            //Data.Files.Selected.Filter.FilterAllUpdated -= Filter_FilterAllUpdated;
            foreach (var item in Data.Files)
            {
                item.Filter.FilterAllUpdated -= Filter_FilterAllUpdated;
            }
            LoadForm();

            foreach (var item in Data.Files)
            {
                item.Filter.FilterAllUpdated += Filter_FilterAllUpdated;
            }
        }

        /// <summary>
        /// The LoadForm
        /// </summary>
        private void LoadForm()
        {
            textBoxProjectID.TextChanged -= UpdateEvent;
            comboBoxDocumentCode.TextChanged -= UpdateEvent;
            dateTimePicker1.TextChanged -= UpdateEvent;
            comboBoxUserAdded.TextChanged -= UpdateEvent;
            //GTH 12/2/2022
            //comboProduct.TextChanged -= UpdateEvent;
            //comboProduct.SelectedIndexChanged -= UpdateEvent;
            //END GTH
            _data.Files.Selected.MatchNameAndFileNameChanged -= Selected_MatchNameAndFileNameChanged;
            _data.Files.Selected.NumberingUpdated -= Selected_NumberingUpdated;

            Filter_FilterAllUpdated(this, new FilterAllEventArgs(_data.Files.Selected.Filter));

            textBoxProjectID.Text = SelectedFile.DestinationProjectCode;
            comboBoxDocumentCode.Text = SelectedFile.DocumentTypeCode;
            dateTimePicker1.Text = SelectedFile.DocumentDate;
            comboBoxUserAdded.Text = SelectedFile.DocumentUserAdded;
            textBoxFileName.Text = SelectedFile.PWNewFilename;
            dateTimePicker1.Enabled = !SelectedFile.UseFileLastModifiedDate;
            Validation();
            textBoxFileName.MaxLength = 127;
            textBoxProjectID.MouseEnter += TextBoxProjectID_MouseEnter;
            //GTH 12/12/2022
            //GTH 12/12/2022
            if (string.IsNullOrEmpty(SelectedFile.DocumentTypeCode))
            {
                comboBoxDocumentCode.SelectedIndex = 0;
            }
            else
            {
                if (comboBoxDocumentCode.Items.Contains(SelectedFile.DocumentTypeCode))
                {
                    comboBoxDocumentCode.Text = SelectedFile.DocumentTypeCode;
                }
                else
                {
                    comboBoxDocumentCode.SelectedIndex = 0;
                }
            }

            textBoxProjectID.TextChanged += UpdateEvent;
            comboBoxDocumentCode.TextChanged += UpdateEvent;
            dateTimePicker1.TextChanged += UpdateEvent;
            comboBoxUserAdded.TextChanged += UpdateEvent;
            //GTH 12/2/2022
            //comboProduct.TextChanged += UpdateEvent;
            //comboProduct.SelectedIndexChanged += UpdateEvent;
            //END GTH
            _data.Files.Selected.MatchNameAndFileNameChanged += Selected_MatchNameAndFileNameChanged;
            _data.Files.Selected.NumberingUpdated += Selected_NumberingUpdated;
            _data.Files.Selected.UseLastModifiedDateChanged += Selected_UseLastModifiedDateChanged;

            //GTH Update record copy for selected file
            chkRecordCopy.Checked = SelectedFile.Record_Copy;
            //End GTH
        }

        private void TextBoxProjectID_MouseEnter(object sender, EventArgs e)
        {
            TextBox TB = (TextBox)sender;
            int VisibleTime = 1000;  //in milliseconds

            ToolTip tt = new ToolTip();
            tt.Show(_data.NamingData.ProjectPath, TB, 0, 0, VisibleTime);
        }

        /// <summary>
        /// The Selected_UseLastModifiedDateChanged
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="FileInfoEventArgs"/></param>
        private void Selected_UseLastModifiedDateChanged(object sender, FileInfoEventArgs e)
        {
            dateTimePicker1.Enabled = !e.File.UseFileLastModifiedDate;
            dateTimePicker1.Text = e.File.DocumentDate;
        }

        /// <summary>
        /// The Selected_NumberingUpdated
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="FileInfoEventArgs"/></param>
        private void Selected_NumberingUpdated(object sender, FileInfoEventArgs e)
        {
            if (_data.Files.MatchAllFilenameToName) textBoxFileName.Text = Data.Files.Selected.PWNewFilename;
        }

        /// <summary>
        /// The TextBoxFileName_TextChanged
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void TextBoxFileName_TextChanged(object sender, EventArgs e)
        {
            Data.Files.Selected.PWOriginalFilename = textBoxFileName.Text;
        }


        #region test

        #endregion test
        /// <summary>
        /// The UpdateEvent
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void UpdateEvent(object sender, EventArgs e)
        {
            //GTH 12/5/2022
            //Prevent this event from running endlesly 
            if (dontRunHandler)
            {
                int fred = 0;
                return;
            }
            
            dontRunHandler = true;
            Application.DoEvents();


            //if (SelectedFile.ProjCode != textBoxProjectID.Text) SelectedFile.ProjCode = textBoxProjectID.Text;
            if (SelectedFile.DocumentTypeCode != comboBoxDocumentCode.Text)
            {
                var currentDoceCode = SelectedFile.DocumentTypeCode;
                SelectedFile.DocumentTypeCode = comboBoxDocumentCode.Text;
                textBoxDocumentCodeDescription.Text = Data.NamingData.GetDocCodeDescription(comboBoxDocumentCode.Text);
                if (currentDoceCode != comboBoxDocumentCode.Text)
                {
                    LoadUserAdded(); 
                }
            }

            var maxLen = 127;
            var bodyLen = 0;

            if (comboBoxUserAdded.Text.Length > 0)
            {
		        bodyLen = SelectedFile.PWNewName.Replace(comboBoxUserAdded.Text, "").Length; 
            }
            else
            {
                bodyLen = SelectedFile.PWNewName.Length;
            }

            comboBoxUserAdded.MaxLength = maxLen - bodyLen;

            Validation();
            //GTH - 6/7/2022 - Should be able to access the boolean control from here and set attribute
            SelectedFile.Record_Copy = chkRecordCopy.Checked;
            //eND gth
            if (SelectedFile.DocumentDate != dateTimePicker1.Text) SelectedFile.DocumentDate = dateTimePicker1.Text;
            if (SelectedFile.DocumentUserAdded != comboBoxUserAdded.Text) SelectedFile.DocumentUserAdded = comboBoxUserAdded.Text;
            if (SelectedFile.PWNewFilename != textBoxFileName.Text) textBoxFileName.Text = Data.Files.Selected.PWNewFilename;

            dontRunHandler = false;
        }

        /// <summary>
        /// The LoadUserAdded
        /// </summary>
        private void LoadUserAdded()
        {
            var bs = new BindingSource { DataSource = Data.NamingData.GetDocCodeRecommendedUserAdded(comboBoxDocumentCode.Text, SelectedFile.Filter) };
            comboBoxUserAdded.DataSource = bs;
        }

        /// <summary>
        /// Gets the SelectedFile
        /// </summary>
        private FileImport SelectedFile
        {
            get
            {
                return _data.Files.Selected;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentName2"/> class.
        /// </summary>
        public DocumentName2()
        {
            InitializeComponent();
        }

        private void chkRecordCopy_CheckedChanged(object sender, EventArgs e)
        {
            SelectedFile.Record_Copy = chkRecordCopy.Checked;
        }
    }
}
