using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Postmodern_UI
{
    class AlignManager
    {
        bool[,] inUse;
        Tile[,] register;
        public AlignManager()
        {
            inUse = new bool[16, 16];
        }

        public void Add(Point point, Tile tile) {
            register[point.Y, point.X] = tile;
            for (int m = point.Y; m < point.Y + (int)tile.THeight; m++)
            {
                for (int n = point.X; n < point.X + (int)tile.TWidth; n++)
                {
                    inUse[m, n] = true;
                }
            }
        }

        public bool canAdd(Point point, Settings.THeight height, Settings.TWidth width)
        {
            for (int m = point.Y; m < point.Y + (int)height; m++)
            {
                for (int n = point.X; n < point.X + (int)width; n++)
                {
                    if (inUse[m, n])
                        return false;
                }
            }
            return true;
        }
    }
}
