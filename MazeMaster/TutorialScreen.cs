using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MazeMaster.Inputs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MazeMaster.Assets;

namespace MazeMaster
{
    public class TutorialScreen : Screen
    {
        public InputHandler iH;
        private int index;
        public TutorialScreen(MazeMaster parent):base(parent)
        {
            iH = new InputHandler();
            index = 0;
        }

        public override void Update(GameTime gameTime)
        {
            iH.Update();
            if (iH.MouseLeft[InputHandler.THIS])
            {
                if (GraphicsAssets.Instance.TutorialNextBound.Contains(iH.Pos))
                {
                    Next();
                }
                else if (GraphicsAssets.Instance.TutorialPrevBound.Contains(iH.Pos))
                {
                    Prev();
                }
                else if (GraphicsAssets.Instance.TutorialExitBound.Contains(iH.Pos))
                {
                    Quit();
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(GraphicsAssets.Instance.Tutorials[index], new Rectangle(0, 0, 600 * MazeMaster.ScreenMultiplier, 384 * MazeMaster.ScreenMultiplier), Color.White);
            spriteBatch.End();
        }

        public void Next()
        {
            index++;
            if (index >= GraphicsAssets.Instance.Tutorials.Length)
            {
                Quit();
            }
        }

        public void Prev()
        {
            index--;
            if (index < 0)
            {
                Quit();
            }
        }

        public void Quit()
        {
            Parent.ShowMain();
        }
    }
}
