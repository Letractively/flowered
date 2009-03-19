namespace Flowered.UI.SimpleClient
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Drawing.Imaging;
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
        /// Gets a new address from user.
        /// </summary>
        /// <param name="oldAddress"></param>
        private string GetNewAddress(string oldAddress)
        {
            string newAddress = string.Empty;
            FormSetAddress formAddAddress = new FormSetAddress();
            formAddAddress.Address = oldAddress;

            DialogResult dialogResult = formAddAddress.ShowDialog(this);
            if (dialogResult == DialogResult.OK)
            {
                newAddress = formAddAddress.Address;
                if (newAddress != oldAddress)
                {
                    // store address for next session(s)
                    settings.Address = newAddress;
                    settings.Save();
                }
            }

            return newAddress;
        }

        /// <summary>
        /// Gets a snapshoot from the current webpage and saves it to a file.
        /// </summary>
        private void GetSnapshot()
        {
            using (Bitmap bitmap = new Bitmap(webBrowser.ClientSize.Width,
                webBrowser.ClientSize.Height))
            {
                using (Graphics graphics = Graphics.FromImage(bitmap))
                {
                    graphics.CopyFromScreen(webBrowser.Parent.PointToScreen(webBrowser.Location),
                        new Point(0, 0), webBrowser.ClientSize,CopyPixelOperation.SourceCopy);
                    DateTime nowUtc = DateTime.Now.ToUniversalTime();
                    string now = nowUtc.ToString("yyyyMMdd_HHmmss");
                    string filename = string.Format(@".\Snapshots\flowered_{0}.png", now);
                    bitmap.Save(filename, ImageFormat.Png);
                }
            }
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
        /// <param name="address"></param>
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
                    string newAddress = GetNewAddress("http://");
                    if (newAddress != string.Empty)
                    {
                        Navigate(newAddress);
                    }
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
            string oldAddress = webBrowser.Url.ToString();
            if (oldAddress == "about:blank")
            {
                oldAddress = "http://";
            }
            string newAddress = GetNewAddress(oldAddress);
            if (newAddress != string.Empty)
            {
                Navigate(newAddress);
            }
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