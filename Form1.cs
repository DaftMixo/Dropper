using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Dropper
{
    public partial class Form1 : Form
    {
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        private List<TextBox> textBoxes = new List<TextBox>();
        private List<Button> buttons = new List<Button>();

        public Form1()
        {
            InitializeComponent();
            TrayMenuContext();

            LoadSettings();

            int id = 0;     // The id of the hotkey. 
            RegisterHotKey(this.Handle, id, (int)KeyModifier.Alt + (int)KeyModifier.Shift, Keys.C.GetHashCode());   // Register Alt + Shift + C as global hotkey. 
            id = 1;
            RegisterHotKey(this.Handle, id, (int)KeyModifier.Alt + (int)KeyModifier.Shift, Keys.H.GetHashCode());
        }

        ~Form1()
        {
            UnregisterHotKey(this.Handle, 0);
            UnregisterHotKey(this.Handle, 1);
        }

        protected override void WndProc(ref Message m)
        {
            FormWindowState org = this.WindowState;

            base.WndProc(ref m);

            if (m.Msg == 0x0312)
            {
                /* Note that the three lines below are not needed if you only want to register one hotkey.
                 * The below lines are useful in case you want to register multiple keys, which you can use a switch with the id as argument, or if you want to know which key/modifier was pressed for some particular reason. */

                Keys key = (Keys)(((int)m.LParam >> 16) & 0xFFFF);                  // The key of the hotkey that was pressed.
                KeyModifier modifier = (KeyModifier)((int)m.LParam & 0xFFFF);       // The modifier of the hotkey that was pressed.
                int id = m.WParam.ToInt32();                                        // The id of the hotkey that was pressed.

                // do something
                if (id == 0)
                    GetColor();
                if (id == 1)
                    UpdateState();
            }
        }

        private void UpdateState()
        {
            if (this.Visible == false)
                this.Show();
            else
                this.Hide();
        }

        private void GetColor()
        {
            Color color = GetPixel(Cursor.Position);

            string hexColor = HexConverter(color);
            //string rgbColor = RGBConverter(color);

            if(this.CopyCheck.Checked)
                CopyToClipboard(hexColor);

            foreach (TextBox item in textBoxes)
            {
                item.Location = new Point(item.Location.X, item.Location.Y + 50);
            }
            foreach (Button item in buttons)
            {
                item.Location = new Point(item.Location.X, item.Location.Y + 50);
            }

            AddItemOnPanel(color);
        }

        private void CopyToClipboard(string hexColor)
        {
            Clipboard.Clear();
            Clipboard.SetText(hexColor);
        }

        private Color GetPixel(Point position)
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

        private String HexConverter(System.Drawing.Color c)
        {
            return "#" + c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2");
        }

        private String RGBConverter(System.Drawing.Color c)
        {
            return "[R=" + c.R.ToString() + ", G=" + c.G.ToString() + ", B=" + c.B.ToString() + "]";
        }

        #region Notify menu button methods

        private void MenuShowHistory_Click(object sender, EventArgs e)
        {
            this.Show();
        }

        private void MenuExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void NotifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.Visible == false)
                this.Show();
            else
                this.Hide();
           
        }
        private void SettingsChanged(object sender, EventArgs e)
        {
            switch ((sender as ToolStripMenuItem).Name)
            {
                case "CollapseCheck":
                    Properties.Settings.Default.CollapseCheck = this.CollapseCheck.Checked;
                    Properties.Settings.Default.Save();
                    break;
                case "CopyCheck":
                    Properties.Settings.Default.CopyToClipboard = this.CopyCheck.Checked;
                    Properties.Settings.Default.Save();
                    break;
            }
        }
        private void LoadSettings()
        {
            this.CollapseCheck.Checked = Properties.Settings.Default.CollapseCheck;
            this.CopyCheck.Checked = Properties.Settings.Default.CopyToClipboard;
        }

        #endregion

        private void GetButtonColor(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            Clipboard.Clear();
            Clipboard.SetText(HexConverter(button.BackColor));
        }
    }
    enum KeyModifier
    {
        None = 0,
        Alt = 1,
        Control = 2,
        Shift = 4,
        WinKey = 8
    }
}
