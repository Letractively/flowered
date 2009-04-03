#region Header

// Copyright (c) 2009 Daniel Schubert
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//    http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

#endregion Header

[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace Flowered.UI.SimpleClient
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Reflection;
    using System.Text;
    using System.Windows.Forms;

    using Flowered.App.SimpleClient;
    using Flowered.App.SimpleClient.Properties;
    using Flowered.UI.Controls;
    using Flowered.UI.Fullscreen;

    using log4net;

    public partial class FormMain : Form
    {
        #region Fields

        private const string addressName = "Address";

        private ScreenManager screenManager = new ScreenManager();
        private Settings settings = Flowered.App.SimpleClient.Properties.Settings.Default;
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
            SetMode(settings.Interactive);
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

                    string methodName = MethodBase.GetCurrentMethod().Name;
                    Logger.Info(methodName, string.Format("\"{0}\"", newAddress));
                }
            }

            return newAddress;
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
            catch (UriFormatException exception)
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                Logger.Exception(methodName, exception);
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
            catch (Exception exception)
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                Logger.Exception(methodName, exception);
            }
        }

        /// <summary>
        /// Gets a snapshoot from the current webpage and saves it to a file.
        /// </summary>
        private void ProcessSnapshot()
        {
            try
            {
                const string path = @".\Snapshots";

                // check for snapshot path before
                bool pathExists = Directory.Exists(path);
                if (!pathExists)
                {
                    Directory.CreateDirectory(path);
                }

                // check free disk space
                FileInfo fileInfo = new FileInfo(path);
                DriveInfo driveInfo = new DriveInfo(fileInfo.FullName);
                if (driveInfo.IsReady == true)
                {
                    long availableFreeSpaceMb = driveInfo.AvailableFreeSpace / 1024 / 1024;
                    if (availableFreeSpaceMb < settings.MinimumFreeSpaceMb)
                    {
                        return;
                    }
                }

                // grab and save snapshot
                DateTime nowUtc = DateTime.Now.ToUniversalTime();
                string now = nowUtc.ToString(@"yyyyMMdd_HHmmss");
                string filename = string.Format(@"flowered_{0}.png", now);

                using (Bitmap bitmap = webBrowser.Snapshot())
                {
                    if (bitmap != null)
                    {
                        bitmap.Save(path + @"\" + filename, ImageFormat.Png);
                    }
                }
            }
            catch (Exception exception)
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                Logger.Exception(methodName, exception);
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
            tmrSnapshot.Interval = 1000 * settings.SnapshotIntervalS;
            tmrRefresh.Interval = 1000 * settings.RefreshIntervalS;
            tmrRefresh.Enabled = (tmrRefresh.Interval > 0) ? true : false;
            timedCursor.Timeout = settings.MouseIntervalMs;
        }

        /// <summary>
        /// Toggles from windows mode into fullscreen mode and vice versa.
        /// </summary>
        /// <param name="form"></param>
        private void ToogleFullScreenMode(Form form)
        {
            screenManager.ToogleFullScreenMode(this);
            menuStrip.Visible = !screenManager.Fullscreen;

            if (!settings.Interactive)
            {
                timedCursor.Enabled = screenManager.Fullscreen;
                tmrSnapshot.Enabled = (tmrSnapshot.Interval > 0) ? screenManager.Fullscreen : false;
            }

            webBrowser.Refresh(WebBrowserRefreshOption.Normal);
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

        private void miInteractive_Click(object sender, EventArgs e)
        {
            miInteractive.Checked = !miInteractive.Checked;

            settings.Interactive = miInteractive.Checked;
            settings.Save();

            SetMode(settings.Interactive);
        }

        private void miRefresh_Click(object sender, EventArgs e)
        {
            webBrowser.Refresh(WebBrowserRefreshOption.Normal);
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

        private void miSnapshot_Click(object sender, EventArgs e)
        {
            ProcessSnapshot();
        }

        private void tmrSnapshot_Tick(object sender, EventArgs e)
        {
            ProcessSnapshot();
        }

        private void webBrowser_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.F11)
            {
                ToogleFullScreenMode(this);
            }
        }

        #endregion Methods

        private void webBrowser_MouseMove(object sender, MouseEventArgs e)
        {
            timedCursor.Show();
        }

        private void SetMode(bool intercative)
        {
            webBrowser.Buried = !intercative;
            miInteractive.Checked = settings.Interactive;

            Text = string.Format("{1} {0}", Application.ProductName,
                intercative ? "Interactive" : "Non-interactive");
        }

        private void tmrRefresh_Tick(object sender, EventArgs e)
        {
            webBrowser.Refresh(WebBrowserRefreshOption.IfExpired);
        }

        public void OnApplicationExit(object sender, EventArgs e)
        {
            try
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                Logger.Info(methodName, "Application shut down");
            }
            catch (NotSupportedException exception)
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                Logger.Exception(methodName, exception);
            }
        }
    }
}
