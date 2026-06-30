namespace USACE_Wizard_GUI.Controls
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Windows.Forms;
    using System.Linq;

    /// <summary>
    /// Defines the <see cref="MultiFileList" />
    /// </summary>
    public partial class MultiFileList : UserControl
    {
        /// <summary>
        /// Defines the _data
        /// </summary>
        private WizardData _data;

        /// <summary>
        /// Gets or sets the Data
        /// </summary>
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
                    DataSource = Data.Files;
                }
            }
        }

        /// <summary>
        /// Defines the _binding
        /// </summary>
        private BindingSource _binding = new BindingSource();

        /// <summary>
        /// Gets or sets the DataSource
        /// </summary>
        private FileImportCollection DataSource
        {
            get { return (FileImportCollection)_binding.DataSource; }
            set
            {
                _binding.DataSource = value;
                if (value != null)
                {
                    DataSource.FileUpdated -= DataSource_FileUpdated;
                    DataSource.SelectionChanged -= DataSource_SelectionChanged;
                    dataGridView1.SelectionChanged -= DataGridView1_SelectionChanged;
                    DataSource.AllFilesUpdated -= DataSource_AllFilesUpdate;
                    dataGridView1.DataSource = _binding;
                    dataGridView1.CellToolTipTextNeeded -= DataGridView1_CellToolTipTextNeeded;
                    Data.NamingData.ProjectChanged -= NamingData_ProjectChanged;
                    dataGridView1.ColumnHeaderMouseDoubleClick -= DataGridView1_ColumnHeaderMouseDoubleClick;

                    foreach (DataGridViewColumn item in dataGridView1.Columns)
                    {
                        item.Visible = false;
                    }

                    if (dataGridView1.DataSource != null)
                    {
                        dataGridView1.Columns["FileName"].Visible = true;
                        dataGridView1.Columns["FileName"].HeaderText = (Data.NamingData.Action == USACE_Wizard_GUI.Action.Create) ? "Importing Filename" : "Original Filename";
                        dataGridView1.Columns["PWNewName"].Visible = true;
                        dataGridView1.Columns["PWNewName"].HeaderText = "New Name";
                        dataGridView1.Columns["PWNewFileName"].Visible = true;
                        dataGridView1.Columns["PWNewFileName"].HeaderText = "New Filename";

                        //dataGridView1.Columns["FilterType"].Visible = true;
                        //dataGridView1.Columns["FilterCoP"].Visible = true;
                        //dataGridView1.Columns["FilterDocumentType"].Visible = true;
                        //dataGridView1.Columns["CurrentFolderName"].Visible = true;
                        //dataGridView1.Columns["PWOriginalKeywords"].Visible = true;
                        //dataGridView1.Columns["PWKeywords"].Visible = true;
                    }

                    addToolStripMenuItem.Visible = (Data.Files.Sum(i => i.DocumentID) == 0);

                    dataGridView1.Refresh();

                    var x = 0;

                    if (dataGridView1.SelectedRows.Count > 0)
                    {
                        x = dataGridView1.SelectedRows[0].Index;
                    }
                    UpdateButtonState(x);

                    dataGridView1.SelectionChanged += DataGridView1_SelectionChanged;
                    DataSource.FileUpdated += DataSource_FileUpdated;
                    DataSource.SelectionChanged += DataSource_SelectionChanged;
                    DataSource.AllFilesUpdated += DataSource_AllFilesUpdate;
                    dataGridView1.CellToolTipTextNeeded += DataGridView1_CellToolTipTextNeeded;
                    Data.NamingData.ProjectChanged += NamingData_ProjectChanged;
                    Data.Files.UseFileModifiedDateChanged += Files_UseFileModifiedDateChanged;
                    dataGridView1.ColumnHeaderMouseDoubleClick += DataGridView1_ColumnHeaderMouseDoubleClick;
                }
            }
        }

        private void DataGridView1_ColumnHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //MessageBox.Show(e.ColumnIndex.ToString());
            //MessageBox.Show(dataGridView1.Columns[e.ColumnIndex].Name);
            var ColName = dataGridView1.Columns[e.ColumnIndex].Name;
            if (ColName == "FileName")
            {
                DataSource.Sort((x, y) => x.FileName.CompareTo(y.FileName));
            }
            else if (ColName == "PWNewName")
            {
                DataSource.Sort((x, y) => x.PWNewName.CompareTo(y.PWNewName));
            }
            else if (ColName == "PWNewFilename")
            {
                DataSource.Sort((x, y) => x.PWNewFilename.CompareTo(y.PWNewFilename));
            }

            RefreshFiles();
        }

        private void Files_UseFileModifiedDateChanged(object sender, EventArgs e)
        {
            var index = dataGridView1.SelectedRows[0].Index;
            dataGridView1.Rows[index].Selected = true;
            dataGridView1.Refresh();
        }

        private void NamingData_ProjectChanged(object sender, EventArgs e)
        {
            dataGridView1.Refresh();
        }

        private void DataGridView1_CellToolTipTextNeeded(object sender, DataGridViewCellToolTipTextNeededEventArgs e)
        {
            if (e.RowIndex >= 0) e.ToolTipText = dataGridView1.Rows[e.RowIndex].Cells["ValidationMessage"].Value.ToString();//string.Format("This cell is in row {0}, col {1}", e.RowIndex, e.ColumnIndex);
        }

        /// <summary>
        /// The DataSource_AllFilesUpdate
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void DataSource_AllFilesUpdate(object sender, EventArgs e)
        {
            DataSource.AllFilesUpdated -= DataSource_AllFilesUpdate;

            //DataSource.SetUseSameNameForAllAppendNumber();

            foreach (var item in DataSource)
            {
                dataGridView1.InvalidateRow(DataSource.IndexOf(item));
            }

            DataSource.AllFilesUpdated += DataSource_AllFilesUpdate;
        }

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
        /// Initializes a new instance of the <see cref="MultiFileList"/> class.
        /// </summary>
        public MultiFileList()
        {
            InitializeComponent();

            buttonTop.Enabled = false;
            buttonUp.Enabled = false;
            buttonBottom.Enabled = false;
            buttonDown.Enabled = false;

            dataGridView1.CellMouseDown += DataGridView1_CellMouseDown;
        }

        /// <summary>
        /// The DataGridView1_CellMouseDown
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="DataGridViewCellMouseEventArgs"/></param>
        private void DataGridView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                int rowSelected = e.RowIndex;
                if (e.RowIndex != -1)
                {
                    dataGridView1.Rows[rowSelected].Selected = true;
                }
            }
        }

        /// <summary>
        /// The DataSource_SelectionChanged
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void DataSource_SelectionChanged(object sender, EventArgs e)
        {
            dataGridView1.Rows[DataSource.SelectedIndex].Selected = true;
        }

        /// <summary>
        /// The DataSource_Updated
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="FileInfoEventArgs"/></param>
        private void DataSource_FileUpdated(object sender, FileInfoEventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0 && DataSource.Contains(e.File))
            {
                dataGridView1.InvalidateRow(DataSource.IndexOf(e.File));
            }
        }

        /// <summary>
        /// The DataGridView1_SelectionChanged
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void DataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataSource.SelectedIndex = dataGridView1.SelectedRows[0].Index;
                UpdateButtonState(DataSource.SelectedIndex);
            }
        }

        /// <summary>
        /// The dataGridView1_RowEnter
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="DataGridViewCellEventArgs"/></param>
        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            var selectedIndex = e.RowIndex;
            UpdateButtonState(selectedIndex);
        }

        /// <summary>
        /// The UpdateButtonState
        /// </summary>
        /// <param name="selectedIndex">The selectedIndex<see cref="int"/></param>
        private void UpdateButtonState(int selectedIndex)
        {
            bool isFirstRow = (selectedIndex == 0);
            buttonTop.Enabled = !isFirstRow;
            buttonUp.Enabled = !isFirstRow;

            bool isLastRow = (dataGridView1.Rows.Count - 1 == selectedIndex);
            buttonBottom.Enabled = !isLastRow;
            buttonDown.Enabled = !isLastRow;
        }

        /// <summary>
        /// The buttonTop_Click
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void buttonTop_Click(object sender, EventArgs e)
        {
            int moveToIndex = 0;
            MoveSelectedTo(moveToIndex);
        }

        /// <summary>
        /// The MoveSelectedTo
        /// </summary>
        /// <param name="moveTo">The moveTo<see cref="int"/></param>
        private void MoveSelectedTo(int moveTo)
        {
            DataSource.MoveSelectedToIndex(moveTo);
        }

        /// <summary>
        /// The buttonBottom_Click
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void buttonBottom_Click(object sender, EventArgs e)
        {
            int moveToIndex = DataSource.Count - 1;
            MoveSelectedTo(moveToIndex);
        }

        /// <summary>
        /// The buttonUp_Click
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void buttonUp_Click(object sender, EventArgs e)
        {
            int moveToIndex = dataGridView1.SelectedRows[0].Index - 1;
            MoveSelectedTo(moveToIndex);
        }

        /// <summary>
        /// The buttonDown_Click
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void buttonDown_Click(object sender, EventArgs e)
        {
            int moveToIndex = dataGridView1.SelectedRows[0].Index + 1;
            MoveSelectedTo(moveToIndex);
        }

        /// <summary>
        /// The removeToolStripMenuItem_Click
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //dataGridView1.DataSource = null;
            DataSource.Remove(DataSource.Selected);
            //dataGridView1.Update();
            //dataGridView1.Refresh();
            //dataGridView1.Parent.Refresh();
            //DataSource = DataSource;
            RefreshFiles();
            DataSource.RenameDuplicate();
        }

        /// <summary>
        /// The openToolStripMenuItem_Click
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(DataSource.Selected.FilePath);
        }

        public void AddFile()
        {
            addToolStripMenuItem_Click(this, new EventArgs());
        }

        /// <summary>
        /// The addToolStripMenuItem_Click
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //FileDialog fd = new OpenFileDialog();
            OpenFileDialog fd = new OpenFileDialog();
            fd.Multiselect = true;

            if (fd.ShowDialog() == DialogResult.OK)
            {
                var addfileList = new List<string>(fd.FileNames);

                foreach (var item in addfileList)
                {
                    var newFile = new FileImport(item, Data.NamingData);
                    newFile.MatchNameAndFileName = Data.Files.MatchAllFilenameToName;
                    Data.Files.Add(newFile);
                }

                RefreshFiles();
                DataSource.RenameDuplicate();
            }
        }

        public void RefreshFiles()
        {
            dataGridView1.DataSource = null;
            dataGridView1.Update();
            dataGridView1.Refresh();
            dataGridView1.Parent.Refresh();
            DataSource = DataSource;
        }

        /// <summary>
        /// The refreshToolStripMenuItem_Click
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataSource = Data.Files;
        }
    }
}
