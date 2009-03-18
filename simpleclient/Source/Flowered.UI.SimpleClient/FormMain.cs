using Microsoft.Win32;
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
        private RegistryKey registryKey = Application.UserAppDataRegistry;
        private const string addressName = "Address";

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
            string url = webBrowser.Url.ToString();
            if (url == "about:blank")
            {
                url = "http://";
            }
            SetUrl(url);
        }

        private void SetUrl(string address)
        {
            FormSetUrl formAddUrl = new FormSetUrl();
            formAddUrl.Url = address;
            
            DialogResult dialogResult = formAddUrl.ShowDialog(this);
            if (dialogResult == DialogResult.OK)
            {
                string url = formAddUrl.Url;
                Navigate(url);

                // check for url to delete
                if (url.Length > 0)
                {
                    StoreAddress(url);
                }
                else
                {
                    if (registryKey.GetValue(addressName) != null)
                    {
                        registryKey.DeleteValue(addressName);
                    }
                }
            }
        }

        // Navigates to the given URL if it is valid.
        // @see: http://msdn.microsoft.com/en-us/library/system.windows.forms.webbrowser.url.aspx#
        private void Navigate(string address)
        {
            if (address.Equals("about:blank"))
            {
                return;
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
                registryKey.SetValue(addressName, address);
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
                if (registryKey.GetValue(addressName) != null)
                {
                    string address = registryKey.GetValue(addressName).ToString();
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
