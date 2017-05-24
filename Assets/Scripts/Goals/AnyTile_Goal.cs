using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnyTile_Goal : Goal
{
    protected override bool ApplyCondition(List<BaseTile> list)
    {
        bool conditionMet = false;

        foreach (BaseTile tile in list) {
            currentAmount++;
        }

        if (requiredAmount >= currentAmount)
            conditionMet = true;

        return conditionMet;
    }

    protected override void SetText()
    {
        textDescription = "Tumble any tile: " + currentAmount + "/" + requiredAmount;
    }
}