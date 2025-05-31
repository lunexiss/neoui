using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Drawing;

namespace GuiBackend
{
    public static class WindowEffect
    {
        private enum DwmWindowAttribute
        {
            DWMWA_SYSTEMBACKDROP_TYPE = 38,
            DWMWA_USE_IMMERSIVE_DARK_MODE = 20,
        }

        private enum DwmBackdropType
        {
            Auto = 0,
            None = 1,
            Mica = 2,
            Acrylic = 3,
            Tabbed = 4
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct MARGINS
        {
            public int cxLeftWidth;
            public int cxRightWidth;
            public int cyTopHeight;
            public int cyBottomHeight;
        }

        [DllImport("dwmapi.dll", CharSet = CharSet.Unicode, PreserveSig = true)]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

        [DllImport("dwmapi.dll", PreserveSig = true)]
        private static extern int DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS pMarInset);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);


        private const int GWL_EXSTYLE = -20;
        private const int WS_EX_LAYERED = 0x80000;
        private const int WS_EX_TRANSPARENT = 0x20;

        private const int WM_MOUSEACTIVATE = 0x0021;

        public static void ApplyEffect(IntPtr hwnd, string effectType, Form form)
        {
            if (!IsWindows11OrGreater())
            {
                MessageBox.Show("Effects only supported on Windows 11 or later.");
                return;
            }

            int backdrop = (int)DwmBackdropType.None;
            string effect = effectType.ToLower();

            if (effect == "mica")
                backdrop = (int)DwmBackdropType.Mica;
            else if (effect == "acrylic")
                backdrop = (int)DwmBackdropType.Acrylic;

            DwmSetWindowAttribute(hwnd, (int)DwmWindowAttribute.DWMWA_SYSTEMBACKDROP_TYPE, ref backdrop, sizeof(int));

            int dark = 1;
            DwmSetWindowAttribute(hwnd, (int)DwmWindowAttribute.DWMWA_USE_IMMERSIVE_DARK_MODE, ref dark, sizeof(int));

            // extend the frame into client area (full window glass)
            MARGINS margins = new MARGINS()
            {
                cxLeftWidth = -1,
                cxRightWidth = -1,
                cyTopHeight = -1,
                cyBottomHeight = -1
            };
            DwmExtendFrameIntoClientArea(hwnd, ref margins);

            // make form background transparent
            form.BackColor = Color.Black;
            form.TransparencyKey = Color.Black;

            int style = GetWindowLong(hwnd, GWL_EXSTYLE);
            style |= WS_EX_LAYERED;
            SetWindowLong(hwnd, GWL_EXSTYLE, style);
        }

        private static bool IsWindows11OrGreater()
        {
            return Environment.OSVersion.Version.Build >= 22000;
        }
    }
}
