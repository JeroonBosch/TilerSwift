using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RemoveIce_Goal : Goal
{
    protected override bool ApplyCondition(List<BaseTile> list)
    {
        bool conditionMet = false;

        if (requiredAmount >= currentAmount)
            conditionMet = true;

        return conditionMet;
    }

    protected override void SetText()
    {
        textDescription = "Remove all ice: " + currentAmount + "/" + requiredAmount + " removed";
    }

    protected override void LevelModifiers()
    {
        Cell[] cellList = GameObject.FindObjectsOfType<Cell>();

        int amountLeft = requiredAmount;
        foreach (Cell cell in cellList)
        {
            if (cell.isIce)
                amountLeft--;
        }
        currentAmount = amountLeft;
        UpdateText();
    }

    
}