using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MazeMaster.Game.Humans
{
    public enum ActionType
    {
        None,
        Move,
        DestroyWeakWall,
        Explode, //Blood A panic attack , TargetGrid will be the current grid.
        Ghost, //Blood B panic attack , TargetGrid will be the grid to the ghost to.
        Rage, //Blood C panic attack , TargetGrid will be the current grid. , rage in the facing direction
        Dash,
    }
}
