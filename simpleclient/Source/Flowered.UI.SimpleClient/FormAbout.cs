namespace Flowered.App.Standalone
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Reflection;
    using System.Text;
    using System.Windows.Forms;

    public partial class FormAbout : Form
    {
        #region Constructors

        public FormAbout()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Methods

        private void FormAbout_Load(object sender, EventArgs e)
        {
            string version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            string runtime = System.Environment.Version.ToString();

            string copyright = lblVersion.Text;
            copyright = copyright.Replace(@"{version}", version);
            copyright = copyright.Replace(@"{runtime}", runtime);
            lblVersion.Text = copyright;
        }

        #endregion Methods
    }
}