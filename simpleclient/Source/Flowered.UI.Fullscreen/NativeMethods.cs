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

namespace Flowered.UI.Fullscreen
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using System.Text;

    [StructLayoutAttribute(LayoutKind.Sequential)]
    public struct HWND__
    {
        /// int
        public int unused;
    }

    public class NativeMethods
    {
        #region Properties

        public static int ScreenX
        {
            get
            {
                return NativeMethods.GetSystemMetrics(NativeConstants.SM_CXSCREEN);
            }
        }

        public static int ScreenY
        {
            get
            {
                return NativeMethods.GetSystemMetrics(NativeConstants.SM_CYSCREEN);
            }
        }

        #endregion Properties

        #region Methods

        /// Return Type: int
        ///nIndex: int
        [DllImportAttribute("user32.dll", EntryPoint = "GetSystemMetrics")]
        public static extern int GetSystemMetrics(int nIndex);

        public static void SetWinFullScreen(IntPtr hwnd)
        {
            NativeMethods.SetWindowPos(hwnd, IntPtr.Zero, 0, 0,
                ScreenX, ScreenY, NativeConstants.SWP_SHOWWINDOW);
        }

        /// Return Type: BOOL->int
        ///hWnd: HWND->HWND__*
        ///hWndInsertAfter: HWND->HWND__*
        ///X: int
        ///Y: int
        ///cx: int
        ///cy: int
        ///uFlags: UINT->unsigned int
        [DllImportAttribute("user32.dll", EntryPoint = "SetWindowPos")]
        [return: MarshalAsAttribute(UnmanagedType.Bool)]
        public static extern bool SetWindowPos([InAttribute()] IntPtr hWnd,
            [InAttribute()] IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        #endregion Methods
    }
}