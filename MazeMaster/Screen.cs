using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MazeMaster
{
    public abstract class Screen
    {
        public MazeMaster Parent;
        public Screen(MazeMaster parent)
        {
            this.Parent = parent;
        }
        public abstract void Update(GameTime gameTime);

        public abstract void Draw(SpriteBatch spriteBatch, GameTime gameTime);
    }
}
