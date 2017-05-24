using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Goal : ScriptableObject
{
    public int requiredAmount; //How often it needs to be completed
    public int currentAmount; //How often it has been completed
    public int amountSimultaneous;
    protected string textDescription;

    protected virtual bool ApplyCondition(List<BaseTile> list)
    {
        return false;
    }

    public bool Condition(List<BaseTile> list) {
        return ApplyCondition(list);
    }

    protected virtual void SetText()
    {
        textDescription = "No description: " + currentAmount + "/" + requiredAmount;
    }
    public void UpdateText()
    {
        currentAmount = Mathf.Min(currentAmount, requiredAmount);
        SetText();
    }

    public string GetText()
    {
        return textDescription;
    }

    protected virtual void LevelModifiers()
    {
    }

    public void LevelModifierUpdate()
    {
        LevelModifiers();
    }
}