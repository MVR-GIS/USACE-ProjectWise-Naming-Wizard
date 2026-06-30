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
    public partial class Description : UserControl
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
                    textBoxDescription.TextChanged -= TextBoxDescription_TextChanged;
                    Data.Files.UseOriginalDescriptionChanged -= Files_UseOriginalDescriptionChanged;

                    Data.Files.SelectionChanged += Files_SelectionChanged;
                    textBoxDescription.TextChanged += TextBoxDescription_TextChanged;
                    Data.Files.UseOriginalDescriptionChanged += Files_UseOriginalDescriptionChanged;
                }
            }
        }

        private void Files_UseOriginalDescriptionChanged(object sender, EventArgs e)
        {
            Files_SelectionChanged(this, e);
        }

        private void TextBoxDescription_TextChanged(object sender, EventArgs e)
        {
            Data.Files.Selected.Description = textBoxDescription.Text;
        }

        private void Files_SelectionChanged(object sender, EventArgs e)
        {
            Data.Files.Selected.DocCodeUpdated -= Selected_DocCodeUpdated1;
            textBoxDescription.TextChanged -= TextBoxDescription_TextChanged;
            textBoxDescription.Text = Data.Files.Selected.Description;
            textBoxDescription.Refresh();
            textBoxDescription.TextChanged += TextBoxDescription_TextChanged;
            Data.Files.Selected.DocCodeUpdated += Selected_DocCodeUpdated1;
        }

        private void Selected_DocCodeUpdated1(object sender, FileInfoEventArgs e)
        {
            textBoxDescription.TextChanged -= TextBoxDescription_TextChanged;
            textBoxDescription.Text = Data.Files.Selected.Description;
            textBoxDescription.TextChanged += TextBoxDescription_TextChanged;
        }

        public Description()
        {
            InitializeComponent();
        }
    }
}
