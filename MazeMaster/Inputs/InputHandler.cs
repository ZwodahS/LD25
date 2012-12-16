using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace MazeMaster.Inputs
{
    public class InputHandler
    {
        public const int NEW = 0;
        public const int CURR = 1;
        public const int THIS = 2;
        public const int RELEASE = 3;
        public bool[] MouseLeft;
        public bool[] MouseRight;
        public Point Pos;
        public InputHandler()
        {
            MouseLeft = new bool[] { false, false, false, false };
            MouseRight = new bool[] { false, false, false, false };
            Pos = Point.Zero;
        }

        public void Update()
        {
            MouseState state = Mouse.GetState();
            MouseLeft[NEW] = state.LeftButton == ButtonState.Pressed;
            MouseRight[NEW] = state.RightButton == ButtonState.Pressed;
            register(MouseLeft);
            register(MouseRight);
            Pos = new Point(state.X, state.Y);
        }
        
        protected void register(bool[] key)
        {
            key[RELEASE] = false;
            if (key[NEW])
            {
                if (!key[CURR])
                {
                    key[CURR] = true;
                    key[THIS] = true;
                }
                else
                {
                    key[THIS] = false;
                }
            }
            else
            {
                if (key[CURR])
                {
                    key[RELEASE] = true;
                }
                else
                {
                    key[RELEASE] = false;
                }
                key[THIS] = false;
                key[CURR] = false;
            }
        }
    }
}
