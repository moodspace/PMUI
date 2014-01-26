using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Postmodern_UI
{
    public class AlignManager
    {
        private Form display;
        private Tile[,] register;

        public AlignManager(Form display)
        {
            register = new Tile[42, 4];
            this.display = display;
        }

        internal bool canAdd(Point point, Settings.TSize size)
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

        internal void Add(Point point, Tile tile, bool isNewTile)
        {
            for (int m = point.Y; m < point.Y + Settings.getTHeight(tile.getTSize()); m++)
            {
                for (int n = point.X; n < point.X + Settings.getTWidth(tile.getTSize()); n++)
                {
                    register[m, n] = tile;
                }
            }

            foreach (Tile t in getTiles(new Point(0, 0))) {
                setPhysicalPosition(getRegisterLocation(t), t); //align the tile on display device
            }

            if (isNewTile)
                display.Controls.Add(tile); //add to display device
            else
                tile.Show(); //recover display
        }

        internal Point getRegisterLocation(Tile tile)
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
            || (tsize == Settings.TSize.wide && current.X % 4 == 0 && current.Y % 2 == 0)
            || (tsize == Settings.TSize.large && current.X % 4 == 0 && current.Y % 4 == 0));
        }

        internal void Remove(Tile tile, bool dispose)
        {
            Point point = getRegisterLocation(tile);

            if (point.X < 0 || point.X >= register.GetLength(1) || point.Y < 0 || point.Y >= register.GetLength(0))
                return;

            for (int m = point.Y; m < point.Y + Settings.getTHeight(tile.getTSize()); m++)
                for (int n = point.X; n < point.X + Settings.getTWidth(tile.getTSize()); n++)
                    register[m, n] = null;

            tile.Hide();

            if (dispose)
            {
                display.Controls.Remove(tile); //remove from display device
                tile.Dispose();
            }
        }

        internal Point findElligibleInsertion(Point originalPoint, Settings.TSize tileSize)
        {
            if (isEligibleInsertion(originalPoint, tileSize))
                return new Point(originalPoint.X, originalPoint.Y);
            else
                return getNext(originalPoint, tileSize);
        }

        internal Point TryAdd(Point tryAddPoint, Tile tile, bool isNewTile)
        {
            int totalTilesMove = Settings.getTWidth(tile.getTSize()) * Settings.getTHeight(tile.getTSize());
            //the first point to claim must be eligible as the top left of a certain sized tile

            Point addPoint = findElligibleInsertion(tryAddPoint, tile.getTSize());
            List<Tile> removedTiles = new List<Tile>();
            if (!canAdd(addPoint, tile.getTSize()))
                removedTiles = RemoveFrom(addPoint, false);

            Add(addPoint, tile, isNewTile);

            AddRange(getNext(addPoint, tile.getTSize()), removedTiles);

            return addPoint;
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

        internal List<Tile> RemoveFrom(Point RemoveStart, bool dispose)
        {
            List<Tile> tiles = getTiles(RemoveStart);
            foreach (Tile t in tiles)
            {
                Remove(t, dispose);
            }

            return tiles; 
        }

        internal void AddRange(Point position, List<Tile> tiles)
        {
            if (tiles.Count == 0)
                return;

            TryAdd(position, tiles[0], false);
            Point lastPosition = position;
            for (int i = 1; i < tiles.Count; i++)
            {
                // re-align tiles, no need to concern the interference in calling TryAdd
                // because all tiles required moving have been unregistered, therefore 
                // no tiles hold while adding. use TryAdd because it excludes impossible positions
                TryAdd(getNext(lastPosition, tiles[i - 1].getTSize()), tiles[i], false);
                lastPosition = getRegisterLocation(tiles[i]);
            }
        }

        
        internal void setPhysicalPosition(Point unwrapPosition, Tile tile)
        {
            tile.Location = getPhysicalPosition(wrapPosition(unwrapPosition));
        }

        private Point getPhysicalPosition(Point wrappedPosition)
        {
            return new Point(Settings.tile_left_screen +
                wrappedPosition.X * Settings.tile_unit_length + wrappedPosition.X * Settings.small_span +
                wrappedPosition.X / 4 * Settings.tile_group_margin_extra, Settings.tile_top_screen +
                wrappedPosition.Y * Settings.tile_unit_length + wrappedPosition.Y * Settings.small_span);
        }

        internal Point getRegisterPosition(Point physicalPosition)
        {
            int group = (physicalPosition.X - Settings.tile_left_screen) / 
                (Settings.tile_4X_unit_length + Settings.tile_group_margin_extra);

            int approx_X = (physicalPosition.X - Settings.tile_left_screen - 
                group * (Settings.tile_4X_unit_length + Settings.small_span + 
                Settings.tile_group_margin_extra)) / (Settings.tile_unit_length + Settings.small_span);

            int approx_Y = group * 8 + (physicalPosition.Y - Settings.tile_top_screen) / 
                (Settings.tile_unit_length + Settings.small_span);

            return new Point(approx_X % 4, approx_Y);
        }

        internal Point wrapPosition(Point unwrappedPosition)
        {
            int wrapY = unwrappedPosition.Y % 8;
            int wrapX = unwrappedPosition.Y / 8 * 4 + unwrappedPosition.X;
            return new Point(wrapX, wrapY);
        }
        internal Point unwrapPosition(Point wrappedPosition)
        {
            int unwrapY = wrappedPosition.Y + wrappedPosition.X / 4 * 8;
            int unwrapX = wrappedPosition.X % 4;
            return new Point(unwrapX, unwrapY);
        }

        internal void move(Point source, Point destination)
        {
            Tile sourceTile = register[source.Y, source.X];
            
            // find the location that fits sourceTile, may be occupied
            Point finalDest = findElligibleInsertion(destination, sourceTile.getTSize());
            Tile destTile = register[finalDest.Y, finalDest.X];

            this.Remove(sourceTile, false);

            if (destTile != null && destTile.getTSize() == sourceTile.getTSize())
            {
                Remove(destTile, false);
                Add(source, destTile, false);
            }

            this.Add(finalDest, sourceTile, false);
        }

        internal void paintProjectedFrame(Point physicalPosition, Settings.TSize ts)
        {
            Label lblTileProj = (Label)display.Controls.Find("projectedTile", false)[0];

            //convert crude physical position into accurate one
            physicalPosition = getPhysicalPosition(wrapPosition(physicalPosition));

            if (lblTileProj.Location != physicalPosition)
            {
                lblTileProj.SendToBack();
                lblTileProj.Hide();
                lblTileProj.BackColor = Color.FromArgb(100, Color.White);

                lblTileProj.Size = Tile.getActualSize(ts);

                lblTileProj.Location = physicalPosition;

                lblTileProj.Left -= 4; lblTileProj.Top -= 4;
                lblTileProj.Width += 8; lblTileProj.Height += 8;

                lblTileProj.Show();
            }
        }

        internal void clearProjectedFrame()
        {
            Label lblTileProj = (Label)display.Controls.Find("projectedTile", false)[0];
            lblTileProj.Hide();
        }
    }
}