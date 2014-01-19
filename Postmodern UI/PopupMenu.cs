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
    public partial class PopupMenu : UserControl
    {
        PopupMenuData data;

        public PopupMenu(PopupMenuData menuData)
        {
            InitializeComponent();
            data = menuData;
        }

        private void PopupMenu_Load(object sender, EventArgs e)
        {
            this.Height = 10 + data.Count() * 40;
            this.Width = data.Width() + 4;


            Bitmap bg = new Bitmap(this.Width, this.Height);
            Graphics g = Graphics.FromImage(bg);

            g.DrawRectangle(new Pen(Color.Black, 2F), new Rectangle(new Point(0, 0), this.Size));
            g.Save(); g.Dispose();
            this.BackgroundImage = bg;

            menuItem1.Width = data.Width();
            menuItem1.Text = "   " + data.Get(0).text;

            for (int i = 1; i < data.Count(); i++)
            {
                Label newMenuItem = new Label();
                newMenuItem.Left = 2;
                newMenuItem.Top = 5 + i * 40;
                newMenuItem.Size = menuItem1.Size;
                newMenuItem.Text = "   " + data.Get(i).text;
                newMenuItem.TextAlign = ContentAlignment.MiddleLeft;
                this.Controls.Add(newMenuItem);
            }
        }
    }
}
