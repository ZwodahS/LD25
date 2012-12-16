using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace MazeMaster.Assets
{
    public class GraphicsAssets
    {
        public Texture2D MainSprite;

        public Rectangle GrassTile;
        public Rectangle ExitTile;

        public Rectangle Horizontal;
        public Rectangle Vertical;
        public Rectangle HorizontalHighlighted;
        public Rectangle VerticalHighlighted;
        public Rectangle HorizontalDamaged;
        public Rectangle VerticalDamaged;
        public Rectangle HorizontalBroken;
        public Rectangle VerticalBroken;
        public Rectangle[] Characters;
        public Rectangle RockObstacle;
        public Rectangle TileHighlight;

        public static GraphicsAssets Instance;

        

        public GraphicsAssets()
        {
            
        }

        public static void Load(ContentManager content)
        {
            Instance = new GraphicsAssets();

            Instance.MainSprite = content.Load<Texture2D>("sprite");

            Instance.GrassTile = new Rectangle(32, 0, 32, 32);

            Instance.Horizontal = new Rectangle(0, 0, 32, 4);
            Instance.Vertical = new Rectangle(0, 0, 4, 32);
            Instance.HorizontalHighlighted = new Rectangle(64, 0, 32, 4);
            Instance.VerticalHighlighted = new Rectangle(64, 0, 4, 32);
            Instance.HorizontalDamaged = new Rectangle(64, 32, 32, 4);
            Instance.VerticalDamaged = new Rectangle(64, 32, 4, 32);
            Instance.HorizontalBroken = new Rectangle(0, 28, 32, 4);
            Instance.VerticalBroken = new Rectangle(28, 0, 4, 32);

            Instance.Characters = new Rectangle[4];
            for (int i = 0; i < Instance.Characters.Length; i++)
            {
                Instance.Characters[i] = new Rectangle(0, 64+(i*32), 32, 32);
            }

            Instance.RockObstacle = new Rectangle(0, 32, 32, 32);

            Instance.TileHighlight = new Rectangle(128, 0, 32, 32);
            Instance.ExitTile = new Rectangle(96, 0, 32, 32);
        }
    }
}
