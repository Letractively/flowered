using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Flowered.App.Standalone
{
    public partial class FormSetUrl : Form
    {
        public FormSetUrl()
        {
            InitializeComponent();
        }

        public string Url
        {
            get
            {
                return tbUrl.Text;
            }
            set
            {
                tbUrl.Text = value;
            }
        }
    }
}
