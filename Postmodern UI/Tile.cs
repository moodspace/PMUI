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

        public Settings.TSize TSize;
        public Bitmap icon;
        public String title, subtitle;
        public Color tileColor;
        public AlignManager alignmanager;

        public Tile()
        {
            InitializeComponent();
        }

        public Tile(String[] textgroup, Bitmap icon, Color tileColor, AlignManager am)
        {
            InitializeComponent();

            this.icon = icon;
            this.title = textgroup[0];
            this.subtitle = textgroup[1];
            this.tileColor = tileColor;

            //set font
            this.Font = new Font("Segoe UI Light", 9.5f);

            alignmanager = am;
            
        }

        public Tile(String[] textgroup, Settings.TSize TSize, Color tileColor, AlignManager am) :
            this(textgroup, Helper.getRandomUsrImg(new Size(128, 128)), tileColor, am) { ;}
        

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

            if (icon == null)
            {
                //fill in solid color
                g.FillRectangle(new SolidBrush(color), new Rectangle(new Point(0, 0), this.Size));
            }
            else
            {
                //fill in gradient color
                Color mainTone = colorMatcher(icon);
                LinearGradientBrush brush = new LinearGradientBrush(new Point(0, 0), new Point(this.Width, 0), mainTone, brighterColor(mainTone));
                g.FillRectangle(brush, new Rectangle(new Point(0, 0), this.Size));
            }

            g.Save(); g.Dispose();
            this.BackgroundImage = bg;
           
        }

        /** Precondition: background with either fill has been set */
        private void printText()
        {
            if (TSize == Settings.TSize.small)
                return;

            Graphics g = prepareGraphics(this.BackgroundImage);

            g.DrawString(title, this.Font, Brushes.White, new PointF(10, this.Height - 20));
            g.DrawString(subtitle, this.Font, Brushes.White, new PointF(10, this.Height - 35));

            g.Save(); g.Dispose();
        }

        public void Tile_Load(object sender, EventArgs e)
        {
            refreshTile();
        }

        internal void printIcon()
        {
            //set tile icon
            int preferredDimension = Math.Min((int)(Math.Min(this.Width, this.Height) / 1.6), icon.Width);
            if (this.icon.Width != preferredDimension || this.icon.Height != preferredDimension)
                this.icon = new Bitmap(icon, new Size(preferredDimension, preferredDimension));
            
            Graphics g = prepareGraphics(this.BackgroundImage);
            int iconTopLeft = (Math.Min(this.Width, this.Height) - preferredDimension) / 2;
            g.DrawImageUnscaled(icon, new Point(iconTopLeft, iconTopLeft));
            g.Save(); g.Dispose();
        }

        internal void resize(Settings.TSize size)
        {
            Point tentativePosition = alignmanager.findTileLocation(this); //store the current location
            alignmanager.Remove(this); //clean up in register, and GUI
            this.Size = getActualSize(size); //change of actual size
            
            TSize = size; //TSize changed, can re-register now
            alignmanager.TryAdd(tentativePosition, this);
        }

        internal static Size getActualSize(Settings.TSize size)
        {
            int width, height;
            switch (size)
            {
                case Settings.TSize.small:
                    height = width = Settings.tile_unit_length; break;
                case Settings.TSize.medium:
                    height = width = Settings.tile_2X_unit_length; break;
                case Settings.TSize.large:
                    height = width = Settings.tile_4X_unit_length; break;
                default:
                    {
                        height = Settings.tile_2X_unit_length;
                        width = Settings.tile_4X_unit_length;
                        break;
                    }
            }

            return new Size(width, height);
        }

        public Size getActualSize()
        {
            return getActualSize(this.TSize);
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

        bool leftMouseIsDown = false;
        bool mouseIsMoving = false;
        Point latestPosition = new Point(0, 0);

        private void Tile_MouseDown(object sender, MouseEventArgs e)
        {
            this.BringToFront();

            if (e.Button != System.Windows.Forms.MouseButtons.Left)
                return;

            BackgroundImage = new Bitmap(getSnapshot(), this.Width - 5, this.Height - 5);
            this.BackgroundImageLayout = ImageLayout.Center;
            hideControls();

            leftMouseIsDown = true;
            latestPosition = e.Location;

            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Tile_MouseMove);
        }

        private void hideControls()
        {
            foreach (Control c in this.Controls)
                c.Visible = false;
        }

        private void Tile_MouseUp(object sender, MouseEventArgs e)
        {
            this.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.Tile_MouseMove);

            if (e.Button != System.Windows.Forms.MouseButtons.Left)
                return;

            this.refreshTile();
            restoreControls();

            leftMouseIsDown = false;

            this.Size = getActualSize();
        }

        internal void refreshTile()
        {
            this.Size = getActualSize();
            setTileColor(tileColor);
            printIcon();
            printText();
        }

        private void restoreControls()
        {
            foreach (Control c in this.Controls)
                c.Visible = true;
        }

        private void select()
        {
            Bitmap selected = getSnapshot();
            Graphics g = Graphics.FromImage(selected);
            g.DrawRectangle(new Pen(Settings.secondColor, 5), new Rectangle(new Point(0, 0), this.Size));
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
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {

                if (selected)
                    deselect();
                else
                    select();

                selected = !selected;
            }
            else
            {
                if (!mouseIsMoving)
                    doAction();
            }
        }

        internal virtual void doAction() { }

        private void Tile_MouseMove(object sender, MouseEventArgs e)
        {

            Point approx = alignmanager.approximatePosition(this.Location, this);
            if (approx != alignmanager.findTileLocation(this))
            {
                alignmanager.Remove(this); alignmanager.TryAdd(approx, this);
            }

            Size originalSize = getActualSize();
            this.Size = new Size(originalSize.Width + (int)Settings.getTWidth(TSize) * 3, originalSize.Height + (int)Settings.getTHeight(TSize) * 3);
            this.Location = new Point(this.Left - latestPosition.X + e.X, this.Top - latestPosition.Y + e.Y);
            this.BackgroundImageLayout = ImageLayout.Stretch;

        }

        private void Tile_DoubleClick(object sender, EventArgs e)
        {
            this.resize(Settings.TSize.medium);
        }

        private void Tile_MouseHover(object sender, EventArgs e)
        {
            this.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.Tile_MouseMove);
        }

        private void Tile_MouseLeave(object sender, EventArgs e)
        {
            this.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.Tile_MouseMove);
        }
    }
}
