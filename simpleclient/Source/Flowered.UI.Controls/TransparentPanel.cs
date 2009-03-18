namespace Flowered.UI.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Windows.Forms;

    public class TransparentPanel : Panel
    {
        #region Constructors

        public TransparentPanel()
        {
        }

        #endregion Constructors

        #region Properties

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams createParams = base.CreateParams;
                createParams.ExStyle |= 0x00000020; // WS_EX_TRANSPARENT
                return createParams;
            }
        }

        #endregion Properties

        #region Methods

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            // Do not paint background.
        }

        #endregion Methods
    }
}