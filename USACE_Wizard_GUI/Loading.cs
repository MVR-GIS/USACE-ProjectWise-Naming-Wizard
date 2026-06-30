namespace USACE_Wizard_GUI
{
    using System.Windows.Forms;

    /// <summary>
    /// Defines the <see cref="Loading" />
    /// </summary>
    public partial class Loading : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Loading"/> class.
        /// </summary>
        /// <param name="message">The message<see cref="string"/></param>
        public Loading(string message)
        {
            InitializeComponent();
            //USACE_Wizard_GUI.Properties.Resources.Loading.MakeTransparent(System.Drawing.Color.FromArgb(254, 255, 252));
            BackColor = System.Drawing.Color.FromArgb(254, 255, 252);
            label1.Text = message;
            BringToFront();
        }

        /// <summary>
        /// The Loading_KeyDown
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="KeyEventArgs"/></param>
        private void Loading_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }
    }
}
