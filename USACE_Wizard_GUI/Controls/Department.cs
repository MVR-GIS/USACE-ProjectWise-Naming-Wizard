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
    public partial class Department : UserControl
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

                if (_data != null)
                {
                    Data.Files.SelectionChanged -= Files_SelectionChanged;
                    comboBoxDepartment.TextChanged -= ComboBoxDepartment_TextChanged;

                    DepartmentDatasource = Data.NamingData.DepartmentsDataTable;

                    Data.Files.SelectionChanged += Files_SelectionChanged;
                    comboBoxDepartment.TextChanged += ComboBoxDepartment_TextChanged;
                }
            }
        }

        private DataTable DepartmentDatasource
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

        private void ComboBoxDepartment_TextChanged(object sender, EventArgs e)
        {
            Data.Files.Selected.Department = DepartmentNo;
        }

        private int DepartmentNo
        {
            get { return Convert.ToInt32(comboBoxDepartment.SelectedValue); }
            set { comboBoxDepartment.SelectedValue = value; }
        }

        private void Files_SelectionChanged(object sender, EventArgs e)
        {
            DepartmentNo = Data.Files.Selected.Department;
        }

        public Department()
        {
            InitializeComponent();
        }
    }
}
