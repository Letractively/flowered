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

namespace Flowered.UI.SimpleClient
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Windows.Forms;

    using log4net;

    internal static class Program
    {
        #region Fields

        private static readonly ILog log = 
            LogManager.GetLogger(typeof(Program));

        #endregion Fields

        #region Methods

        static FormMain CreateMainForm()
        {
            FormMain formMain = new FormMain();
            Application.ApplicationExit += new EventHandler(formMain.OnApplicationExit);

            return formMain;
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                Logger.Info(methodName, "Application start [" + Application.ProductVersion + "]");

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(Program.CreateMainForm());
            }
            catch (Exception exception)
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                Logger.Exception(methodName, exception);
            }
        }

        #endregion Methods
    }
}