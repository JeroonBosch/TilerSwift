using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTile : MonoBehaviour {

    public Transform rotationPivot;
    public Transform meshObject;
    public Material[] possibleMaterials;
    public TileTypes tileType;

    protected Cell cell;
    public int size = 1;
    public bool canBeDestroyed = true;
    protected GridController grid;

    public bool selected = false;
    public bool moving = false;

    protected bool falling = false;
    protected float fallWaitTime = 0;
    protected float fallWaitCounter = 0;

    protected virtual void Awake()
    {
        tileType = new TileTypes();
    }
    protected virtual void DerrivedInit(Cell cell, int size, GridController grid) {
        SetCell(cell);

        this.size = size;
        transform.parent = cell.transform;

        float height = 1f + size * .2f;
        float xSize = 0.25f + 0.08f * size;
        float ySize = 0.1f + 0.05f * size;
        //float height = 2.2f;
        //float xSize = 0.5f;
        //loat ySize = 0.2f;
        meshObject.localScale = new Vector3(xSize, ySize, height);
        meshObject.localPosition = new Vector3(0, 0, -(height / 2));
        transform.localScale = new Vector3(1f, 1f, 1f);
        //transform.localPosition = new Vector3(0, 0, 0);
        transform.position = new Vector3(cell.transform.position.x, 0, 10f);

        if (grid.CheckForWhiteTiles(cell))
        {
            this.size = 6;
            grid.WhiteTileAdded();
            meshObject.localScale = new Vector3(.7f, .45f, 2.3f);
        }
        else if (grid.CheckForBlackTiles(cell))
        {
            this.size = 7;
            grid.BlackTileAdded();
            meshObject.localScale = new Vector3(.7f, .45f, 2.3f);
        }

        meshObject.GetComponent<MeshRenderer>().material = possibleMaterials[this.size - 1];

        this.grid = grid;

        switch (size)
        {
            case 1:
                tileType.State = TileTypes.ESubState.Purple;
                break;
            case 2:
                tileType.State = TileTypes.ESubState.Green;
                break;
            case 3:
                tileType.State = TileTypes.ESubState.Blue;
                break;
            case 4:
                tileType.State = TileTypes.ESubState.Yellow;
                break;
            case 5:
                tileType.State = TileTypes.ESubState.Red;
                break;
            case 6:
                tileType.State = TileTypes.ESubState.White;
                break;
            default:
                break;
        }
    }
    public void InitTile (Cell cell, int size, GridController grid)
    {
        DerrivedInit(cell, size, grid);
    }

    protected virtual void Update ()
    {
        if (falling)
        {
            if (fallWaitCounter < fallWaitTime)
                fallWaitCounter++;
            else
                transform.localEulerAngles = Vector3.RotateTowards(transform.localEulerAngles,new Vector3(transform.localEulerAngles.x + 30f * grid.lastSwipeDirection, transform.localEulerAngles.y, transform.localEulerAngles.z), 1.5f, .5f);
        }
        else if (!selected)
        {
            IdleRotation();
        }

        if (!grid.destructionLockOut)
        {
            if (transform.localPosition.y != 0 || transform.localPosition.x != 0)
            {
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, new Vector3(0, 0, 0), Constants.tileFallRate);
                moving = true;
            } else
            {
                moving = false;
            }
        }
    }


    public Cell GetCell()
    {
        return cell;
    }

    public void SetCell(Cell cell)
    {
        this.cell = cell;
        cell.tile = this;
        transform.parent = cell.transform;
    }

    public void TurnTowards(float Z, float Y)
    {
        rotationPivot.Rotate(Z * 80, Y * 80, 0, Space.Self);
    }

    void IdleRotation()
    {
        rotationPivot.localRotation = Quaternion.RotateTowards(
            rotationPivot.localRotation,
            Quaternion.identity,
            1
        );
    }

    //Actions
    protected virtual void SpecialTumbleActions (GridController gridController) { }
    public void TumbleColumn(float direction, GridController gridController, int prevSize)
    {
        //if (prevSize == size && size != 6)
        if (prevSize >= size && size != 6 && size != 7)
        {
            iVec2 targetCellPosition;
            Cell targetCell;
            gridController.attemptedTumble.Add(this);
            
            if (direction > 0)
            {
                Debug.Log("Tumble up.");
                grid.lastSwipeDirection = 1;
                this.selected = false;
                targetCellPosition = new iVec2(this.cell.position.x, this.cell.position.y + 1);
            }
            else
            {
                Debug.Log("Tumble down.");
                grid.lastSwipeDirection = -1;
                this.selected = false;
                targetCellPosition = new iVec2(this.cell.position.x, this.cell.position.y - 1);
            }
            targetCell = gridController.GetCellAtPosition(targetCellPosition);
            if (targetCell)
                targetCell.tile.TumbleColumn(direction, gridController, size);
        }
        SpecialTumbleActions(gridController);
    }

    public void TumbleRow(float direction, GridController gridController, int prevSize)
    {

        if (prevSize >= size) {
            gridController.attemptedTumble.Add(this);
            iVec2 targetCellPosition;
            if (direction > 0)
            {
                Debug.Log("Tumble left.");
                this.selected = false;
                targetCellPosition = new iVec2(this.cell.position.x - 1, this.cell.position.y);

            }
            else
            {
                Debug.Log("Tumble right.");
                this.selected = false;
                targetCellPosition = new iVec2(this.cell.position.x + 1, this.cell.position.y);
            }
            Cell targetCell = gridController.GetCellAtPosition(targetCellPosition);
            if (targetCell)
                targetCell.tile.TumbleRow(direction, gridController, size);
        }
    }

    public void Swap(float direction, GridController gridController)
    {
        iVec2 targetCellPosition;
        if (direction > 0)
        {
            Debug.Log("Swap left.");
            this.selected = false;
            targetCellPosition = new iVec2(this.cell.position.x - 1, this.cell.position.y);
        }
        else
        {
            Debug.Log("Swap right.");
            this.selected = false;
            targetCellPosition = new iVec2(this.cell.position.x + 1, this.cell.position.y);
        }
        Cell targetCell = gridController.GetCellAtPosition(targetCellPosition);
        if (targetCell)
        {
            BaseTile targetTileSwap = targetCell.tile;
            Cell thisCell = this.cell;

            if (this.size == 7 && targetTileSwap.size == 7) {
                gridController.BlackTileMerged();
                targetTileSwap.Destroy(1);
                Destroy(1);
            } else { 
                targetCell.tile = this;
                thisCell.tile = targetTileSwap;
                this.SetCell(targetTileSwap.cell);
                targetTileSwap.SetCell(thisCell);
            }

            RootController.Instance.GameManager().turnsUsed++;
            gridController.MoveUpWhiteTiles();
        }
    }

    public void JumpUp(GridController gridController) {
        iVec2 targetCellPosition = new iVec2(this.cell.position.x, this.cell.position.y + 1);

        Cell targetCell = gridController.GetCellAtPosition(targetCellPosition);
        if (targetCell)
        {
            if (this.cell.transform.tag != "CellRow0" || !canBeDestroyed) { 
                BaseTile targetTileSwap = targetCell.tile;
                Cell thisCell = this.cell;

                targetCell.tile = this;
                thisCell.tile = targetTileSwap;
                this.SetCell(targetTileSwap.cell);
                targetTileSwap.SetCell(thisCell);

                canBeDestroyed = true;
            }
        }
    }

    public void Destroy(int count)
    {
        cell.tile = null;
        cell.RemoveIce();
        cell = null;
        selected = false;

        falling = true;
        fallWaitTime = count * 3f - 1f;
        fallWaitCounter = 0;
        Destroy(this.gameObject, count * Constants.destroyDelay);
    }
}
