using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Flowered.App.Standalone
{
    public partial class FormSetUrl : Form
    {
        private static ErrorProvider errorProvider = new ErrorProvider();
        private string validationExpression = @"(?<protocol>http(s)?|ftp)://(?<server>([A-Za-z0-9-]+\.)*(?<basedomain>[A-Za-z0-9-]+\.[A-Za-z0-9]+))+((/?)(?<path>(?<dir>[A-Za-z0-9\._\-]+)(/){0,1}[A-Za-z0-9.-/]*)){0,1}";

        public FormSetUrl()
        {
            InitializeComponent();
            InitializeValidatingHandler();
        }

        private void InitializeValidatingHandler()
        {
            tbUrl.Validating += new CancelEventHandler(ValidatingHandler);
        }

        private void ValidatingHandler(object sender, CancelEventArgs e)
        {
            Validate();
        }

        protected bool EvaluateIsValid()
        {
            if (tbUrl.Text.Trim() == string.Empty)
            {
                return true;
            }
            string field = tbUrl.Text.Trim();
            bool isMatch = Regex.IsMatch(field, validationExpression.Trim());

            return isMatch;
        }

        public new void Validate()
        {
            bool isValid = EvaluateIsValid();

            string errorMessage = string.Empty;
            if (!isValid)
            {
                errorMessage = "Input is not a valid URL!";
                errorProvider.Icon = new Icon(typeof(ErrorProvider), "Error.ico");
            }
            errorProvider.SetError(tbUrl, errorMessage);
            btnSet.Enabled = isValid;
        }

        public string Url
        {
            get
            {
                return tbUrl.Text;
            }
            set
            {
                tbUrl.Text = value;
            }
        }

        private void FormSetUrl_Load(object sender, EventArgs e)
        {
            Validate();
        }

        private void tbUrl_TextChanged(object sender, EventArgs e)
        {
            Validate();
        }
    }
}
