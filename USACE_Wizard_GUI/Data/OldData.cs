namespace USACE_Wizard_GUI
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Windows.Forms;

    /// <summary>
    /// Defines the <see cref="Data" />
    /// </summary>
    public class Data
    {
        /// <summary>
        /// Gets the EnvironmentID
        /// </summary>
        public int EnvironmentID
        {
            get { return _EnvironmentId; }
        }

        /// <summary>
        /// Defines the _EnvironmentId
        /// </summary>
        private int _EnvironmentId;

        /// <summary>
        /// Gets or sets the OriginalPath
        /// </summary>
        public string OriginalPath { get; set; }

        /// <summary>
        /// Gets or sets the DestProjId
        /// </summary>
        public int DestProjId { get; set; }

        /// <summary>
        /// Gets or sets the StdNamingDataTable
        /// </summary>
        public DataTable StdNamingDataTable { get; set; }

        /// <summary>
        /// Gets or sets the DisciplinesDataTable
        /// </summary>
        public DataTable DisciplinesDataTable { get; set; }

        /// <summary>
        /// Gets or sets the ProgramsDataTable
        /// </summary>
        public DataTable ProgramsDataTable { get; set; }

        /// <summary>
        /// Gets or sets the AttMapDataTable
        /// </summary>
        public DataTable AttMapDataTable { get; set; }

        /// <summary>
        /// Gets or sets the ProjectPropertiesDataTable
        /// </summary>
        public DataTable ProjectPropertiesDataTable { get; set; }

        /// <summary>
        /// Gets or sets the ExistingFilesDataTable
        /// </summary>
        public DataTable ExistingFilesDataTable { get; set; }

        /// <summary>
        /// Gets or sets the DepartmentsDataTable
        /// </summary>
        public DataTable DepartmentsDataTable { get; set; }

        /// <summary>
        /// Gets or sets the DicConfig
        /// </summary>
        public Dictionary<string, string> DicConfig { get; set; }

        /// <summary>
        /// Gets or sets the OriginalName
        /// </summary>
        public string OriginalName { get; set; }

        /// <summary>
        /// Gets or sets the OriginalFileName
        /// </summary>
        public string OriginalFileName { get; set; }

        /// <summary>
        /// Gets or sets the OriginalDesc
        /// </summary>
        public string OriginalDesc { get; set; }

        /// <summary>
        /// Gets or sets the OriginalAttributes
        /// </summary>
        public string OriginalAttributes { get; set; }

        /// <summary>
        /// Gets or sets the DocumentID
        /// </summary>
        public int DocumentID { get; set; }

        /// <summary>
        /// Gets the CurrentFolderName
        /// </summary>
        public string CurrentFolderName
        {
            get
            {
                return PWWrapper.GetProjectNamePath(DestProjId);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Data"/> class.
        /// </summary>
        /// <param name="ProjectID">The ProjectID<see cref="int"/></param>
        public Data(int ProjectID)
        {
            DestProjId = ProjectID;
            Populate();
        }

        /// <summary>
        /// The Populate
        /// </summary>
        private void Populate()
        {
            var dtConfig = PWWrapper.CreateDataTableFromSQLSelect("SELECT Name, Value FROM StdNaming_Config", "Configuration");
            var msg = string.Empty;

            if (dtConfig.Rows.Count <= 0)
            {
                msg = string.Format("{0}\r\nThe StdNaming_Config table does not have configuration definitions.", msg);
                MessageBox.Show(msg, "USACE DCW",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var selectStdNamingTableName = dtConfig.Select("Name = 'StdNamingTableName'");
            var selectdisciplineTableName = dtConfig.Select("Name = 'DisciplineTableName'");
            var selectprogramTableName = dtConfig.Select("Name = 'ProgramTableName'");
            var selectattributeMapTableName = dtConfig.Select("Name = 'AttributeMapTableName'");
            var selectprojectPropertiesTableName = dtConfig.Select("Name = 'ProjectPropertiesTableName'");

            if (selectStdNamingTableName.Length == 0 || selectStdNamingTableName[0][1] == null)
            {
                MessageBox.Show("The Document Creation Wizard Standard Naming TableName is not configured properly.", "USACE DCW",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (selectdisciplineTableName.Length == 0 || selectdisciplineTableName[0][1] == null)
            {
                MessageBox.Show("The Document Creation Wizard Standard Discipline TableName is not configured properly.", "USACE DCW",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (selectprogramTableName.Length == 0 || selectprogramTableName[0][1] == null)
            {
                MessageBox.Show("The Document Creation Wizard Standard Program TableName is not configured properly.", "USACE DCW",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (selectattributeMapTableName.Length == 0 || selectattributeMapTableName[0][1] == null)
            {
                MessageBox.Show("The Document Creation Wizard Standard Attribute Mapping TableName is not configured properly.", "USACE DCW",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (selectprojectPropertiesTableName.Length == 0 || selectprojectPropertiesTableName[0][1] == null)
            {
                MessageBox.Show("The Document Creation Wizard Standard Project Properties TableName is not configured properly.", "USACE DCW",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var stdNamingTableName = selectStdNamingTableName[0][1].ToString();
            var disciplineTableName = selectdisciplineTableName[0][1].ToString();
            var programTableName = selectprogramTableName[0][1].ToString();
            var attributeMapTableName = selectattributeMapTableName[0][1].ToString();
            var projectPropertiesTableName = selectprojectPropertiesTableName[0][1].ToString();

            StdNamingDataTable = PWWrapper.CreateDataTableFromSQLSelect(string.Format("SELECT * from {0}", stdNamingTableName), "data");
            DisciplinesDataTable = PWWrapper.CreateDataTableFromSQLSelect(string.Format("SELECT ID, DisciplineName from {0}", disciplineTableName), "Disciplines");
            ProgramsDataTable = PWWrapper.CreateDataTableFromSQLSelect(string.Format("SELECT * from {0}", programTableName), "Programs");
            AttMapDataTable = PWWrapper.CreateDataTableFromSQLSelect(string.Format("SELECT * FROM {0}", attributeMapTableName), "AttMap");
            ProjectPropertiesDataTable = PWWrapper.CreateDataTableFromSQLSelect(string.Format("SELECT * FROM {0} ORDER BY priority DESC", projectPropertiesTableName), "ProjectProperties");

            DepartmentsDataTable = PWWrapper.CreateDataTableFromSQLSelect("SELECT * FROM dms_depa", "Departments");

            ExistingFilesDataTable = PWWrapper.CreateDataTableFromSQLSelect(string.Format("SELECT o_filename, o_itemname FROM dms_doc WHERE o_projectno = {0}", DestProjId), "Files");

            PWWrapper.aaApi_SelectProject(DestProjId);
            _EnvironmentId = PWWrapper.aaApi_GetProjectNumericProperty(PWWrapper.ProjectProperty.EnvironmentID, 0);

            if (StdNamingDataTable.Rows.Count <= 0)
                msg = "The StdNaming table does not have document type definitions.";
            if (ProgramsDataTable.Rows.Count <= 0)
            {
                msg = string.Format("{0}\r\nThe StdNaming_Program table does not have program definitions.", msg);
            }
            if (DisciplinesDataTable.Rows.Count <= 0)
            {
                msg = string.Format("{0}\r\nThe StdNaming_Disciplines table does not have discipline definitions.", msg);
            }
            if (AttMapDataTable.Rows.Count <= 0)
            {
                msg = string.Format("{0}\r\nThe StdNaming_AttMap table does not have Environment Attribute Mapping definitions.", msg);
            }
            if (ProjectPropertiesDataTable.Rows.Count <= 0)
            {
                msg = string.Format("{0}\r\nThe StdNaming_ProjectProperties table does not have any Project Property definitions.", msg);
            }

            if (!string.IsNullOrEmpty(msg))
                MessageBox.Show(msg, "USACE DCW", MessageBoxButtons.OK, MessageBoxIcon.Information);

            LoadConfig(dtConfig);
        }

        /// <summary>
        /// The LoadConfig
        /// </summary>
        /// <param name="dtConfig">The dtConfig<see cref="DataTable"/></param>
        private void LoadConfig(DataTable dtConfig)
        {
            DicConfig = new Dictionary<string, string>();

            foreach (DataRow config in dtConfig.Rows)
            {
                DicConfig.Add(config.ItemArray[0].ToString(), config.ItemArray[1].ToString());
            }
        }

        //public static string GetCurrentFolderDesc(int ProjectID)
        //{
        //    var currentFolderDesc = string.Empty;
        //    PWWrapper.aaApi_SelectProject(ProjectID);

        //    var currentFolderDescription = PWWrapper.aaApi_GetProjectStringProperty(PWWrapper.ProjectProperty.Desc, 0);

        //    if (!string.IsNullOrEmpty(currentFolderDescription))
        //    {
        //        var split = currentFolderDescription.Split(' ');

        //        if (split.Length == 0)
        //        {
        //            split = currentFolderDescription.Split('-');
        //        }

        //        currentFolderDesc = split[0];
        //    }

        //    return currentFolderDesc;
        //}

        //public static string GetCurrentFolderName(int ProjectID)
        //{
        //    var currentFolderName = PWWrapper.GetProjectNamePath(ProjectID);

        //    if (!string.IsNullOrEmpty(currentFolderName))
        //    {
        //        var currentFolderSplit = currentFolderName.Split('\\');
        //        currentFolderName = currentFolderSplit[currentFolderSplit.Length - 1];

        //        if (!string.IsNullOrEmpty(currentFolderName))
        //        {
        //            var split = currentFolderName.Split(' ');

        //            if (split.Length == 0)
        //            {
        //                split = currentFolderName.Split('-');
        //            }

        //            currentFolderName = split[0];
        //        }
        //    }

        //    return currentFolderName;
        //}
        /// <summary>
        /// The GetAttDomainValues
        /// </summary>
        /// <param name="sAttName">The sAttName<see cref="string"/></param>
        /// <param name="iEnvId">The iEnvId<see cref="int"/></param>
        /// <returns>The <see cref="List{string}"/></returns>
        public static List<string> GetAttDomainValues(string sAttName, int iEnvId)
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

        /// <summary>
        /// The GetValueListItems
        /// </summary>
        /// <param name="iEnvId">The iEnvId<see cref="int"/></param>
        /// <param name="iTableId">The iTableId<see cref="int"/></param>
        /// <param name="iColumnId">The iColumnId<see cref="int"/></param>
        /// <returns>The <see cref="List{string}"/></returns>
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

        /// <summary>
        /// The CurrentFileNameDesc
        /// </summary>
        /// <param name="currentFolderDesc">The currentFolderDesc<see cref="string"/></param>
        /// <param name="currentFolderName">The currentFolderName<see cref="string"/></param>
        public void CurrentFileNameDesc(out string currentFolderDesc, out string currentFolderName)
        {
            currentFolderDesc = PWWrapper.aaApi_GetProjectStringProperty(PWWrapper.ProjectProperty.Desc, 0);
            currentFolderName = PWWrapper.GetProjectNamePath(DestProjId);
            var currentFolderSplit = currentFolderName.Split('\\');
            currentFolderName = currentFolderSplit[currentFolderSplit.Length - 1];
        }
    }
}
