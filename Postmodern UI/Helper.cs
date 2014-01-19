using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace Postmodern_UI
{
    public class Helper
    {
        public static Bitmap getRandomUsrImg(Size size)
        {
            Bitmap pattern = new Bitmap(size.Width, size.Height);
            Graphics g = Graphics.FromImage(pattern);
            for (int i = 0; i < 10; i++)
            {
                //g.FillEllipse(new SolidBrush(getRandomColor()),
                    //new Rectangle(getRand(-size.Width, size.Width), getRand(-size.Height, size.Height), getRand(size.Width), getRand(size.Height)));
                g.FillRectangle(new SolidBrush(getRandomColor()),
                        new Rectangle(getRand(-size.Width, size.Width), 
                            getRand(-size.Height, size.Height), getRand(size.Width), 
                            getRand(size.Height)));

                g.FillPolygon(new SolidBrush(getRandomColor()), 
                    getRandomPoints(new Rectangle(new Point(0, 0), pattern.Size), 
                    getRand(5) + 1));
            }
            g.Save(); g.Dispose();

            Bitmap b02 = new Bitmap(pattern);
            b02.RotateFlip(RotateFlipType.Rotate90FlipNone);
            Bitmap b03 = new Bitmap(pattern);
            b03.RotateFlip(RotateFlipType.Rotate180FlipNone);
            Bitmap b04 = new Bitmap(pattern);
            b04.RotateFlip(RotateFlipType.Rotate270FlipNone);

            Bitmap fancyPattern = new Bitmap(size.Width * 2, size.Height * 2);
            Graphics fg = Graphics.FromImage(fancyPattern);
            fg.DrawImageUnscaled(pattern, new Point(0, 0));
            fg.DrawImageUnscaled(b02, new Point(size.Width, 0));
            fg.DrawImageUnscaled(b03, new Point(size.Width, size.Height));
            fg.DrawImageUnscaled(b04, new Point(0, size.Height));
            fg.Save(); fg.Dispose();

            return fancyPattern;
        }

        public static Color getRandomColor()
        {
            int r = getRand(256);
            int g = getRand(256);
            int b = getRand(256);
            return Color.FromArgb(r, g, b);
        }

        public static int getRand(int exclusiveMax)
        {
            return new Random(Guid.NewGuid().GetHashCode()).Next(exclusiveMax);
        }

        public static int getRand(int inclusiveMin, int exclusiveMax)
        {
            return new Random(Guid.NewGuid().GetHashCode()).Next(exclusiveMax - inclusiveMin) + inclusiveMin;
        }

        public static Point[] getRandomPoints(Rectangle range, int count)
        {
            Point[] points = new Point[count];
            for (int i = 0; i < count; i++)
            {
                points[i] = new Point(getRand(range.X, range.X + range.Width), 
                    getRand(range.Y, range.Y + range.Height));
            }

            return points;
        }

        public static List<String> albumGrabber(String dir, Size minSize)
        {
            DirectoryInfo info = new DirectoryInfo(dir);

            FileInfo[] files = info.GetFiles("*.jpg", SearchOption.AllDirectories);
            List<String> usableImgs = new List<String>();
            foreach (FileInfo file in files)
            {
                if (file.Length > 104448)
                    usableImgs.Add(file.FullName);
            }
            return usableImgs;
        }

    }
}