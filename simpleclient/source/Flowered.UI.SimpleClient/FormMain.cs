using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Flowered.UI.Fullscreen;

namespace Flowered.UI.SimpleClient
{
    public partial class FormMain : Form
    {
        private ScreenManager screenManager;

        public FormMain()
        {
            InitializeComponent();
            InitializeHelpers();
        }

        private void InitializeHelpers()
        {
            screenManager = new ScreenManager();
        }

        private void FormMain_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.F11)
            {
                ToogleFullScreenMode(this);
            }
        }

        private void ToogleFullScreenMode(Form form)
        {
            screenManager.ToogleFullScreenMode(this);
            menuStrip.Visible = !screenManager.Fullscreen;
        }

        private void miFullscreen_Click(object sender, EventArgs e)
        {
            ToogleFullScreenMode(this);
        }
    }
}
