using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace USACE_Wizard_GUI.Controls
{
    public partial class DocumentName : UserControl
    {
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Bindable(true)]
        public override string Text
        {
            get { return groupBox1.Text; }
            set { groupBox1.Text = value; }
        }
        public event EventHandler NameChanged;
        public FileImport NewFile { get; set; }
        public DataView DocTypeDataView { get; set; }

        //public DataTable ProjectProperties { get; set; }

        public StandardNamingData Datasource { get; set; } = new StandardNamingData(0);

        public string ProjectID
        {
            get
            {
                return textBoxProjectID.Text;
            }
        }

        public string DocName
        {
            get
            {
                return labelNewDocumentName.Text;
            }
        }

        public string DocFileName
        {
            get
            {
                return textBoxFileName.Text;
            }
        }

        public string DocCode
        {
            get
            {
                return comboBoxDocumentCode.Text;
            }
        }

        public string SelectedDocType
        {
            get
            {
                return comboBoxDocumentCode.Text;
            }
        }

        public bool HighlightDocumentType
        {
            get
            {
                return (labelDocumentCode.BackColor == Color.Red);
            }
            set
            {
                if (value)
                {
                    labelDocumentCode.BackColor = Color.Red;
                    labelDocumentCode.ForeColor = Color.Yellow; 
                }
                else
                {
                    labelDocumentCode.BackColor = SystemColors.Control;
                    labelDocumentCode.ForeColor = SystemColors.ControlText;
                }
            }
        }

        public bool HighlightProjectID
        {
            get
            {
                return (labelProjectID.BackColor == Color.Red);
            }
            set
            {
                if (value)
                {
                    labelProjectID.BackColor = Color.Red;
                    labelProjectID.ForeColor = Color.Yellow;
                }
                else
                {
                    labelProjectID.BackColor = SystemColors.Control;
                    labelProjectID.ForeColor = SystemColors.ControlText;
                }
            }
        }

        public bool HighlightDocumentName
        {
            get
            {
                return (labelNewDocumentName.BackColor == Color.Red);
            }
            set
            {
                if (value)
                {
                    labelNewDocumentName.BackColor = Color.Red;
                    labelNewDocumentName.ForeColor = Color.Yellow;
                }
                else
                {
                    labelNewDocumentName.BackColor = SystemColors.Control;
                    labelNewDocumentName.ForeColor = SystemColors.HotTrack;
                }
            }
        }

        public bool HighlightDocumentFilename
        {
            get
            {
                return (textBoxFileName.BackColor == Color.Red);
            }
            set
            {
                if (value)
                {
                    textBoxFileName.BackColor = Color.Red;
                    textBoxFileName.ForeColor = Color.Yellow;
                }
                else
                {
                    textBoxFileName.BackColor = SystemColors.Control;
                    textBoxFileName.ForeColor = SystemColors.ControlText;
                }
            }
        }

        public DocumentName()
        {
            InitializeComponent();
        }

        private void checkBoxMatchDocumentName_CheckedChanged(object sender, EventArgs e)
        {
            textBoxFileName.ReadOnly = checkBoxMatchDocumentName.Checked;
            textBoxFileName.Enabled = !checkBoxMatchDocumentName.Checked;

            UpdateNewFileName();

            NameChanged?.Invoke(sender, e);
        }

        public void Populate()
        {
            var docTypeList = new List<string> { "{Select Document Type}" };
            docTypeList.AddRange(from DataRowView dataRowView in DocTypeDataView select dataRowView["DocumentType"].ToString());

            var bs = new BindingSource { DataSource = docTypeList };
            comboBoxDocumentCode.DataSource = bs;

            if (comboBoxDocumentCode.Items.Count > 0)
            {
                comboBoxDocumentCode.SelectedIndex = 0;
                UpdateNewFileName();
            }
            else
            {
                MessageBox.Show("No document types found with selected options", "USACE DCW", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            UpdateDocTypeDescription();
            PopulateProjectID();
        }

        private void UpdateDocTypeDescription()
        {
            if (comboBoxDocumentCode.SelectedIndex == 0)
            {
                textBoxDocumentCodeDescription.Text = string.Empty;
                return;
            }

            foreach (DataRow row in Datasource.StdNamingDataTable.Rows)
            {
                if (row["DocumentType"].ToString().ToLower() != comboBoxDocumentCode.SelectedValue.ToString().ToLower())
                    continue;

                if (string.IsNullOrEmpty(row["DocTypeDescription"].ToString()))
                {
                    textBoxDocumentCodeDescription.Text = string.Empty;
                    continue;
                }
                textBoxDocumentCodeDescription.Text = string.Format("{0} {1}", row["DocTypeDescription"],
                                                              row["Std_Folders"]);
            }
        }

        private void UpdateDocTypeUserAdded()
        {
            if (comboBoxDocumentCode.SelectedIndex == 0)
            {
                comboBoxUserAdded.Text = string.Empty;
                return;
            }

            bool setUserAdded = false;

            foreach (DataRow row in Datasource.StdNamingDataTable.Rows)
            {
                if (row["DocumentType"].ToString().ToLower() == comboBoxDocumentCode.SelectedValue.ToString().ToLower() && !string.IsNullOrEmpty(row["UserAdded"].ToString()))
                {
                    var usr = new List<string>(row["UserAdded"].ToString().Split(';'));
                    usr.Insert(0, "");
                    usr = usr.AsQueryable<string>().Select(m => m.Trim()).ToList().OrderBy(o => o).ToList();
                    var bs = new BindingSource { DataSource = usr };
                    comboBoxUserAdded.DataSource = bs;
                    setUserAdded = true;
                }
            }

            if (!setUserAdded)
            {
                var bs = new BindingSource { DataSource = new List<string>() };
                comboBoxUserAdded.DataSource = bs;
            }
        }

        private void UpdateNewFileName()
        {
            labelNewDocumentName.Text = textBoxProjectID.Text;

            if (comboBoxDocumentCode.SelectedValue != null)
                labelNewDocumentName.Text += "-" + comboBoxDocumentCode.SelectedValue;

            labelNewDocumentName.Text += "-" + dateTimePicker1.Value.Date.ToString("yyyy-MM-dd");

            if (!string.IsNullOrEmpty(comboBoxUserAdded.Text))
                labelNewDocumentName.Text += "-" + comboBoxUserAdded.Text;

            labelNewDocumentName.Text += Path.GetExtension(NewFile.FilePath);

            if (checkBoxMatchDocumentName.Checked)
            {
                textBoxFileName.Text = labelNewDocumentName.Text;
            }
            else
            {
                textBoxFileName.Text = Path.GetFileName(NewFile.FilePath);
            }
        }

        private void comboBoxDocumentCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateNewFileName();

            if (sender.GetType() == typeof(ComboBox) && ((ComboBox)sender).Name == "comboBoxDocumentCode")
            {
                UpdateDocTypeDescription();
                UpdateDocTypeUserAdded();
            }
        }

        private void labelNewDocumentName_TextChanged(object sender, EventArgs e)
        {
            if (NameChanged != null)
            {
                NameChanged(sender, e);
            }
        }

        private void textBoxFileName_TextChanged(object sender, EventArgs e)
        {
            if (NameChanged != null)
            {
                NameChanged(sender, e);
            }
        }

        private void PopulateProjectID()
        {
            var str = "Civil Works";

            var htProjProps = PWWrapper.GetProjectProperties(Datasource.ProjectID);

            if (htProjProps.Count > 0 && Datasource.ProjectPropertiesDataTable != null)
            {
                var draProgramFilter = Datasource.ProjectPropertiesDataTable.Select(string.Format("Program = '{0}'", str));
                var dtProgram = Datasource.ProjectPropertiesDataTable.Clone();

                draProgramFilter.CopyToDataTable(dtProgram, LoadOption.Upsert);

                var vProgram = dtProgram.DefaultView;

                vProgram.Sort = "Priority DESC";

                foreach (DataRowView drProjProp in vProgram)
                {
                    var projPropName = drProjProp["ProjectProperty"].ToString();

                    if (string.IsNullOrEmpty(projPropName) || !htProjProps.ContainsKey(projPropName) || string.IsNullOrEmpty(htProjProps[projPropName].ToString())) continue;

                    labelProjectID.Text = Datasource.ProjectPropertiesDataTable.Columns.Contains("Description") ? drProjProp["Description"].ToString() : "Project Properties table does not have a description column";

                    textBoxProjectID.Text = htProjProps[projPropName].ToString();
                    break;
                }
            }
        }

        private void comboBoxUserAdded_TextChanged(object sender, EventArgs e)
        {
            comboBoxDocumentCode_SelectedIndexChanged(sender, e);
        }
    }
}
