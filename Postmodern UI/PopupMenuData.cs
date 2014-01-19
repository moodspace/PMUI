using System;
using System.Collections.Generic;

namespace Postmodern_UI
{
    public class PopupMenuData
    {
        private List<menuDataEntry> data;
        private int width;

        public PopupMenuData(int menuWidth)
        {
            data = new List<menuDataEntry>();
            width = menuWidth;
        }

        public void Add(String menuText)
        {
            data.Add(new menuDataEntry(menuText, false));
        }

        public void Remove(int index)
        {
            data.RemoveAt(index);
        }

        public int Count()
        {
            return data.Count;
        }

        public int Width()
        {
            return width;
        }

        public menuDataEntry Get(int index)
        {
            return data[index];
        }

        public struct menuDataEntry
        {
            public String text;
            public bool selected;

            public menuDataEntry(String menuText, bool isSelected)
            {
                text = menuText;
                selected = isSelected;
            }
        }
    }
}