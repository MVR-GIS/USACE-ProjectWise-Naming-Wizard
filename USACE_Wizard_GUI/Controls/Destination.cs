using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace USACE_Wizard_GUI.Controls
{
    public partial class Destination : UserControl
    {
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Bindable(true)]
        public override string Text
        {
            get { return groupBoxDestination.Text; }
            set { groupBoxDestination.Text = value; }
        }
        public bool ReadOnly
        {
            get
            {
                return !buttonChangeLocation.Visible;
            }
            set
            {
                buttonChangeLocation.Visible = !value;
            }
        }

        public int DestinationProjID
        {
            get
            {
                if (!string.IsNullOrEmpty(DestinationPath))
                {
                    return PWWrapper.GetProjectNoFromPath(DestinationPath);
                }
                else
                {
                    return 0;
                }
            }
        }
        public string DestinationPath
        {
            get { return textBoxPath.Text; }
            set { textBoxPath.Text = value; }
        }

        public event EventHandler DestinationPathChanged;
        public Destination()
        {
            InitializeComponent();
        }

        private void textBoxPath_TextChanged(object sender, EventArgs e)
        {
            if (DestinationPathChanged != null)
            {
                DestinationPathChanged(sender, e);
            }
        }

        private void buttonChangeLocation_Click(object sender, EventArgs e)
        {
            var _mIDestProjId = PWWrapper.aaApi_SelectProjectDlg(IntPtr.Zero, "Select Destination", 0);

            if (_mIDestProjId <= 0)
                return;

            var projectPath = PWWrapper.GetProjectNamePath(_mIDestProjId);

            DestinationPath = projectPath;
        }
    }
}
