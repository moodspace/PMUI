using System;
using System.Drawing;

namespace Postmodern_UI
{
    public static class Settings
    {
        public static int display_width;
        public static int display_height;
        public static int tile_unit_length = 56;
        public static int small_span = 8;
        public static int tile_2X_unit_length = tile_unit_length * 2 + small_span;
        public static int tile_4X_unit_length = tile_2X_unit_length * 2 + small_span;

        public static int userImg_right = 46;
        public static int userImg_top = 52;
        public static int userImg_menu_top = 109;

        public static int tile_left_screen = 120;
        public static int tile_top_screen = 172;
        public static int tile_group_margin_extra = 56;

        public enum TSize { small = 1, medium = 4, wide = 8, large = 16 };

        public static Color backColor;
        public static Color secondColor;

        public static int getTWidth(TSize size)
        {
            switch ((int)size)
            {
                case 1:
                    return 1;

                case 4:
                    return 2;

                default:
                    return 4;
            }
        }

        public static int getTHeight(TSize size)
        {
            switch ((int)size)
            {
                case 1:
                    return 1;

                case 16:
                    return 4;

                default:
                    return 2;
            }
        }

        public static TSize getTSize(String sizeInfo)
        {
            sizeInfo = sizeInfo.ToLower();
            if (sizeInfo.Contains("med"))
                return TSize.medium;
            else if (sizeInfo.Contains("big") || sizeInfo.Contains("large"))
                return TSize.large;
            else if (sizeInfo.Contains("wide") || sizeInfo.Contains("banner"))
                return TSize.wide;
            else
                return TSize.small;
        }
    }
}