using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace USACE_Wizard_GUI
{
    public partial class UsaceDcwGui2 : Form
    {
        //public string MsTemplateFileName;

        
        public int DocumentId;

        //private string _mSDestDocumentName = "New Document";
        //private int _mIEnvironmentId;

        public Data _data;

      
        private UserSettings.Setting _userSettings;

        public UsaceDcwGui2(int iDestProjId, string sTemplateFileName)
        {
            InitializeComponent();

            PopulateData(iDestProjId);

            _data.OriginalPath = sTemplateFileName;

            _userSettings = new UserSettings.Setting();

            if (string.IsNullOrEmpty(_data.OriginalPath)) //return;
            {
                FileDialog fd = new OpenFileDialog();

                if (fd.ShowDialog(this) == DialogResult.OK)
                {
                    _data.OriginalPath = fd.FileName;
                }
            }

            Text = "Importing \"" + Path.GetFileName(_data.OriginalPath).ToUpper() + "\"";

            documentName1.NewFileData = _data;

            filterDocumentTypeBy1.ProjectID = iDestProjId;

            filterDocumentTypeBy1.CoPDataTable = _data.DisciplinesDataTable;
            filterDocumentTypeBy1.CoPDisplayMember = "DisciplineName";
            filterDocumentTypeBy1.CoPValueMember = "ID";

            filterDocumentTypeBy1.DocFilterDataTable = _data.StdNamingDataTable;
            filterDocumentTypeBy1.DocFilterDisplayMember = "DocFilter";
            filterDocumentTypeBy1.DocFilterValueMember = "DocFilter";

            filterDocumentTypeBy1.Populate();

            filterDocumentTypeBy1.FilterDocumentTypeChanged += FilterDocumentTypeBy1_FilterDocumentTypeChanged;

            filterDocumentTypeBy1.CoP = _userSettings.CoP;

            documentName1.DocTypeDataView = filterDocumentTypeBy1.FilteredDocTypesDataView;
            documentName1.ProjectProperties = _data.ProjectPropertiesDataTable;
            documentName1.Populate(_data.DestProjId);

            destination1.DestinationPath = PWWrapper.GetProjectNamePath(_data.DestProjId);

            attributes1.SetAttributeByStandardNameing(_data.StdNamingDataTable, documentName1.DocCode);
            attributes1.DepartmentDatasource = _data.DepartmentsDataTable;
            attributes1.DepartmentNo = _userSettings.DepartmentNumber;
        }

        public void StatusCheck()
        {
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
                buttonsAndStatus1.SetStatus(USACE_Wizard_GUI.Controls.ButtonsAndStatus.Status.TypeEnum.Info, "File is ready to be created.");
                documentName1.HighlightProjectID = false;
                documentName1.HighlightDocumentType = false;
                documentName1.HighlightDocumentName = false;
                documentName1.HighlightDocumentFilename = false;
            }
        }

        private void FilterDocumentTypeBy1_FilterDocumentTypeChanged(object sender, EventArgs e)
        {
            _userSettings.CoP = filterDocumentTypeBy1.CoP;
            _userSettings.Filter = filterDocumentTypeBy1.Filter.ToString();
        }

        private void PopulateData(int ProjectID)
        {
            _data = new Data(ProjectID);

            //if (_data.DicConfig.Count > 0)
            //{
            //    BindingSource bs;
            //    if (_data.DicConfig.ContainsKey("DisciplineEnvAttribute"))
            //    {
            //        if (!string.IsNullOrEmpty(_data.DicConfig["DisciplineEnvAttribute"]))
            //        {
            //            var ds = Data.GetAttDomainValues(_data.DicConfig["DisciplineEnvAttribute"], _data.EnvironmentID);
            //            if (ds.Count > 0)
            //            {
            //                bs = new BindingSource { DataSource = ds };
            //                attributes1.DepartmentDatasource = bs;
            //            }
            //        }
            //        else
            //        {
            //            MessageBox.Show("The discipline environment attribute is not configured properly.", "USACE DCW",
            //                            MessageBoxButtons.OK, MessageBoxIcon.Error);
            //        }
            //    }
            //    else
            //    {
            //        MessageBox.Show("The discipline environment attribute is not configured properly.", "USACE DCW",
            //                        MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    }
            //}
        }

        private void filterDocumentTypeBy1_DocumentTypeChanged(object sender, EventArgs e)
        {
            documentName1.DocTypeDataView = filterDocumentTypeBy1.FilteredDocTypesDataView;
            documentName1.Populate(_data.DestProjId);
        }

        private void destination1_DestinationPathChanged(object sender, EventArgs e)
        {
            filterDocumentTypeBy1.ProjectID = destination1.DestinationProjID;
            _data.DestProjId = destination1.DestinationProjID;
            filterDocumentTypeBy1.SetIfFolderFiltering();
        }

        private void documentName1_NameChanged(object sender, EventArgs e)
        {
            StatusCheck();
            attributes1.SetAttributeByStandardNameing(_data.StdNamingDataTable, documentName1.DocCode);
        }

        private void buttonsAndStatus1_CreateClicked(object sender, EventArgs e)
        {
            _userSettings.DepartmentNumber = attributes1.DepartmentNo;
            buttonsAndStatus1.SetStatus(USACE_Wizard_GUI.Controls.ButtonsAndStatus.Status.TypeEnum.Submit, string.Format("Creating document {0} in ProjectWise.", documentName1.DocName));
            var sbWorking = new StringBuilder(1024);
            var iAttId = 0;
            var sDestFileName = string.Format("{0}{1}", Path.GetFileNameWithoutExtension(documentName1.DocFileName), Path.GetExtension(_data.OriginalPath));
            var extension = Path.GetExtension(sDestFileName);
            var iAppId = 0;
            //string _mSDestDocumentName;

            if (!string.IsNullOrEmpty(extension))
            {
                iAppId = PWWrapper.aaApi_GetFExtensionApplication(extension.TrimStart('.'));
            }

            //if (!string.IsNullOrEmpty(documentName1.DocFileName))
            //    _mSDestDocumentName = documentName1.DocFileName;

            if (PWWrapper.aaApi_CreateDocument(ref DocumentId, _data.DestProjId, 0, 0, PWWrapper.DocumentType.Normal, iAppId,
                                               attributes1.DepartmentNo, 0, _data.OriginalPath, sDestFileName,
                                               documentName1.DocName, attributes1.Description, null,
                                               false,
                                               PWWrapper.DocumentCreationFlag.CreateAttributeRecord, sbWorking,
                                               sbWorking.Capacity, ref iAttId))
            {
                if (!ApplyEnvironmentAttributeValues(PWWrapper.aaApi_GetProjectNumericProperty(PWWrapper.ProjectProperty.EnvironmentID, 0)))
                {
                    //MessageBox.Show("New Document created.  However, ProjectWise attributes could not be set.", "USACE DCW", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    buttonsAndStatus1.SetStatus(USACE_Wizard_GUI.Controls.ButtonsAndStatus.Status.TypeEnum.Submit, string.Format("Document {0} created in ProjectWise. However, ProjectWise attributes could not be set.", documentName1.DocName));
                }
                else
                {
                    //MessageBox.Show("New Document Created.", "USACE DCW", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    buttonsAndStatus1.SetStatus(USACE_Wizard_GUI.Controls.ButtonsAndStatus.Status.TypeEnum.Info, string.Format("Document {0} created in ProjectWise.", documentName1.DocName));
                }

                PWWrapper.aaApi_UpdateDocumentWindows();
                PWWrapper.aaApi_SelectProject(_data.DestProjId);
                PWWrapper.aaApi_SelectDocument(_data.DestProjId, DocumentId);

                Close();
                return;
            }

            buttonsAndStatus1.SetStatus(USACE_Wizard_GUI.Controls.ButtonsAndStatus.Status.TypeEnum.Info, string.Format("Error creating document.  Error ID: {0}", PWWrapper.aaApi_GetLastErrorId().ToString(CultureInfo.InvariantCulture)));
            //MessageBox.Show(this, string.Format("Error creating document.  Error ID: {0}",
            //    PWWrapper.aaApi_GetLastErrorId().ToString(CultureInfo.InvariantCulture)), "USACE DCW", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

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

                    return PWWrapper.SetAttributesValues(_data.DestProjId, DocumentId, htAttVals);
                }
            }
            return false;
        }
    }
}
