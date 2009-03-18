using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Flowered.App.Standalone;
using Flowered.UI.Controls;
using Flowered.UI.Fullscreen;

namespace Flowered.UI.SimpleClient
{
    public partial class FormMain : Form
    {
        private ScreenManager screenManager;
        private TimedCursor timeCursor;

        public FormMain()
        {
            InitializeComponent();
            InitializeHelpers();
        }

        private void InitializeHelpers()
        {
            screenManager = new ScreenManager();
            timeCursor = new TimedCursor();
        }

        private void ToogleFullScreenMode(Form form)
        {
            screenManager.ToogleFullScreenMode(this);
            menuStrip.Visible = !screenManager.Fullscreen;
            timeCursor.Enabled = screenManager.Fullscreen;
        }

        private void miFullscreen_Click(object sender, EventArgs e)
        {
            ToogleFullScreenMode(this);
        }

        private void miExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void miRefresh_Click(object sender, EventArgs e)
        {
            webBrowser.Refresh();
        }

        private void webBrowser_DocumentCompleted(object sender,
            WebBrowserDocumentCompletedEventArgs e)
        {
            this.Focus();
        }

        private void miAbout_Click(object sender, EventArgs e)
        {
            FormAbout formAbout = new FormAbout();
            formAbout.ShowDialog(this);
        }

        private void miSetUrl_Click(object sender, EventArgs e)
        {
            SetUrl(webBrowser.Url.ToString());
        }

        private void SetUrl(string address)
        {
            FormSetUrl formAddUrl = new FormSetUrl();
            formAddUrl.Url = address;
            DialogResult dialogResult = formAddUrl.ShowDialog(this);
            if (dialogResult == DialogResult.OK)
            {
                Navigate(formAddUrl.Url);

                if (formAddUrl.Url.Length > 0)
                {
                    StoreAddress(formAddUrl.Url);
                }
                else
                {
                    Application.UserAppDataRegistry.DeleteValue("Address");
                }
            }
        }

        // Navigates to the given URL if it is valid.
        // @see: http://msdn.microsoft.com/en-us/library/system.windows.forms.webbrowser.url.aspx#
        private void Navigate(string address)
        {
            if (String.IsNullOrEmpty(address))
            {
                return;
            }
            if (address.Equals("about:blank"))
            {
                return;
            }
            if (!address.StartsWith("http://") && !address.StartsWith("https://"))
            {
                address = "http://" + address;
            }

            try
            {
                webBrowser.Navigate(new Uri(address));
            }
            catch (System.UriFormatException)
            {
                return;
            }
        }

        private void StoreAddress(string address)
        {
            try
            {
                // Save the connection string to the registry, if it has changed.
                Application.UserAppDataRegistry.SetValue("Address", address);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ReadAddress()
        {
            try
            {
                // Get the connection string from the registry.
                if (Application.UserAppDataRegistry.GetValue("Address") != null)
                {
                    string address = Application.UserAppDataRegistry.GetValue("Address").ToString();
                    Navigate(address);
                }
                else
                {
                    SetUrl("http://");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FormMain_Shown(object sender, EventArgs e)
        {
            ReadAddress();
        }

        private void transparentPanel_MouseMove(object sender, MouseEventArgs e)
        {
            timeCursor.Show();
        }
    }
}
