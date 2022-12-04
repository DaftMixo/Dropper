using System.Drawing;
using System.Windows.Forms;
using System;
using System.Collections.Generic;

namespace Dropper
{
    public class Utils
    {
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        public static Color GetColor(List<TextBox> textBoxes, List<Button> buttons)
        {
            Color color = GetPixel(Cursor.Position);

            foreach (TextBox item in textBoxes)
            {
                item.Location = new Point(item.Location.X, item.Location.Y + 50);
            }
            foreach (Button item in buttons)
            {
                item.Location = new Point(item.Location.X, item.Location.Y + 50);
            }
            return color;
        }

        public static void CopyToClipboard(string hexColor)
        {
            Clipboard.Clear();
            Clipboard.SetText(hexColor);
        }

        private static Color GetPixel(Point position)
        {
            using (var bitmap = new Bitmap(1, 1))
            {
                using (var graphics = Graphics.FromImage(bitmap))
                {
                    graphics.CopyFromScreen(position, new Point(0, 0), new Size(1, 1));
                }
                return bitmap.GetPixel(0, 0);
            }
        }

        public static String HexConverter(System.Drawing.Color c)
        {
            return "#" + c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2");
        }

        public static String RGBConverter(System.Drawing.Color c)
        {
            return "[R=" + c.R.ToString() + ", G=" + c.G.ToString() + ", B=" + c.B.ToString() + "]";
        }
    }
}