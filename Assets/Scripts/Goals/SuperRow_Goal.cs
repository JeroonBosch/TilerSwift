using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SuperRow_Goal : Goal
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
        textDescription = "Form 1-color rows (and destroy): " + currentAmount + "/" + requiredAmount;
    }

    protected override void LevelModifiers()
    {

    }
}