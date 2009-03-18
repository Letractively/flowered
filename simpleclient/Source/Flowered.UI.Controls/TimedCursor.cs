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

        #endregion Properties

        #region Methods

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