using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Postmodern_UI
{
    public static class Settings
    {
        public static int display_width;
        public static int display_height;
        public static int tile_unit_length;
        public enum TWidth { small = 1, medium = 2, large = 4 };
        public enum THeight { small = 1, medium = 2, large = 4 };

        public static Color backColor;

    }
}
