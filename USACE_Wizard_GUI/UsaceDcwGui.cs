using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace USACE_Wizard_GUI
{
    public partial class UsaceDcwGui : Form
    {
        private int _mIDestProjId;
        public string MsTemplateFileName;
        public int DocumentId;
        private string _mSDestDocumentName = "New Document";
        private int _mIEnvironmentId;
        private DataTable _mDtStdNaming;
        private DataTable _mDtDisciplines;
        private DataTable _mDtPrograms;
        private DataTable _mDtAttMap;
        private DataTable _mDtProjectProperties;
        private DataTable _mDtExistingFiles;
        private Dictionary<string, string> _mDicConfig;
        private UserSettings.Setting _userSettings;

        public UsaceDcwGui(int iDestProjId, string sTemplateFileName)
        {
            InitializeComponent();
            _mIDestProjId = iDestProjId;
            MsTemplateFileName = sTemplateFileName;

            _userSettings = new UserSettings.Setting();

            if (!string.IsNullOrEmpty(MsTemplateFileName)) return;

            FileDialog fd = new OpenFileDialog();

            if (fd.ShowDialog(this) == DialogResult.OK)
            {
                MsTemplateFileName = fd.FileName;
            }
        }

        public void PopulateData()
        {
            var pleaseWait = new Form
                {
                    Text = "Please Wait...",
                    StartPosition = FormStartPosition.CenterScreen,
                    FormBorderStyle = FormBorderStyle.FixedDialog,
                    Size = new Size(200, 100),
                    MaximizeBox = false,
                    MinimizeBox = false,

                };
            var lblWait = new Label
                {
                    Text = "Please wait",
                    ForeColor = Color.Black,
                    Location = new Point(25, 25),
                    Size = new Size(65, 15)
                };
            pleaseWait.Controls.Add(lblWait);
            pleaseWait.Show();

            var dtConfig = PWWrapper.CreateDataTableFromSQLSelect("SELECT Name, Value FROM StdNaming_Config", "Configuration");
            var msg = string.Empty;

            if (dtConfig.Rows.Count <= 0)
            {
                msg = string.Format("{0}\r\nThe StdNaming_Config table does not have configuration definitions.", msg);
                pleaseWait.Close();
                MessageBox.Show(msg, "USACE DCW",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var selectStdNamingTableName = dtConfig.Select("Name = 'StdNamingTableName'");

            if (selectStdNamingTableName.Length == 0 || selectStdNamingTableName[0][1] == null)
            {
                pleaseWait.Close();
                MessageBox.Show("The Document Creation Wizard Standard Naming TableName is not configured properly.", "USACE DCW",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var stdNamingTableName = selectStdNamingTableName[0][1].ToString();

            _mDtStdNaming = PWWrapper.CreateDataTableFromSQLSelect(string.Format("SELECT * from {0}", stdNamingTableName ), "data");
            _mDtDisciplines = PWWrapper.CreateDataTableFromSQLSelect("SELECT ID, DisciplineName from StdNaming_Discipline", "Disciplines");
            _mDtPrograms = PWWrapper.CreateDataTableFromSQLSelect("SELECT * from StdNaming_Program", "Programs");
            _mDtAttMap = PWWrapper.CreateDataTableFromSQLSelect("SELECT * FROM StdNaming_AttMap", "AttMap");
            _mDtProjectProperties = PWWrapper.CreateDataTableFromSQLSelect("SELECT * FROM StdNaming_ProjectProperties ORDER BY priority DESC", "ProjectProperties");

            _mDtExistingFiles = PWWrapper.CreateDataTableFromSQLSelect(string.Format("SELECT o_filename, o_itemname FROM dms_doc WHERE o_projectno = {0}", _mIDestProjId), "Files");

            if (_mDtStdNaming.Rows.Count <= 0)
                msg = "The StdNaming table does not have document type definitions.";
            if (_mDtPrograms.Rows.Count <= 0)
            {
                msg = string.Format("{0}\r\nThe StdNaming_Program table does not have program definitions.", msg);
            }
            if (_mDtDisciplines.Rows.Count <= 0)
            {
                msg = string.Format("{0}\r\nThe StdNaming_Disciplines table does not have discipline definitions.", msg);
            }
            if (_mDtAttMap.Rows.Count <= 0)
            {
                msg = string.Format("{0}\r\nThe StdNaming_AttMap table does not have Environment Attribute Mapping definitions.", msg);
            }
            if (_mDtProjectProperties.Rows.Count <= 0)
            {
                msg = string.Format("{0}\r\nThe StdNaming_ProjectProperties table does not have any Project Property definitions.", msg);
            }

            if(!string.IsNullOrEmpty(msg))
                MessageBox.Show(msg, "USACE DCW", MessageBoxButtons.OK, MessageBoxIcon.Information); 

            _mDicConfig = new Dictionary<string, string>();

            foreach (DataRow config in dtConfig.Rows)
            {
                _mDicConfig.Add(config.ItemArray[0].ToString(), config.ItemArray[1].ToString());
            }

            if (_mDtPrograms.Rows.Count > 0)
            {
                var view = _mDtPrograms.DefaultView;
                view.Sort = "ProgramName ASC";
                comboBoxProgram.DataSource = view;
                comboBoxProgram.DisplayMember = "ProgramName";
                comboBoxProgram.ValueMember = "ID";
                comboBoxProgram.BindingContext = BindingContext;
                comboBoxProgram.SelectedIndexChanged += comboBoxProgram_SelectedIndexChanged;
                comboBoxProgram.SelectedIndex = 0;
            }

            GetProjectPropertyValue();
            BindCopData();
            SetDocFilterValues();
            SetDocTypeValues();

            txtBoxDestPath.Text = PWWrapper.GetProjectNamePath(_mIDestProjId);
            txtBoxDestPath.TextChanged += txtBoxDestPath_TextChanged;

            _mSDestDocumentName = Path.GetFileNameWithoutExtension(MsTemplateFileName);

            PWWrapper.aaApi_SelectProject(_mIDestProjId);
            _mIEnvironmentId = PWWrapper.aaApi_GetProjectNumericProperty(PWWrapper.ProjectProperty.EnvironmentID, 0);

            //look to database configuration table for Discipline attribute name
            if (_mDicConfig.Count > 0)
            {
                BindingSource bs;
                if (_mDicConfig.ContainsKey("DisciplineEnvAttribute"))
                {
                    if (!string.IsNullOrEmpty(_mDicConfig["DisciplineEnvAttribute"]))
                    {
                        var ds = GetAttDomainValues(_mDicConfig["DisciplineEnvAttribute"], _mIEnvironmentId);
                        if (ds.Count > 0)
                        {
                            bs = new BindingSource {DataSource = ds};
                            comboBoxDisciplineAttribute.DataSource = bs;
                            comboBoxDisciplineAttribute.SelectedIndex = 0;
                        }
                    }
                    else
                    {
                        MessageBox.Show("The discipline environment attribute is not configured properly.", "USACE DCW",
                                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("The discipline environment attribute is not configured properly.", "USACE DCW",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                if (_mDicConfig.ContainsKey("StatusEnvAttribute"))
                {
                    if (!string.IsNullOrEmpty(_mDicConfig["StatusEnvAttribute"]))
                    {
                        var ds = GetAttDomainValues(_mDicConfig["StatusEnvAttribute"], _mIEnvironmentId);
                        if (ds.Count > 0)
                        {

                            bs = new BindingSource {DataSource = ds};
                            comboBoxStatus.DataSource = bs;
                            comboBoxStatus.SelectedIndex = 0;
                        }
                    }
                    else
                    {
                        MessageBox.Show("The status environment attribute is not configured properly.", "USACE DCW",
                                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("The status environment attribute is not configured properly.", "USACE DCW",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            txtBoxDocName.Text = Path.GetFileName(MsTemplateFileName);

            var currentFolderName = GetCurrentFolderName();
            var currentFolderDesc = GetCurrentFolderDesc();

            for (var i = _mDtStdNaming.Rows.Count - 1; i >= 0; i--)
            {
                var dr = _mDtStdNaming.Rows[i];
                var folders = dr["Std_Folders"].ToString();
                var foldersSplit = folders.Split(',');

                var lsFolderNames = new List<string>(foldersSplit);

                if (lsFolderNames.Contains(currentFolderName) || lsFolderNames.Contains(currentFolderDesc))
                {
                    rbFolderName.Enabled = true;
                }
            }

            SetDefaults();

            pleaseWait.Close();
        }

        private void SetDefaults()
        {
            checkBoxUseDocFilter.Checked = true;
            checkBoxMatchNewFileName.Checked = true;
        }

        private void GetProjectPropertyValue()
        {
            var dRow = (DataRowView)comboBoxProgram.SelectedItem;
            var str = dRow.Row.ItemArray[1].ToString();

            var htProjProps = PWWrapper.GetProjectProperties(_mIDestProjId);

            var bHasProjProp = false;
            if (htProjProps.Count > 0)
            {
                var draProgramFilter = _mDtProjectProperties.Select(string.Format("Program = '{0}'", str));
                var dtProgram = _mDtProjectProperties.Clone();

                draProgramFilter.CopyToDataTable(dtProgram, LoadOption.Upsert);

                var vProgram = dtProgram.DefaultView;

                vProgram.Sort = "Priority DESC";

                foreach (DataRowView drProjProp in vProgram)
                {
                    var projPropName = drProjProp["ProjectProperty"].ToString();

                    if (string.IsNullOrEmpty(projPropName) || !htProjProps.ContainsKey(projPropName) || string.IsNullOrEmpty(htProjProps[projPropName].ToString())) continue;

                    lblProjectNumber.Text = _mDtProjectProperties.Columns.Contains("Description") ? drProjProp["Description"].ToString() : "Project Properties table does not have a description column";

                    txtBoxProjectNumber.Text = htProjProps[projPropName].ToString();
                    bHasProjProp = true;
                    break;
                }
            }

            if (!bHasProjProp)
            {
                MessageBox.Show("The destination folder does not have a parent ProjectWise project.", "USACE DCW",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtBoxProjectNumber.Text = string.Empty;
                btnCreate.Enabled = false;
            }

            UpdateNewFileName();
        }

        private void txtBoxDestPath_TextChanged(object sender, EventArgs e)
        {
            SetDocTypeValues();
        }

        private static List<string> GetAttDomainValues(string sAttName, int iEnvId)
        {
            var lsPickListVals = new List<string>();

            var iEnvAttrDefCount = PWWrapper.aaApi_SelectEnvAttrDefs(iEnvId, -1, -1);

            for (var i = 0; i < iEnvAttrDefCount; i++)
            {
                if (i > 0)
                    PWWrapper.aaApi_SelectEnvAttrDefs(iEnvId, -1, -1);

                var iTableId =
                    PWWrapper.aaApi_GetEnvAttrDefNumericProperty(PWWrapper.AttributeDefinitionProperty.TableID, i);
                var iColumnId =
                    PWWrapper.aaApi_GetEnvAttrDefNumericProperty(PWWrapper.AttributeDefinitionProperty.ColumnID, i);

                switch (PWWrapper.aaApi_GetEnvAttrDefNumericProperty(PWWrapper.AttributeDefinitionProperty.ValueListType, i))
                {
                    case 1:
                        {
                            PWWrapper.aaApi_SelectColumn(iTableId, iColumnId);
                            var sColName = PWWrapper.aaApi_GetColumnStringProperty(PWWrapper.ColumnProperty.Name, 0);

                            if (sAttName.ToLower() == sColName.ToLower())
                            {
                                lsPickListVals = new List<string>();
                                var sSql =
                                    PWWrapper.aaApi_GetEnvAttrDefStringProperty(
                                        PWWrapper.AttributeDefinitionProperty.ValueListSource, i);

                                if (!string.IsNullOrEmpty(sSql))
                                {
                                    var iNumCols = 0;
                                    var iNumRows = PWWrapper.aaApi_SqlSelect(sSql, IntPtr.Zero, ref iNumCols);

                                    if (iNumRows > 0 && iNumCols > 0)
                                    {
                                        for (var iRowCount = 0; iRowCount < iNumRows; iRowCount++)
                                        {
                                            lsPickListVals.Add(PWWrapper.aaApi_SqlSelectGetData(iRowCount, 0));
                                        }
                                    }
                                }
                            }
                        }
                        break;
                    case 3:
                        {

                            PWWrapper.aaApi_SelectColumn(iTableId, iColumnId);
                            var sColName = PWWrapper.aaApi_GetColumnStringProperty(PWWrapper.ColumnProperty.Name, 0);
                            if (sAttName.ToLower() == sColName.ToLower())
                            {
                                lsPickListVals = GetValueListItems(iEnvId, iTableId, iColumnId);
                            }
                        }
                        break;
                }
            }
            return lsPickListVals;
        }

        private static List<string> GetValueListItems(int iEnvId, int iTableId, int iColumnId)
        {
            var lsValueListItems = new List<string>();
            var iValueCount = PWWrapper.aaApi_SelectEnvValListItems(iEnvId, iTableId, iColumnId, -1);

            if (iValueCount > 0)
            {
                for (var j = 0; j < iValueCount; j++)
                {
                    var sValue = PWWrapper.aaApi_GetEnvValListStringProperty(PWWrapper.ValueListProperty.Value, j);

                    if (!lsValueListItems.Contains(sValue) && !string.IsNullOrEmpty(sValue))
                    {
                        lsValueListItems.Add(sValue);
                    }
                }
            }

            return lsValueListItems;
        }

        private void BtnCreateClick(object sender, EventArgs e)
        {
            var sbWorking = new StringBuilder(1024);
            var iAttId = 0;
            var sDestFileName = string.Format("{0}{1}", Path.GetFileNameWithoutExtension(lblNewFileNameValue.Text), Path.GetExtension(MsTemplateFileName));
            var extension = Path.GetExtension(sDestFileName);
            var iAppId = 0;

            if (!string.IsNullOrEmpty(extension))
            {
                iAppId = PWWrapper.aaApi_GetFExtensionApplication(extension.TrimStart('.'));
            }

            if (!string.IsNullOrEmpty(txtBoxDocName.Text))
                _mSDestDocumentName = txtBoxDocName.Text;

            if (PWWrapper.aaApi_CreateDocument(ref DocumentId, _mIDestProjId, 0, 0, PWWrapper.DocumentType.Normal, iAppId,
                                               0, 0, MsTemplateFileName, sDestFileName,
                                               _mSDestDocumentName, txtBoxDocumentDescription.Text, null,
                                               false,
                                               PWWrapper.DocumentCreationFlag.CreateAttributeRecord, sbWorking,
                                               sbWorking.Capacity, ref iAttId))
            {
                PWWrapper.aaApi_UpdateDocumentWindows();
                PWWrapper.aaApi_SelectProject(_mIDestProjId);

                if (!ApplyEnvironmentAttributeValues(PWWrapper.aaApi_GetProjectNumericProperty(PWWrapper.ProjectProperty.EnvironmentID, 0)))
                {
                    MessageBox.Show("New Document created.  However, ProjectWise attributes could not be set.", "USACE DCW", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show("New Document Created.", "USACE DCW", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                return;
            }

            MessageBox.Show(this, string.Format("Error creating document.  Error ID: {0}",
                PWWrapper.aaApi_GetLastErrorId().ToString(CultureInfo.InvariantCulture)), "USACE DCW", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="iEnvId"></param>
        /// <returns></returns>
        private bool ApplyEnvironmentAttributeValues(int iEnvId)
        {
            //select attributes from StdNaming table for the selected document type
            //attribute values from discipline and status comboboxes
            //attribute value from keyword textbox
            if (_mDtAttMap.Rows.Count == 0)
                return true;

            var sEnvName = string.Empty;
            if (PWWrapper.aaApi_SelectEnv(iEnvId) > 0)
            {
                sEnvName = PWWrapper.aaApi_GetEnvStringProperty(PWWrapper.EnvironmentProperty.Name, 0);
            }
            if (!string.IsNullOrEmpty(sEnvName))
            {
                var view = new DataView(_mDtAttMap);
                var dtSelectedAttMap = view.ToTable(true, "StdNamingColumnName", sEnvName);

                var htAttVals = new Hashtable(512)
                    {
                        {_mDicConfig["StatusEnvAttribute"].ToLower(), comboBoxStatus.SelectedValue},
                        {_mDicConfig["DisciplineEnvAttribute"].ToLower(), comboBoxDisciplineAttribute.SelectedItem},
                        {_mDicConfig["KeywordEnvAttribute"].ToLower(), txtBoxKeyword.Text}
                    };
                if (_mDtStdNaming.Rows.Count > 0)
                {
                    var dv = _mDtStdNaming.DefaultView;
                    dv.RowFilter = comboBoxDocFilter.SelectedIndex > 0 ? 
                        string.Format("DocumentType='{0}' AND DocFilter = '{1}'", comboBoxDocType.SelectedValue, comboBoxDocFilter.SelectedValue) 
                        : string.Format("DocumentType='{0}'", comboBoxDocType.SelectedValue);
                    var dt = dv.ToTable();

                    foreach (DataRow dataRow in dtSelectedAttMap.Rows)
                    {
                        var attName = dataRow[sEnvName].ToString().ToLower();

                        if (string.IsNullOrEmpty(attName))
                            continue;

                        var stdNamingColumnName = dataRow["StdNamingColumnName"].ToString();

                        if (!dt.Columns.Contains(stdNamingColumnName) ||
                            stdNamingColumnName.ToLower() == _mDicConfig["StatusEnvAttribute"].ToLower() ||
                            stdNamingColumnName.ToLower() == _mDicConfig["DisciplineEnvAttribute"].ToLower() ||
                            stdNamingColumnName.ToLower() == _mDicConfig["KeywordEnvAttribute"].ToLower())
                        {
                            continue;
                        }

                        var sValue = dt.Rows[0].ItemArray[dt.Columns.IndexOf(stdNamingColumnName)];

                        if (!string.IsNullOrEmpty(sValue.ToString()))
                            htAttVals.Add(attName, sValue);
                    }

                    return PWWrapper.SetAttributesValues(_mIDestProjId, DocumentId, htAttVals);
                }
            }
            return false;
        }


        private void BtnCancelClick(object sender, EventArgs e)
        {
            Close();
        }

        private void BtnSelectNewDestClick(object sender, EventArgs e)
        {
            _mIDestProjId = PWWrapper.aaApi_SelectProjectDlg(IntPtr.Zero, "Select Destination", 0);

            if (_mIDestProjId <= 0)
                return;

            var projectPath = PWWrapper.GetProjectNamePath(_mIDestProjId);

            txtBoxDestPath.Text = projectPath;

            var currentFolderName = GetCurrentFolderName();
            var currentFolderDesc = GetCurrentFolderDesc();

            for (var i = _mDtStdNaming.Rows.Count - 1; i >= 0; i--)
            {
                var dr = _mDtStdNaming.Rows[i];
                var folders = dr["Std_Folders"].ToString();
                var foldersSplit = folders.Split(',');

                var lsFolderNames = new List<string>(foldersSplit);

                if (lsFolderNames.Contains(currentFolderName) || lsFolderNames.Contains(currentFolderDesc))
                {
                    rbFolderName.Enabled = true;
                }
            }

            GetProjectPropertyValue();
        }

        private void ComboBoxCopValueSelectedIndexChanged(object sender, EventArgs e)
        {
            //MessageBox.Show(comboBoxCop.Text);
            _userSettings.CoP = comboBoxCop.Text;

            SetDocFilterValues();
            SetDocTypeValues();
        }

        private void SetDocFilterValues()
        {
            if (_mDtStdNaming.Rows.Count == 0)
                return;

            var newDataTable = _mDtStdNaming.Copy();

            for (var i = newDataTable.Rows.Count - 1; i >= 0; i--)
            {
                var dr = newDataTable.Rows[i];
                var programs = dr["program"].ToString();
                var programsSplit = programs.Split(',');

                var lsPrograms = new List<string>(programsSplit);

                if (!lsPrograms.Contains(comboBoxProgram.SelectedValue.ToString()))
                {
                    dr.Delete();
                }
            }

            if (rbCoP.Checked)
            {
                BindCopData();
                comboBoxCop.Enabled = true;

                for (var i = newDataTable.Rows.Count - 1; i >= 0; i--)
                {
                    var dr = newDataTable.Rows[i];
                    var disciplines = dr["Discipline"].ToString();
                    var disciplinesSplit = disciplines.Split(',');

                    var lsDiscipline = new List<string>(disciplinesSplit);

                    if (!lsDiscipline.Contains(comboBoxCop.SelectedValue.ToString()))
                    {
                        dr.Delete();
                    }
                }
            }
            else if (rbFolderName.Checked)
            {
                var currentFolderDesc = GetCurrentFolderDesc();
                var currentFolderName = GetCurrentFolderName();

                for (var i = newDataTable.Rows.Count - 1; i >= 0; i--)
                {
                    var dr = newDataTable.Rows[i];
                    var folders = dr["Std_Folders"].ToString();
                    var foldersSplit = folders.Split(',');

                    var lsFolderNames = new List<string>(foldersSplit);

                    if (lsFolderNames.Contains(currentFolderName) || lsFolderNames.Contains(currentFolderDesc))
                    {
                        rbFolderName.Enabled = true;
                        continue;
                    }

                    dr.Delete();
                }
                comboBoxCop.Enabled = false;
            }
            else
            {
                comboBoxCop.Enabled = false;
            }

            var dtFinalResult = newDataTable.DefaultView.ToTable(true, new[]{"docfilter"});
            var vFinalResult = dtFinalResult.DefaultView;
            vFinalResult.Sort = "docfilter ASC";

            var docFilterList = new List<string> { "{Select Document Filter}" };

            docFilterList.AddRange(from DataRowView dataRowView in vFinalResult select dataRowView["DocFilter"].ToString());

            var bs = new BindingSource { DataSource = docFilterList };
            comboBoxDocFilter.DataSource = bs;

            if (comboBoxDocFilter.Items.Count > 0)
            {
                //comboBoxDocFilter.SelectedIndex = 0;
                GetLastFilterSelection();
            }
            else
            {
                MessageBox.Show("No document filters found with selected options", "USACE DCW", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            comboBoxDocFilter.SelectedIndexChanged += comboBoxDocFilter_SelectedIndexChanged;
            checkBoxUseDocFilter.CheckedChanged += checkBoxUseDocFilter_CheckedChanged;
        }

        private void SetDocTypeValues()
        {
            comboBoxDocType.SelectedIndexChanged -= ComboBoxDocTypeSelectedIndexChanged;

            btnCreate.Enabled = false;

            if (_mDtStdNaming.Rows.Count == 0)
                return;

            var newDataTable = _mDtStdNaming.Copy();

            for (var i = newDataTable.Rows.Count - 1; i >= 0; i--)
            {
                var dr = newDataTable.Rows[i];
                var programs = dr["program"].ToString();
                var programsSplit = programs.Split(',');

                var lsPrograms = new List<string>(programsSplit);

                if (!lsPrograms.Contains(comboBoxProgram.SelectedValue.ToString()))
                {
                    dr.Delete();
                }
            }

            if (rbCoP.Checked)
            {
                BindCopData();
                comboBoxCop.Enabled = true;
                for (var i = newDataTable.Rows.Count - 1; i >= 0; i--)
                {
                    var dr = newDataTable.Rows[i];
                    var disciplines = dr["Discipline"].ToString();
                    var disciplinesSplit = disciplines.Split(',');

                    var lsDiscipline = new List<string>(disciplinesSplit);

                    if (!lsDiscipline.Contains(comboBoxCop.SelectedValue.ToString()))
                    {
                        dr.Delete();
                    }
                }
            }
            else if (rbFolderName.Checked)
            {
                var currentFolderDesc = GetCurrentFolderDesc();
                var currentFolderName = GetCurrentFolderName();

                for (var i = newDataTable.Rows.Count - 1; i >= 0; i--)
                {
                    var dr = newDataTable.Rows[i];
                    var folders = dr["Std_Folders"].ToString();
                    var foldersSplit = folders.Split(',');

                    var lsFolderNames = new List<string>(foldersSplit);

                    if (lsFolderNames.Contains(currentFolderName) || lsFolderNames.Contains(currentFolderDesc))
                    {
                        rbFolderName.Enabled = true;
                        continue;
                    }

                    dr.Delete();
                }
                comboBoxCop.Enabled = false;
            }
            else
            {
                comboBoxCop.Enabled = false;
            }

            var finalResult = newDataTable.DefaultView;

            if (checkBoxUseDocFilter.Checked && comboBoxDocFilter.SelectedIndex > 0)
                finalResult.RowFilter = string.Format("docfilter = '{0}'", comboBoxDocFilter.SelectedValue);

            finalResult.Sort = "DocumentType ASC";

            var docTypeList = new List<string> {"{Select Document Type}"};
            docTypeList.AddRange(from DataRowView dataRowView in finalResult select dataRowView["DocumentType"].ToString());

            var bs = new BindingSource {DataSource = docTypeList};
            comboBoxDocType.DataSource = bs;

            if (comboBoxDocType.Items.Count > 0)
            {
                comboBoxDocType.SelectedIndex = 0;
                UpdateNewFileName();
            }
            else
            {
                MessageBox.Show("No document types found with selected options", "USACE DCW", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            UpdateDocTypeDescription();
            comboBoxDocType.SelectedIndexChanged += ComboBoxDocTypeSelectedIndexChanged;
        }

        private string GetCurrentFolderDesc()
        {
            var currentFolderDesc = string.Empty;
            PWWrapper.aaApi_SelectProject(_mIDestProjId);

            var currentFolderDescription = PWWrapper.aaApi_GetProjectStringProperty(PWWrapper.ProjectProperty.Desc, 0);

            if (!string.IsNullOrEmpty(currentFolderDescription))
            {
                var split = currentFolderDescription.Split(' ');

                if (split.Length == 0)
                {
                    split = currentFolderDescription.Split('-');
                }

                currentFolderDesc = split[0];
            }

            return currentFolderDesc;
        }

        private string GetCurrentFolderName()
        {
            var currentFolderName = PWWrapper.GetProjectNamePath(_mIDestProjId);

            if (!string.IsNullOrEmpty(currentFolderName))
            {
                var currentFolderSplit = currentFolderName.Split('\\');
                currentFolderName = currentFolderSplit[currentFolderSplit.Length - 1];

                if (!string.IsNullOrEmpty(currentFolderName))
                {
                    var split = currentFolderName.Split(' ');

                    if (split.Length == 0)
                    {
                        split = currentFolderName.Split('-');
                    }

                    currentFolderName = split[0];
                }
            }

            return currentFolderName;
        }

        private void DtPickerDateValueChanged(object sender, EventArgs e)
        {
            UpdateNewFileName();
        }

        private void TxtBoxUserAddedLeave(object sender, EventArgs e)
        {
            UpdateNewFileName();
        }

        private void ComboBoxDocTypeSelectedIndexChanged(object sender, EventArgs e)
        {
            if (checkBoxUseDocFilter.Checked && comboBoxDocFilter.SelectedIndex == 0)
            {
                MessageBox.Show("Please select a document filter", "USACE DCW", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            comboBoxDocFilter.SelectedIndexChanged -= comboBoxDocFilter_SelectedIndexChanged;
            UpdateNewFileName();
            UpdateDocTypeDescription();

            CheckIfNameIsValidAndCanBeCreated();

            var vDocFilter = _mDtStdNaming.Select(string.Format("documenttype = '{0}'", comboBoxDocType.SelectedItem));

            var newTable = _mDtStdNaming.Clone();

            vDocFilter.CopyToDataTable(newTable, LoadOption.Upsert);

            if (newTable.Rows.Count > 0)
            {
                var index = -1;

                try
                {
                    index = comboBoxDocFilter.Items.IndexOf(newTable.Rows[0]["DocFilter"].ToString());
                }
                catch (Exception ex)
                {
                    BPSUtilities.WriteLog(ex.Message);
                }

                if (index > -1)
                    comboBoxDocFilter.SelectedIndex = index;
                else
                    MessageBox.Show("Document filter not found for selected document type.", "USACE DCW", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            else
                comboBoxDocFilter.SelectedIndex = 0;

            var _defaultKeywords = vDocFilter.FirstOrDefault()["Keyword"].ToString();

            txtBoxKeyword.Text = _defaultKeywords;

            var _defaultDescription = vDocFilter.FirstOrDefault()["Description"].ToString();

            txtBoxDocumentDescription.Text = _defaultDescription;

            comboBoxDocFilter.SelectedIndexChanged += comboBoxDocFilter_SelectedIndexChanged;
        }

        private void CheckIfNameIsValidAndCanBeCreated()
        {
            var docExist = _mDtExistingFiles.AsEnumerable().Any(row => row["o_itemname"].ToString() == lblNewFileNameValue.Text);
            var fileExist = _mDtExistingFiles.AsEnumerable().Any(row => row["o_filename"].ToString() == txtBoxDocName.Text);

            if (string.IsNullOrEmpty(txtBoxProjectNumber.Text) || comboBoxDocType.SelectedIndex == 0 || docExist || fileExist)
                btnCreate.Enabled = false;
            else
                btnCreate.Enabled = true;

            if (docExist)
            {
                toolStripStatusLabel1.Text = "Document name already exist!";
            }
            else if (fileExist)
            {
                toolStripStatusLabel1.Text = "File name already exist!";
            }
            else if (string.IsNullOrEmpty(txtBoxProjectNumber.Text))
            {
                toolStripStatusLabel1.Text = "Project number cannot be empty!";
            }
            else if (comboBoxDocType.SelectedIndex == 0)
            {
                toolStripStatusLabel1.Text = "Need to select a document type!";
            }
            else
            {
                toolStripStatusLabel1.Text = "Everything is good to create document.";
            }
        }

        private void UpdateDocTypeDescription()
        {
            if (comboBoxDocType.SelectedIndex == 0)
            {
                txtBoxDocTypeDescription.Text = string.Empty;
                return;
            }

            foreach (DataRow row in _mDtStdNaming.Rows)
            {
                if (row["DocumentType"].ToString().ToLower() != comboBoxDocType.SelectedValue.ToString().ToLower())
                    continue;

                if (string.IsNullOrEmpty(row["DocTypeDescription"].ToString()))
                {
                    txtBoxDocTypeDescription.Text = string.Empty;
                    continue;
                }
                txtBoxDocTypeDescription.Text = string.Format("{0} {1}", row["DocTypeDescription"],
                                                              row["Std_Folders"]);
            }
        }

        private void UpdateNewFileName()
        {
            //if (!string.IsNullOrEmpty(txtBoxProjectNumber.Text))
                lblNewFileNameValue.Text = txtBoxProjectNumber.Text;

            if (comboBoxDocType.SelectedValue != null)
                lblNewFileNameValue.Text += "-" + comboBoxDocType.SelectedValue;

            lblNewFileNameValue.Text += "-" + dtPickerDate.Value.Date.ToString("yyyy-MM-dd");

            if (!string.IsNullOrEmpty(txtBoxUserAdded.Text))
                lblNewFileNameValue.Text += "-" + txtBoxUserAdded.Text;

            lblNewFileNameValue.Text += Path.GetExtension(MsTemplateFileName);

            if(checkBoxMatchNewFileName.Checked)
                txtBoxDocName.Text = lblNewFileNameValue.Text;
        }

        private void RadioButtonsCheckedChanged(object sender, EventArgs e)
        {
            var rb = sender as RadioButton;

            if (rb == null || !rb.Checked) return;

            SetDocFilterValues();
            SetDocTypeValues();

            GetCopLastCoPSelection();
        }

        private void BindCopData()
        {
            if (comboBoxDocType.DataSource != null) return;
            if (_mDtDisciplines.Rows.Count == 0) return;

            var view = _mDtDisciplines.DefaultView;
            view.Sort = "DisciplineName ASC";

            comboBoxCop.SelectedIndexChanged -= ComboBoxCopValueSelectedIndexChanged;
            comboBoxCop.Enabled = true;
            comboBoxCop.DataSource = view;
            comboBoxCop.DisplayMember = "DisciplineName";
            comboBoxCop.ValueMember = "ID";
            comboBoxCop.BindingContext = BindingContext;
            //comboBoxCop.SelectedIndex = 0;
            GetCopLastCoPSelection();
            comboBoxCop.SelectedIndexChanged += ComboBoxCopValueSelectedIndexChanged;
        }

        private void comboBoxProgram_SelectedIndexChanged(object sender, EventArgs e)
        {
            rbCoP.Checked = false;

            SetDocFilterValues();
            SetDocTypeValues();
            GetCopLastCoPSelection();

            GetProjectPropertyValue();
        }

        private void GetCopLastCoPSelection()
        {
            if (comboBoxCop.Items.Count > 0 && rbCoP.Checked)
            {
                if (string.IsNullOrEmpty(_userSettings.CoP))
                {
                    comboBoxCop.SelectedIndex = 0;
                }
                else
                {
                    //comboBoxCop.SelectedIndex = Convert.ToInt32(_userSettings.CoP);
                    comboBoxCop.SelectedIndex = SelectByValue(comboBoxCop, _userSettings.CoP);
                }
            }
        }

        private void GetLastFilterSelection()
        {
            if (comboBoxDocFilter.Items.Count > 0 && checkBoxUseDocFilter.Checked)
            {
                if (string.IsNullOrEmpty(_userSettings.Filter))
                {
                    comboBoxDocFilter.SelectedIndex = 0;
                }
                else
                {
                    //comboBoxDocFilter.SelectedIndex = Convert.ToInt32(_userSettings.Filter);
                    comboBoxDocFilter.SelectedIndex = SelectByValue(comboBoxDocFilter, _userSettings.Filter);
                }
            }
        }

        public static int SelectByValue(ComboBox comboBox, string value)
        {
            int i = 0;
            for (i = 0; i <= comboBox.Items.Count - 1; i++)
            {
                DataRowView cb;
                cb = (DataRowView)comboBox.Items[i];

                if (cb.Row.ItemArray[1].ToString() == value)// Change the 0 index if your want to Select by Text as 1 Index
                {
                    return i;
                }
            }
            return 0;
        }


        private void checkBoxUseDocFilter_CheckedChanged(object sender, EventArgs e)
        {
            comboBoxDocFilter.Enabled = checkBoxUseDocFilter.Checked;
            SetDocTypeValues();
        }

        private void checkBoxMatchNewFileName_CheckedChanged(object sender, EventArgs e)
        {
            txtBoxDocName.Text = checkBoxMatchNewFileName.Checked ? lblNewFileNameValue.Text : Path.GetFileName(MsTemplateFileName);
        }

        private void comboBoxDocFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(checkBoxUseDocFilter.Checked && comboBoxDocFilter.SelectedIndex > 0)
                SetDocTypeValues();

            _userSettings.Filter = comboBoxDocFilter.Text;
        }

        private void txtBoxUserAdded_TextChanged(object sender, EventArgs e)
        {
            UpdateNewFileName();
            CheckIfNameIsValidAndCanBeCreated();
        }

        private void btnCreate_MouseHover(object sender, EventArgs e)
        {
            CheckIfNameIsValidAndCanBeCreated();
        }

        private void txtBoxDocName_TextChanged(object sender, EventArgs e)
        {
            CheckIfNameIsValidAndCanBeCreated();
        }
    }
}
