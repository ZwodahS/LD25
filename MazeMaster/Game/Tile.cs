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

        public WallType LeftWall;
        public WallType RightWall;
        public WallType UpWall;
        public WallType DownWall;

        public ObstacleType Obstacle;

        private int Row;
        private int Col;

        private Rectangle DrawBound;
        private Rectangle UpBound;
        private Rectangle DownBound;
        private Rectangle LeftBound;
        private Rectangle RightBound;

        public Tile(int r,int c)
        {
            Row = r;
            Col = c;
            UpdateBound();
            Texture = GraphicsAssets.Instance.MainSprite;
            LeftWall = WallType.None;
            RightWall = WallType.None;
            UpWall = WallType.None;
            DownWall = WallType.None;
        }
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(Texture, DrawBound, GraphicsAssets.Instance.GrassTile, Color.White);
            if (UpWall == WallType.Sealed)
            {
                spriteBatch.Draw(Texture, UpBound, GraphicsAssets.Instance.Horizontal, Color.White);
            }
            else if (UpWall == WallType.Broken)
            {
                spriteBatch.Draw(Texture, UpBound, GraphicsAssets.Instance.HorizontalBroken, Color.White);
            }

            if (DownWall == WallType.Sealed)
            {
                spriteBatch.Draw(Texture, DownBound, GraphicsAssets.Instance.Horizontal, Color.White);
            }
            else if (DownWall == WallType.Broken)
            {
                spriteBatch.Draw(Texture, DownBound, GraphicsAssets.Instance.HorizontalBroken, Color.White);
            }

            if (LeftWall == WallType.Sealed)
            {
                spriteBatch.Draw(Texture, LeftBound, GraphicsAssets.Instance.Vertical, Color.White);
            }
            else if (LeftWall == WallType.Broken)
            {
                spriteBatch.Draw(Texture, LeftBound, GraphicsAssets.Instance.VerticalBroken, Color.White);
            }

            if (RightWall == WallType.Sealed)
            {
                spriteBatch.Draw(Texture, RightBound, GraphicsAssets.Instance.Vertical, Color.White);
            }
            else if (RightWall == WallType.Broken)
            {
                spriteBatch.Draw(Texture, RightBound, GraphicsAssets.Instance.VerticalBroken, Color.White);
            }
        }

        public void Update(GameTime gameTime)
        {

        }

        private void UpdateBound()
        {
            DrawBound = new Rectangle(Col * 32, Row * 32, 32, 32);
            UpBound = new Rectangle(Col * 32, Row * 32, 32, 4);
            DownBound = new Rectangle(Col * 32, Row * 32 + 28, 32, 4);
            LeftBound = new Rectangle(Col * 32, Row * 32, 4, 32);
            RightBound = new Rectangle(Col * 32 + 28, Row * 32, 4, 32);

        }
    }
}
