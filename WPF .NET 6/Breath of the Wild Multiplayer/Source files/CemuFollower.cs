using Breath_of_the_Wild_Multiplayer.MVVM.Model;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Threading;

namespace Breath_of_the_Wild_Multiplayer.Source_files
{
    public static class CemuFollower
    {
        [DllImport("dwmapi.dll")]
        static extern int DwmGetWindowAttribute(IntPtr hwnd, DwmWindowAttribute dwAttribute, out RECT pvAttribute, int cbAttribute);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetWindowRect(IntPtr hWnd, ref RECT lpRect);

        [DllImport("user32.dll")]
        static extern bool GetClientRect(IntPtr hwnd, ref RECT lpRect);

        [DllImport("user32.dll")]
        static extern int GetDpiForWindow(IntPtr hwnd);

        [DllImport("user32.dll", SetLastError = true)]
        static extern UInt32 GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, UInt32 dwNewLong);

        [DllImport("user32.dll")]
        static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        [DllImport("user32.dll")]
        public static extern int SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UpdateWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool IsIconic(IntPtr hWnd);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const UInt32 WS_POPUP = 0x80000000;
        const UInt32 WS_CHILD = 0x40000000;
        const UInt32 WS_SYSMENU = 0x00080000;

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        [Flags]
        public enum DwmWindowAttribute : uint
        {
            DWMWA_NCRENDERING_ENABLED = 1,
            DWMWA_NCRENDERING_POLICY,
            DWMWA_TRANSITIONS_FORCEDISABLED,
            DWMWA_ALLOW_NCPAINT,
            DWMWA_CAPTION_BUTTON_BOUNDS,
            DWMWA_NONCLIENT_RTL_LAYOUT,
            DWMWA_FORCE_ICONIC_REPRESENTATION,
            DWMWA_FLIP3D_POLICY,
            DWMWA_EXTENDED_FRAME_BOUNDS,
            DWMWA_HAS_ICONIC_BITMAP,
            DWMWA_DISALLOW_PEEK,
            DWMWA_EXCLUDED_FROM_PEEK,
            DWMWA_CLOAK,
            DWMWA_CLOAKED,
            DWMWA_FREEZE_REPRESENTATION,
            DWMWA_LAST
        }

        public static IntPtr WindowHandle;
        public static IntPtr CemuWindowHandle;
        public static Process CemuProcess;
        public static RECT ClientRect;
        private static RECT WindowRect;
        private static double WindowScalingFactor;
        private static double CemuScalingFactor;
        private static bool lastState = true;
        private static bool isBorderless = false;
        private static uint cemuOriginalStyle;
        private static uint cemuBorderlessStyle;
        private static RECT lastCemuSize;
        private static RECT lastClientSize;

        private static DispatcherTimer Timer = new DispatcherTimer();
        public static EventHandler<RECT> SizeChanged;

        public static void Setup(Process cemuProcess)
        {
            WindowHandle = new WindowInteropHelper(SharedData.MainView.Window).Handle;
            CemuProcess = cemuProcess;
            CemuWindowHandle = cemuProcess.MainWindowHandle;
            lastClientSize = new RECT();

            PutWindowOnTop();

            Timer.Interval = TimeSpan.FromMilliseconds(50);
            Timer.Tick += new EventHandler(timer_tick);
            Timer.Start();
        }

        public static void SwitchTimer(bool status)
        {
            if (status)
                Timer.Start();
            else
                Timer.Stop();
        }

        private static void PutWindowOnTop()
        {
            SetParent(WindowHandle, CemuWindowHandle);
            uint style = GetWindowLong(WindowHandle, -16);
            style = (style & ~(WS_POPUP)) | WS_CHILD;
            SetWindowLong(WindowHandle, -16, style);

            cemuOriginalStyle = GetWindowLong(CemuWindowHandle, -16);
            cemuBorderlessStyle = (style & ~(WS_SYSMENU));
        }

        public static void Borderless()
        {
            uint cemuStyle = GetWindowLong(CemuWindowHandle, -16);

            if (!isBorderless)
            {
                lastCemuSize = WindowRect;
                SetWindowLong(CemuWindowHandle, -16, cemuBorderlessStyle);
                ShowWindow(CemuWindowHandle, 3);
            }
            else
            {
                SetWindowLong(CemuWindowHandle, -16, cemuOriginalStyle);
                ShowWindow(CemuWindowHandle, 1);
                MoveWindow(CemuWindowHandle, lastCemuSize.Left, lastCemuSize.Top, lastCemuSize.Right - lastCemuSize.Left, lastCemuSize.Bottom - lastCemuSize.Top, true);
            }

            isBorderless = !isBorderless;
        }

        private static void timer_tick(object sender, EventArgs e)
        {
            if(CemuProcess.HasExited)
            {
                Environment.Exit(0);
            }

            SharedData.MainView.Window.WindowState = WindowState.Maximized;

            WindowScalingFactor = GetDpiForWindow(WindowHandle) / 0.96;
            CemuScalingFactor = GetDpiForWindow(CemuWindowHandle) / 0.96;
            DwmGetWindowAttribute(CemuWindowHandle, DwmWindowAttribute.DWMWA_EXTENDED_FRAME_BOUNDS, out WindowRect, Marshal.SizeOf(typeof(RECT)));
            GetClientRect(CemuWindowHandle, ref ClientRect);

            int width = ClientRect.Right - ClientRect.Left;
            int height = ClientRect.Bottom - ClientRect.Top;

            MoveWindow(WindowHandle, 0, 0, width, height, true);

            if (width != (lastClientSize.Right - lastClientSize.Left) || height != (lastClientSize.Bottom - lastClientSize.Top))
            {
                lastClientSize = ClientRect;
                SizeChanged.Invoke(null, ClientRect);
            }
        }
    }
}
