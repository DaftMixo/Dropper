using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Dropper
{
    public partial class Form1 : Form
    {
        private List<TextBox> textBoxes = new List<TextBox>();
        private List<Button> buttons = new List<Button>();

        private bool _isDrag = false;
        private Point _startPoint= new Point(0,0);

        public bool CopyChecked => CopyCheck.Checked;

        public Form1()
        {
            InitializeComponent();
            TrayMenuInit();
            TitleInit();

            LoadSettings();

            int id = 0;
            Utils.RegisterHotKey(Handle, id, (int)KeyModifier.Alt + (int)KeyModifier.Shift, Keys.C.GetHashCode());
            id = 1;
            Utils.RegisterHotKey(Handle, id, (int)KeyModifier.Alt + (int)KeyModifier.Shift, Keys.H.GetHashCode());
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == 0x0312)
            {
                int id = m.WParam.ToInt32();

                switch(id)
                {
                    case 0:
                        GetColor();
                        break;
                    case 1:
                        VisualStateUpdate();
                        break;
                }
            }
        }

        private void GetColor()
        {
            Color color = Utils.GetColor(textBoxes, buttons);
            AddItemOnPanel(color);
            if (CopyChecked)
                Utils.CopyToClipboard(Utils.HexConverter(color));
        }

        private void VisualStateUpdate()
        {
            if (!Visible)
                Show();
            else
                Hide();
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
        #region Title bar methods
        private void CloseButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void TitleMouseMove(object sender, MouseEventArgs e)
        {
            if (_isDrag)
            {
                Point p1 = new Point(e.X, e.Y);
                Point p2 = PointToScreen(p1);
                Point p3 = new Point(p2.X - _startPoint.X,
                                     p2.Y - _startPoint.Y);
                Location = p3;
            }
        }

        private void TitleMouseUp(object sender, MouseEventArgs e)
        {
            _isDrag = false;
        }

        private void TitleMouseDown(object sender, MouseEventArgs e)
        {
            _startPoint = e.Location;
            _isDrag = true;
        }
        #endregion

        private void GetButtonColor(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            Clipboard.Clear();
            Clipboard.SetText(Utils.HexConverter(button.BackColor));
        }

        ~Form1()
        {
            Utils.UnregisterHotKey(Handle, 0);
            Utils.UnregisterHotKey(Handle, 1);
        }
    }
}
