using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class BringDownWhite_Goal : Goal
{

    public int whiteTilesSpawned = 0;

    protected override bool ApplyCondition(List<BaseTile> list)
    {
        bool conditionMet = false;

        if (requiredAmount >= currentAmount)
            conditionMet = true;

        return conditionMet;
    }

    protected override void SetText()
    {
        textDescription = "Bring white tiles to the bottom: " + currentAmount + "/" + requiredAmount;
    }

    protected override void LevelModifiers()
    {
        /*GameObject[] bottomCells = GameObject.FindGameObjectsWithTag("CellRow0");
        List<Cell> cellList = new List<Cell>();
        foreach (GameObject go in bottomCells)
            cellList.Add(go.GetComponent<Cell>());

        foreach (Cell cell in cellList)
        {
            if (cell.tile.size == 6)
            {
                cell.tile.Destroy(1);
                currentAmount++;
                UpdateText();
            }
        }*/

        GameObject[] bottomCells = GameObject.FindGameObjectsWithTag("CellRow0");
        List<Cell> cellList = new List<Cell>();
        foreach (GameObject go in bottomCells)
            cellList.Add(go.GetComponent<Cell>());

        foreach (Cell cell in cellList)
        {
            if (cell.tile.size == 6)
            {
                cell.tile.Destroy(1);
                currentAmount++;
                UpdateText();
            }
        }
    }
}