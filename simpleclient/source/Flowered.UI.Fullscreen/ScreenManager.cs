using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Flowered.UI.Fullscreen
{
    public class ScreenManager
    {
        private bool fullscreen = false;
        private Rectangle bounds;
        private FormWindowState formWindowState;
        private FormBorderStyle formBorderStyle;
        private bool topMost;

        private void Save(Form form)
        {
            bounds = form.Bounds;
            formWindowState = form.WindowState;
            formBorderStyle = form.FormBorderStyle;
            topMost = form.TopMost;
        }

        private void Restore(Form form)
        {
            form.Bounds = bounds;
            form.WindowState = formWindowState;
            form.FormBorderStyle = formBorderStyle;
            form.TopMost = topMost;

            fullscreen = false;
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
            }
        }

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
    }
}
