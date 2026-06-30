namespace USACE_Wizard_GUI
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Windows.Forms;
    using Microsoft.Build.Framework.XamlTypes;

    /// <summary>
    /// Defines the <see cref="DocumentWizard2" />
    /// </summary>
    public partial class DocumentWizard2 : Form
    {
        /// <summary>
        /// Gets or sets the Data
        /// </summary>
        public WizardData Data { get; set; }

        /// <summary>
        /// Defines the isLoadingComplete
        /// </summary>
        private bool isLoadingComplete;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentWizard2"/> class.
        /// </summary>
        /// <param name="projectid">The projectid<see cref="int"/></param>
        /// <param name="files">The importFiles<see cref="List{string}"/></param>
        public DocumentWizard2(int projectid, object files)
        {
            InitializeComponent();

            LoadData(projectid, files);
        }

        private bool DontRunFiles_SelectionChanged = false;
        /// <summary>
        /// The Validation
        /// </summary>
        private void Validation()
        {
            var msgType = (Data.Files.IsValid) ? USACE_Wizard_GUI.Controls.ButtonsAndStatus.Status.TypeEnum.Info : USACE_Wizard_GUI.Controls.ButtonsAndStatus.Status.TypeEnum.Error;
            var msg = Data.Files.ValidationMessage;

            buttonsAndStatus1.SetStatus(msgType, msg);
        }

        /// <summary>
        /// The LoadData
        /// </summary>
        /// <param name="projectid">The projectid<see cref="int"/></param>
        /// <param name="importFiles">The importFiles<see cref="List{string}"/></param>
        private void LoadData(int projectid, object importFiles)
        {
            DisplaySplash("Loading...");

            Data = new WizardData();
            Data.NamingData = new StandardNamingData(projectid);
            Data.NamingData.Action = (importFiles.GetType() == typeof(List<string>)) ? Action.Create : Action.Rename;
            Data.NamingData.Load();
            Data.NamingData.UserSettings = new UserSettings.Setting();
            if (importFiles.GetType() == typeof(List<string>)) Data.Files = new FileImportCollection((List<string>)importFiles, Data.NamingData);
            if (importFiles.GetType() == typeof(List<int>)) Data.Files = new FileImportCollection((List<int>)importFiles, Data.NamingData);
            //GTH If rename, lookup record_copy and set it

            if (Data.NamingData.Action == Action.Rename)
            {
                foreach (FileImport file in Data.Files)
                {
                    //GTH Temp Remove
                    SetRecordCopy(projectid, file);

                    //GTH 12/2/2022
                    //Also set discipline and doc type
                    setSavedAttributes(projectid, file);
                    //END GTH
                }
            }

            LoadForm();

            isLoadingComplete = true;
        }

        private void setSavedAttributes(int ProjectID, FileImport item)
        {
            //GTH 12/12/2022
            //Change the hard coded attribute names to use the attribute map...
            try
            {
                // CoP/Discipline
                String Discipline = PWWrapper.GetAttributeColumnValue(ProjectID, item.DocumentID, "Discipline");
                item.Discipline = Discipline;
                //item.Filter.FilterCoP 
                //Doc Type
                String Doc_Type = PWWrapper.GetAttributeColumnValue(ProjectID, item.DocumentID, "DOC_TYPE");
                item.Doc_Type = Doc_Type;
                //Sub Type
                String Doc_SubType = PWWrapper.GetAttributeColumnValue(ProjectID, item.DocumentID, "DOC_SUBTYPE");
                item.DocumentTypeCode = Doc_SubType;
                //Product
                String Product = PWWrapper.GetAttributeColumnValue(ProjectID, item.DocumentID, "Product_Name");
                item.ProductName = Product;
            }
            catch (Exception ex)
            {

            }
        }
        private void SetRecordCopy(int ProjectID, FileImport item)
        {
            try
            {
                //GTH 6/9/2022
                String rec_copy = PWWrapper.GetAttributeColumnValue(ProjectID, item.DocumentID, "record_copy");
                Boolean setVal = false;
                if (rec_copy.ToLower() == "yes")
                {
                    setVal = true;
                }
                item.Record_Copy = setVal;

                //End GTH
            }
            catch (Exception ex)
            {
                //Do nothing?
            }
        }
        /// <summary>
        /// The LoadForm
        /// </summary>
        private void LoadForm()
        {
            buttonsAndStatus1.ActionButtonText = (Data.NamingData.Action == Action.Create) ? "Create" : "Rename";

            this.Text = "Document Wizard 1.8.4";

            //if (toolStripComboCoP.Items.Count == 0)
            //{
            //    Data.NamingData.DisciplinesDataTable.DefaultView.Sort = "DisciplineName ASC";

            //    foreach (DataRow item in Data.NamingData.DisciplinesDataTable.DefaultView.ToTable().Rows)
            //    {
            //        toolStripComboCoP.Items.Add(item["DisciplineName"]);
            //    }
            //}

            if (comboBoxCoPs.Items.Count == 0)
            {
                Data.NamingData.DisciplinesDataTable.DefaultView.Sort = "DisciplineName ASC";

                foreach (DataRow item in Data.NamingData.DisciplinesDataTable.DefaultView.ToTable().Rows)
                {
                    comboBoxCoPs.Items.Add(item["DisciplineName"]);
                }
            }

            //toolStripComboCoP.Width = 300;
            //toolStripComboCoP.DropDownWidth = 300;

            FilterDocumentTypes();

            autoAppendForDuplicateNamesToolStripMenuItem.CheckedChanged += AutoAppendForDuplicateNamesToolStripMenuItem_CheckedChanged;
            //toolStripComboCoP.SelectedIndexChanged += ToolStripComboCoP_SelectedIndexChanged;
            //toolStripComboFilterType.SelectedIndexChanged += ToolStripComboFilterType_SelectedIndexChanged;
            //new filter
            comboBoxCoPs.SelectedIndexChanged += ComboBoxCoPs_SelectedIndexChanged;
            comboBoxType.SelectedIndexChanged += ComboBoxType_SelectedIndexChanged;
            ComboBoxType_SelectedIndexChanged(this, new EventArgs());
            comboBoxDocType.SelectedIndexChanged += ComboBoxDocType_SelectedIndexChanged;

            //toolStripComboBox3.SelectedIndexChanged += ToolStripComboBox3_SelectedIndexChanged;
            //ToolStripComboFilterType_SelectedIndexChanged(this, new EventArgs());
            filenameMatchesDocumentNameToolStripMenuItem.CheckedChanged += FilenameMatchesDocumentNameToolStripMenuItem_CheckedChanged;
            Data.Files.MatchAllFilenameToNameChanged += Files_MatchAllFilenameToNameChanged;
            useSameNameForAllFilesAppendToolStripMenuItem.CheckedChanged += UseSameNameForAllFilesAppendToolStripMenuItem_CheckedChanged;
            Data.Files.SelectionChanged += Files_SelectionChanged;
            Data.Files.FileUpdated += Files_Updated;
            Data.Files.MatchAllChanged += Files_MatchAllChanged;
            useFileLastModifiedDateToolStripMenuItem.CheckedChanged += UseFileLastModifiedDateToolStripMenuItem_CheckedChanged;
            Data.Files.AllFilesUpdated += Files_AllFilesUpdated;
            useOriginalDescriptionToolStripMenuItem.CheckedChanged += useOriginalDescriptionToolStripMenuItem_CheckedChanged;
            buttonsAndStatus1.ActionButtonEnabled += ButtonsAndStatus1_ActionButtonEnabled;
            buttonsAndStatus1.CreateClicked += ButtonsAndStatus1_CreateClicked;
            addFileToolStripMenuItem.Click += AddFileToolStripMenuItem_Click;
            changeDestinationToolStripMenuItem.Click += ChangeDestinationToolStripMenuItem_Click;
            Data.NamingData.ProjectChanged += NamingData_ProjectChanged;

            filterDocumentTypesToolStripMenuItem.DropDown.MouseLeave += Filter_DropDown_MouseLeave;
            fileToolStripMenuItem.DropDown.MouseLeave += File_DropDown_MouseLeave;
            optionsToolStripMenuItem.DropDown.MouseLeave += Options_DropDown_MouseLeave;

            multiFileList1.Text = "Files to " + Data.NamingData.Action;
            multiFileList1.Data = Data;
            Data.Files.RenameDuplicate();
            documentName21.Data = Data;
            description1.Data = Data;
            keywords1.Data = Data;
            department1.Data = Data;
            textBox1.Text = Data.NamingData.ProjectPath;

            addFileToolStripMenuItem.Visible = (Data.Files.Sum(i => i.DocumentID) == 0);


            fileToolStripMenuItem.Visible = (Data.NamingData.Action == Action.Create);


            validateToolStripMenuItem.Visible = false;


            useFileLastModifiedDateToolStripMenuItem.Checked = Data.NamingData.UserSettings.UseModifiedDate;  //Data.Files.UseFileModifiedDate;


            if (Data.Files != null && Data.Files.Count > 0) Filter_FilterAllUpdated(this, new FilterAllEventArgs(Data.Files.Selected.Filter));

            //12/15/2022
            //GTH
            LoadProductCodes(comboBoxCoPs.Text);


            if (string.IsNullOrEmpty(Data.Files.Selected.ProductName))
            {
                //comboProduct.SelectedIndex = 0;
            }

            else
            {
                
                if (comboProduct.Items.Contains(Data.Files.Selected.ProductName))
                {
                    comboProduct.Text = Data.Files.Selected.ProductName;
                }
                else
                {
                    comboProduct.SelectedIndex = 0;
                }
            }
            //END GTH
            //END GTH
        }


        //GTH 12/1/2022
        // 12/15 - Moved this from DocumentName2 to DocumentWizard2
        //Add data and control for Product
        private void LoadProductCodes(string Discipline)
        {
            if (Data.Files != null && Data.Files.Count > 0)
            {
                if (null == Discipline || Discipline.Trim() == "")
                { 
                    string prev = comboProduct.Text;

                    BindingSource bs = new BindingSource { DataSource = Data.Files.Selected.Filter.FilteredProductCodes };
                    comboProduct.DataSource = bs;

                    if (comboProduct.Items.Contains(prev))
                    {
                        comboProduct.Text = prev;
                    }
                    else if (comboProduct.Items.Count > 0)
                    {
                        comboProduct.SelectedIndex = 0;
                    }
                    else
                    {
                        //MessageBox.Show("No Products found with selected options", "USACE DCW", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        //As of now, Product is not mandatory 
                        return;
                    }
                }
                else
                {

                    //GTH 02/13/2025
                    string prev = comboProduct.Text;

                    string SQL = "SELECT [Product] from StdNaming where discipline like '%" + Data.Files.Selected.Filter.CoPID + "%' and [Product] is not null order by [Product] asc";
                    DataTable dt = PWWrapper.CreateDataTableFromSQLSelect(SQL, "StdNaming");
                    if (dt.Rows.Count > 0)
                    {
                        comboProduct.DataSource = null;
                        comboProduct.Items.Clear();

                        comboProduct.Items.Add("");
                            foreach (DataRow dr in dt.Rows)
                            {
                                comboProduct.Items.Add(dr.ItemArray[0]);
                            }
                        dt.Dispose();


                    }
                    else
                    {
                        comboProduct.DataSource = null;
                        comboProduct.Items.Clear();
                    }

                }
            }
        }
        //END GTH
        private void EnableEvents()
        {
            autoAppendForDuplicateNamesToolStripMenuItem.CheckedChanged += AutoAppendForDuplicateNamesToolStripMenuItem_CheckedChanged;
            comboBoxCoPs.SelectedIndexChanged += ComboBoxCoPs_SelectedIndexChanged;
            comboBoxType.SelectedIndexChanged += ComboBoxType_SelectedIndexChanged;
            ComboBoxType_SelectedIndexChanged(this, new EventArgs());
            comboBoxDocType.SelectedIndexChanged += ComboBoxDocType_SelectedIndexChanged;
            filenameMatchesDocumentNameToolStripMenuItem.CheckedChanged += FilenameMatchesDocumentNameToolStripMenuItem_CheckedChanged;
            Data.Files.MatchAllFilenameToNameChanged += Files_MatchAllFilenameToNameChanged;
            useSameNameForAllFilesAppendToolStripMenuItem.CheckedChanged += UseSameNameForAllFilesAppendToolStripMenuItem_CheckedChanged;
            Data.Files.SelectionChanged += Files_SelectionChanged;
            Data.Files.FileUpdated += Files_Updated;
            Data.Files.MatchAllChanged += Files_MatchAllChanged;
            useFileLastModifiedDateToolStripMenuItem.CheckedChanged += UseFileLastModifiedDateToolStripMenuItem_CheckedChanged;
            Data.Files.AllFilesUpdated += Files_AllFilesUpdated;
            useOriginalDescriptionToolStripMenuItem.CheckedChanged += useOriginalDescriptionToolStripMenuItem_CheckedChanged;
            buttonsAndStatus1.ActionButtonEnabled += ButtonsAndStatus1_ActionButtonEnabled;
            buttonsAndStatus1.CreateClicked += ButtonsAndStatus1_CreateClicked;
            addFileToolStripMenuItem.Click += AddFileToolStripMenuItem_Click;
            changeDestinationToolStripMenuItem.Click += ChangeDestinationToolStripMenuItem_Click;
            Data.NamingData.ProjectChanged += NamingData_ProjectChanged;

            filterDocumentTypesToolStripMenuItem.DropDown.MouseLeave += Filter_DropDown_MouseLeave;
            fileToolStripMenuItem.DropDown.MouseLeave += File_DropDown_MouseLeave;
            optionsToolStripMenuItem.DropDown.MouseLeave += Options_DropDown_MouseLeave;
        }

        private void DisableEvents()
        {
            autoAppendForDuplicateNamesToolStripMenuItem.CheckedChanged -= AutoAppendForDuplicateNamesToolStripMenuItem_CheckedChanged;
            comboBoxCoPs.SelectedIndexChanged -= ComboBoxCoPs_SelectedIndexChanged;
            comboBoxType.SelectedIndexChanged -= ComboBoxType_SelectedIndexChanged;
            ComboBoxType_SelectedIndexChanged(this, new EventArgs());
            comboBoxDocType.SelectedIndexChanged -= ComboBoxDocType_SelectedIndexChanged;
            filenameMatchesDocumentNameToolStripMenuItem.CheckedChanged -= FilenameMatchesDocumentNameToolStripMenuItem_CheckedChanged;
            Data.Files.MatchAllFilenameToNameChanged -= Files_MatchAllFilenameToNameChanged;
            useSameNameForAllFilesAppendToolStripMenuItem.CheckedChanged -= UseSameNameForAllFilesAppendToolStripMenuItem_CheckedChanged;
            Data.Files.SelectionChanged -= Files_SelectionChanged;
            Data.Files.FileUpdated -= Files_Updated;
            Data.Files.MatchAllChanged -= Files_MatchAllChanged;
            useFileLastModifiedDateToolStripMenuItem.CheckedChanged -= UseFileLastModifiedDateToolStripMenuItem_CheckedChanged;
            Data.Files.AllFilesUpdated -= Files_AllFilesUpdated;
            useOriginalDescriptionToolStripMenuItem.CheckedChanged -= useOriginalDescriptionToolStripMenuItem_CheckedChanged;
            buttonsAndStatus1.ActionButtonEnabled -= ButtonsAndStatus1_ActionButtonEnabled;
            buttonsAndStatus1.CreateClicked -= ButtonsAndStatus1_CreateClicked;
            addFileToolStripMenuItem.Click -= AddFileToolStripMenuItem_Click;
            changeDestinationToolStripMenuItem.Click -= ChangeDestinationToolStripMenuItem_Click;
            Data.NamingData.ProjectChanged -= NamingData_ProjectChanged;
            filterDocumentTypesToolStripMenuItem.DropDown.MouseLeave -= Filter_DropDown_MouseLeave;
            fileToolStripMenuItem.DropDown.MouseLeave -= File_DropDown_MouseLeave;
            optionsToolStripMenuItem.DropDown.MouseLeave -= Options_DropDown_MouseLeave;
        }
        private void ComboBoxDocType_SelectedIndexChanged(object sender, EventArgs e)
        {
            Data.Files.Selected.Filter.FilterDocumentTypeUpdated -= Filter_FilterDocumentTypeUpdated;
            Data.Files.Selected.Filter.FilterDocumentType = comboBoxDocType.Text;
            filterDocumentTypesToolStripMenuItem.HideDropDown();
            Data.Files.Selected.Filter.FilterDocumentTypeUpdated += Filter_FilterDocumentTypeUpdated;
        }

        private void ComboBoxCoPs_SelectedIndexChanged(object sender, EventArgs e)
        {
            Data.Files.Selected.Filter.FilterCoPUpdated -= Filter_FilterCoPUpdated;
            if (!string.IsNullOrEmpty(comboBoxCoPs.Text)) Data.Files.Selected.Filter.FilterCoP = comboBoxCoPs.Text;
            FilterDocumentTypes();
            Data.Files.Selected.Filter.FilterCoPUpdated += Filter_FilterCoPUpdated;
            LoadProductCodes(comboBoxCoPs.Text);
            //documentName21.LoadDocumentCodes();
        }

        private void ComboBoxType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Data.Files != null && Data.Files.Count > 0)
            {
                Data.Files.Selected.Filter.FilterTypeUpdated -= Filter_FilterTypeUpdated;
                if (!string.IsNullOrEmpty(comboBoxType.Text)) Data.Files.Selected.Filter.FilterType = comboBoxType.Text;
                FilterDocumentTypes();
                Data.Files.Selected.Filter.FilterTypeUpdated += Filter_FilterTypeUpdated;
            }
        }

        private void AutoAppendForDuplicateNamesToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            Data.Files.AutoNumberDuplicates = autoAppendForDuplicateNamesToolStripMenuItem.Checked;
            Data.Files.RenameDuplicate();
            multiFileList1.Refresh();
            Validation();
        }

        /// <summary>
        /// The Options_DropDown_MouseLeave
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void Options_DropDown_MouseLeave(object sender, EventArgs e)
        {
            if (!optionsToolStripMenuItem.DropDown.Bounds.Contains(Cursor.Position))
            {
                optionsToolStripMenuItem.HideDropDown();
            }
        }

        /// <summary>
        /// The File_DropDown_MouseLeave
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void File_DropDown_MouseLeave(object sender, EventArgs e)
        {
            if (!fileToolStripMenuItem.DropDown.Bounds.Contains(Cursor.Position))
            {
                fileToolStripMenuItem.HideDropDown();
            }
        }

        /// <summary>
        /// The Filter_DropDown_MouseLeave
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void Filter_DropDown_MouseLeave(object sender, EventArgs e)
        {
            if (!filterDocumentTypesToolStripMenuItem.DropDown.Bounds.Contains(Cursor.Position))
            {
                filterDocumentTypesToolStripMenuItem.HideDropDown();
            }
        }

        /// <summary>
        /// The NamingData_ProjectChanged
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void NamingData_ProjectChanged(object sender, EventArgs e)
        {
            textBox1.Text = Data.NamingData.ProjectPath;
        }

        /// <summary>
        /// The ChangeDestinationToolStripMenuItem_Click
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void ChangeDestinationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var _mIDestProjId = PWWrapper.aaApi_SelectProjectDlg(IntPtr.Zero, "Select Destination", 0);

            if (_mIDestProjId <= 0)
            {
                MessageBox.Show("Unable to change destination", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            multiFileList1.Data = Data;

            Data.Files.ChangeDestination(_mIDestProjId, Data.NamingData);
        }

        /// <summary>
        /// The AddFileToolStripMenuItem_Click
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void AddFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            multiFileList1.AddFile();
        }

        /// <summary>
        /// The ButtonsAndStatus1_ActionButtonEnabled
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="bool"/></param>
        private void ButtonsAndStatus1_ActionButtonEnabled(object sender, bool e)
        {

        }

        /// <summary>
        /// The ButtonsAndStatus1_CreateClicked
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void ButtonsAndStatus1_CreateClicked(object sender, EventArgs e)
        {
            isLoadingComplete = false;
            DisplaySplash("Creating");

            buttonsAndStatus1.Enabled = false;

            foreach (var item in Data.Files)
            {
                switch (Data.NamingData.Action)
                {
                    case Action.Create:
                        var sbWorking = new StringBuilder(1024);
                        var iAttId = 0;
                        var sDestFileName = string.Format("{0}{1}", Path.GetFileNameWithoutExtension(item.PWNewFilename), Path.GetExtension(item.Extension));
                        var extension = Path.GetExtension(sDestFileName);
                        var iAppId = 0;

                        if (!string.IsNullOrEmpty(extension))
                        {
                            iAppId = PWWrapper.aaApi_GetFExtensionApplication(extension.TrimStart('.'));
                        }

                        var DocumentId = 0;

                        if (PWWrapper.aaApi_CreateDocument(ref DocumentId, item.DestinationProjectID, 0, 0, PWWrapper.DocumentType.Normal, iAppId,
                                                           item.Department, 0, item.FilePath, sDestFileName,
                                                           item.PWNewName, item.Description, null,
                                                           false,
                                                           PWWrapper.DocumentCreationFlag.CreateAttributeRecord, sbWorking,
                                                           sbWorking.Capacity, ref iAttId))

                        {
                            item.DocumentID = DocumentId;

                            if (!ApplyEnvironmentAttributeValues(PWWrapper.aaApi_GetProjectNumericProperty(PWWrapper.ProjectProperty.EnvironmentID, 0), item))
                            {
                                buttonsAndStatus1.SetStatus(USACE_Wizard_GUI.Controls.ButtonsAndStatus.Status.TypeEnum.Submit, string.Format("Document {0} created in ProjectWise. However, ProjectWise attributes could not be set.", item.PWNewName));
                            }
                            else
                            {
                                buttonsAndStatus1.SetStatus(USACE_Wizard_GUI.Controls.ButtonsAndStatus.Status.TypeEnum.Info, string.Format("Document {0} created in ProjectWise.", item.PWNewName));
                            }

                            PWWrapper.aaApi_UpdateDocumentWindows();
                            PWWrapper.aaApi_SelectProject(item.DestinationProjectID);
                            PWWrapper.aaApi_SelectDocument(item.DestinationProjectID, item.DocumentID);
                        }
                        break;
                    case Action.Rename:
                        if (PWWrapper.aaApi_ModifyDocument(item.DestinationProjectID, item.DocumentID, 0, 0, 0, item.Department, 0, item.PWNewFilename, item.PWNewName, item.Description))
                        {

                            if (!ApplyEnvironmentAttributeValues(PWWrapper.aaApi_GetProjectNumericProperty(PWWrapper.ProjectProperty.EnvironmentID, 0), item))
                            {
                                buttonsAndStatus1.SetStatus(USACE_Wizard_GUI.Controls.ButtonsAndStatus.Status.TypeEnum.Submit, string.Format("Document {0} created in ProjectWise. However, ProjectWise attributes could not be set.", item.PWNewName));
                            }
                            else
                            {
                                buttonsAndStatus1.SetStatus(USACE_Wizard_GUI.Controls.ButtonsAndStatus.Status.TypeEnum.Info, string.Format("Document {0} created in ProjectWise.", item.PWNewName));
                            }

                            PWWrapper.aaApi_UpdateDocumentWindows();
                            PWWrapper.aaApi_SelectProject(item.DestinationProjectID);
                            PWWrapper.aaApi_SelectDocument(item.DestinationProjectID, item.DocumentID);

                        }
                        break;
                    default:
                        break;
                }

                var errorID = PWWrapper.aaApi_GetLastErrorId();
                if (errorID != 0)
                {
                    isLoadingComplete = true;
                    MessageBox.Show(item.PWNewName + ":" + Environment.NewLine + PWWrapper.aaApi_GetMessageByErrorId(errorID), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            buttonsAndStatus1.Enabled = true;

            //var error = PWWrapper.aaApi_GetLastErrorId().ToString(CultureInfo.InvariantCulture);

            //buttonsAndStatus1.SetStatus(USACE_Wizard_GUI.Controls.ButtonsAndStatus.Status.TypeEnum.Info, string.Format("Error renaming document.  Error ID: {0}", error));

            isLoadingComplete = true;
            Close();
        }

        /// <summary>
        /// The ApplyEnvironmentAttributeValues
        /// </summary>
        /// <param name="iEnvId">The iEnvId<see cref="int"/></param>
        /// <param name="file">The file<see cref="FileImport"/></param>
        /// <returns>The <see cref="bool"/></returns>
        private bool ApplyEnvironmentAttributeValues(int iEnvId, FileImport file)
        {
            //select attributes from StdNaming table for the selected document type
            //attribute values from discipline and status comboboxes
            //attribute value from keyword textbox
            if (Data.NamingData.AttMapDataTable.Rows.Count == 0)
                return true;

            var sEnvName = string.Empty;
            if (PWWrapper.aaApi_SelectEnv(iEnvId) > 0)
            {
                sEnvName = PWWrapper.aaApi_GetEnvStringProperty(PWWrapper.EnvironmentProperty.Name, 0);
            }
            if (!string.IsNullOrEmpty(sEnvName))
            {
                var view = new DataView(Data.NamingData.AttMapDataTable);
                var dtSelectedAttMap = view.ToTable(true, "StdNamingColumnName", sEnvName);

                var htAttVals = new Hashtable(512)
                    {
                        //{_data.DicConfig["StatusEnvAttribute"].ToLower(), comboBoxStatus.SelectedValue},
                        {Data.NamingData.DicConfig["DisciplineEnvAttribute"].ToLower(), file.FilterCoP},
                        {Data.NamingData.DicConfig["KeywordEnvAttribute"].ToLower(), file.Keywords}
                    };


                if (Data.NamingData.StdNamingDataTable.Rows.Count > 0)
                {
                    var dv = Data.NamingData.StdNamingDataTable.DefaultView;
                    //dv.RowFilter = file.FilterDocumentType != "*All*" ?
                    //    string.Format("DocumentType='{0}' AND DocFilter = '{1}'", file.DocumentTypeCode, file.FilterDocumentType)
                    //    : string.Format("DocumentType='{0}'", file.DocumentTypeCode);

                    var CoPID = Data.NamingData.DisciplinesDataTable.AsEnumerable().Where(r => r.Field<string>("DisciplineName") == file.FilterCoP).FirstOrDefault()["ID"].ToString();
                    var dt = dv.ToTable().AsEnumerable().Where(r => r.Field<string>("Discipline").Split(',').Contains(CoPID) && r.Field<string>("DocumentType") == file.DocumentTypeCode).CopyToDataTable();
                    //_namingData.StdNamingDataTable.AsEnumerable().Where(r => r.Field<string>("Discipline").Split(',').Contains(CoPID) && r.Field<string>("DocumentType") == docCode).FirstOrDefault()["Description"].ToString();

                    foreach (DataRow dataRow in dtSelectedAttMap.Rows)
                    {
                        var attName = dataRow[sEnvName].ToString().ToLower();


                        if (string.IsNullOrEmpty(attName))
                            continue;

                        string stdNamingColumnName = dataRow["StdNamingColumnName"].ToString();


                        if (!dt.Columns.Contains(stdNamingColumnName) ||
                        stdNamingColumnName.ToLower() == Data.NamingData.DicConfig["DisciplineEnvAttribute"].ToLower() ||
                        stdNamingColumnName.ToLower() == Data.NamingData.DicConfig["KeywordEnvAttribute"].ToLower())
                        {
                            continue;
                        }

                        var ttt = file.PWOriginalFilename;
                        var colidx = dt.Columns.IndexOf(stdNamingColumnName);


                        string sValue = dt.Rows[0].ItemArray[colidx].ToString(); //Need to filter by disipline

                        //GTH 6/9/2022
                        string CompareItem = "Record_Number";
                        if (stdNamingColumnName.Equals(CompareItem))
                        {
                            //If the record_number attribute is present and has a value, pull the rest of the ARIMS info to populate the attributes
                            //GTH Temp Remove
                            htAttVals = getARIMSData(sValue, htAttVals);
                        }
                        //End GTH

                        if (!string.IsNullOrEmpty(sValue.ToString()))
                            htAttVals.Add(attName, sValue);
                    }

                    //GTH - Set the boolean attribute record_copy here

                    try
                    {
                        string strValue = "No";
                        if (file.Record_Copy)
                        {
                            strValue = "Yes";
                        }
                        //GTH Temp remove
                        htAttVals.Add("record_copy", strValue);
                    }
                    catch (Exception ex)
                    {
                        //Do nothing
                    }

                    return PWWrapper.SetAttributesValues(file.DestinationProjectID, file.DocumentID, htAttVals);
                }
            }
            return false;
        }

        private Hashtable getARIMSData(string RecordNumber, Hashtable existingAtts)
        {
            /*  GTH 6/9/2022
             *  Attributes:         Database: 
             *  ARIMS_DURATION  -   ACRS_Duration
             *  ARIMS_INSTRUCTION   -   No Value
             *  ARIMS_ORL   -       No Value
             *  ARIMS_REC_CAT   -   Record_Category 
             *  ARIMS_REC_SERIES    -   ACRS_Record_Series
             *  ARIMS_SUB_SERIES    -   Already set as Record_number
             */

            try
            {
                if (!string.IsNullOrEmpty(RecordNumber))
                {
                    // Here we are using the hard coded table name of ARIMS. Might want to pull that from the standard naming config?
                    string SQL = "SELECT [ACRS_Record_Series],[ACRS_Duration],[Record_Category] FROM [dbo].[ARIMS] WHERE [Record_Number] = '" + RecordNumber + "'";
                    DataTable dt = PWWrapper.CreateDataTableFromSQLSelect(SQL, "ARIMS");
                    if (dt.Rows.Count > 0)
                    {
                        {
                            //The attribute names need to be in lower case
                            existingAtts.Add("arims_duration", dt.Rows[0]["ACRS_Duration"].ToString());
                            existingAtts.Add("arims_rec_cat", dt.Rows[0]["Record_Category"].ToString());
                            existingAtts.Add("arims_rec_series", dt.Rows[0]["ACRS_Record_Series"].ToString());
                        }
                    }
                    dt.Dispose();
                }
            }
            catch (Exception ex)
            {                
                //Do nothing?
            }
            return existingAtts;
        }

        /// <summary>
        /// The useOriginalDescriptionToolStripMenuItem_CheckedChanged
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void useOriginalDescriptionToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            Data.Files.UserOrignalDescription = useOriginalDescriptionToolStripMenuItem.Checked;
        }

        /// <summary>
        /// The Files_AllFilesUpdated
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void Files_AllFilesUpdated(object sender, EventArgs e)
        {
            Validation();
        }

        /// <summary>
        /// The UseFileLastModifiedDateToolStripMenuItem_CheckedChanged
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void UseFileLastModifiedDateToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            Data.Files.UseFileModifiedDate = useFileLastModifiedDateToolStripMenuItem.Checked;
            //Data.NamingData.UserSettings.UseModifiedDate = useFileLastModifiedDateToolStripMenuItem.Checked;
            Validation();
        }

        /// <summary>
        /// The Files_MatchAllChanged
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void Files_MatchAllChanged(object sender, EventArgs e)
        {
            Validation();
        }

        /// <summary>
        /// The Files_Updated
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="FileInfoEventArgs"/></param>
        private void Files_Updated(object sender, FileInfoEventArgs e)
        {
            Validation();
        }

        /// <summary>
        /// The Files_SelectionChanged
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void Files_SelectionChanged(object sender, EventArgs e)
        {
            if (DontRunFiles_SelectionChanged) return;
            DontRunFiles_SelectionChanged = true;
            //GTH 12/12/2022
            //DisableEvents();
            //documentName21.dontRunHandler = true;
            //END GTH
            Data.Files.Selected.Filter.FilterCoPUpdated -= Filter_FilterCoPUpdated;
            Data.Files.Selected.Filter.FilterTypeUpdated -= Filter_FilterTypeUpdated;
            Data.Files.Selected.Filter.FilterDocumentTypeUpdated -= Filter_FilterDocumentTypeUpdated;
            Data.Files.Selected.Filter.FilterAllUpdated -= Filter_FilterAllUpdated;


            Data.Files.Selected.Updated -= Selected_Updated;
            toolStripStatusLabel2.Text = Data.Files.Selected.PWNewName + " - " + Data.Files.Selected.ValidationMessage;
            Validation();
            Data.Files.Selected.Updated += Selected_Updated;

            Data.Files.Selected.Filter.FilterCoPUpdated += Filter_FilterCoPUpdated;
            Data.Files.Selected.Filter.FilterTypeUpdated += Filter_FilterTypeUpdated;
            Data.Files.Selected.Filter.FilterDocumentTypeUpdated += Filter_FilterDocumentTypeUpdated;
            Data.Files.Selected.Filter.FilterAllUpdated += Filter_FilterAllUpdated;

            if (string.IsNullOrEmpty(Data.Files.Selected.Filter.FilterCoP))
            {
                //toolStripComboCoP.SelectedIndex = 0;
                comboBoxCoPs.SelectedIndex = 0;
            }
            else
            {
                //toolStripComboCoP.Text = Data.Files.Selected.Filter.FilterCoP;
                comboBoxCoPs.Text = Data.Files.Selected.Filter.FilterCoP;
            }
            
            if (string.IsNullOrEmpty(Data.Files.Selected.Filter.FilterType))
            {
                //toolStripComboFilterType.SelectedIndex = 0;
                comboBoxType.SelectedIndex = 0;
            }
            else
            {
                //toolStripComboFilterType.Text = Data.Files.Selected.Filter.FilterType;
                comboBoxType.Text = Data.Files.Selected.Filter.FilterType;                
            }

            if (string.IsNullOrEmpty(Data.Files.Selected.Filter.FilterDocumentType))
            {
                //toolStripComboBox3.SelectedIndex = 0;
                //Data.Files.Selected.Filter.FilterDocumentType = toolStripComboBox3.Text;
                comboBoxDocType.SelectedIndex = 0;
                Data.Files.Selected.Filter.FilterDocumentType = comboBoxDocType.Text;
            }
            else
            {
                //toolStripComboBox3.Text = Data.Files.Selected.Filter.FilterDocumentType;
                comboBoxDocType.Text = Data.Files.Selected.Filter.FilterDocumentType;
            }
            //GTH 12/12/2022
            //EnableEvents();
            DontRunFiles_SelectionChanged = false;
            //documentName21.dontRunHandler = false;
            //END GTH
        }

        /// <summary>
        /// The Selected_Updated
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="FileInfoEventArgs"/></param>
        private void Selected_Updated(object sender, FileInfoEventArgs e)
        {
            Files_SelectionChanged(this, e);
            Validation();
        }

        /// <summary>
        /// The Filter_FilterAllUpdated
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="FilterAllEventArgs"/></param>
        private void Filter_FilterAllUpdated(object sender, FilterAllEventArgs e)
        {
            string filter = "Document Type Filter:  {0}, {1}";
            string filter1 = (e.Filter.FilterType == "CoP") ? e.Filter.FilterCoP : e.Filter.FilterType;
            string filter2 = e.Filter.FilterDocumentType;
            toolStripStatusLabel1.Text = string.Format(filter, filter1, filter2);
            Validation();
        }

        /// <summary>
        /// The UseSameNameForAllFilesAppendToolStripMenuItem_CheckedChanged
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void UseSameNameForAllFilesAppendToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            Data.Files.MatchAll = useSameNameForAllFilesAppendToolStripMenuItem.Checked;
            Data.Files.SetUseSameNameForAllAppendNumber();
        }

        /// <summary>
        /// The Files_MatchAllFilenameToNameChanged
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void Files_MatchAllFilenameToNameChanged(object sender, EventArgs e)
        {
            if (filenameMatchesDocumentNameToolStripMenuItem.Checked != Data.Files.MatchAllFilenameToName)
            {
                filenameMatchesDocumentNameToolStripMenuItem.CheckedChanged -= FilenameMatchesDocumentNameToolStripMenuItem_CheckedChanged;
                filenameMatchesDocumentNameToolStripMenuItem.Checked = Data.Files.MatchAllFilenameToName;
                filenameMatchesDocumentNameToolStripMenuItem.CheckedChanged += FilenameMatchesDocumentNameToolStripMenuItem_CheckedChanged;
            }
        }

        /// <summary>
        /// The FilenameMatchesDocumentNameToolStripMenuItem_CheckedChanged
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void FilenameMatchesDocumentNameToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (filenameMatchesDocumentNameToolStripMenuItem.Checked != Data.Files.MatchAllFilenameToName)
            {
                Data.Files.MatchAllFilenameToNameChanged -= Files_MatchAllFilenameToNameChanged;
                Data.Files.MatchAllFilenameToName = filenameMatchesDocumentNameToolStripMenuItem.Checked;
                multiFileList1.Refresh();
                Data.Files.MatchAllFilenameToNameChanged += Files_MatchAllFilenameToNameChanged;
            }

            Validation();
        }

        /// <summary>
        /// The ToolStripComboBox3_SelectedIndexChanged
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        //private void ToolStripComboBox3_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    Data.Files.Selected.Filter.FilterDocumentTypeUpdated -= Filter_FilterDocumentTypeUpdated;
        //    Data.Files.Selected.Filter.FilterDocumentType = toolStripComboBox3.Text;
        //    filterDocumentTypesToolStripMenuItem.HideDropDown();
        //    Data.Files.Selected.Filter.FilterDocumentTypeUpdated += Filter_FilterDocumentTypeUpdated;
        //}

        /// <summary>
        /// The Filter_FilterDocumentTypeUpdated
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="FilterEventArgs"/></param>
        private void Filter_FilterDocumentTypeUpdated(object sender, FilterEventArgs e)
        {
            //toolStripComboBox3.Text = e.Value;
            comboBoxDocType.Text = e.Value;
        }

        /// <summary>
        /// The FilterDocumentTypes
        /// </summary>
        private void FilterDocumentTypes()
        {
            //toolStripComboBox3.Items.Clear();
            comboBoxDocType.Items.Clear();

            if (Data.Files != null && Data.Files.Count > 0)
            {
                foreach (var item in Data.Files.Selected.Filter.FilteredDocumentTypes)
                {
                    //toolStripComboBox3.Items.Add(item);
                    comboBoxDocType.Items.Add(item);
                }
            }

            //if (toolStripComboBox3.Items.Count > 0)
            //{
            //    toolStripComboBox3.SelectedIndexChanged -= ToolStripComboBox3_SelectedIndexChanged;

            //    if (Data.Files.Selected.Filter.FilterDocumentType != null && toolStripComboBox3.Items.Contains(Data.Files.Selected.Filter.FilterDocumentType))
            //    {
            //        toolStripComboBox3.Text = Data.Files.Selected.Filter.FilterDocumentType;
            //    }
            //    else
            //    {
            //        toolStripComboBox3.SelectedIndex = 0;
            //        if (!string.IsNullOrEmpty(toolStripComboBox3.Text)) Data.Files.Selected.Filter.FilterDocumentType = toolStripComboBox3.Text;
            //    }

            //    toolStripComboBox3.SelectedIndexChanged += ToolStripComboBox3_SelectedIndexChanged;

            //}

            if (comboBoxDocType.Items.Count > 0)
            {
                comboBoxDocType.SelectedIndexChanged -= ComboBoxDocType_SelectedIndexChanged;

                if (Data.Files.Selected.Filter.FilterDocumentType != null && comboBoxDocType.Items.Contains(Data.Files.Selected.Filter.FilterDocumentType))
                {
                    comboBoxDocType.Text = Data.Files.Selected.Filter.FilterDocumentType;
                }
                else
                {
                    comboBoxDocType.SelectedIndex = 0;
                    if (!string.IsNullOrEmpty(comboBoxDocType.Text)) Data.Files.Selected.Filter.FilterDocumentType = comboBoxDocType.Text;
                }

                comboBoxDocType.SelectedIndexChanged += ComboBoxDocType_SelectedIndexChanged;

            }
        }

        /// <summary>
        /// The ToolStripComboFilterType_SelectedIndexChanged
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        //private void ToolStripComboFilterType_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (Data.Files != null && Data.Files.Count > 0)
        //    {
        //        Data.Files.Selected.Filter.FilterTypeUpdated -= Filter_FilterTypeUpdated;
        //        if (!string.IsNullOrEmpty(toolStripComboFilterType.Text)) Data.Files.Selected.Filter.FilterType = toolStripComboFilterType.Text;
        //        toolStripComboCoP.Visible = (toolStripComboFilterType.Text == "CoP");
        //        toolStripSeparator1.Visible = toolStripComboCoP.Visible;
        //        coPToolStripMenuItem.Visible = toolStripComboCoP.Visible;
        //        FilterDocumentTypes();
        //        Data.Files.Selected.Filter.FilterTypeUpdated += Filter_FilterTypeUpdated;
        //    }
        //}

        /// <summary>
        /// The ToolStripComboCoP_SelectedIndexChanged
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        //private void ToolStripComboCoP_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    Data.Files.Selected.Filter.FilterCoPUpdated -= Filter_FilterCoPUpdated;
        //    if (!string.IsNullOrEmpty(toolStripComboCoP.Text)) Data.Files.Selected.Filter.FilterCoP = toolStripComboCoP.Text;
        //    FilterDocumentTypes();
        //    Data.Files.Selected.Filter.FilterCoPUpdated += Filter_FilterCoPUpdated;
        //}

        /// <summary>
        /// The Filter_FilterTypeUpdated
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="FilterEventArgs"/></param>
        private void Filter_FilterTypeUpdated(object sender, FilterEventArgs e)
        {
            //toolStripComboFilterType.Text = e.Value;
            //comboBoxType.Text = e.Value;
        }

        /// <summary>
        /// The Filter_FilterCoPUpdated
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="FilterEventArgs"/></param>
        private void Filter_FilterCoPUpdated(object sender, FilterEventArgs e)
        {
            //toolStripComboCoP.Text = e.Value;
            comboBoxCoPs.Text = e.Value;
        }

        /// <summary>
        /// The DisplaySplash
        /// </summary>
        /// <param name="message">The message<see cref="string"/></param>
        private void DisplaySplash(string message)
        {
            isLoadingComplete = false;
            ThreadPool.QueueUserWorkItem((x) =>
            {
                using (var splashForm = new Loading(message))
                {
                    splashForm.Show();
                    while (!isLoadingComplete)
                        Application.DoEvents();
                    splashForm.Close();
                }
            });
        }

        /// <summary>
        /// The validateToolStripMenuItem_Click
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void validateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Validation();
        }

        private void comboProduct_SelectedIndexChanged(object sender, EventArgs e)
        {

            //GTH 12/7/2022
            try
            {
                            //Compare current value vs what the document holds
                     if (Data.Files.Selected.ProductName != comboProduct.Text)
                     {
                    Data.Files.Selected.ProductName = comboProduct.Text;
                    if (comboProduct.SelectedIndex != 0)
                     {
                        Data.Files.Selected.Filter.ProductFilter = comboProduct.Text;
                     }
                      else
                     {
                        Data.Files.Selected.Filter.ProductFilter = null;
                      }
                         //comboBoxDocumentCode.DataSource = SelectedFile.Filter.FilteredDocumentCodes;
                      }

                documentName21.LoadDocumentCodes();
                            //Subscribe to this event
                            //comboBoxDocumentCode.TextChanged += UpdateEvent;
                            //comboProduct.TextChanged += UpdateEvent;

            }
            catch (Exception ex)
            {
                BPSUtilities.WriteLog("Error: {0}\n{1}", new object[] { ex.Message, ex.StackTrace });
            }
            finally
            {
                //comboBoxDocumentCode.TextChanged += UpdateEvent;
                //comboProduct.TextChanged += UpdateEvent;
                //dontRunHandler = false;
            }

        }

        private void comboBoxCoPs_TextChanged(object sender, EventArgs e)
        {
            LoadProductCodes(comboBoxCoPs.Text);
        }

        private void comboBoxCoPs_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            //GTH 2/12/2025  Refilter Product list
        }
    }
}
