using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BlueTile_Goal : Goal
{
    protected override bool ApplyCondition(List<BaseTile> list)
    {
        bool conditionMet = false;

        foreach (BaseTile tile in list)
        {
            if (tile.tileType.State == TileTypes.ESubState.Blue)
                currentAmount++;
        }

        if (requiredAmount >= currentAmount)
            conditionMet = true;

        return conditionMet;
    }

    protected override void SetText()
    {
        textDescription = "Tumble blue tiles: " + currentAmount + "/" + requiredAmount;
    }
}