using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level2 : LevelAbstract
{
    public override void MoveBlocks()
    {
        this.LoopToMoveBlocks(LevelCodeName.level2);
    }
}
