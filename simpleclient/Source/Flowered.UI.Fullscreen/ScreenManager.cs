namespace Flowered.UI.Fullscreen
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Text;
    using System.Windows.Forms;

    public class ScreenManager
    {
        #region Fields

        private Rectangle bounds;
        private FormBorderStyle formBorderStyle;
        private FormWindowState formWindowState;
        private bool fullscreen = false;
        private bool topMost;

        #endregion Fields

        #region Properties

        public bool Fullscreen
        {
            set
            {
                fullscreen = value;
            }
            get
            {
                return fullscreen;
            }
        }

        #endregion Properties

        #region Methods

        public void ToogleFullScreenMode(Form form)
        {
            if (!fullscreen)
            {
                Maximize(form);
            }
            else
            {
                Restore(form);
            }
        }

        private void Maximize(Form form)
        {
            if (!fullscreen)
            {
                fullscreen = true;
                Save(form);

                form.WindowState = FormWindowState.Maximized;
                form.FormBorderStyle = FormBorderStyle.None;
                form.TopMost = true;

                NativeMethods.SetWinFullScreen(form.Handle);
            }
        }

        private void Restore(Form form)
        {
            form.Bounds = bounds;
            form.WindowState = formWindowState;
            form.FormBorderStyle = formBorderStyle;
            form.TopMost = topMost;

            fullscreen = false;
        }

        private void Save(Form form)
        {
            bounds = form.Bounds;
            formWindowState = form.WindowState;
            formBorderStyle = form.FormBorderStyle;
            topMost = form.TopMost;
        }

        #endregion Methods
    }
}