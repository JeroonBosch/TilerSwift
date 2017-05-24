using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BlueTileSimultaneous_Goal : Goal
{
    protected override bool ApplyCondition(List<BaseTile> list)
    {
        bool conditionMet = false;

        List<BaseTile> testList = new List<BaseTile>();

        foreach (BaseTile tile in list)
        {
            if (tile.tileType.State == TileTypes.ESubState.Blue)
                testList.Add(tile);
        }

        if (testList.Count >= amountSimultaneous)
           currentAmount++;


        if (requiredAmount >= currentAmount)
            conditionMet = true;

        return conditionMet;
    }

    protected override void SetText()
    {
        textDescription = "Tumble " + amountSimultaneous + " blue tiles in one move: " + currentAmount + "/" + requiredAmount;
    }
}