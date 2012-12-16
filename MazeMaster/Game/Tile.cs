using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MazeMaster.Assets;

namespace MazeMaster.Game
{
    public class Tile
    {
        public Texture2D Texture;

        public WallState LeftWall;
        public WallState RightWall;
        public WallState UpWall;
        public WallState DownWall;

        public const int Up = 0;
        public const int Down = 1;
        public const int Left = 2;
        public const int Right = 3;
        public float[] WallAnimateTime; // for showing destruction animation , [up,down,left,right]

        public ObstacleType Obstacle;

        private int Row;
        private int Col;

        public bool IsExit;
        public Grid CurrentGrid
        {
            get
            {
                return new Grid(Row, Col);
            }
            set
            {
                Row = value.Row;
                Col = value.Col;
                UpdateBound();
            }
        }
        private Rectangle DrawBound;
        private Rectangle UpBound;
        private Rectangle DownBound;
        private Rectangle LeftBound;
        private Rectangle RightBound;

        public Tile(int r,int c)
        {
            Row = r;
            Col = c;
            WallAnimateTime = new float[] { 0 , 0 , 0 , 0 };
            UpdateBound();
            Texture = GraphicsAssets.Instance.MainSprite;
            LeftWall = WallState.None;
            RightWall = WallState.None;
            UpWall = WallState.None;
            DownWall = WallState.None;
        }
        public void DrawAt(SpriteBatch spriteBatch, GameTime gameTime, Point point)
        {
            Rectangle drawBound = new Rectangle(point.X, point.Y, MazeMaster.TileSize, MazeMaster.TileSize);
            Rectangle upBound = new Rectangle(point.X, point.Y, MazeMaster.TileSize, MazeMaster.WallSize);
            Rectangle downBound = new Rectangle(point.X, point.Y + MazeMaster.TileSize - MazeMaster.WallSize, MazeMaster.TileSize, MazeMaster.WallSize);
            Rectangle leftBound = new Rectangle(point.X, point.Y, MazeMaster.WallSize, MazeMaster.TileSize);
            Rectangle rightBound = new Rectangle(point.X + MazeMaster.TileSize - MazeMaster.WallSize, point.Y, MazeMaster.WallSize, MazeMaster.TileSize);
            if (IsExit)
            {
                spriteBatch.Draw(Texture, drawBound, GraphicsAssets.Instance.ExitTile, Color.White);
            }
            else
            {
                spriteBatch.Draw(Texture, drawBound, GraphicsAssets.Instance.GrassTile, Color.White);
                if (UpWall == WallState.Sealed)
                {
                    spriteBatch.Draw(Texture, upBound, GraphicsAssets.Instance.Horizontal, Color.White);
                }
                else if (UpWall == WallState.Broken)
                {
                    spriteBatch.Draw(Texture, upBound, GraphicsAssets.Instance.HorizontalBroken, Color.White);
                }
                else if (UpWall == WallState.Breaking)
                {
                    spriteBatch.Draw(Texture, upBound, GraphicsAssets.Instance.Horizontal, Color.White);
                    spriteBatch.Draw(Texture, upBound, GraphicsAssets.Instance.HorizontalDamaged, new Color(255, 0, 0, WallAnimateTime[0] * 0.5f));
                }

                if (DownWall == WallState.Sealed)
                {
                    spriteBatch.Draw(Texture, downBound, GraphicsAssets.Instance.Horizontal, Color.White);
                }
                else if (DownWall == WallState.Broken)
                {
                    spriteBatch.Draw(Texture, downBound, GraphicsAssets.Instance.HorizontalBroken, Color.White);
                }
                else if (DownWall == WallState.Breaking)
                {
                    spriteBatch.Draw(Texture, downBound, GraphicsAssets.Instance.Horizontal, Color.White);
                    spriteBatch.Draw(Texture, downBound, GraphicsAssets.Instance.HorizontalDamaged, new Color(255, 0, 0, WallAnimateTime[1] * 0.5f));
                }

                if (LeftWall == WallState.Sealed)
                {
                    spriteBatch.Draw(Texture, leftBound, GraphicsAssets.Instance.Vertical, Color.White);
                }
                else if (LeftWall == WallState.Broken)
                {
                    spriteBatch.Draw(Texture, leftBound, GraphicsAssets.Instance.VerticalBroken, Color.White);
                }
                else if (LeftWall == WallState.Breaking)
                {
                    spriteBatch.Draw(Texture, leftBound, GraphicsAssets.Instance.Vertical, Color.White);
                    spriteBatch.Draw(Texture, leftBound, GraphicsAssets.Instance.VerticalDamaged, new Color(255, 0, 0, WallAnimateTime[2] * 0.5f));
                }

                if (RightWall == WallState.Sealed)
                {
                    spriteBatch.Draw(Texture, rightBound, GraphicsAssets.Instance.Vertical, Color.White);
                }
                else if (RightWall == WallState.Broken)
                {
                    spriteBatch.Draw(Texture, rightBound, GraphicsAssets.Instance.VerticalBroken, Color.White);
                }
                else if (RightWall == WallState.Breaking)
                {
                    spriteBatch.Draw(Texture, rightBound, GraphicsAssets.Instance.Vertical, Color.White);
                    spriteBatch.Draw(Texture, rightBound, GraphicsAssets.Instance.VerticalDamaged, new Color(255, 0, 0, WallAnimateTime[3] * 0.5f));
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (IsExit)
            {
                spriteBatch.Draw(Texture, DrawBound, GraphicsAssets.Instance.ExitTile, Color.White);
            }
            else
            {
                spriteBatch.Draw(Texture, DrawBound, GraphicsAssets.Instance.GrassTile, Color.White);
                if (UpWall == WallState.Sealed)
                {
                    spriteBatch.Draw(Texture, UpBound, GraphicsAssets.Instance.Horizontal, Color.White);
                }
                else if (UpWall == WallState.Broken)
                {
                    spriteBatch.Draw(Texture, UpBound, GraphicsAssets.Instance.HorizontalBroken, Color.White);
                }
                else if (UpWall == WallState.Breaking)
                {
                    spriteBatch.Draw(Texture, UpBound, GraphicsAssets.Instance.Horizontal, Color.White);
                    spriteBatch.Draw(Texture, UpBound, GraphicsAssets.Instance.HorizontalDamaged, new Color(255, 0, 0, WallAnimateTime[0] * 0.5f));
                }

                if (DownWall == WallState.Sealed)
                {
                    spriteBatch.Draw(Texture, DownBound, GraphicsAssets.Instance.Horizontal, Color.White);
                }
                else if (DownWall == WallState.Broken)
                {
                    spriteBatch.Draw(Texture, DownBound, GraphicsAssets.Instance.HorizontalBroken, Color.White);
                }
                else if (DownWall == WallState.Breaking)
                {
                    spriteBatch.Draw(Texture, DownBound, GraphicsAssets.Instance.Horizontal, Color.White);
                    spriteBatch.Draw(Texture, DownBound, GraphicsAssets.Instance.HorizontalDamaged, new Color(255, 0, 0, WallAnimateTime[1] * 0.5f));
                }

                if (LeftWall == WallState.Sealed)
                {
                    spriteBatch.Draw(Texture, LeftBound, GraphicsAssets.Instance.Vertical, Color.White);
                }
                else if (LeftWall == WallState.Broken)
                {
                    spriteBatch.Draw(Texture, LeftBound, GraphicsAssets.Instance.VerticalBroken, Color.White);
                }
                else if (LeftWall == WallState.Breaking)
                {
                    spriteBatch.Draw(Texture, LeftBound, GraphicsAssets.Instance.Vertical, Color.White);
                    spriteBatch.Draw(Texture, LeftBound, GraphicsAssets.Instance.VerticalDamaged, new Color(255, 0, 0, WallAnimateTime[2] * 0.5f));
                }

                if (RightWall == WallState.Sealed)
                {
                    spriteBatch.Draw(Texture, RightBound, GraphicsAssets.Instance.Vertical, Color.White);
                }
                else if (RightWall == WallState.Broken)
                {
                    spriteBatch.Draw(Texture, RightBound, GraphicsAssets.Instance.VerticalBroken, Color.White);
                }
                else if (RightWall == WallState.Breaking)
                {
                    spriteBatch.Draw(Texture, RightBound, GraphicsAssets.Instance.Vertical, Color.White);
                    spriteBatch.Draw(Texture, RightBound, GraphicsAssets.Instance.VerticalDamaged, new Color(255, 0, 0, WallAnimateTime[3] * 0.5f));
                }
            }
        }

        public void Update(GameTime gameTime)
        {

        }

        private void UpdateBound()
        {
            DrawBound = new Rectangle(Col * MazeMaster.TileSize, Row * MazeMaster.TileSize, MazeMaster.TileSize, MazeMaster.TileSize);
            UpBound = new Rectangle(Col * MazeMaster.TileSize, Row * MazeMaster.TileSize, MazeMaster.TileSize, MazeMaster.WallSize);
            DownBound = new Rectangle(Col * MazeMaster.TileSize, Row * MazeMaster.TileSize + MazeMaster.TileSize - MazeMaster.WallSize, MazeMaster.TileSize, MazeMaster.WallSize);
            LeftBound = new Rectangle(Col * MazeMaster.TileSize, Row * MazeMaster.TileSize, MazeMaster.WallSize, MazeMaster.TileSize);
            RightBound = new Rectangle(Col * MazeMaster.TileSize + MazeMaster.TileSize - MazeMaster.WallSize, Row * MazeMaster.TileSize, MazeMaster.WallSize, MazeMaster.TileSize);

        }
    }
}
