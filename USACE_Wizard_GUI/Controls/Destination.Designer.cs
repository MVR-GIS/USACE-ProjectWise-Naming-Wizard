namespace USACE_Wizard_GUI.Controls
{
    partial class Destination
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBoxDestination = new System.Windows.Forms.GroupBox();
            this.textBoxPath = new System.Windows.Forms.TextBox();
            this.buttonChangeLocation = new System.Windows.Forms.Button();
            this.groupBoxDestination.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxDestination
            // 
            this.groupBoxDestination.Controls.Add(this.textBoxPath);
            this.groupBoxDestination.Controls.Add(this.buttonChangeLocation);
            this.groupBoxDestination.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxDestination.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxDestination.Location = new System.Drawing.Point(3, 3);
            this.groupBoxDestination.Name = "groupBoxDestination";
            this.groupBoxDestination.Padding = new System.Windows.Forms.Padding(0);
            this.groupBoxDestination.Size = new System.Drawing.Size(299, 44);
            this.groupBoxDestination.TabIndex = 0;
            this.groupBoxDestination.TabStop = false;
            this.groupBoxDestination.Text = "Destination";
            // 
            // textBoxPath
            // 
            this.textBoxPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxPath.Enabled = false;
            this.textBoxPath.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxPath.Location = new System.Drawing.Point(3, 19);
            this.textBoxPath.Name = "textBoxPath";
            this.textBoxPath.Size = new System.Drawing.Size(233, 20);
            this.textBoxPath.TabIndex = 0;
            this.textBoxPath.TextChanged += new System.EventHandler(this.textBoxPath_TextChanged);
            // 
            // buttonChangeLocation
            // 
            this.buttonChangeLocation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonChangeLocation.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonChangeLocation.Location = new System.Drawing.Point(241, 18);
            this.buttonChangeLocation.Margin = new System.Windows.Forms.Padding(2);
            this.buttonChangeLocation.MaximumSize = new System.Drawing.Size(52, 21);
            this.buttonChangeLocation.MinimumSize = new System.Drawing.Size(52, 21);
            this.buttonChangeLocation.Name = "buttonChangeLocation";
            this.buttonChangeLocation.Size = new System.Drawing.Size(52, 21);
            this.buttonChangeLocation.TabIndex = 1;
            this.buttonChangeLocation.Text = "Change";
            this.buttonChangeLocation.UseVisualStyleBackColor = true;
            this.buttonChangeLocation.Click += new System.EventHandler(this.buttonChangeLocation_Click);
            // 
            // Destination
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxDestination);
            this.MaximumSize = new System.Drawing.Size(0, 50);
            this.MinimumSize = new System.Drawing.Size(305, 50);
            this.Name = "Destination";
            this.Padding = new System.Windows.Forms.Padding(3);
            this.Size = new System.Drawing.Size(305, 50);
            this.groupBoxDestination.ResumeLayout(false);
            this.groupBoxDestination.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxDestination;
        private System.Windows.Forms.TextBox textBoxPath;
        private System.Windows.Forms.Button buttonChangeLocation;
    }
}
