using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MergeBlackTiles_Goal : Goal
{

    public int blackTilesSpawned = 0;

    protected override bool ApplyCondition(List<BaseTile> list)
    {
        bool conditionMet = false;

        if (requiredAmount >= currentAmount)
            conditionMet = true;

        return conditionMet;
    }

    protected override void SetText()
    {
        textDescription = "Merge black tiles: " + currentAmount + "/" + requiredAmount;
    }

    protected override void LevelModifiers()
    {

    }
}