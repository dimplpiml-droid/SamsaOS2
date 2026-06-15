using Cosmos.Core;
using Cosmos.Core.Memory;
using Cosmos.System;
using Cosmos.System.Graphics;
using SamsaOS.Commands;
using System.Drawing;

namespace SamsaOS.GUI
{
    internal class Settings : Window
    {
        static readonly Bitmap cursor1 = new(Resources.Cursor);
        static readonly Bitmap cursor1L = new(Resources.CursorLoad);
        public static readonly Bitmap cursor2 = new(Resources.CursorWhite);
        public static readonly Bitmap cursor2L = new(Resources.CursorWhiteLoad);
        private static int _wallpapernum = 0;
        public static int wallpapernum
        {
            get => _wallpapernum;
            set
            {
                if (value < 0) _wallpapernum = 0;
                else _wallpapernum = value % 6;
            }
        }
        public static bool cursorWhite = false;

        public Settings() : base("Settings", 300, 300, new Bitmap(Resources.Settings), ProcessManager.Priority.None) { OnMoved = Moved; OnStartMoving = StartMoving; OnClicked = Clicked; }
        private void Clicked(int x2, int y2)
        {
            if (x2 > 5 && x2 < 140 && y2 > 70 && y2 < 85)
            {
                if (GUI.ScreenX * GUI.ScreenY > Sysinfo.AvailableRAM * 28000) { Notify("Not enough system memory to complete operation!"); return; }
                double beforebefore = Sysinfo.UsedRAM;
                GUI.Loading = true;
                Toast.Display("Please wait...");
                GUI.canvas.Display();
                wallpapernum++;
                LoadWallpaper();
                double before = Sysinfo.UsedRAM;
                GUI.ApplyRes();
                double almost = Sysinfo.UsedRAM;
                AlphaBackground();
                UI();
                GUI.Loading = false;
                double end = Sysinfo.UsedRAM;
                if (Kernel.Debug) { Notify("Before operation: " + (int)beforebefore + "  Before ApplyRes: " + (int)before + "  After ApplyRes: " + (int)almost + "  After operation: " + (int)end); }
            }
            if (y2 > 25 && y2 < 45)
            {
                if (x2 > 15 && x2 < 35)
                {
                    Icons.cursor = cursor1;
                    Icons.cursorload = cursor1L;
                    cursorWhite = false;
                }
                else if (x2 > 50 && x2 < 70)
                {
                    Icons.cursor = cursor2;
                    Icons.cursorload = cursor2L;
                    cursorWhite = true;
                }
            }
        }
        internal override int Start() { AlphaBackground(); UI(); return 0; }
        private void Moved() { AlphaBackground(); UI(); }
        private void StartMoving() { Background(0); UI(); }
        private void UI()
        {
            DrawStringAlpha("Cursor", Color.White.ToArgb(), 5, 5);
            DrawHorizontalLine(Color.White.ToArgb(), 60, 11, x);
            DrawStringAlpha("Wallpaper", Color.White.ToArgb(), 5, 50);
            DrawHorizontalLine(Color.White.ToArgb(), 85, 56, x);
            DrawStringAlpha("Change background", Color.White.ToArgb(), 5, 70);
            DrawImageAlpha(cursor1, 15, 25);
            DrawImageAlpha(cursor2, 50, 25);
        }
        public static void LoadWallpaper()
        {
            Images.systemWallpaper = null;
            Heap.Collect();
            Images.systemWallpaper = wallpapernum switch
            {
                1 => new Bitmap(Resources.WallpaperOld),
                2 => new Bitmap(Resources.WallpaperLock),
                3 => new Bitmap(Resources.WallpaperOrigami),
                4 => new Bitmap(Resources.Wallpaper2005s),
                5 => new Bitmap(Resources.WallpaperCosmos),
                _ => new Bitmap(Resources.Wallpaper),
            };
        }
    }
}