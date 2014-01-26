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

        private Object[] data;
        //0 Text; 1 Style; 2 Size; 3 Action; 4 Extensible
        public AlignManager alignmanager;

        public Tile()
        {
            InitializeComponent();
        }

        public Tile(Object[] data, AlignManager alignManager)
        {
            InitializeComponent();

            //set data
            this.data = data;

            //set font
            this.Font = new Font("Segoe UI Light", 9.5f);

            alignmanager = alignManager;
            
        }

        private Graphics prepareGraphics(Image image)
        {
            Graphics g = Graphics.FromImage(image);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            return g;
        }

        private void setTileBack()
        {
            //set tile color
            Bitmap bg = new Bitmap(this.Width, this.Height);

            Graphics g = prepareGraphics(bg);

            if (data[1] is Bitmap)
            {
                //data[1] is Bitmap, containing icon but no background image
                //use gradient color background
                Color mainTone = colorMatcher((Bitmap)data[1]);
                LinearGradientBrush brush = new LinearGradientBrush(new Point(0, 0),
                    new Point(this.Width, 0), mainTone, brighterColor(mainTone));
                g.FillRectangle(brush, new Rectangle(new Point(0, 0), this.Size));
            }
            else if (data[1] is Bitmap[])
            {
                //data[1] is Bitmap[], containing icon and background image
                g.DrawImageUnscaled(((Bitmap[])data[1])[1], new Point(0, 0));
            }
            else if (data[1] is Object[])
            {
                //data is Object[], containing icon and backcolor
                g.FillRectangle(new SolidBrush((Color)((Object[])data[1])[1]), new Rectangle(new Point(0, 0), this.Size));
            }

            g.Save(); g.Dispose();
            this.BackgroundImage = bg;
        }

        internal void setText(String[] texts)
        {
            data[0] = texts;
        }

        /** Precondition: background with either fill has been set */
        private void printText()
        {
            if ((Settings.TSize)data[2] == Settings.TSize.small)
                return;

            Graphics g = prepareGraphics(this.BackgroundImage);

            if (data[0] is String)
            {
                g.DrawString((String)data[0], this.Font, Brushes.White, new PointF(10, this.Height - 20));
            }
            else
            {
                String[] texts = (String[])data[0];
                for (int i = 0; i < texts.Length; i++)
                    g.DrawString(texts[i], this.Font, Brushes.White, new PointF(10, this.Height - 20 - 15 * i));
            }

            g.Save(); g.Dispose();
        }

        public void Tile_Load(object sender, EventArgs e)
        {
            refreshTile();
        }

        internal void printIcon()
        {
            //set tile icon
            Bitmap icon;
            if (data[1] is Bitmap)
                icon = (Bitmap)data[1];
            else if (data[1] is Bitmap[])
            {
                icon = ((Bitmap[])data[1])[0];
            }
            else if (data[1] is Object[])
            {
                icon = (Bitmap)((Object[])data[1])[0];
            }
            else
            { return; }

            int preferredDimension = Math.Min((int)(Math.Min(this.Width, this.Height) / 1.6), icon.Width);
            if (icon.Width != preferredDimension || icon.Height != preferredDimension)
                icon = new Bitmap(icon, new Size(preferredDimension, preferredDimension));
            
            Graphics g = prepareGraphics(this.BackgroundImage);
            int iconTopLeft = (Math.Min(this.Width, this.Height) - preferredDimension) / 2;
            g.DrawImageUnscaled(icon, new Point(iconTopLeft, iconTopLeft));
            g.Save(); g.Dispose();
        }

        internal void resize(Settings.TSize size)
        {
            Point tentativePosition = alignmanager.getRegisterLocation(this); //store the current location
            alignmanager.Remove(this, false); //clean up in register, and GUI
            this.Size = getActualSize(size); //change of actual size

            setTSize(size); //TSize changed, can re-register now
            alignmanager.TryAdd(tentativePosition, this, false);
        }

        internal void setTSize(Settings.TSize size)
        {
            data[2] = size;
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
            return getActualSize((Settings.TSize)data[2]);
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

        private void hideControls()
        {
            foreach (Control c in this.Controls)
                c.Visible = false;
        }

        internal void refreshTile()
        {
            this.Size = getActualSize();
            setTileBack();
            printIcon();
            printText();
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
                if (validMove)
                {
                    //validMove = false;
                }
                else
                {
                    doAction();
                }
            }
        }

        internal virtual void doAction() { }

        private void Tile_DoubleClick(object sender, EventArgs e)
        {
            this.resize(Settings.TSize.medium);
        }

        private void Tile_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != System.Windows.Forms.MouseButtons.Left)
                return;

            this.BringToFront();

            lastPhysicalPosition = this.Location;

            BackgroundImage = new Bitmap(getSnapshot(), this.Width - 4, this.Height - 4);
            this.Width -= 4; this.Left += 2; this.Top += 2; this.Height -= 4;
            this.BackgroundImageLayout = ImageLayout.Center;

            lastMouseDown = new Point(e.X - 2, e.Y - 2);
            leftMouseDown = true;
        }

        private void Tile_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != System.Windows.Forms.MouseButtons.Left)
                return;


            this.Width += 4; this.Left -= 2; this.Top -= 2; this.Height += 4;

            this.refreshTile();
            this.Size = getActualSize();

            
            leftMouseDown = false;
            lastMouseDown = new Point(-1, -1);

            alignmanager.move(alignmanager.getRegisterPosition(lastPhysicalPosition), 
                alignmanager.getRegisterPosition(new Point(Left + Width / 2, Top + Height / 2)));

            alignmanager.clearProjectedFrame();
        }

        bool leftMouseDown;
        bool validMove = false; //triggered after dragged to a certain distance
        Point lastMouseDown = new Point(-1, -1);
        Point lastPhysicalPosition = new Point(-1, -1);

        private void Tile_MouseMove(object sender, MouseEventArgs e)
        {
            if (leftMouseDown && (e.Location != lastMouseDown))
            {
                this.BringToFront();
                this.Left += (e.X - lastMouseDown.X);
                this.Top += (e.Y - lastMouseDown.Y); 
                
                if (((this.Left - lastPhysicalPosition.X) ^ 2 + (this.Top - lastPhysicalPosition.Y) ^ 2) > 4)
                    validMove = true;

                Point projectedPosition = 
                    alignmanager.getRegisterPosition(new Point(Left + Width / 2, Top + Height / 2));

                alignmanager.paintProjectedFrame(projectedPosition, (Settings.TSize)data[2]);
            }
        }

        internal Settings.TSize getTSize()
        {
            return (Settings.TSize)data[2];
        }
    }
}
