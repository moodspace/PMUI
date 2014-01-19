using System.Collections.Generic;
using System.Drawing;

namespace Postmodern_UI
{
    public class AlignManager
    {
        private Form1 display;
        private Tile[,] register;

        public AlignManager(Form1 workingForm)
        {
            register = new Tile[8, 16];
            display = workingForm;
        }

        public bool canAdd(Point point, Settings.TSize size)
        {
            //a bigger tile can't be added at certain point
            int theight = Settings.getTHeight(size);
            int twidth = Settings.getTWidth(size);

            for (int m = point.Y; m < point.Y + (int)theight; m++)
            {
                for (int n = point.X; n < point.X + (int)twidth; n++)
                {
                    if (register[m, n] != null)
                        return false;
                }
            }
            return true;
        }

        public Tile getTile(int Y, int X)
        {
            return register[Y, X];
        }

        internal void Add(Point point, Tile tile)
        {
            for (int m = point.Y; m < point.Y + Settings.getTHeight(tile.TSize); m++)
            {
                for (int n = point.X; n < point.X + Settings.getTWidth(tile.TSize); n++)
                {
                    register[m, n] = tile;
                }
            }

            fixToAnchor(point, tile); //align the tile on display device
            display.Controls.Add(tile); //show on display device
        }

        internal Point findTileLocation(Tile tile)
        {
            for (int m = 0; m < register.GetLength(0); m++)
            {
                for (int n = 0; n < register.GetLength(1); n++)
                {
                    if (register[m, n] == tile)
                        return new Point(n, m);
                }
            }

            return new Point(-1, -1);
        }

        internal Point getNext(Point current, Settings.TSize tsize)
        {
            Point nextTentative;

            if (current.Y % 2 == 1)
            {
                if (current.X % 4 == 1)
                    nextTentative = new Point(current.X + 1, current.Y - 1);
                else if (current.X % 4 == 3)
                    nextTentative = new Point(current.X - 3, current.Y + 1);
                else
                    nextTentative = new Point(current.X + 1, current.Y);
            }
            else
            {
                if (current.X % 2 == 0)
                    nextTentative = new Point(current.X + 1, current.Y);
                else
                    nextTentative = new Point(current.X - 1, current.Y + 1);
            }

            //keep finding new position if nextTentative is not eligible
            if (!isEligibleInsertion(nextTentative, tsize))
                nextTentative = getNext(nextTentative, tsize);

            return nextTentative;
        }

        internal Point getNextAvailable(Point current, Settings.TSize tsize)
        {
            Point nextAvailableTentative = getNext(current, tsize);
            if (canAdd(nextAvailableTentative, tsize))
            {
                return nextAvailableTentative;
            }
            else
            {
                return getNextAvailable(getNext(nextAvailableTentative, tsize), tsize);
            }
        }

        internal bool isEligibleInsertion(Point current, Settings.TSize tsize)
        {
            return (tsize == Settings.TSize.small
            || (tsize == Settings.TSize.medium && current.X % 2 == 0 && current.Y % 2 == 0)
            || (tsize == Settings.TSize.wide && current.X % 4 != 0 && current.Y % 2 != 0)
            || (tsize == Settings.TSize.large && current.X % 4 != 0 && current.Y % 4 != 0));
        }

        internal void Remove(Tile tile)
        {
            Point point = findTileLocation(tile);
            for (int m = point.Y; m < point.Y + Settings.getTHeight(tile.TSize); m++)
            {
                for (int n = point.X; n < point.X + Settings.getTWidth(tile.TSize); n++)
                {
                    register[m, n] = null;
                }
            }
            display.Controls.Remove(tile); //remove from display device
        }

        internal void TryAdd(Point insertionPoint, Tile newTile)
        {
            int totalTilesMove = Settings.getTWidth(newTile.TSize) * Settings.getTHeight(newTile.TSize);
            //the first point to claim must be eligible as the top left of a certain sized tile
            Point elligibleInsertion;
            if (isEligibleInsertion(insertionPoint, newTile.TSize))
                elligibleInsertion = new Point(insertionPoint.X, insertionPoint.Y);
            else
                elligibleInsertion = getNext(insertionPoint, newTile.TSize);

            List<Tile> removedTiles = new List<Tile>();
            if (!canAdd(elligibleInsertion, newTile.TSize))
                removedTiles = RemoveFrom(elligibleInsertion);

            Add(elligibleInsertion, newTile);

            AddRange(getNext(elligibleInsertion, newTile.TSize), removedTiles);
        }

        /** clear the entry in register and wipe related blocks in inUse*/
        /** align to screen position */

        internal List<Tile> getTiles(Point startPosition)
        {
            List<Tile> tiles = new List<Tile>();

            if (register[startPosition.Y, startPosition.X] != null)
                tiles.Add(register[startPosition.Y, startPosition.X]);

            Point nextPoint = getNext(startPosition, Settings.TSize.small);
            

            while (nextPoint.Y < register.GetLength(0) && nextPoint.X < register.GetLength(1))
            {
                Tile nextTile = register[nextPoint.Y, nextPoint.X];
                if (register[nextPoint.Y, nextPoint.X] != null && !tiles.Contains(nextTile))
                {
                    tiles.Add(nextTile);
                }
                nextPoint = getNext(nextPoint, Settings.TSize.small);
            }

            return tiles;
        }

        internal List<Tile> RemoveFrom(Point RemoveStart)
        {
            List<Tile> tiles = getTiles(RemoveStart);
            foreach (Tile t in tiles)
            {
                Remove(t);
            }

            return tiles;

            
        }

        internal void AddRange(Point position, List<Tile> tiles)
        {
            if (tiles.Count == 0)
                return;

            TryAdd(position, tiles[0]);
            Point lastPosition = position;
            for (int i = 1; i < tiles.Count; i++)
            {
                // re-align tiles, no need to concern the interference in calling TryAdd
                // because all tiles required moving have been unregistered, therefore 
                // no tiles hold while adding. use TryAdd because it excludes impossible positions
                TryAdd(getNext(lastPosition, tiles[i - 1].TSize), tiles[i]);
                lastPosition = findTileLocation(tiles[i]);
            }
        }

        private void fixToAnchor(Point point, Tile tile)
        {
            tile.Location = new Point(Settings.tile_left_screen +
                point.X * Settings.tile_unit_length + point.X * Settings.small_span +
                point.X / 4 * Settings.tile_group_margin_extra, Settings.tile_top_screen +
                point.Y * Settings.tile_unit_length + point.Y * Settings.small_span);
        }

        /** add a new tile and re-allocate the existing tile holding the position when needed */
    }
}