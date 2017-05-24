using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnyTileSimultaneous_Goal : Goal
{
    protected override bool ApplyCondition(List<BaseTile> list)
    {
        bool conditionMet = false;

        if (list.Count >= amountSimultaneous)
           currentAmount++;


        if (requiredAmount >= currentAmount)
            conditionMet = true;

        return conditionMet;
    }

    protected override void SetText()
    {
        textDescription = "Tumble " + amountSimultaneous + " tiles in one move: " + currentAmount + "/" + requiredAmount;
    }
}