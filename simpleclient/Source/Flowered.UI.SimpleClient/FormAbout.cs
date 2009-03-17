using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Reflection;
using System.Windows.Forms;

namespace Flowered.App.Standalone
{
    public partial class FormAbout : Form
    {
        public FormAbout()
        {
            InitializeComponent();
        }

        private void FormAbout_Load(object sender, EventArgs e)
        {
            string version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            string runtime = System.Environment.Version.ToString();

            string copyright = lblVersion.Text;
            copyright = copyright.Replace(@"{version}", version);
            copyright = copyright.Replace(@"{runtime}", runtime);
            lblVersion.Text = copyright;
        }
    }
}
