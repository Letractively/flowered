using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Flowered.UI.Controls
{
    public class TimedCursor
    {
        private Timer timer = new Timer();
        private bool showMouse = true;
        private bool enabled = false;

        public TimedCursor()
        {
            timer.Enabled = false;
            timer.Interval = 2000;
            timer.Tick += new System.EventHandler(timer_Tick);
        }

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

        public void Show()
        {
            if (enabled)
            {
                timer.Enabled = true;
            }
            if (!this.showMouse)
            {
                Cursor.Show();
                showMouse = true;
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (enabled)
            {
                timer.Enabled = false;

                if (showMouse)
                {
                    Cursor.Hide();
                    showMouse = false;
                }
            }
        }
    }
}
