using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MazeMaster.Game;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MazeMaster.Game.Humans;
using MazeMaster.Inputs;
using MazeMaster.Assets;

namespace MazeMaster
{
    public class GameScreen
    {
        private Maze CurrentMaze;

        public Tile NextTile;
        public InputHandler iH;

        public GameState State;

        public List<Unit> Targets;
        public List<Rectangle> TargetBGDraws;
        public List<Point> TargetIcons;
        public List<Vector2> TargetTime;
        public List<Vector2> TargetMoney;
        public List<Vector2> TargetTips;


        public GameScreen()
        {
            iH = new InputHandler();
            PrepareTargetDraw();
            TotalTime = 0;
            HighestRPM = 0;
            TotalRansom = 0;
        }

        public float TotalTime;

        public float TotalRansom;
        public float HighestRPM;
        public Rectangle KidnapButtonDraw = new Rectangle(400 * MazeMaster.ScreenMultiplier, 140 * MazeMaster.ScreenMultiplier, 20 * MazeMaster.ScreenMultiplier, 20 * MazeMaster.ScreenMultiplier);
        public Rectangle PausePlayButtonDraw = new Rectangle(420 * MazeMaster.ScreenMultiplier, 140 * MazeMaster.ScreenMultiplier, 20 * MazeMaster.ScreenMultiplier, 20 * MazeMaster.ScreenMultiplier);

        public void RestartLevel()
        {
            TotalTime = 0;
            HighestRPM = 0;
            TotalRansom = 0;
            Random rng = new Random();
            CurrentMaze = new Maze(MazeGenerator.GenerateMaze(12, 12, rng.Next()));
            CurrentMaze.Parent = this;
            RandomNext();
            State = GameState.Pause;
            
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Begin();
            CurrentMaze.Draw(spriteBatch, gameTime);
            if (NextTile.CurrentGrid.Row != -1)
            {
                NextTile.Draw(spriteBatch, gameTime);
                spriteBatch.Draw(GraphicsAssets.Instance.MainSprite, new Rectangle(NextTile.CurrentGrid.Col * MazeMaster.TileSize, NextTile.CurrentGrid.Row * MazeMaster.TileSize, MazeMaster.TileSize, MazeMaster.TileSize), GraphicsAssets.Instance.TileHighlight, new Color(255, 255, 255, 0.2f));
            }

            spriteBatch.DrawString(GraphicsAssets.Instance.SideFont, "Next :", new Vector2(400, 20) * MazeMaster.ScreenMultiplier, Color.Black);
            NextTile.DrawAt(spriteBatch, gameTime, new Point(MazeMaster.ScreenMultiplier * 450,MazeMaster.ScreenMultiplier*20));

            spriteBatch.DrawString(GraphicsAssets.Instance.SideFont, "Career Time   : " + (int)(TotalTime / 60) + " mins  " + (TotalTime % 60).ToString("0.00") + " secs", new Vector2(400 * MazeMaster.ScreenMultiplier, 60 * MazeMaster.ScreenMultiplier), Color.Black);
            spriteBatch.DrawString(GraphicsAssets.Instance.SideFont, "Total Earning : " + "$" + (TotalRansom == 0 ? "0" : TotalRansom.ToString("#,#;(#,#)")), new Vector2(400 * MazeMaster.ScreenMultiplier, 80 * MazeMaster.ScreenMultiplier), Color.Black);
            float RPM = TotalRansom == 0 ? 0 : TotalRansom / TotalTime * 60;
            HighestRPM = RPM > HighestRPM ? RPM : HighestRPM;
            spriteBatch.DrawString(GraphicsAssets.Instance.SideFont, "Ransom Per min: " + "$" + (RPM == 0 ? "0" : RPM.ToString("#,#;(#,#)")), new Vector2(400 * MazeMaster.ScreenMultiplier, 100 * MazeMaster.ScreenMultiplier), Color.Black);
            spriteBatch.DrawString(GraphicsAssets.Instance.SideFont, "Highest RPM   : " + "$" + (HighestRPM == 0 ? "0" : HighestRPM.ToString("#,#;(#,#)")), new Vector2(400 * MazeMaster.ScreenMultiplier, 120 * MazeMaster.ScreenMultiplier), Color.Black);
            for (int i = 0; i < CurrentMaze.Humans.Count; i++)
            {
                Unit unit = CurrentMaze.Humans[i];
                int pos =  (200 + i * 20)*MazeMaster.ScreenMultiplier;
                unit.DrawAt(spriteBatch, gameTime, new Point(400*MazeMaster.ScreenMultiplier,pos));
                spriteBatch.DrawString(GraphicsAssets.Instance.SideFont,unit.RansomTimeLeft.ToString("0.00") + " Secs To ransom", new Vector2(425*MazeMaster.ScreenMultiplier, pos+(7*MazeMaster.ScreenMultiplier)), Color.Black);
            }
            spriteBatch.Draw(GraphicsAssets.Instance.MainSprite, KidnapButtonDraw, GraphicsAssets.Instance.KidnapButton, Color.White);
            if (State == GameState.Pause)
            {
                spriteBatch.Draw(GraphicsAssets.Instance.MainSprite, PausePlayButtonDraw, GraphicsAssets.Instance.PlayButton, Color.White);
            }
            else if (State == GameState.Placement)
            {
                spriteBatch.Draw(GraphicsAssets.Instance.MainSprite, PausePlayButtonDraw, GraphicsAssets.Instance.PauseButton, Color.White);
            }
            else
            {
                spriteBatch.Draw(GraphicsAssets.Instance.MainSprite, PausePlayButtonDraw, GraphicsAssets.Instance.PauseButton, Color.White);
                DrawTargets(spriteBatch,gameTime);
            }
            




            spriteBatch.End();
        }

        private void DrawTargets(SpriteBatch spriteBatch, GameTime gameTime)
        {
            for(int i = 0 ; i < Targets.Count ; i++)
            {
                spriteBatch.Draw(GraphicsAssets.Instance.MainSprite, TargetBGDraws[i], GraphicsAssets.Instance.KidnapChoice, Color.White);
                Targets[i].DrawAt(spriteBatch, gameTime, TargetIcons[i]);
                spriteBatch.DrawString(GraphicsAssets.Instance.SideFont, Targets[i].RansomAmount.ToString("$#,#;(#,#)"), TargetMoney[i], Color.Black);
                spriteBatch.DrawString(GraphicsAssets.Instance.SideFont, Targets[i].RansomTimeLeft.ToString("0.00 secs"), TargetTime[i], Color.Black);
                if (Targets[i] is BloodB)
                {
                    spriteBatch.DrawString(GraphicsAssets.Instance.SideFont, "(Break Wall)", TargetTips[i], Color.Black);
                }
                
            }
        }

        

        public void Update(GameTime gameTime)
        {
            iH.Update();
            
            if (State == GameState.Placement)
            {
                TotalTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                CurrentMaze.Update(gameTime);
                Grid highlightedGrid = PointToGrid(iH.Pos);
                if (CurrentMaze.CanPlace(highlightedGrid))
                {
                    NextTile.CurrentGrid = highlightedGrid;
                }
                else
                {
                    NextTile.CurrentGrid = new Grid(-1, -1);
                }
                if (iH.MouseLeft[InputHandler.THIS])
                {
                    if (CurrentMaze.CanPlace(highlightedGrid))
                    {
                        PlaceTile(highlightedGrid);
                    }
                    else
                    {
                        if (KidnapButtonDraw.Contains(iH.Pos))
                        {
                            ToggleKidnap();
                        }
                        else if (PausePlayButtonDraw.Contains(iH.Pos))
                        {
                            TogglePausePlay();
                        }
                    }
                }
            }
            else if (State == GameState.Pause)
            {
                if (iH.MouseLeft[InputHandler.THIS])
                {
                    if (PausePlayButtonDraw.Contains(iH.Pos))
                    {
                        TogglePausePlay();
                    }
                }
            }
            else if (State == GameState.ChoosingTarget)
            {
                if (iH.MouseLeft[InputHandler.THIS])
                {
                    int target = -1;
                    for (int i = 0; i < TargetBGDraws.Count; i++)
                    {
                        if (TargetBGDraws[i].Contains(iH.Pos))
                        {
                            target = i;
                            break;
                        }
                    }
                    if (target != -1)
                    {
                        ChooseKidnapTarget(target);
                    }
                }
            }
            
                
        }
        private void ChooseKidnapTarget(int tID)
        {
            Unit target = Targets[tID];
            CurrentMaze.Kidnap(target);

            Targets = null;
            State = GameState.Placement;
        }
        private void TogglePausePlay()
        {
            if (State == GameState.Pause)
            {
                State = GameState.Placement;
            }
            else if (State == GameState.Placement)
            {
                State = GameState.Pause;
            }
        }

        private void ToggleKidnap()
        {
            State = GameState.ChoosingTarget;
            Targets = Unit.GetRandomTargets(6);
        }
        private void PrepareTargetDraw()
        {
            TargetBGDraws = new List<Rectangle>();
            TargetIcons = new List<Point>();
            TargetTime = new List<Vector2>();
            TargetMoney = new List<Vector2>();
            TargetTips = new List<Vector2>();
            int mul = MazeMaster.ScreenMultiplier;
            //assuming the number of targets is 6 (since I hardcoded it)
            TargetBGDraws.Add(new Rectangle(100 * mul, 67 * mul, 100 * mul, 100 * mul));
            TargetBGDraws.Add(new Rectangle(250 * mul, 67 * mul, 100 * mul, 100 * mul));
            TargetBGDraws.Add(new Rectangle(400 * mul, 67 * mul, 100 * mul, 100 * mul));
            TargetBGDraws.Add(new Rectangle(100 * mul, 217 * mul, 100 * mul, 100 * mul));
            TargetBGDraws.Add(new Rectangle(250 * mul, 217 * mul, 100 * mul, 100 * mul));
            TargetBGDraws.Add(new Rectangle(400 * mul, 217 * mul, 100 * mul, 100 * mul));

            TargetIcons.Add(new Point(134 * mul, 70 * mul));
            TargetIcons.Add(new Point(284 * mul, 70 * mul));
            TargetIcons.Add(new Point(434 * mul, 70 * mul));
            TargetIcons.Add(new Point(134 * mul, 220 * mul));
            TargetIcons.Add(new Point(284 * mul, 220 * mul));
            TargetIcons.Add(new Point(434 * mul, 220 * mul));

            TargetMoney.Add(new Vector2(114 * mul, 100 * mul));
            TargetMoney.Add(new Vector2(264 * mul, 100 * mul));
            TargetMoney.Add(new Vector2(414 * mul, 100 * mul));
            TargetMoney.Add(new Vector2(114 * mul, 250 * mul));
            TargetMoney.Add(new Vector2(264 * mul, 250 * mul));
            TargetMoney.Add(new Vector2(414 * mul, 250 * mul));

            TargetTime.Add(new Vector2(114 * mul, 110 * mul));
            TargetTime.Add(new Vector2(264 * mul, 110 * mul));
            TargetTime.Add(new Vector2(414 * mul, 110 * mul));
            TargetTime.Add(new Vector2(114 * mul, 260 * mul));
            TargetTime.Add(new Vector2(264 * mul, 260 * mul));
            TargetTime.Add(new Vector2(414 * mul, 260 * mul));

            TargetTips.Add(new Vector2(114 * mul, 130 * mul));
            TargetTips.Add(new Vector2(264 * mul, 130 * mul));
            TargetTips.Add(new Vector2(414 * mul, 130 * mul));
            TargetTips.Add(new Vector2(114 * mul, 280 * mul));
            TargetTips.Add(new Vector2(264 * mul, 280 * mul));
            TargetTips.Add(new Vector2(414 * mul, 280 * mul));
        }

        public Grid PointToGrid(Point p)
        {
            Grid g = new Grid((int)p.Y / MazeMaster.TileSize, (int)p.X / MazeMaster.TileSize);
            return g;
        }

        public void RandomNext()
        {
            NextTile = MazeGenerator.RandomTile(0,0,new Random());
            NextTile.CurrentGrid = new Grid(-1, -1);
        }

        public void PlaceTile(Grid g)
        {
            if (CurrentMaze.CanPlace(g))
            {
                CurrentMaze.PlaceTile(NextTile, g);
                RandomNext();
            }
        }

        public void UnitEscape(Unit unit)
        {

        }

        public void RansomPaid(Unit unit)
        {
            TotalRansom += unit.RansomAmount;
        }
    }
}
