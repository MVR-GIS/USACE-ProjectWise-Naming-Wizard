using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace USACE_Wizard_GUI
{
    public partial class USACEDrwGUI : Form
    {
        public string OriginalName { get; set; }
        public string OriginalFileName { get; set; }
        public string OriginalPath { get; set; }
        public string OriginalDesc { get; set; }
        public string OriginalAttributes { get; set; }

        private int DocumentID;

        public Data _data;

        private UserSettings.Setting _userSettings;
        public USACEDrwGUI(int projectid, int documentid)
        {
            InitializeComponent();

            DocumentID = documentid;

            PopulateData(projectid);

            OriginalPath = PWWrapper.GetDocumentNamePath(projectid, documentid);

            DataTable dtDoc = PWWrapper.CreateDataTableFromSQLSelect(string.Format("SELECT o_filename, o_itemname, o_itemdesc FROM dms_doc WHERE o_projectno = {0} AND o_itemno = {1}", projectid, documentid), "CurrentDocument");

            OriginalAttributes = PWWrapper.GetAttributeColumnValue(projectid, documentid, _data.DicConfig["KeywordEnvAttribute"].ToLower());

            foreach (DataRow row in dtDoc.Rows)
            {
                Text = "Renaming \"" + row["o_itemname"].ToString() + "\"";
                OriginalFileName = row["o_filename"].ToString();
                OriginalName = row["o_itemname"].ToString();
                OriginalDesc = row["o_itemdesc"].ToString();
            }

            attributes1.Description = OriginalDesc;
            attributes1.Keywords = OriginalAttributes;

            _data.OriginalPath = OriginalPath;

            _userSettings = new UserSettings.Setting();

            buttonsAndStatus1.ActionButtonText = "Rename";
            destination1.ReadOnly = true;

            documentName1.NewFileData = _data;

            filterDocumentTypeBy1.ProjectID = projectid;

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
                buttonsAndStatus1.SetStatus(USACE_Wizard_GUI.Controls.ButtonsAndStatus.Status.TypeEnum.Info, "File is ready to be renamed.");
                documentName1.HighlightProjectID = false;
                documentName1.HighlightDocumentType = false;
                documentName1.HighlightDocumentName = false;
                documentName1.HighlightDocumentFilename = false;
            }
        }

        private void filterDocumentTypeBy1_DocumentTypeChanged(object sender, EventArgs e)
        {
            documentName1.DocTypeDataView = filterDocumentTypeBy1.FilteredDocTypesDataView;
            documentName1.Populate(_data.DestProjId);
        }

        private void documentName1_NameChanged(object sender, EventArgs e)
        {
            StatusCheck();
            attributes1.SetAttributeByStandardNameing(_data.StdNamingDataTable, documentName1.DocCode);
        }

        private void buttonsAndStatus1_CreateClicked(object sender, EventArgs e)
        {
            _userSettings.DepartmentNumber = attributes1.DepartmentNo;
            if (PWWrapper.aaApi_ModifyDocument(_data.DestProjId, DocumentID, 0, 0, 0, attributes1.DepartmentNo, 0, documentName1.DocFileName, documentName1.DocName, attributes1.Description))
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
                //PWWrapper.aaApi_SelectProject(_data.DestProjId);
                PWWrapper.aaApi_SelectDocument(_data.DestProjId, DocumentID);

                Close();
            }

            buttonsAndStatus1.SetStatus(USACE_Wizard_GUI.Controls.ButtonsAndStatus.Status.TypeEnum.Info, string.Format("Error renaming document.  Error ID: {0}", PWWrapper.aaApi_GetLastErrorId().ToString(CultureInfo.InvariantCulture)));
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

                    return PWWrapper.SetAttributesValues(_data.DestProjId, DocumentID, htAttVals);
                }
            }
            return false;
        }
    }
}
