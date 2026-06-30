namespace USACE_Wizard_GUI.Controls
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    /// <summary>
    /// Defines the <see cref="ButtonsAndStatus" />
    /// </summary>
    public partial class ButtonsAndStatus : UserControl
    {
        public event EventHandler<bool> ActionButtonEnabled;
        /// <summary>
        /// Gets or sets the ActionButtonText
        /// </summary>
        public string ActionButtonText
        {
            get
            {
                return buttonCreate.Text;
            }
            set
            {
                buttonCreate.Text = value;
            }
        }

        /// <summary>
        /// Defines the CreateClicked
        /// </summary>
        public event EventHandler CreateClicked;

        /// <summary>
        /// Defines the CancelClicked
        /// </summary>
        public event EventHandler CancelClicked;

        /// <summary>
        /// The SetStatus
        /// </summary>
        /// <param name="Type">The Type<see cref="Status.TypeEnum"/></param>
        /// <param name="Message">The Message<see cref="string"/></param>
        public void SetStatus(Status.TypeEnum Type, string Message)
        {
            var CurrentStatus = new Status(Type, Message);
            label1.Text = Type.ToString() + ": " + Message;
            switch (Type)
            {
                case Status.TypeEnum.Info:
                    label1.ForeColor = Color.Green;
                    buttonCreate.Enabled = true;
                    break;
                case Status.TypeEnum.Warning:
                    label1.ForeColor = Color.OrangeRed;
                    buttonCreate.Enabled = false;
                    break;
                case Status.TypeEnum.Error:
                    label1.ForeColor = Color.Red;
                    buttonCreate.Enabled = false;
                    break;
                case Status.TypeEnum.Submit:
                    label1.ForeColor = Color.Blue;
                    buttonCreate.Enabled = false;
                    break;
                default:
                    break;
            }
            
            ActionButtonEnabled?.Invoke(this, buttonCreate.Enabled);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ButtonsAndStatus"/> class.
        /// </summary>
        public ButtonsAndStatus()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Defines the <see cref="Status" />
        /// </summary>
        public class Status
        {
            /// <summary>
            /// Defines the TypeEnum
            /// </summary>
            public enum TypeEnum
            {
                /// <summary>
                /// Defines the Info
                /// </summary>
                Info,
                /// <summary>
                /// Defines the Warning
                /// </summary>
                Warning,
                /// <summary>
                /// Defines the Error
                /// </summary>
                Error,
                /// <summary>
                /// Defines the Submit
                /// </summary>
                Submit
            }

            /// <summary>
            /// Gets or sets the Type
            /// </summary>
            public TypeEnum Type { get; set; }

            /// <summary>
            /// Gets or sets the Message
            /// </summary>
            public string Message { get; set; }

            /// <summary>
            /// Initializes a new instance of the <see cref="Status"/> class.
            /// </summary>
            /// <param name="type">The type<see cref="TypeEnum"/></param>
            /// <param name="message">The message<see cref="string"/></param>
            public Status(TypeEnum type, string message)
            {
                Type = type;
                Message = message;
            }
        }

        /// <summary>
        /// The buttonCreate_Click
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void buttonCreate_Click(object sender, EventArgs e)
        {
            if (CreateClicked != null)
            {
                CreateClicked(sender, e);
            }
        }

        /// <summary>
        /// The buttonCancel_Click
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void buttonCancel_Click(object sender, EventArgs e)
        {
            if (CancelClicked != null)
            {
                CancelClicked(sender, e);
            }

            ((Form)this.TopLevelControl).Close();
        }

        /// <summary>
        /// The label1_DoubleClick
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void label1_DoubleClick(object sender, EventArgs e)
        {
            if (label1.Text.StartsWith("Error"))
            {
                var msg = label1.Text.Replace("Error:", "").Trim().Replace(", ", Environment.NewLine + Environment.NewLine);
                MessageBox.Show(msg, "Found Issues", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
