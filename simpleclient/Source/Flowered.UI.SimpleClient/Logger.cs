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
    using System.Text;

    using log4net;

    public class Logger
    {
        #region Fields

        private static readonly ILog log = 
            LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #endregion Fields

        #region Methods

        /// <summary>
        ///
        /// </summary>
        public static void Exception(string methodName, Exception exception)
        {
            if (log.IsErrorEnabled)
            {
                using (log4net.ThreadContext.Stacks["NDC"].Push(methodName))
                {
                    // log.Error(string.Format("Exception during processing {0}", exception));
                    log.Error(exception);
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        public static void Info(string methodName, string message)
        {
            if (log.IsInfoEnabled)
            {
                using (log4net.ThreadContext.Stacks["NDC"].Push(methodName))
                {
                    log.Info(message);
                }
            }
        }

        #endregion Methods
    }
}