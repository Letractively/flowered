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
    using System.Text;
    using System.Windows.Forms;

    public class TimedCursor
    {
        #region Fields

        private bool enabled = false;
        private bool showMouse = true;
        private Timer timer = new Timer();

        #endregion Fields

        #region Constructors

        public TimedCursor()
        {
            timer.Enabled = false;
            timer.Interval = 2000;
            timer.Tick += new System.EventHandler(timer_Tick);
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        ///
        /// </summary>
        public bool Enabled
        {
            get
            {
                return enabled;
            }
            set
            {
                enabled = value;
                timer.Enabled = enabled;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public int Timeout
        {
            get
            {
                return timer.Interval;
            }
            set
            {
                timer.Interval = value;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        ///
        /// </summary>
        public void Show()
        {
            if (enabled)
            {
                timer.Enabled = true;
                timer.Start();
            }
            if (!this.showMouse)
            {
                Cursor.Show();
                showMouse = true;
            }
        }

        /// <summary>
        ///
        /// </summary>
        private void timer_Tick(object sender, EventArgs e)
        {
            if (enabled)
            {
                timer.Enabled = false;
                timer.Stop();

                if (showMouse)
                {
                    Cursor.Hide();
                    showMouse = false;
                }
            }
        }

        #endregion Methods
    }
}