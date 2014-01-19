using System;
using System.Drawing;
using System.Windows.Forms;

namespace Postmodern_UI
{
    public partial class Form1 : Form
    {
        public AlignManager am;

        public Form1()
        {
            InitializeComponent();
            //display settings during startup
            Settings.display_width = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Width;
            Settings.display_height = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height;

            am = new AlignManager(this);
        }

        private void addRandTile(int x, int y, Settings.TSize TSize)
        {
            Tile newTile = new Tile(new String[] { DateTime.Now.Second.ToString(), DateTime.Now.Minute.ToString() },
                Helper.getRandomUsrImg(new Size(128, 128)), TSize, Color.Red, am);

            am.TryAdd(new Point(x, y), newTile);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //set startup color
            Settings.backColor = this.BackColor;
            Settings.secondColor = Color.Sienna;
            //set window size & location
            this.Location = new Point(0, 0);
            this.Size = new Size(Settings.display_width, Settings.display_height);
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            this.BackColor = Settings.backColor;
        }

        private void labelUsername_Click(object sender, EventArgs e)
        {
            PopupMenuData data = new PopupMenuData(274);
            data.Add("Change account picture");
            data.Add("Lock");
            data.Add("Sign out");

            PopupMenu menu = new PopupMenu(data);
            menu.Anchor = AnchorStyles.Top & AnchorStyles.Right;
            this.Controls.Add(menu);
            menu.Left = this.Width - Settings.userImg_right - menu.Width;
            menu.Top = Settings.userImg_menu_top;
        }

        private void labelUsername_MouseLeave(object sender, EventArgs e)
        {
            labelUsername.BackColor = Color.Transparent;
        }

        private void labelUsername_MouseMove(object sender, MouseEventArgs e)
        {
            labelUsername.BackColor = Settings.secondColor;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            addRandTile(0, 0, Settings.TSize.small);
            /* addRandTile(1, 0, Settings.TSize.small);
            addRandTile(0, 1, Settings.TSize.small);
            addRandTile(1, 1, Settings.TSize.small);

            addRandTile(2, 0, Settings.TSize.medium);
            addRandTile(1, 2, Settings.TSize.small);
            addRandTile(1, 1, Settings.TSize.small); */
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

        private void userInfo_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}