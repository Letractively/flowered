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

namespace Flowered.UI.Controls
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Text;
    using System.Windows.Forms;

    #region Delegates

    public delegate void PreviewKeyDownHandler(object sender, PreviewKeyDownEventArgs e);

    #endregion Delegates

    public partial class BuriedWebBrowser : UserControl
    {
        #region Constructors

        public BuriedWebBrowser()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Events

        public new event PreviewKeyDownHandler PreviewKeyDown;

        #endregion Events

        #region Properties

        public bool Buried
        {
            get
            {
                return transparentPanel.Enabled;
            }
            set
            {
                transparentPanel.Enabled = value;
            }
        }

        public Uri Url
        {
            get
            {
                return webBrowser.Url;
            }
            set
            {
                webBrowser.Url = value;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        ///
        /// </summary>
        public void Navigate(Uri url)
        {
            webBrowser.Navigate(url);
        }

        /// <summary>
        ///
        /// </summary>
        public void Refresh(WebBrowserRefreshOption opt)
        {
            webBrowser.Refresh(opt);
        }

        /// <summary>
        ///
        /// </summary>
        public Bitmap Snapshot()
        {
            // grab and save snapshot
            Bitmap bitmap = new Bitmap(webBrowser.ClientSize.Width, webBrowser.ClientSize.Height);
            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                graphics.CopyFromScreen(webBrowser.Parent.PointToScreen(webBrowser.Location),
                    new Point(0, 0), webBrowser.ClientSize, CopyPixelOperation.SourceCopy);
            }

            return bitmap;
        }

        /// <summary>
        ///
        /// </summary>
        protected new void OnPreviewKeyDown(PreviewKeyDownEventArgs e)
        {
            if (PreviewKeyDown != null)
            {
                // Invokes the delegates.
                PreviewKeyDown(this, e);
            }
        }

        /// <summary>
        ///
        /// </summary>
        private void transparentPanel_MouseMove(object sender, MouseEventArgs e)
        {
            OnMouseMove(e);
        }

        /// <summary>
        ///
        /// </summary>
        private void webBrowser_DocumentCompleted(object sender,
            WebBrowserDocumentCompletedEventArgs e)
        {
            WebBrowser navigated = sender as WebBrowser;
            if (navigated == null)
            {
                return;
            }

            Focus();
        }

        /// <summary>
        ///
        /// </summary>
        private void webBrowser_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            OnPreviewKeyDown(e);
        }

        #endregion Methods
    }
}