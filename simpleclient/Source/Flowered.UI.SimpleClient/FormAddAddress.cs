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

namespace Flowered.App.SimpleClient
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Windows.Forms;

    public partial class FormSetAddress : Form
    {
        #region Fields

        private static ErrorProvider errorProvider = new ErrorProvider();

        // see: http://geekswithblogs.net/casualjim/archive/2005/12/01/61722.aspx
        private string validationExpression =
            // @"^(?#Protocol)(?:(?:ht|f)tp(?:s?)\:\/\/|~/|/)?" +
            @"^(?#Protocol)(?:http(?:s?)\:\/\/|~/|/)?" +
            @"(?#Username:Password)(?:\w+:\w+@)?" +
            @"(?#Domains)((?:\w+)|" +
            @"(?#Subdomains)(?:(?:[-\w]+\.)+" +
            @"(?#TopLevel Domains)" +
            @"(?:com|org|net|gov|mil|biz|info|mobi|name|aero|jobs|museum|travel|[a-z]{2})))" +
            @"(?#Port)(?::[\d]{1,5})?" +
            @"(?#Directories)(?:(?:(?:/(?:[-\w~!$+|.,=]|%[a-f\d]{2})+)+|/)+|\?|#)?" +
            @"(?#Query)(?:(?:\?(?:[-\w~!$+|.,*:]|%[a-f\d{2}])+=(?:[-\w~!$+|.,*:=]|" +
            @"%[a-f\d]{2})*)(?:&(?:[-\w~!$+|.,*:]|%[a-f\d{2}])+=(?:[-\w~!$+|.,*:=]|" +
            @"%[a-f\d]{2})*)*)*" +
            @"(?#Anchor)(?:#(?:[-\w~!$+|.,*:=]|%[a-f\d]{2})*)?$";

        #endregion Fields

        #region Constructors

        public FormSetAddress()
        {
            InitializeComponent();
            InitializeValidatingHandler();
        }

        #endregion Constructors

        #region Properties

        public string Address
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

        #endregion Properties

        #region Methods

        public new void Validate()
        {
            string errorMessage = string.Empty;
            bool isValid = EvaluateIsValid();
            if (!isValid)
            {
                errorMessage = "Input is not a valid URL!";
                errorProvider.Icon = new Icon(typeof(ErrorProvider), "Error.ico");
            }
            errorProvider.SetError(tbUrl, errorMessage);
            btnSet.Enabled = isValid;
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

        private void FormSetUrl_Load(object sender, EventArgs e)
        {
            Validate();
        }

        private void InitializeValidatingHandler()
        {
            tbUrl.Validating += new CancelEventHandler(ValidatingHandler);
        }

        private void ValidatingHandler(object sender, CancelEventArgs e)
        {
            Validate();
        }

        private void tbUrl_TextChanged(object sender, EventArgs e)
        {
            Validate();
        }

        #endregion Methods
    }
}