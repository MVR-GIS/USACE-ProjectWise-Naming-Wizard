namespace USACE_Wizard_GUI
{
    using System;
    using System.Collections;
    using System.Data;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Windows.Forms;

    /// <summary>
    /// Defines the <see cref="DocumentWizard" />
    /// </summary>
    public partial class DocumentWizard : Form
    {
        /// <summary>
        /// Defines the isLoadingComplete
        /// </summary>
        private bool isLoadingComplete = false;

        /// <summary>
        /// Gets or sets the action
        /// </summary>
        public Action action { get; set; }

        /// <summary>
        /// Defines the _data
        /// </summary>
        public StandardNamingData _data;

        /// <summary>
        /// Defines the _file
        /// </summary>
        public FileImport _file;

        /// <summary>
        /// Defines the _userSettings
        /// </summary>
        private UserSettings.Setting _userSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentWizard"/> class.
        /// </summary>
        /// <param name="projectid">The projectid<see cref="int"/></param>
        /// <param name="documentid">The documentid<see cref="int"/></param>
        public DocumentWizard(int projectid, int documentid)
        {
            InitializeComponent();

            action = Action.Rename;

            LoadData(projectid);

            _file.DocumentID = documentid;

            _file.FilePath = PWWrapper.GetDocumentNamePath(projectid, documentid);

            DataTable dtDoc = PWWrapper.CreateDataTableFromSQLSelect(string.Format("SELECT o_filename, o_itemname, o_itemdesc FROM dms_doc WHERE o_projectno = {0} AND o_itemno = {1}", projectid, documentid), "CurrentDocument");

            _file.Keywords = PWWrapper.GetAttributeColumnValue(projectid, documentid, _data.DicConfig["KeywordEnvAttribute"].ToLower());

            foreach (DataRow row in dtDoc.Rows)
            {
                Text = "Renaming \"" + row["o_itemname"].ToString() + "\"";
                _file.PWOriginalFilename = row["o_filename"].ToString();
                _file.PWOriginalName = row["o_itemname"].ToString();
                _file.Description = row["o_itemdesc"].ToString();
            }

            attributes1.Description = _file.Description;
            attributes1.Keywords = _file.Keywords;

            if (string.IsNullOrEmpty(attributes1.Description))
            {
                attributes1.ShowUseOriginal = false;
            }

            buttonsAndStatus1.ActionButtonText = "Rename";
            destination1.ReadOnly = true;

            LoadFormData(projectid);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentWizard"/> class.
        /// </summary>
        /// <param name="projectid">The projectid<see cref="int"/></param>
        /// <param name="importFilename">The importFilename<see cref="string"/></param>
        public DocumentWizard(int projectid, string importFilename)
        {
            InitializeComponent();

            action = Action.Create;

            LoadData(projectid);

            //_file = new FileInfo();

            if (string.IsNullOrEmpty(importFilename))
            {
                Hide();

                FileDialog fd = new OpenFileDialog();

                if (fd.ShowDialog(this) == DialogResult.OK)
                {
                    importFilename = fd.FileName;
                    Show();
                }
            }

            _file = new FileImport(importFilename, _data);

            attributes1.ShowUseOriginal = false;

            Text = "Importing \"" + Path.GetFileName(_file.FilePath).ToUpper() + "\"";

            LoadFormData(projectid);
        }

        /// <summary>
        /// The LoadFormData
        /// </summary>
        /// <param name="projectid">The projectid<see cref="int"/></param>
        private void LoadFormData(int projectid)
        {
            documentName1.NewFile = _file;

            documentName1.Datasource = _data;

            //filterDocumentTypeBy1.ProjectID = projectid;

            //filterDocumentTypeBy1.CoPDataTable = _data.DisciplinesDataTable;
            //filterDocumentTypeBy1.CoPDisplayMember = "DisciplineName";
            //filterDocumentTypeBy1.CoPValueMember = "ID";

            //filterDocumentTypeBy1.DocFilterDataTable = _data.StdNamingDataTable;
            //filterDocumentTypeBy1.DocFilterDisplayMember = "DocFilter";
            //filterDocumentTypeBy1.DocFilterValueMember = "DocFilter";
            filterDocumentTypeBy1.DataSource = _data;

            filterDocumentTypeBy1.Populate();

            filterDocumentTypeBy1.FilterDocumentTypeChanged += FilterDocumentTypeBy1_FilterDocumentTypeChanged;

            filterDocumentTypeBy1.CoP = _userSettings.CoP;

            documentName1.DocTypeDataView = filterDocumentTypeBy1.FilteredDocTypesDataView;
            //documentName1.ProjectProperties = _data.ProjectPropertiesDataTable;
            documentName1.Populate();

            destination1.DestinationPath = PWWrapper.GetProjectNamePath(_data.ProjectID);

            attributes1.SetAttributeByStandardNameing(_data.StdNamingDataTable, documentName1.DocCode);
            attributes1.DepartmentDatasource = _data.DepartmentsDataTable;
            attributes1.DepartmentNo = _userSettings.DepartmentNumber;
        }

        /// <summary>
        /// The LoadData
        /// </summary>
        /// <param name="projectid">The projectid<see cref="int"/></param>
        private void LoadData(int projectid)
        {
            //Hide();

            DisplaySplash("Loading...");

            _data = new StandardNamingData(projectid);
            _userSettings = new UserSettings.Setting();

            isLoadingComplete = true;
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
        /// The FilterDocumentTypeBy1_FilterDocumentTypeChanged
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void FilterDocumentTypeBy1_FilterDocumentTypeChanged(object sender, EventArgs e)
        {
            _userSettings.CoP = filterDocumentTypeBy1.CoP;
            _userSettings.Filter = filterDocumentTypeBy1.Filter.ToString();
        }

        /// <summary>
        /// The StatusCheck
        /// </summary>
        public void StatusCheck()
        {
            var task = (action == Action.Create) ? "created" : "renamed";
            var docExist = _data.ExistingFilesDataTable.AsEnumerable().Any(row => row["o_itemname"].ToString() == documentName1.DocName);
            var fileExist = _data.ExistingFilesDataTable.AsEnumerable().Any(row => row["o_filename"].ToString() == documentName1.DocFileName);

            if (string.IsNullOrEmpty(documentName1.ProjectID))
            {
                buttonsAndStatus1.SetStatus(USACE_Wizard_GUI.Controls.ButtonsAndStatus.Status.TypeEnum.Error, "Unique ID required contact your ProjectWise administrator.");
                documentName1.HighlightProjectID = true;
            }
            else if (documentName1.SelectedDocType == "{Select Document Type}")
            {
                buttonsAndStatus1.SetStatus(USACE_Wizard_GUI.Controls.ButtonsAndStatus.Status.TypeEnum.Warning, "You must select a document code as part of the name.");
                documentName1.HighlightProjectID = false;
                documentName1.HighlightDocumentType = true;
            }
            else if (docExist)
            {
                buttonsAndStatus1.SetStatus(USACE_Wizard_GUI.Controls.ButtonsAndStatus.Status.TypeEnum.Warning, "Document name already exist!");
                documentName1.HighlightProjectID = false;
                documentName1.HighlightDocumentType = false;
                documentName1.HighlightDocumentName = true;
            }
            else if (fileExist)
            {
                buttonsAndStatus1.SetStatus(USACE_Wizard_GUI.Controls.ButtonsAndStatus.Status.TypeEnum.Warning, "File name already exist!");
                documentName1.HighlightProjectID = false;
                documentName1.HighlightDocumentType = false;
                documentName1.HighlightDocumentName = false;
                documentName1.HighlightDocumentFilename = true;
            }
            else
            {
                buttonsAndStatus1.SetStatus(USACE_Wizard_GUI.Controls.ButtonsAndStatus.Status.TypeEnum.Info, string.Format("File is ready to be {0}.", task));
                documentName1.HighlightProjectID = false;
                documentName1.HighlightDocumentType = false;
                documentName1.HighlightDocumentName = false;
                documentName1.HighlightDocumentFilename = false;
            }
        }

        /// <summary>
        /// The filterDocumentTypeBy1_DocumentTypeChanged
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void filterDocumentTypeBy1_DocumentTypeChanged(object sender, EventArgs e)
        {
            documentName1.DocTypeDataView = filterDocumentTypeBy1.FilteredDocTypesDataView;
            documentName1.Populate();
        }

        /// <summary>
        /// The documentName1_NameChanged
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void documentName1_NameChanged(object sender, EventArgs e)
        {
            StatusCheck();
            attributes1.SetAttributeByStandardNameing(_data.StdNamingDataTable, documentName1.DocCode);
        }

        /// <summary>
        /// The ApplyEnvironmentAttributeValues
        /// </summary>
        /// <param name="iEnvId">The iEnvId<see cref="int"/></param>
        /// <returns>The <see cref="bool"/></returns>
        private bool ApplyEnvironmentAttributeValues(int iEnvId)
        {
            //select attributes from StdNaming table for the selected document type
            //attribute values from discipline and status comboboxes
            //attribute value from keyword textbox
            if (_data.AttMapDataTable.Rows.Count == 0)
                return true;

            var sEnvName = string.Empty;
            if (PWWrapper.aaApi_SelectEnv(iEnvId) > 0)
            {
                sEnvName = PWWrapper.aaApi_GetEnvStringProperty(PWWrapper.EnvironmentProperty.Name, 0);
            }
            if (!string.IsNullOrEmpty(sEnvName))
            {
                var view = new DataView(_data.AttMapDataTable);
                var dtSelectedAttMap = view.ToTable(true, "StdNamingColumnName", sEnvName);

                var htAttVals = new Hashtable(512)
                    {
                        //{_data.DicConfig["StatusEnvAttribute"].ToLower(), comboBoxStatus.SelectedValue},
                        {_data.DicConfig["DisciplineEnvAttribute"].ToLower(), filterDocumentTypeBy1.CoP},
                        {_data.DicConfig["KeywordEnvAttribute"].ToLower(), attributes1.Keywords}
                    };
                if (_data.StdNamingDataTable.Rows.Count > 0)
                {
                    var dv = _data.StdNamingDataTable.DefaultView;
                    dv.RowFilter = filterDocumentTypeBy1.DocumentType != "*All*" ?
                        string.Format("DocumentType='{0}' AND DocFilter = '{1}'", documentName1.DocCode, filterDocumentTypeBy1.DocumentType)
                        : string.Format("DocumentType='{0}'", documentName1.DocCode);
                    var dt = dv.ToTable();

                    foreach (DataRow dataRow in dtSelectedAttMap.Rows)
                    {
                        var attName = dataRow[sEnvName].ToString().ToLower();

                        if (string.IsNullOrEmpty(attName))
                            continue;

                        var stdNamingColumnName = dataRow["StdNamingColumnName"].ToString();

                        if (!dt.Columns.Contains(stdNamingColumnName) ||
                            //stdNamingColumnName.ToLower() == _data.DicConfig["StatusEnvAttribute"].ToLower() ||
                            stdNamingColumnName.ToLower() == _data.DicConfig["DisciplineEnvAttribute"].ToLower() ||
                            stdNamingColumnName.ToLower() == _data.DicConfig["KeywordEnvAttribute"].ToLower())
                        {
                            continue;
                        }

                        var sValue = dt.Rows[0].ItemArray[dt.Columns.IndexOf(stdNamingColumnName)];

                        if (!string.IsNullOrEmpty(sValue.ToString()))
                            htAttVals.Add(attName, sValue);
                    }

                    return PWWrapper.SetAttributesValues(_data.ProjectID, _file.DocumentID, htAttVals);
                }
            }
            return false;
        }

        /// <summary>
        /// The buttonsAndStatus1_CreateClicked
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void buttonsAndStatus1_CreateClicked(object sender, EventArgs e)
        {
            _userSettings.DepartmentNumber = attributes1.DepartmentNo;

            buttonsAndStatus1.Enabled = false;

            switch (action)
            {
                case Action.Create:
                    DisplaySplash("Creating " + documentName1.DocName);

                    Create();

                    isLoadingComplete = true;
                    Close();

                    break;
                case Action.Rename:
                    DisplaySplash("Renaming " + documentName1.DocName);
                    if (PWWrapper.aaApi_ModifyDocument(_data.ProjectID, _file.DocumentID, 0, 0, 0, attributes1.DepartmentNo, 0, documentName1.DocFileName, documentName1.DocName, attributes1.Description))
                    {
                        if (!ApplyEnvironmentAttributeValues(PWWrapper.aaApi_GetProjectNumericProperty(PWWrapper.ProjectProperty.EnvironmentID, 0)))
                        {
                            buttonsAndStatus1.SetStatus(USACE_Wizard_GUI.Controls.ButtonsAndStatus.Status.TypeEnum.Submit, string.Format("Document {0} created in ProjectWise. However, ProjectWise attributes could not be set.", documentName1.DocName));
                        }
                        else
                        {
                            buttonsAndStatus1.SetStatus(USACE_Wizard_GUI.Controls.ButtonsAndStatus.Status.TypeEnum.Info, string.Format("Document {0} created in ProjectWise.", documentName1.DocName));
                        }

                        PWWrapper.aaApi_UpdateDocumentWindows();
                        PWWrapper.aaApi_SelectDocument(_data.ProjectID, _file.DocumentID);

                        isLoadingComplete = true;
                        Close();
                    }
                    break;
                default:
                    break;
            }

            buttonsAndStatus1.Enabled = true;

            buttonsAndStatus1.SetStatus(USACE_Wizard_GUI.Controls.ButtonsAndStatus.Status.TypeEnum.Info, string.Format("Error renaming document.  Error ID: {0}", PWWrapper.aaApi_GetLastErrorId().ToString(CultureInfo.InvariantCulture)));
        }

        private void Create()
        {
            var sbWorking = new StringBuilder(1024);
            var iAttId = 0;
            var sDestFileName = string.Format("{0}{1}", Path.GetFileNameWithoutExtension(documentName1.DocFileName), Path.GetExtension(_file.FilePath));
            var extension = Path.GetExtension(sDestFileName);
            var iAppId = 0;

            if (!string.IsNullOrEmpty(extension))
            {
                iAppId = PWWrapper.aaApi_GetFExtensionApplication(extension.TrimStart('.'));
            }

            var DocumentId = 0;

            if (PWWrapper.aaApi_CreateDocument(ref DocumentId, _data.ProjectID, 0, 0, PWWrapper.DocumentType.Normal, iAppId,
                                               attributes1.DepartmentNo, 0, _file.FilePath, sDestFileName,
                                               documentName1.DocName, attributes1.Description, null,
                                               false,
                                               PWWrapper.DocumentCreationFlag.CreateAttributeRecord, sbWorking,
                                               sbWorking.Capacity, ref iAttId))
            {
                _file.DocumentID = DocumentId;

                if (!ApplyEnvironmentAttributeValues(PWWrapper.aaApi_GetProjectNumericProperty(PWWrapper.ProjectProperty.EnvironmentID, 0)))
                {
                    buttonsAndStatus1.SetStatus(USACE_Wizard_GUI.Controls.ButtonsAndStatus.Status.TypeEnum.Submit, string.Format("Document {0} created in ProjectWise. However, ProjectWise attributes could not be set.", documentName1.DocName));
                }
                else
                {
                    buttonsAndStatus1.SetStatus(USACE_Wizard_GUI.Controls.ButtonsAndStatus.Status.TypeEnum.Info, string.Format("Document {0} created in ProjectWise.", documentName1.DocName));
                }

                PWWrapper.aaApi_UpdateDocumentWindows();
                PWWrapper.aaApi_SelectProject(_data.ProjectID);
                PWWrapper.aaApi_SelectDocument(_data.ProjectID, _file.DocumentID);
            }
        }
    }
}
