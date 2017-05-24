using UnityEngine;
using System.Collections.Generic;

public class Level1_Settings : Level_Settings
{
    protected override void Awake()
    {
        base.Awake();
        /*for (int i = 0; i < goals.Length; i++)
        {
            goals[i] = ScriptableObject.CreateInstance<Goal>();
        }*/

        iceCells = new iVec2[32];

        /*goals[0] = ScriptableObject.CreateInstance<AnyTileSimultaneous_Goal>();
        goals[0].requiredAmount = 3;
        goals[0].amountSimultaneous = 8;
        goals[0].UpdateText(); */

        /*goals[1] = ScriptableObject.CreateInstance<YellowTileSimultaneous_Goal>();
        goals[1].requiredAmount = 3;
        goals[1].amountSimultaneous = 4;
        goals[1].UpdateText();

        goals[2] = ScriptableObject.CreateInstance<AnyTileSimultaneous_Goal>();
        goals[2].requiredAmount = 6;
        goals[2].amountSimultaneous = 5;
        goals[2].UpdateText();*/

        //goals[0] = ScriptableObject.CreateInstance<MergeBlackTiles_Goal>();
        //goals[0].requiredAmount = 8;
        //goals[0].UpdateText();


        goals[0] = ScriptableObject.CreateInstance<BringDownCagedWhite_Goal>();
        goals[0].requiredAmount = 4;
        goals[0].UpdateText();

        goals[1] = ScriptableObject.CreateInstance<RemoveIce_Goal>();
        goals[1].requiredAmount = 24;
        goals[1].UpdateText();
    }

    protected override void SetData() {
        _x_size = 6; //columns
        _y_size = 8; //size
        _turnsAllowed = 30;
        _amountOfGoals = 2;
        _hasSpecialTileSetup = true;

        iceCells = new iVec2[32];
        int i = 0;
        for (i = 0; i < 8; i++)
            iceCells[i] = new iVec2(0, i);
        for (i = 8; i < 16; i++)
            iceCells[i] = new iVec2(1, i-8);
        for (i = 16; i < 24; i++)
            iceCells[i] = new iVec2(4, i-16);
        for (i = 24; i < 32; i++)
            iceCells[i] = new iVec2(5, i-24);
    }
}
