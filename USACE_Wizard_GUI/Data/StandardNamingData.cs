namespace USACE_Wizard_GUI
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Windows.Forms;

    /// <summary>
    /// Defines the <see cref="StandardNamingData" />
    /// </summary>
    public class StandardNamingData
    {
        /// <summary>
        /// Gets or sets the Action
        /// </summary>
        public Action Action { get; set; }

        /// <summary>
        /// Defines the ProjectChanged
        /// </summary>
        public event EventHandler ProjectChanged;

        /// <summary>
        /// Gets or sets the UserSettings
        /// </summary>
        public UserSettings.Setting UserSettings { get; set; } = new UserSettings.Setting();

        /// <summary>
        /// Gets or sets the EnvironmentID
        /// </summary>
        public int EnvironmentID { get; set; }

        /// <summary>
        /// Defines the _projectID
        /// </summary>
        private int _projectID;

        /// <summary>
        /// Gets or sets the DestinationProjectID
        /// </summary>
        public int DestinationProjectID
        {
            get
            {
                return _projectID;
            }
            set
            {
                if (_projectID != value)
                {
                    _projectID = value;
                    GetProjectCodeAndDesc();
                    ProjectChanged?.Invoke(this, new EventArgs());
                }
            }
        }

        /// <summary>
        /// Gets or sets the SourceProjectID
        /// </summary>
        public int SourceProjectID { get; set; }

        /// <summary>
        /// Defines the _projectCode
        /// </summary>
        private string _projectCode;

        /// <summary>
        /// Defines the _projectDesc
        /// </summary>
        private string _projectDesc;

        /// <summary>
        /// Gets the DestinationProjectCode
        /// </summary>
        public string DestinationProjectCode
        {
            get
            {
                if (string.IsNullOrEmpty(_projectCode))
                {
                    GetProjectCodeAndDesc();
                }

                return _projectCode;
            }
        }

        /// <summary>
        /// The GetProjectCodeAndDesc
        /// </summary>
        private void GetProjectCodeAndDesc()
        {
            var str = "Civil Works";

            var htProjProps = PWWrapper.GetProjectProperties(DestinationProjectID);

            if (htProjProps.Count > 0 && ProjectPropertiesDataTable != null)
            {
                var draProgramFilter = ProjectPropertiesDataTable.Select(string.Format("Program = '{0}'", str));
                var dtProgram = ProjectPropertiesDataTable.Clone();

                draProgramFilter.CopyToDataTable(dtProgram, LoadOption.Upsert);

                var vProgram = dtProgram.DefaultView;

                vProgram.Sort = "Priority DESC";

                foreach (DataRowView drProjProp in vProgram)
                {
                    var projPropName = drProjProp["ProjectProperty"].ToString();

                    if (string.IsNullOrEmpty(projPropName) || !htProjProps.ContainsKey(projPropName) || string.IsNullOrEmpty(htProjProps[projPropName].ToString())) continue;

                    _projectDesc = ProjectPropertiesDataTable.Columns.Contains("Description") ? drProjProp["Description"].ToString() : "Project Properties table does not have a description column";

                    _projectCode = htProjProps[projPropName].ToString();

                    break;
                }
            }
        }

        /// <summary>
        /// Gets the ProjectPath
        /// </summary>
        public string ProjectPath
        {
            get
            {
                return PWWrapper.GetProjectNamePath(DestinationProjectID);
            }
        }

        /// <summary>
        /// Gets the ProjectDesc
        /// </summary>
        public string ProjectDesc
        {
            get
            {
                //var t = ProjectCode;
                return _projectDesc;
            }
        }

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

        //GTH 12/1/2022
        /// <summary>
        /// Gets or sets the ProjectPropertiesDataTable
        /// </summary>
        public DataTable ProductDataTable { get; set; }

        /// <summary>
        /// Gets or sets the ProjectPropertiesDataTable
        /// </summary>
        public DataTable ARIMSDataTable { get; set; }
        //END GTH
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
        /// Initializes a new instance of the <see cref="StandardNamingData"/> class.
        /// </summary>
        /// <param name="projectID">The projectID<see cref="int"/></param>
        public StandardNamingData(int projectID)
        {
            DestinationProjectID = projectID;
            Load();
        }

        /// <summary>
        /// The Load
        /// </summary>
        public void Load()
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

            //GTH 12/1/2022
            var selectProductTableName = dtConfig.Select("Name = 'ProductTableName'");
            var selectARIMSTableName = dtConfig.Select("Name = 'ARIMSTableName'");
            //End GTH

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

            //GTH 12/1/2022
            else if (selectProductTableName.Length == 0 || selectProductTableName[0][1] == null)
            {
                MessageBox.Show("The Document Creation Wizard Standard Product TableName is not configured properly.", "USACE DCW",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (selectARIMSTableName.Length == 0 || selectARIMSTableName[0][1] == null)
            {
                MessageBox.Show("The Document Creation Wizard Standard ARIMS TableName is not configured properly.", "USACE DCW",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //END GTH

            var stdNamingTableName = selectStdNamingTableName[0][1].ToString();
            var disciplineTableName = selectdisciplineTableName[0][1].ToString();
            var programTableName = selectprogramTableName[0][1].ToString();
            var attributeMapTableName = selectattributeMapTableName[0][1].ToString();
            var projectPropertiesTableName = selectprojectPropertiesTableName[0][1].ToString();

            //GTH 12/1/2022
            var productTableName = selectProductTableName[0][1].ToString();
            var ARIMSTableName = selectARIMSTableName[0][1].ToString();
            //END GTH

            StdNamingDataTable = PWWrapper.CreateDataTableFromSQLSelect(string.Format("SELECT * from {0}", stdNamingTableName), "data");
            DisciplinesDataTable = PWWrapper.CreateDataTableFromSQLSelect(string.Format("SELECT ID, DisciplineName from {0}", disciplineTableName), "Disciplines");
            ProgramsDataTable = PWWrapper.CreateDataTableFromSQLSelect(string.Format("SELECT * from {0}", programTableName), "Programs");
            AttMapDataTable = PWWrapper.CreateDataTableFromSQLSelect(string.Format("SELECT * FROM {0}", attributeMapTableName), "AttMap");
            ProjectPropertiesDataTable = PWWrapper.CreateDataTableFromSQLSelect(string.Format("SELECT * FROM {0} ORDER BY priority DESC", projectPropertiesTableName), "ProjectProperties");

            DepartmentsDataTable = PWWrapper.CreateDataTableFromSQLSelect("SELECT * FROM dms_depa", "Departments");

            //GTH 12/1/2022
            //ProductDataTable = PWWrapper.CreateDataTableFromSQLSelect(string.Format("SELECT * FROM {0}", productTableName), "Products");
            //ProductDataTable = PWWrapper.CreateDataTableFromSQLSelect(string.Format("SELECT * FROM {0} order by product", productTableName), "Product");
            //GTH - INC1114883 - Fix this sort
            ProductDataTable = PWWrapper.CreateDataTableFromSQLSelect(string.Format("SELECT product FROM {0} where product is not null order by product", productTableName), "Products");
            
            //ARIMS table isn't currently used
            ARIMSDataTable = PWWrapper.CreateDataTableFromSQLSelect(string.Format("SELECT * FROM {0}", ARIMSTableName), "ARIMSS");
            //END GTH

            ExistingFilesDataTable = PWWrapper.CreateDataTableFromSQLSelect(string.Format("SELECT o_filename, o_itemname FROM dms_doc WHERE o_projectno = {0}", DestinationProjectID), "Files");

            //GTH 12/1/2022

            //END GTH
            PWWrapper.aaApi_SelectProject(DestinationProjectID);
            EnvironmentID = PWWrapper.aaApi_GetProjectNumericProperty(PWWrapper.ProjectProperty.EnvironmentID, 0);

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

            //GTH 12/1/2022
            if (ProductDataTable.Rows.Count <= 0)
            {
                msg = string.Format("{0}\r\nThe StdNaming_Product table does not have any Product Property definitions.", msg);
            }
            if (ARIMSDataTable.Rows.Count <= 0)
            {
                msg = string.Format("{0}\r\nThe StdNaming_ARIMS table does not have any ARIMS Property definitions.", msg);
            }
            //END GTH

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

        /// <summary>
        /// The GetDocCodeDescription
        /// </summary>
        /// <param name="Code">The Code<see cref="string"/></param>
        /// <returns>The <see cref="string"/></returns>
        public string GetDocCodeDescription(string Code)
        {
            var retVal = string.Empty;

            foreach (DataRow row in StdNamingDataTable.Rows)
            {
                if (row["DocumentType"].ToString().ToLower() != Code.ToLower())
                    continue;

                retVal = string.Format("{0} {1}", row["DocTypeDescription"], row["Std_Folders"]);
            }

            return retVal;
        }

        /// <summary>
        /// The GetDocCodeRecommendedUserAdded
        /// </summary>
        /// <param name="Code">The Code<see cref="string"/></param>
        /// <returns>The <see cref="List{string}"/></returns>
        public List<string> GetDocCodeRecommendedUserAdded(string Code, Filter filter)
        {
            var retVal = new List<string>();

            //
            
            //var r = from rows in StdNamingDataTable.AsEnumerable()
            //        where rows.Field<string>("DocumentType").ToLower() == Code.ToLower() && !string.IsNullOrEmpty(rows.Field<string>("UserAdded"))
            //        && rows.Field<string>("Discipline").Contains(filter.FilterCoP)

            var CoPID = DisciplinesDataTable.AsEnumerable().Where(r => r.Field<string>("DisciplineName") == filter.FilterCoP).FirstOrDefault()["ID"].ToString();
            var t = StdNamingDataTable.AsEnumerable().Where(r => r.Field<string>("Discipline").Split(',').Contains(CoPID) && r.Field<string>("DocumentType") == Code).ToList();//.FirstOrDefault()["Description"].ToString();
            //

            //foreach (DataRow row in StdNamingDataTable.Rows)
            foreach (DataRow row in t)
            {
                //if (row["DocumentType"].ToString().ToLower() == Code.ToLower() && !string.IsNullOrEmpty(row["UserAdded"].ToString()))
                //{
                    retVal = new List<string>(row["UserAdded"].ToString().Split(';'));
                    retVal.Insert(0, "");
                    retVal = retVal.AsQueryable<string>().Select(m => m.Trim()).ToList().OrderBy(o => o).ToList();
                //}
            }

            return retVal;
        }
    }
}
