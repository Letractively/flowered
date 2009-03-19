namespace Flowered.UI.SimpleClient
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Reflection;
    using System.Text;
    using System.Windows.Forms;

    using Flowered.App.Standalone;
    using Flowered.App.Standalone.Properties;
    using Flowered.UI.Controls;
    using Flowered.UI.Fullscreen;

    public partial class FormMain : Form
    {
        #region Fields

        private const string addressName = "Address";

        private ScreenManager screenManager = new ScreenManager();
        private Settings settings = Flowered.App.Standalone.Properties.Settings.Default;
        private TimedCursor timedCursor = new TimedCursor();

        #endregion Fields

        #region Constructors

        public FormMain()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Methods

        private void FormMain_Shown(object sender, EventArgs e)
        {
            ReadSettings();
            ProcessAddress(settings.Address);
        }

        /// <summary>
        /// Navigates to the given URL if it is valid.
        /// </summary>
        /// <param name="form"></param>
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

        /// <summary>
        ///
        /// </summary>
        private void ProcessAddress(string address)
        {
            try
            {
                // Get the connection string from the registry.
                if (address != string.Empty)
                {
                    Navigate(address);
                }
                else
                {
                    GetNewAddress("http://");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        ///
        /// </summary>
        private void ReadSettings()
        {
            // upgrade user settings from old settings
            if (settings.Initialized == false)
            {
                settings.Upgrade();
                settings.Initialized = true;
                settings.Save();
            }

            // fullscreen mode settings
            if (settings.FullscreenMode)
            {
                ToogleFullScreenMode(this);
            }

            // timer settings
            tmrSnapshot.Interval = 1000 * settings.SnapshotInterval;
            tmrRefresh.Interval = 1000 * settings.RefreshInterval;
        }

        /// <summary>
        /// Gets a new address from user.
        /// </summary>
        /// <param name="oldAddress"></param>
        private void GetNewAddress(string oldAddress)
        {
            FormSetAddress formAddAddress = new FormSetAddress();
            formAddAddress.Address = oldAddress;

            DialogResult dialogResult = formAddAddress.ShowDialog(this);
            if (dialogResult == DialogResult.OK)
            {
                string newAddress = formAddAddress.Address;
                if (newAddress != oldAddress)
                {
                    Navigate(newAddress);

                    // store address for next session(s)
                    settings.Address = newAddress;
                    settings.Save();
                }
            }
        }

        /// <summary>
        /// Toggles from windows mode into fullscreen mode and vice versa.
        /// </summary>
        /// <param name="form"></param>
        private void ToogleFullScreenMode(Form form)
        {
            screenManager.ToogleFullScreenMode(this);
            menuStrip.Visible = !screenManager.Fullscreen;
            timedCursor.Enabled = screenManager.Fullscreen;
        }

        private void miAbout_Click(object sender, EventArgs e)
        {
            FormAbout formAbout = new FormAbout();
            formAbout.ShowDialog(this);
        }

        private void miExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void miFullscreen_Click(object sender, EventArgs e)
        {
            ToogleFullScreenMode(this);
        }

        private void miRefresh_Click(object sender, EventArgs e)
        {
            webBrowser.Refresh();
        }

        private void miSetUrl_Click(object sender, EventArgs e)
        {
            string address = webBrowser.Url.ToString();
            if (address == "about:blank")
            {
                address = "http://";
            }
            GetNewAddress(address);
        }

        private void transparentPanel_MouseMove(object sender, MouseEventArgs e)
        {
            timedCursor.Show();
        }

        private void webBrowser_DocumentCompleted(object sender,
            WebBrowserDocumentCompletedEventArgs e)
        {
            this.Focus();
        }

        #endregion Methods
    }
}