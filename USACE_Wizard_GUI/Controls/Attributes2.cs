using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace USACE_Wizard_GUI.Controls
{
    public partial class Attributes2 : UserControl
    {
        private WizardData _data;
        public WizardData Data
        {
            get
            {
                return _data;
            }
            set
            {
                _data = value;

                if (_data != null && _data.Files != null && _data.Files.Count > 0)
                {
                    Data.Files.SelectionChanged -= Files_SelectionChanged;
                    textBoxDescription.TextChanged -= TextBoxDescription_TextChanged;
                    checkedListBox1.ItemCheck -= CheckedListBox1_ItemCheck;
                    textBoxKeywords.TextChanged -= TextBoxKeywords_TextChanged;
                    comboBoxDepartment.TextChanged -= ComboBoxDepartment_TextChanged;
                    if (Data.Files.Selected != null)Data.Files.Selected.Updated -= Selected_Updated;

                    if (Data.Files.Selected != null)
                    {
                        Description = Data.Files.Selected.Description;
                        Keywords = Data.Files.Selected.Keywords;
                        SetAttributeByStandardNameing(Data.NamingData.StdNamingDataTable, Data.Files.Selected.DocCode);
                        DepartmentDatasource = Data.NamingData.DepartmentsDataTable;
                        DepartmentNo = Data.NamingData.UserSettings.DepartmentNumber; 
                    }

                    Data.Files.SelectionChanged += Files_SelectionChanged;
                    textBoxDescription.TextChanged += TextBoxDescription_TextChanged;
                    checkedListBox1.ItemCheck += CheckedListBox1_ItemCheck;
                    textBoxKeywords.TextChanged += TextBoxKeywords_TextChanged;
                    comboBoxDepartment.TextChanged += ComboBoxDepartment_TextChanged;
                    if (Data.Files.Selected != null) Data.Files.Selected.Updated += Selected_Updated;
                }
            }
        }

        private void Selected_Updated(object sender, FileInfoEventArgs e)
        {
            SetDescription();
        }

        private void ComboBoxDepartment_TextChanged(object sender, EventArgs e)
        {
            Data.Files.Selected.Department = DepartmentNo;
            Data.NamingData.UserSettings.DepartmentNumber = DepartmentNo;
        }

        private void TextBoxKeywords_TextChanged(object sender, EventArgs e)
        {
            Data.Files.Selected.Keywords = Keywords;
        }

        private void CheckedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            Data.Files.Selected.Keywords = Keywords;
        }

        private void TextBoxDescription_TextChanged(object sender, EventArgs e)
        {
            Data.Files.Selected.Description = Description;
        }

        private void Files_SelectionChanged(object sender, EventArgs e)
        {
            
        }

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

        private string _originalDescription;

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
                if (!string.IsNullOrEmpty(value))
                {
                    _keywordsList = value.Split(';').ToList().Select(m => m.Trim()).ToList(); 
                }
                else
                {
                    _keywordsList = new List<string>();
                }
            }
        }

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

        public int DepartmentNo
        {
            get { return Convert.ToInt32(comboBoxDepartment.SelectedValue); }
            set { comboBoxDepartment.SelectedValue = value; }
        }

        private string _originalKeywords;

        private List<string> _keywordsList;

        public Attributes2()
        {
            InitializeComponent();
        }

        private DataRow[] vDocFilter;

        public void SetAttributeByStandardNameing(DataTable StandardNamingTable, string SelectedDocumentCode,string breakme)
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

        private void SetDescription()
        {
            var _defaultDescription = vDocFilter.FirstOrDefault()["Description"].ToString();

            if (!checkBoxKeepOrig.Checked && !string.IsNullOrEmpty(_defaultDescription))
            {
                Description = _defaultDescription;
            }
        }

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
