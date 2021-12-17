using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Dropper
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        
        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                this.notifyIcon.Visible = false;

                UnregisterHotKey(this.Handle, 0);

                components.Dispose();
            }
            base.Dispose(disposing);
        }

        
        protected override void OnClosing(CancelEventArgs e)
        {
            if(this.CollapseCheck.Checked)
                e.Cancel = true;
            this.WindowState = FormWindowState.Minimized;
        }
        

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.historyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.CollapseCheck = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // notifyIcon
            // 
            this.notifyIcon.ContextMenuStrip = this.contextMenuStrip;
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "Dropper";
            this.notifyIcon.Visible = true;
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.historyToolStripMenuItem,
            this.settingsToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(181, 92);
            // 
            // historyToolStripMenuItem
            // 
            this.historyToolStripMenuItem.Image = global::Dropper.Properties.Resources.History;
            this.historyToolStripMenuItem.Name = "historyToolStripMenuItem";
            this.historyToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.historyToolStripMenuItem.Text = "History";
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CollapseCheck});
            this.settingsToolStripMenuItem.Image = global::Dropper.Properties.Resources.Settings;
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.settingsToolStripMenuItem.Text = "Settings";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Image = global::Dropper.Properties.Resources.dismiss;
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(284, 411);
            this.panel1.TabIndex = 1;
            // 
            // CollapseCheck
            // 
            this.CollapseCheck.Checked = true;
            this.CollapseCheck.CheckOnClick = true;
            this.CollapseCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CollapseCheck.Name = "CollapseCheck";
            this.CollapseCheck.Size = new System.Drawing.Size(180, 22);
            this.CollapseCheck.Text = "Collapse on close";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 411);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(300, 450);
            this.MinimumSize = new System.Drawing.Size(300, 450);
            this.Name = "Form1";
            this.ShowInTaskbar = false;
            this.Text = "Dropper history";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.contextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private void TrayMenuContext()
        {
            this.historyToolStripMenuItem.Click += MenuShowHistory_Click;
            this.exitToolStripMenuItem.Click += MenuExit_Click;
            this.CollapseCheck.CheckedChanged += SettingsChanged;
            this.notifyIcon.MouseDoubleClick += NotifyIcon_MouseDoubleClick;
        }


        private void AddItemOnPanel(Color color)
        {
            string hexColor = HexConverter(color);
            string rgbColor = RGBConverter(color);

            TextBox textBox = new TextBox();
            textBox.Location = new System.Drawing.Point(50, 10);
            textBox.Width = 220;
            textBox.Height = 50;
            textBox.Font = new Font(textBox.Font.FontFamily, 12);
            textBox.ReadOnly = true;
            textBox.BorderStyle = BorderStyle.None;
            textBox.TabStop = false;
            textBox.BackColor = this.BackColor;
            textBox.Multiline = true;
            textBox.AcceptsReturn = true;
            textBox.AppendText($"Color: {rgbColor}\r\nHEX: {hexColor}");
            textBox.Parent = this.panel1;

            Button button = new Button();
            button.Location = new System.Drawing.Point(10, 15);
            button.Size = new System.Drawing.Size(30, 30);
            button.Text = string.Empty;
            button.BackColor = color;
            button.Parent = this.panel1;
            button.Click += GetButtonColor;

            if (textBoxes.Count > 10)
            {
                buttons[0].Dispose();
                buttons.RemoveAt(0);
                textBoxes[0].Dispose();
                textBoxes.RemoveAt(0);
            }

            buttons.Add(button);
            textBoxes.Add(textBox);
        }

        private NotifyIcon notifyIcon;
        private ContextMenuStrip contextMenuStrip;
        private ToolStripMenuItem historyToolStripMenuItem;
        private ToolStripMenuItem settingsToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
        private Panel panel1;
        private ToolStripMenuItem CollapseCheck;
    }
}

