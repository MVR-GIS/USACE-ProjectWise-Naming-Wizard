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
    public partial class Keywords : UserControl
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
                    textBoxKeywords.TextChanged -= TextBoxKeywords_TextChanged;
                    checkedListBox1.ItemCheck -= CheckedListBox1_ItemCheck;

                    Data.Files.SelectionChanged += Files_SelectionChanged;
                    textBoxKeywords.TextChanged += TextBoxKeywords_TextChanged;
                    checkedListBox1.ItemCheck += CheckedListBox1_ItemCheck;
                }
            }
        }

        private void CheckedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            checkedListBox1.ItemCheck -= CheckedListBox1_ItemCheck;
            var isChecked = (e.NewValue == CheckState.Checked) ? true : false;
            checkedListBox1.SetItemChecked(e.Index, isChecked);
            TextBoxKeywords_TextChanged(this, new EventArgs());
            checkedListBox1.ItemCheck += CheckedListBox1_ItemCheck;
        }

        private void TextBoxKeywords_TextChanged(object sender, EventArgs e)
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

            Data.Files.Selected.Keywords = retVal;
        }

        private void Files_SelectionChanged(object sender, EventArgs e)
        {
            Data.Files.Selected.DocCodeUpdated -= Selected_DocCodeUpdated;

            Populate();

            Data.Files.Selected.DocCodeUpdated += Selected_DocCodeUpdated;
        }

        private BindingSource _binding = new BindingSource(); 

        private void Populate()
        {
            _binding.Clear();
            textBoxKeywords.Text = string.Empty;

            if (!string.IsNullOrEmpty(Data.Files.Selected.DocumentTypeCode) && !Data.Files.Selected.DocumentTypeCode.StartsWith("{"))
            {
                _binding = new BindingSource { DataSource = Data.Files.Selected.KeywordsAvailable.OrderBy(m => m).ToList() };
                checkedListBox1.DataSource = _binding;

                foreach (string item in Data.Files.Selected.KeywordsSelected)
                {
                    if (!string.IsNullOrEmpty(item)) checkedListBox1.SetItemChecked(checkedListBox1.Items.IndexOf(item), true);
                }
            }
        }

        private void Selected_DocCodeUpdated(object sender, FileInfoEventArgs e)
        {
            Populate();
        }

        public Keywords()
        {
            InitializeComponent();
        }
    }
}
