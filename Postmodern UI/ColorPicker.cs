using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Postmodern_UI
{
    public partial class ColorPicker : UserControl
    {

        public ColorPicker()
        {
            InitializeComponent();
            wheel.Image = new Bitmap(wheel.Image, new Size(wheel.Width, wheel.Width));
        }

        private void wheel_MouseMove(object sender, MouseEventArgs e)
        {
            this.BackColor = ((Bitmap)wheel.Image).GetPixel(e.X, e.Y);
        }

        public void setColor(Color color)
        {
            Settings.backColor = this.BackColor = color;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void wheel_MouseLeave(object sender, EventArgs e)
        {
            this.BackColor = Settings.backColor;
        }

        private void wheel_MouseClick(object sender, MouseEventArgs e)
        {
            Settings.backColor = this.BackColor;
        }
    }
}
