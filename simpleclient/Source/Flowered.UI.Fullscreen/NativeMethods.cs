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