using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour {
    private iVec2 _position;
    private BaseTile _tile;
    private GridController _grid;

    private bool _isIce;
    public bool isIce { get { return _isIce; } }
    private Material _originalMaterial;
    public Material iceMaterial;

    public void InitCell (iVec2 position, GridController grid, bool isIce)
    {
        _originalMaterial = gameObject.GetComponent<MeshRenderer>().material;
        _position = position;
        _tile = null;
        _grid = grid;
        _isIce = isIce;
        if (isIce)
            gameObject.GetComponent<MeshRenderer>().material = iceMaterial;
    }

    public iVec2 position
    {
        get
        {
             return _position;
        }
        set
        {
                _position = value;
        }
    }

    public BaseTile tile
    {
        get
        {
            return _tile;
        }
        set
        {
            _tile = value;
        }
    }

    public bool MoveDownCheck()
    {
        bool belowIsEmpty = false;

        Cell belowCell = _grid.GetCellAtPosition(new iVec2(_position.x, _position.y - 1));
        if (belowCell == null || this.tile == null) {
            return false;
        }
        if (belowCell.tile == null)
        {
            belowIsEmpty = true;
        }

        return belowIsEmpty;
    }

    public void MoveDown()
    {
        if (this.tile != null) {
            this.tile.SetCell(_grid.GetCellAtPosition(new iVec2(_position.x, _position.y - 1)));
            this.tile = null;
        }
    }

    public void DestroyTileBeneath()
    {
        iVec2 targetCellPosition;

        targetCellPosition = new iVec2(this.position.x, this.position.y - 1);
        Cell targetCell = _grid.GetCellAtPosition(targetCellPosition);
        if (targetCell)
        {
            targetCell.tile.Destroy(1);
        }
    }

    public void RemoveIce()
    {
        if (_isIce) {
            _isIce = false;
            gameObject.GetComponent<MeshRenderer>().material = _originalMaterial;
        }
    }
}
