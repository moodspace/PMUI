using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace Postmodern_UI
{
    public partial class Tile : UserControl
    {
        public bool selected = false;

        public Settings.TWidth TWidth;
        public Settings.THeight THeight;
        public Bitmap icon;
        public String title, subtitle;
        public Color tileColor;

        public Tile(String title, String subtitle, String status, Bitmap icon, Settings.TWidth TWidth, Settings.THeight THeight, Color tileColor)
        {
            InitializeComponent();

            this.icon = icon;

            this.TWidth = TWidth;
            this.THeight = THeight;

            this.title = title;
            this.subtitle = subtitle;
            this.tileColor = tileColor;

            //set font
            this.Font = new Font("Segoe UI Light", 9.5f);

            
            
        }

        private Graphics prepareGraphics(Image image)
        {
            Graphics g = Graphics.FromImage(image);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            return g;
        }

        private void setTileColor(Color color)
        {
            //set tile color
            Bitmap bg = new Bitmap(this.Width, this.Height);

            Graphics g = prepareGraphics(bg);

            if (iconBox.Image == null)
            {
                //fill in solid color
                g.FillRectangle(new SolidBrush(color), new Rectangle(new Point(0, 0), this.Size));
            }
            else
            {
                //fill in gradient color
                Color mainTone = colorMatcher((Bitmap)iconBox.Image);
                LinearGradientBrush brush = new LinearGradientBrush(new Point(0, 0), new Point(this.Width, 0), mainTone, brighterColor(mainTone));
                g.FillRectangle(brush, new Rectangle(new Point(0, 0), this.Size));
            }

            g.Save(); g.Dispose();
            this.BackgroundImage = bg;
           
        }

        /** Precondition: background with either fill has been set */
        private void printText()
        {
            Graphics g = prepareGraphics(this.BackgroundImage);

            g.DrawString(title, this.Font, Brushes.White, new PointF(10, this.Height - 20));
            g.DrawString(subtitle, this.Font, Brushes.White, new PointF(10, this.Height - 35));

            g.Save(); g.Dispose();
        }

        private void Tile_Load(object sender, EventArgs e)
        {
            refreshTile();
        }

        public void loadIcon(Bitmap icon)
        {
            //set tile icon
            int preferredDimension = Math.Min((int)(Math.Min(this.Width, this.Height) / 1.6), icon.Width);
            this.iconBox.Image = new Bitmap(icon, new Size(preferredDimension, preferredDimension));
        }

        public void resize(Settings.TWidth width, Settings.THeight height)
        {
            //set tile size
            this.Size = new Size(Settings.tile_unit_length * (int)width, Settings.tile_unit_length * (int)height);
        }

        private Color colorMatcher(Bitmap iconBitmap)
        {
            Bitmap scaledBitmap = new Bitmap(iconBitmap, new Size(3, 3));
            Color c11 = scaledBitmap.GetPixel(1, 1);
            Color c0022 = medianColor(scaledBitmap.GetPixel(0, 0), scaledBitmap.GetPixel(2, 2));
            Color c0220 = medianColor(scaledBitmap.GetPixel(0, 2), scaledBitmap.GetPixel(2, 0));

            Color c0121 = medianColor(scaledBitmap.GetPixel(0, 1), scaledBitmap.GetPixel(2, 1));
            Color c1012 = medianColor(scaledBitmap.GetPixel(1, 0), scaledBitmap.GetPixel(1, 2));

            Color c0022c0220 = medianColor(c0022, c0220);
            Color c0121c1012 = medianColor(c0121, c1012);

            Color c0022c0220c0121c1012 = medianColor(c0022c0220, c0121c1012);
            return medianColor(c11, c0022c0220c0121c1012);
            
        }

        private Color medianColor(Color c1, Color c2)
        {
            return Color.FromArgb((c1.R + c2.R) / 2, (c1.G + c2.G) / 2, (c1.B + c2.B) / 2);
        }

        private Color brighterColor(Color mainTone)
        {
            return Color.FromArgb(Math.Min(mainTone.R + 25, 255), Math.Min(mainTone.G + 25, 255), Math.Min(mainTone.B + 25, 255));
        }

        private Bitmap getSnapshot()
        {
            Bitmap snapshot = new Bitmap(this.Width, this.Height);
            this.DrawToBitmap(snapshot, new Rectangle(new Point(0, 0), this.Size));
            return snapshot;
        }

        bool readyMove = false;
        Point latestPosition = new Point(0, 0);

        private void iconBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != System.Windows.Forms.MouseButtons.Left)
                return;

            BackgroundImage = new Bitmap(getSnapshot(), this.Width - 5, this.Height - 5);
            this.BackgroundImageLayout = ImageLayout.Center;
            hideControls();

            readyMove = true;
            latestPosition = e.Location;
        }

        private void hideControls()
        {
            foreach (Control c in this.Controls)
                c.Visible = false;
        }

        private void iconBox_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != System.Windows.Forms.MouseButtons.Left)
                return;

            this.refreshTile();
            restoreControls();

            readyMove = false;
        }

        private void refreshTile()
        {
            resize(TWidth, THeight);
            loadIcon(icon);
            setTileColor(tileColor);
            printText();
        }

        private void restoreControls()
        {
            foreach (Control c in this.Controls)
                c.Visible = true;
        }

        private void iconBox_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != System.Windows.Forms.MouseButtons.Right)
                return;

            if (!selected)
                select();

            selected = !selected;
        }

        private void select()
        {
            Bitmap selected = getSnapshot();
            Graphics g = Graphics.FromImage(selected);
            g.DrawRectangle(new Pen(Color.Gray, 7), new Rectangle(new Point(0, 0), this.Size));
            g.Save();
            BackgroundImage = selected;
            hideControls();
        }

        private void deselect()
        {
            refreshTile();
            restoreControls();
        }

        private void Tile_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != System.Windows.Forms.MouseButtons.Right)
                return;

            if (selected)
                deselect();

            selected = !selected;
        }

        private void iconBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (readyMove)
                this.Location = new Point(this.Left - latestPosition.X + e.X, this.Top - latestPosition.Y + e.Y);

        }
    }
}
