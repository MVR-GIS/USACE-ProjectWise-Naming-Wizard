namespace USACE_Wizard_GUI.Controls
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Linq;
    using System.Windows.Forms;

    /// <summary>
    /// Defines the <see cref="Attributes" />
    /// </summary>
    public partial class Attributes : UserControl
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
            get { return groupBox1.Text; }
            set { groupBox1.Text = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether ShowUseOriginal
        /// </summary>
        public bool ShowUseOriginal
        {
            get
            {
                return checkBoxKeepOrig.Visible;
            }
            set
            {
                checkBoxKeepOrig.Hide();
            }
        }

        /// <summary>
        /// Gets or sets the Description
        /// </summary>
        public string Description
        {
            get
            {
                return textBoxDescription.Text;
            }
            set
            {
                textBoxDescription.Text = value;
                if (_originalDescription == null && !string.IsNullOrEmpty(value))
                {
                    _originalDescription = value;
                }
            }
        }

        /// <summary>
        /// Defines the _originalDescription
        /// </summary>
        private string _originalDescription;

        /// <summary>
        /// Gets or sets the Keywords
        /// </summary>
        public string Keywords
        {
            get
            {
                string retVal = textBoxKeywords.Text;

                foreach (string item in checkedListBox1.CheckedItems)
                {
                    if (string.IsNullOrEmpty(retVal))
                    {
                        retVal = item;
                    }
                    else
                    {
                        retVal += "; " + item;
                    }
                }

                return retVal;
            }
            set
            {
                _originalKeywords = value;
                _keywordsList = value.Split(';').ToList().Select(m => m.Trim()).ToList();
            }
        }

        /// <summary>
        /// Sets the DepartmentDatasource
        /// </summary>
        public DataTable DepartmentDatasource
        {
            set
            {
                value.DefaultView.Sort = "o_departname";
                DataTable dt = value.DefaultView.ToTable();
                DataRow dr = dt.NewRow();
                dr["o_departno"] = 0;
                dr["o_departname"] = "---Select---";
                dr["o_departdesc"] = "";
                dt.Rows.InsertAt(dr, 0);
                comboBoxDepartment.DataSource = dt;
                comboBoxDepartment.DisplayMember = "o_departname";
                comboBoxDepartment.ValueMember = "o_departno";
                comboBoxDepartment.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Gets or sets the DepartmentNo
        /// </summary>
        public int DepartmentNo
        {
            get { return Convert.ToInt32(comboBoxDepartment.SelectedValue); }
            set { comboBoxDepartment.SelectedValue = value; }
        }

        /// <summary>
        /// Defines the _originalKeywords
        /// </summary>
        private string _originalKeywords;

        /// <summary>
        /// Defines the _keywordsList
        /// </summary>
        private List<string> _keywordsList;

        /// <summary>
        /// Initializes a new instance of the <see cref="Attributes"/> class.
        /// </summary>
        public Attributes()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Defines the vDocFilter
        /// </summary>
        private DataRow[] vDocFilter;

        /// <summary>
        /// The SetAttributeByStandardNameing
        /// </summary>
        /// <param name="StandardNamingTable">The StandardNamingTable<see cref="DataTable"/></param>
        /// <param name="SelectedDocumentCode">The SelectedDocumentCode<see cref="string"/></param>
        public void SetAttributeByStandardNameing(DataTable StandardNamingTable, string SelectedDocumentCode)
        {
            if (SelectedDocumentCode != "{Select Document Type}")
            {
                vDocFilter = StandardNamingTable.Select(string.Format("documenttype = '{0}'", SelectedDocumentCode));


                var _keywords = new List<string>(vDocFilter.FirstOrDefault()["Keyword"].ToString().Split(';'));
                var _selectedKeywords = new List<string>(_keywords.Where(k => k.StartsWith("!")).ToList().Select(m => m.StartsWith("!") ? m.Replace("!", "") : m).ToList().Select(m => m.Trim()).ToList().Where(m => !string.IsNullOrEmpty(m)).ToList());
                _keywords = _keywords.AsQueryable().Select(m => m.Trim()).ToList();
                _keywords = _keywords.Select(m => m.StartsWith("!") ? m.Replace("!", "") : m).ToList().Where(m => !string.IsNullOrEmpty(m)).ToList();

                if (_keywordsList.Count > 0)
                {
                    foreach (var item in _keywordsList)
                    {
                        if (!string.IsNullOrEmpty(item) && !_selectedKeywords.Contains(item))
                        {
                            _selectedKeywords.Add(item);
                        }
                    }
                }

                if (_selectedKeywords.Count > 0)
                {
                    foreach (var item in _selectedKeywords)
                    {
                        if (!string.IsNullOrEmpty(item) && !_keywords.Contains(item))
                        {
                            _keywords.Add(item);
                        }
                    }
                }

                var bs = new BindingSource { DataSource = _keywords.OrderBy(m => m).ToList() };
                checkedListBox1.DataSource = bs;

                foreach (int i in checkedListBox1.CheckedIndices)
                {
                    checkedListBox1.SetItemCheckState(i, CheckState.Unchecked);
                }

                foreach (string item in _selectedKeywords)
                {
                    checkedListBox1.SetItemChecked(checkedListBox1.Items.IndexOf(item), true);
                }

                SetDescription();
            }
        }

        /// <summary>
        /// The SetDescription
        /// </summary>
        private void SetDescription()
        {
            var _defaultDescription = vDocFilter.FirstOrDefault()["Description"].ToString();

            if (!checkBoxKeepOrig.Checked && !string.IsNullOrEmpty(_defaultDescription))
            {
                Description = _defaultDescription;
            }
        }

        /// <summary>
        /// The checkBoxKeepOrig_CheckedChanged
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void checkBoxKeepOrig_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxKeepOrig.Checked)
            {
                textBoxDescription.Text = _originalDescription;
            }

            SetDescription();
        }
    }
}
