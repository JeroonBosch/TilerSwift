using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CagedTile : BaseTile
{
    Transform cage;
    TileCage tileCage;
    protected override void Awake()
    {
        tileType = new TileTypes();

        cage = meshObject.Find("Cage");
        tileCage = cage.GetComponent<TileCage>();
    }

    protected override void DerrivedInit(Cell cell, int size, GridController grid) {
        SetCell(cell);

        this.size = size;
        transform.parent = cell.transform;

        meshObject.localScale = new Vector3(.7f, .3f, 2f);
        meshObject.localPosition = new Vector3(0, 0, -(2f / 2));
        transform.localScale = new Vector3(1f, 1f, 1f);
        //transform.localPosition = new Vector3(0, 0, 0);
        transform.position = new Vector3(cell.transform.position.x, 0, 10f);

        this.grid = grid;

        tileType.State = TileTypes.ESubState.White;
    }

    public void DamageCage() {
        Debug.Log("Damaging Cage.");
        
        if (cage)
        {
            if (tileCage)
                tileCage.hitpoints--;
        }
    }

    protected override void SpecialTumbleActions(GridController gridController)
    {
        if (gridController.attemptedTumble.Count >= 3)
        {
            if (cell.tag == "CellRow0")
                canBeDestroyed = false;
            DamageCage();
        }
    }
}