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
        //private bool cursorVisible = true;

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
            /* if (screenManager.Fullscreen)
            {
                Cursor.Hide();
            }
            else
            {
                Cursor.Show();
            } */
            //cursorVisible = !screenManager.Fullscreen;
            menuStrip.Visible = !screenManager.Fullscreen;
        }

        private void miFullscreen_Click(object sender, EventArgs e)
        {
            ToogleFullScreenMode(this);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ToogleFullScreenMode(this);
        }

        private void tmrCursor_Tick(object sender, EventArgs e)
        {
            //tmrCursor.Stop();

            //if (cursorVisible)
            //{
            //    Cursor.Hide();
            //    cursorVisible = false;
            //}
        }

        private void FormMain_MouseMove(object sender, MouseEventArgs e)
        {
            //tmrCursor.Start();
            //tmrCursor.Enabled = false;

            //if (!cursorVisible)
            //{
            //    Cursor.Show();
            //    cursorVisible = true;
            //}
        }

        private void miExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
