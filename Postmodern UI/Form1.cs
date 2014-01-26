using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Postmodern_UI
{
    public partial class Form1 : Form
    {
        public AlignManager am;
        List<String> foundImgs;

        public Form1()
        {
            InitializeComponent();
            //display settings during startup
            Settings.display_width = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
            Settings.display_height = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;

            am = new AlignManager(this);

            foundImgs = Helper.albumGrabber(
                Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), new Size(1360, 760));
            imgEnum = foundImgs.GetEnumerator();
        }

        private Tile addRandTile(int x, int y, Settings.TSize TSize)
        {
            String[] texts = new String[] { DateTime.Now.Second.ToString(), DateTime.Now.Minute.ToString() };
            Bitmap icon = Helper.getRandomUsrImg(new Size(128, 128));

            Tile newTile = new Tile(new Object[] { texts, icon, Settings.TSize.small, null }, am);
            am.TryAdd(new Point(x, y), newTile, true);
            return newTile;
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
            this.SuspendLayout();

            addRandTile(0, 0, Settings.TSize.small);


            this.ResumeLayout();
            //addRandTile(0, 0, Settings.TSize.wide);
            //addRandTile(0, 0, Settings.TSize.wide);

            /* addRandTile(1, 0, Settings.TSize.small);
            addRandTile(0, 1, Settings.TSize.small);
            addRandTile(1, 1, Settings.TSize.small);

            addRandTile(2, 0, Settings.TSize.medium);
            addRandTile(1, 2, Settings.TSize.small);
            addRandTile(1, 1, Settings.TSize.small); */
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            /*ColorPicker picker = new ColorPicker();
            picker.Left = this.Width - 25 - picker.Width;
            picker.Top = this.Height - 25 - picker.Height;
            picker.Anchor = AnchorStyles.Right & AnchorStyles.Bottom;
            this.Controls.Add(picker);
            picker.BringToFront();*/

            timerBackground.Enabled = !timerBackground.Enabled;
        }

        private void userInfo_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        List<String>.Enumerator imgEnum;

        private void timerBackground_Tick(object sender, EventArgs e)
        {
            imgEnum.MoveNext();
            Bitmap background;

            using (Image img = Image.FromFile(imgEnum.Current))
            {
                background = new Bitmap(img);
            }
            this.BackgroundImage = background;
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            this.SuspendLayout();

            String[] texts = new String[] { DateTime.Now.Second.ToString(), DateTime.Now.Minute.ToString() };
            TileWebsite newTile = new TileWebsite(new Object[] { texts, null, Settings.TSize.medium, null }, am);
            InputBox webInput = new InputBox(new String[] { "Caption", "Icon URL", "Website URL" }, new String[] { "", "http://", "http://www.g.cn" }, newTile, 1);
            webInput.Location = new Point(40, 40);
            Controls.Add(webInput);
            webInput.BringToFront();

            this.ResumeLayout();
        }

    }
}