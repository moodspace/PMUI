using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Postmodern_UI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            //display settings during startup
            Settings.display_width = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Width;
            Settings.display_height = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height;
            Settings.tile_unit_length = Settings.display_height / 12;

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //set startup color
            Settings.backColor = this.BackColor;

            //set window size & location
            this.Location = new Point(0, 0);
            this.Size = new Size(Settings.display_width, Settings.display_height);
            //this.tableLayoutPanel1.Height = 8 * Settings.tile_unit_length + 10;
            //this.tableLayoutPanel1.Width = 16 * Settings.tile_unit_length + 30;
            Tile newTile = new Tile("Untitled", "Sub", "", new Bitmap(@"D:\C_STORE\SkyDrive\design\PMUI\testIcon.png"), Settings.TWidth.medium, Settings.THeight.medium, Color.Red);
            this.Controls.Add(newTile);
        }

        private void userInfo_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Tile newTile = new Tile("Eclipse", "  java tools", "", new Bitmap(@"D:\C_STORE\SkyDrive\design\PMUI\eclipse.png"), Settings.TWidth.medium, Settings.THeight.medium, Color.Red);
            this.Controls.Add(newTile);
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            ColorPicker picker = new ColorPicker();
            picker.Left = this.Width - 25 - picker.Width;
            picker.Top = this.Height - 25 - picker.Height;
            picker.Anchor = AnchorStyles.Right & AnchorStyles.Bottom;
            this.Controls.Add(picker);
            picker.BringToFront();
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            this.BackColor = Settings.backColor;
        }

    }
}
