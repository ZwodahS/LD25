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
    public class MainScreen : Screen
    {
        public InputHandler iH;
        public Rectangle startButtonBound;
        public Rectangle tutorialButtonBound;
        public MainScreen(MazeMaster parent):base(parent)
        {
            iH = new InputHandler();
            startButtonBound = new Rectangle(250 * MazeMaster.ScreenMultiplier, 100 * MazeMaster.ScreenMultiplier, 100 * MazeMaster.ScreenMultiplier, 30 * MazeMaster.ScreenMultiplier);
            tutorialButtonBound = new Rectangle(250 * MazeMaster.ScreenMultiplier, 150 * MazeMaster.ScreenMultiplier, 100 * MazeMaster.ScreenMultiplier, 30 * MazeMaster.ScreenMultiplier);

        }

        public override void Update(GameTime gameTime)
        {
            iH.Update();
            if (iH.MouseLeft[InputHandler.THIS])
            {
                if (startButtonBound.Contains(iH.Pos))
                {
                    Parent.StartGame();
                }
                else if (tutorialButtonBound.Contains(iH.Pos))
                {
                    Parent.ShowTutorial();
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(GraphicsAssets.Instance.Home, new Rectangle(0, 0, 600 * MazeMaster.ScreenMultiplier, 384 * MazeMaster.ScreenMultiplier), Color.White);
            spriteBatch.Draw(GraphicsAssets.Instance.MainSprite, startButtonBound,GraphicsAssets.Instance.StartButton, Color.White);
            spriteBatch.Draw(GraphicsAssets.Instance.MainSprite, tutorialButtonBound, GraphicsAssets.Instance.TutorialButton, Color.White);
            spriteBatch.End();
        }
    }
}
