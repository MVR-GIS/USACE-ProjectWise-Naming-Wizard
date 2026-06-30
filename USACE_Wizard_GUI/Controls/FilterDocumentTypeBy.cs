namespace USACE_Wizard_GUI.Controls
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Linq;
    using System.Windows.Forms;

    /// <summary>
    /// Defines the <see cref="FilterDocumentTypeBy" />
    /// </summary>
    public partial class FilterDocumentTypeBy : UserControl
    {
        /// <summary>
        /// Gets or sets the Text
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Bindable(true)]
        public override string Text
        {
            get { return groupBoxFilterDocTypeBy.Text; }
            set { groupBoxFilterDocTypeBy.Text = value; }
        }

        /// <summary>
        /// Gets or sets the CoP
        /// </summary>
        public string CoP
        {
            get
            {
                return comboBoxCoP.Text;
            }
            set
            {
                if (!string.IsNullOrEmpty(value) && comboBoxCoP.Items.Count > 0)
                {
                    comboBoxCoP.SelectedIndex = Common.SelectByValue(comboBoxCoP, value);
                }
            }
        }

        /// <summary>
        /// Gets or sets the DocumentType
        /// </summary>
        public string DocumentType
        {
            get
            {
                return comboBoxDocTypes.Text;
            }
            set
            {
                if (!string.IsNullOrEmpty(value) && comboBoxDocTypes.Items.Count > 0)
                {
                    comboBoxDocTypes.SelectedIndex = Common.SelectByValue(comboBoxDocTypes, value);
                }
            }
        }

        /// <summary>
        /// Sets the DataSource
        /// </summary>
        public StandardNamingData DataSource
        {
            set
            {
                _CoPDataTable = value.DisciplinesDataTable;
                _CoPDisplayMember = "DisciplineName";
                _CoPValueMember = "ID";

                _DocFilterDataTable = value.StdNamingDataTable;
                _DocFilterDisplayMember = "DocFilter";
                _DocFilterValueMember = "DocFilter";

                _ProjectID = value.ProjectID;
            }
        }

        /// <summary>
        /// Gets or sets the _CoPDataTable
        /// </summary>
        private DataTable _CoPDataTable { get; set; }

        /// <summary>
        /// Gets or sets the _CoPDisplayMember
        /// </summary>
        private string _CoPDisplayMember { get; set; }

        /// <summary>
        /// Gets or sets the _CoPValueMember
        /// </summary>
        private string _CoPValueMember { get; set; }

        /// <summary>
        /// Gets or sets the _DocFilterDataTable
        /// </summary>
        private DataTable _DocFilterDataTable { get; set; }

        /// <summary>
        /// Gets or sets the _DocFilterDisplayMember
        /// </summary>
        private string _DocFilterDisplayMember { get; set; }

        /// <summary>
        /// Gets or sets the _DocFilterValueMember
        /// </summary>
        private string _DocFilterValueMember { get; set; }

        /// <summary>
        /// Gets or sets the _ProjectID
        /// </summary>
        private int _ProjectID { get; set; }

        /// <summary>
        /// Defines the FilterType
        /// </summary>
        public enum FilterType
        { 
            ///<summary>
            /// Defines the CoP
            /// </summary>
            CoP,
            /// <summary>
            /// Defines the Folder
            /// </summary>
            Folder,
            /// <summary>
            /// Defines the All
            /// </summary>
            All
        }

        /// <summary>
        /// Gets the FilteredDocTypesDataView
        /// </summary>
        public DataView FilteredDocTypesDataView
        {
            get
            {
                if (_DocFilterDT != null)
                {
                    var _FilteredDocTypes = _DocFilterDT.DefaultView;

                    if (comboBoxDocTypes.Text != "*All*")
                    {
                        _FilteredDocTypes.RowFilter = string.Format("docfilter = '{0}'", comboBoxDocTypes.SelectedValue);
                    }
                    else
                    {
                        FilterDocumentTypes();
                        _FilteredDocTypes = _DocFilterDT.DefaultView;
                    }

                    _FilteredDocTypes.Sort = "DocumentType ASC";
                    return _FilteredDocTypes;
                }
                else
                {
                    FilterDocumentTypes();
                    return _DocFilterDT.DefaultView;
                }
            }
        }

        /// <summary>
        /// Gets or sets the Filter
        /// </summary>
        public FilterType Filter
        {
            get
            {
                if (radioButtonCoP.Checked)
                {
                    return FilterType.CoP;
                }
                else if (radioButtonAll.Checked)
                {
                    return FilterType.All;
                }
                else if (radioButtonFolder.Checked)
                {
                    return FilterType.Folder;
                }
                else
                {
                    radioButtonCoP.Checked = true;
                    return FilterType.CoP;
                }
            }
            set
            {
                switch (value)
                {
                    case FilterType.CoP:
                        radioButtonCoP.Checked = true;
                        break;
                    case FilterType.Folder:
                        radioButtonFolder.Checked = true;
                        break;
                    case FilterType.All:
                        radioButtonAll.Checked = true;
                        break;
                    default:
                        radioButtonCoP.Checked = true;
                        break;
                }
            }
        }

        /// <summary>
        /// Defines the FilterDocumentTypeChanged
        /// </summary>
        public event EventHandler FilterDocumentTypeChanged;

        /// <summary>
        /// Defines the DocumentTypeChanged
        /// </summary>
        public event EventHandler DocumentTypeChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="FilterDocumentTypeBy"/> class.
        /// </summary>
        public FilterDocumentTypeBy()
        {
            InitializeComponent();
        }

        /// <summary>
        /// The Populate
        /// </summary>
        public void Populate()
        {
            if (_CoPDataTable != null && _CoPDataTable.Rows.Count > 0)
            {
                var view = _CoPDataTable.DefaultView;
                view.Sort = string.Format("{0} ASC", _CoPDisplayMember);

                comboBoxCoP.Enabled = true;
                comboBoxCoP.DataSource = view;
                comboBoxCoP.DisplayMember = _CoPDisplayMember;
                comboBoxCoP.ValueMember = _CoPValueMember;
                comboBoxCoP.BindingContext = BindingContext;
            }
            else
            {
                MessageBox.Show("No CoP data found.");
            }

            SetIfFolderFiltering();
        }

        /// <summary>
        /// The SetIfFolderFiltering
        /// </summary>
        public void SetIfFolderFiltering()
        {
            if (_ProjectID > 0)
            {
                string currentFolderDesc, currentFolderName;

                CurrentFileNameDesc(out currentFolderDesc, out currentFolderName);

                List<string> stdFolder = new List<string>();

                foreach (DataRow row in _DocFilterDataTable.Rows)
                {
                    if (!string.IsNullOrEmpty(row["Std_Folders"].ToString()))
                    {
                        stdFolder.AddRange(row["Std_Folders"].ToString().ToUpper().Split(';').ToList().Where(f => !stdFolder.Contains(f)).ToList());
                    }
                }

                if (stdFolder.Contains(currentFolderName.ToUpper()) || stdFolder.Contains(currentFolderDesc.ToUpper()))
                {
                    radioButtonFolder.Enabled = true;
                    radioButtonFolder.Checked = true;
                    Filter = FilterType.Folder;
                }
                else
                {
                    radioButtonFolder.Enabled = false;
                    Filter = FilterType.CoP;

                }

                FilterDocumentTypes();
            }
        }

        /// <summary>
        /// The CurrentFileNameDesc
        /// </summary>
        /// <param name="currentFolderDesc">The currentFolderDesc<see cref="string"/></param>
        /// <param name="currentFolderName">The currentFolderName<see cref="string"/></param>
        private void CurrentFileNameDesc(out string currentFolderDesc, out string currentFolderName)
        {
            currentFolderDesc = PWWrapper.aaApi_GetProjectStringProperty(PWWrapper.ProjectProperty.Desc, 0);
            currentFolderName = PWWrapper.GetProjectNamePath(_ProjectID);
            var currentFolderSplit = currentFolderName.Split('\\');
            currentFolderName = currentFolderSplit[currentFolderSplit.Length - 1];
        }

        /// <summary>
        /// Defines the _DocFilterDT
        /// </summary>
        private DataTable _DocFilterDT;

        /// <summary>
        /// The FilterDocumentTypes
        /// </summary>
        private void FilterDocumentTypes()
        {
            if (_DocFilterDataTable != null && _DocFilterDataTable.Rows.Count > 0)
            {
                _DocFilterDT = _DocFilterDataTable.Copy();
                switch (Filter)
                {
                    case FilterType.CoP:
                        for (var i = _DocFilterDT.Rows.Count - 1; i >= 0; i--)
                        {
                            var dr = _DocFilterDT.Rows[i];
                            var disciplines = dr["Discipline"].ToString();
                            var disciplinesSplit = disciplines.Split(',');

                            var lsDiscipline = new List<string>(disciplinesSplit);

                            if (!lsDiscipline.Contains(comboBoxCoP.SelectedValue.ToString()))
                            {
                                dr.Delete();
                            }
                        }
                        break;
                    case FilterType.Folder:
                        string currentFolderDesc, currentFolderName;

                        CurrentFileNameDesc(out currentFolderDesc, out currentFolderName);

                        for (var i = _DocFilterDT.Rows.Count - 1; i >= 0; i--)
                        {
                            var dr = _DocFilterDT.Rows[i];
                            var folders = dr["Std_Folders"].ToString().ToUpper().Split(';').ToList().Select(f => f.ToUpper()).ToList();

                            if (string.IsNullOrEmpty(folders[0].ToString()))
                            {
                                dr.Delete();
                            }
                            else
                            {
                                if (folders.Contains(currentFolderName.ToUpper()) || folders.Contains(currentFolderDesc.ToUpper()))
                                {

                                }
                                else
                                {
                                    dr.Delete();
                                }
                            }

                        }
                        break;
                    case FilterType.All:
                        break;
                    default:
                        break;
                }

                var dtFinalResult = _DocFilterDT.DefaultView.ToTable(true, new[] { "docfilter" });
                var vFinalResult = dtFinalResult.DefaultView;
                vFinalResult.Sort = "docfilter ASC";

                var docFilterList = new List<string> { "*All*" };

                docFilterList.AddRange(from DataRowView dataRowView in vFinalResult select dataRowView[_DocFilterDisplayMember].ToString());

                var bs = new BindingSource { DataSource = docFilterList };
                comboBoxDocTypes.DataSource = bs;

                if (comboBoxDocTypes.Items.Count > 0)
                {
                    comboBoxDocTypes.SelectedIndex = 0;
                }
                else
                {
                    MessageBox.Show("No document filters found with selected options", "USACE DCW", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// The FilterChanged
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void FilterChanged(object sender, EventArgs e)
        {
            if (FilterDocumentTypeChanged != null)
            {
                FilterDocumentTypeChanged(this, e);
                FilterDocumentTypes();
            }
        }

        /// <summary>
        /// The radioButtonCoP_CheckedChanged
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void radioButtonCoP_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonCoP.Checked)
            {
                comboBoxCoP.Enabled = true;
            }
            else
            {
                comboBoxCoP.Enabled = false;
            }
        }

        /// <summary>
        /// The comboBoxDocTypes_SelectedValueChanged
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void comboBoxDocTypes_SelectedValueChanged(object sender, EventArgs e)
        {
            if (DocumentTypeChanged != null)
            {
                DocumentTypeChanged(this, e);
            }
        }
    }
}
