using System.Drawing;

namespace Dropper
{
    partial class Form1
    {
        private Color _lightBG = Color.WhiteSmoke;
        private Color _lightFG = Color.FromArgb(23, 23, 23);

        private Color _darkBG = Color.FromArgb(23, 23, 23); //#2b2b2b
        private Color _darkFG = Color.FromArgb(230, 230, 230); //#f1f1f1f

        public void SetDarkTheme(bool enable) 
        {
            if (enable)
                SetColors(_darkBG, _darkFG);
            else
                SetColors(_lightBG, _lightFG);
        }

        private void SetColors(Color bg, Color fg)
        {
            this.BackColor = bg;
            this.ForeColor = fg;

            panel1.BackColor = bg;
            CloseButton.BackColor = bg;
            CloseButton.ForeColor = fg;
            TitleBar.BackColor = bg;
            TitleLable.BackColor = bg;
            TitleLable.ForeColor = fg;

            foreach(var item in textBoxes)
            {
                item.BackColor = bg;
                item.ForeColor = fg;
            }
        }
    }
}
