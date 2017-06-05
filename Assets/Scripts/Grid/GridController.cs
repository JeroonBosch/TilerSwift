using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour {
    public GameObject cellTemplate;
    public GameObject tileTemplate;
    public GameObject cageTileTemplate;

    //private default_config config;
    private Level_Settings config;
    protected List<Cell>[] cellColumns;
    public List<BaseTile> attemptedTumble = new List<BaseTile>();

    private bool interactable = true;
    public int lastSwipeDirection = -1;

    private bool requiresSwap = false;
    private int swapTimer = 0;

    private BaseTile activeTile;
    public Vector3 cursor = new Vector3();
    public Vector3 lastCursorPos = new Vector3();

    public bool destructionLockOut = false;
    private float destructionLockOutTime = 0;
    private int destructionLockOutCounter = 0;

    public bool movementLockOut = false;


    private bool goMad = false;

    void Start() {
        config = RootController.Instance.GetConfigFile();
        GenerateGrid();
        GenerateTiles(true);
        for (int i = 0; i < config.goalArray.Length; i++)
        {
            config.goalArray[i].UpdateText();
        }

    }

    void Update()
    {
        float enter = 0;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //Vector3 mousePosition = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        //Ray ray = Camera.main.ScreenPointToRay(Camera.main.projectionMatrix.inverse.MultiplyPoint(mousePosition));
        //Ray ray = Camera.main.ViewportPointToRay(Input.mousePosition);
        cursor = ray.GetPoint(enter);

        MovementLockout();
        if (destructionLockOut || movementLockOut)
            interactable = false;
        else
            interactable = true;

        if (interactable)
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    if (hit.collider.gameObject.tag == "InteractableTile")
                    {
                        BaseTile tile = hit.collider.transform.parent.parent.gameObject.GetComponent<BaseTile>();
                        if (tile)
                        {
                            Debug.Log("tile at " + tile.GetCell().position.x + " | " + tile.GetCell().position.y);
                            tile.selected = true;
                            activeTile = tile;
                            attemptedTumble.Clear();
                        }
                    } else if (hit.collider.gameObject.tag == "SuperRowButton") {
                        Debug.Log("superrowbutton clicked");
                        Transform cellTransform = hit.collider.transform.parent;
                        string tag = cellTransform.tag;
                        List<Cell> rowCells = GetRowCells(ConvertTagToRowNumber(tag));

                        foreach (Cell cell in rowCells)
                            cell.tile.Destroy(1);

                        for (int w = 0; w < config.goalArray.Length; w++)
                        {
                            if (config.goalArray[w] is SuperRow_Goal)
                            {
                                SuperRow_Goal goal = config.goalArray[w] as SuperRow_Goal;
                                goal.currentAmount++;
                                goal.UpdateText();
                            }
                        }

                        Destroy(hit.collider.gameObject);
                        Debug.Log("superrowbutton handled");
                    }
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                if (activeTile != null)
                {
                    activeTile.selected = false;
                    activeTile = null;
                }
            }

            if (activeTile != null)
            {
                float cursorDeltaZ = (cursor.z - lastCursorPos.z);
                float cursorDeltaX = (cursor.x - lastCursorPos.x);

                activeTile.TurnTowards(cursorDeltaZ, -cursorDeltaX);

                if (Mathf.Abs(activeTile.rotationPivot.localRotation.y) > Constants.flickThresholdHorizontal)
                {
                    //activeTile.TumbleRow(activeTile.rotationPivot.localRotation.y, this, 999);
                    activeTile.Swap(activeTile.rotationPivot.localRotation.y, this);
                    activeTile = null;
                }
                else if (Mathf.Abs(activeTile.rotationPivot.localRotation.x) > Constants.flickThresholdVertical)
                {
                    activeTile.TumbleColumn(activeTile.rotationPivot.localRotation.x, this, activeTile.size);
                    activeTile = null;
                }
            }
        }

        if (goMad)
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), Random.Range(-10f, 10f)), 10f);
    }

    void FixedUpdate()
    {
        if (destructionLockOut)
        {
            if (destructionLockOutCounter < destructionLockOutTime)
            {
                destructionLockOutCounter++;
            }
            else
            {
                destructionLockOutTime = 0;
                destructionLockOutCounter = 0;
                destructionLockOut = false;
            }
        }
        MovementLockout();
        if (requiresSwap && !destructionLockOut && !movementLockOut) {
            if (swapTimer < 15f)
                swapTimer++;
            else {
                swapTimer = 0;
                MoveUpWhiteTiles();
            }
        }
            
    }

    void LateUpdate()
    {
        lastCursorPos = cursor;

        if (attemptedTumble.Count > 2) {
            RootController.Instance.GameManager().turnsUsed++;
            requiresSwap = true;
            int count = 0;
            for (int i = 0; i < config.goalArray.Length; i++)
            {
                config.goalArray[i].Condition(attemptedTumble);
                config.goalArray[i].UpdateText();
            }

            foreach (BaseTile tile in attemptedTumble)
            {
                tile.Destroy(count);
                count++;
            }
            destructionLockOutTime = 20f + count * Constants.destroyDelay * 60f;
            destructionLockOut = true;
            attemptedTumble.Clear();
        }

        bool needsMoving = true;
        ShiftDown(needsMoving);

        GenerateTiles(false);
        CheckForSuperRow();
    }

    void ShiftDown (bool needsMoving)
    {
        needsMoving = false;
        foreach (List<Cell> column in cellColumns)
        {
            foreach (Cell cell in column)
            {
                if (cell.MoveDownCheck())
                {
                    needsMoving = true;
                    cell.MoveDown();
                }   
            }
        }

        if (needsMoving == true)
            ShiftDown(needsMoving);
    }

    void MovementLockout ()
    {
        bool isMoving = false;
        foreach (List<Cell> column in cellColumns)
        {
            foreach (Cell cell in column)
            {
                if (cell.tile.moving)
                {
                    isMoving = true;
                }
            }
        }
        movementLockOut = isMoving;
    }

    #region grid
    void GenerateGrid()
    {
        Debug.Log("Columns: " + config.x_size + " | Rows: " + config.y_size);
        cellColumns = new List<Cell>[config.x_size];
        for (int x = 0; x < cellColumns.Length; x++)
        {
            cellColumns[x] = new List<Cell>();
            GameObject columnObject = new GameObject();
            columnObject.transform.SetParent(gameObject.transform, false);
            columnObject.name = "Column" + x;
            for (int y = 0; y < config.y_size; y++)
            {
                Cell newCell = GenerateCell(new iVec2(x, y), columnObject);
                cellColumns[x].Add(newCell);
            }
        }
    }

    Cell GenerateCell(iVec2 pos, GameObject columnObject)
    {
        GameObject cell = Instantiate(cellTemplate);
        Cell cellScript = cell.GetComponent<Cell>();
        cell.transform.SetParent(columnObject.transform);
        cell.transform.localPosition = new Vector3(pos.x, pos.y, 0);
        cell.transform.localScale = new Vector3(1f, 1f, 1f);
        cell.transform.localRotation = new Quaternion(0, 0, 0, 0);
        cell.name = pos.x + " | " + pos.y;
        cell.tag = ConvertRowNumberToTag(pos.y);

        bool isIce = false;
        if (pos.y == 0 || pos.y == 1 || pos.y == 2 || pos.y == 3)
            isIce = true;
        cellScript.InitCell(pos, this, isIce);

        return cellScript;
    }

    public Cell GetCellAtPosition(iVec2 pos)
    {
        Cell returnCell = null;
        if ((pos.x < config.x_size && pos.x >= 0) && (pos.y < config.y_size && pos.y >= 0))
        {
            List<Cell> cellColumn = cellColumns[pos.x];
            returnCell = cellColumn[pos.y];
        }
        return returnCell;
    }
    #endregion

    #region tiles
    void GenerateTiles(bool firstTime)
    {
        if (!firstTime || !config.hasSpecialTileSetup) { 
            foreach (List<Cell> column in cellColumns)
            {
                foreach (Cell cell in column)
                {
                    if (cell.tile == null)
                        CreateTile(cell);
                }
            }
        }
        else {
            int amountOfCagedTiles = 0;
            int i = 0;
            for (i = 0; i < config.goalArray.Length; i++)
            {
                if (config.goalArray[i] is BringDownCagedWhite_Goal)
                {
                    BringDownCagedWhite_Goal goal = config.goalArray[i] as BringDownCagedWhite_Goal;
                    amountOfCagedTiles = goal.requiredAmount;
                }
            }

            List<int> randomColumnList = new List<int>();
            for (i = 0; i < config.x_size; i++)
                randomColumnList.Add(i);
            int remove = config.x_size - amountOfCagedTiles;
            for (i = 0; i < remove; i++) { 
                int random = Random.Range(0, config.x_size -i);
                randomColumnList.RemoveAt(random);
            }

            int columnCount = 0;
            foreach (List<Cell> column in cellColumns)
            {
                bool allowCagedTile = true;
                if (!randomColumnList.Contains(columnCount))
                    allowCagedTile = false;

                int randomRow = Random.Range(0, config.y_size);
                int rowCount = 0;
                foreach (Cell cell in column)
                {
                    if (cell.tile == null && allowCagedTile && randomRow == rowCount)
                        CreateCageTile(cell);
                    else if (cell.tile == null)
                        CreateTile(cell);

                    rowCount++;
                }
                columnCount++;
            }
        }
    }

    void CreateTile(Cell cell)
    {
        GameObject tile = Instantiate(tileTemplate);
        BaseTile baseTile = tile.GetComponent<BaseTile>();
        baseTile.InitTile(cell, Random.Range(1, Constants.amountOfColors + 1), this);
    }

    void CreateCageTile(Cell cell)
    {
        GameObject tile = Instantiate(cageTileTemplate);
        CagedTile baseTile = tile.GetComponent<CagedTile>();
        baseTile.InitTile(cell, 6, this);
    }

    public bool CheckForWhiteTiles(Cell cell)
    {
        Transform column = cell.transform.parent;
        foreach (Transform child in column) {
            if (child.GetComponent<Cell>()) {
                Cell checkCell = child.GetComponent<Cell>();
                if (checkCell.tile) {
                    if (checkCell.tile.size == 6)
                        return false;
                }
            }
        }

        for (int i = 0; i < config.goalArray.Length; i++)
        {
            if (config.goalArray[i] is BringDownWhite_Goal) {
                BringDownWhite_Goal goal = config.goalArray[i] as BringDownWhite_Goal;
                if (goal.currentAmount < goal.requiredAmount && goal.whiteTilesSpawned < goal.requiredAmount)
                {
                    int turnsUsed = RootController.Instance.GameManager().turnsUsed;
                    if (goal.whiteTilesSpawned == 0 && turnsUsed >= 1)
                        return true;
                    else if (goal.whiteTilesSpawned >= 1 && turnsUsed >= (5* goal.whiteTilesSpawned))
                        return true;
                }
            }
        }
        
        return false;
    }

    public void WhiteTileAdded()
    {
        for (int i = 0; i < config.goalArray.Length; i++)
        {
            if (config.goalArray[i] is BringDownWhite_Goal)
            {
                BringDownWhite_Goal goal = config.goalArray[i] as BringDownWhite_Goal;
                goal.whiteTilesSpawned++;
            }
        } 
    }

    public bool CheckForBlackTiles(Cell cell)
    {
        Transform column = cell.transform.parent;
        foreach (Transform child in column)
        {
            if (child.GetComponent<Cell>())
            {
                Cell checkCell = child.GetComponent<Cell>();
                if (checkCell.tile)
                {
                    if (checkCell.tile.size == 7)
                        return false;
                }
            }
        }

        for (int i = 0; i < config.goalArray.Length; i++)
        {
            if (config.goalArray[i] is MergeBlackTiles_Goal)
            {
                MergeBlackTiles_Goal goal = config.goalArray[i] as MergeBlackTiles_Goal;
                if (goal.currentAmount < goal.requiredAmount && goal.blackTilesSpawned < goal.requiredAmount)
                {
                    int turnsUsed = RootController.Instance.GameManager().turnsUsed;
                    if (goal.blackTilesSpawned == 0 && turnsUsed >= 1)
                        return true;
                    else if (goal.blackTilesSpawned >= 1 && turnsUsed >= (5 * goal.blackTilesSpawned))
                        return true;
                }
            }
        }

        return false;
    }

    public void BlackTileAdded()
    {
        for (int i = 0; i < config.goalArray.Length; i++)
        {
            if (config.goalArray[i] is MergeBlackTiles_Goal)
            {
                MergeBlackTiles_Goal goal = config.goalArray[i] as MergeBlackTiles_Goal;
                goal.blackTilesSpawned++;
            }
        }
    }

    public void BlackTileMerged()
    {
        for (int i = 0; i < config.goalArray.Length; i++)
        {
            if (config.goalArray[i] is MergeBlackTiles_Goal)
            {
                MergeBlackTiles_Goal goal = config.goalArray[i] as MergeBlackTiles_Goal;
                goal.currentAmount++;
                goal.currentAmount++;
            }
        }
    }


    public void MoveUpWhiteTiles()
    {
        requiresSwap = false;
        bool hasWhiteTilesGoal = false;
        BringDownCagedWhite_Goal goal = null;
        for (int i = 0; i < config.goalArray.Length; i++)
        {
            if (config.goalArray[i] is BringDownWhite_Goal)
            {
                hasWhiteTilesGoal = true;
            }
            else if (config.goalArray[i] is BringDownCagedWhite_Goal)
            {
                hasWhiteTilesGoal = true;
                goal = config.goalArray[i] as BringDownCagedWhite_Goal;
            }
        }
        bool isCaged = false;

        if (hasWhiteTilesGoal) {
            BaseTile[] tiles = GameObject.FindObjectsOfType<BaseTile>();
            foreach (BaseTile tile in tiles) {
                if (goal)
                    isCaged = goal.CheckForCage(tile);

                if (tile.size == 6 && !isCaged)
                    tile.JumpUp(this);
            }
        }
    }

    List<Cell> GetRowCells(int rowNumber) {
        List<Cell> row = new List<Cell>();

        foreach (List<Cell> column in cellColumns)
        {
            foreach (Cell cell in column)
            {
                string requiredTag = ConvertRowNumberToTag(rowNumber);

                if (requiredTag == cell.tag)
                    row.Add(cell);
            }
        }

        return row;
    }

    int ConvertTagToRowNumber(string tag) {
        int rowNumber = 0;

        switch (tag)
        {
            case "CellRow0":
                rowNumber = 0;
                break;
            case "CellRow1":
                rowNumber = 1;
                break;
            case "CellRow2":
                rowNumber = 2;
                break;
            case "CellRow3":
                rowNumber = 3;
                break;
            case "CellRow4":
                rowNumber = 4;
                break;
            case "CellRow5":
                rowNumber = 5;
                break;
            case "CellRow6":
                rowNumber = 6;
                break;
            case "CellRow7":
                rowNumber = 7;
                break;
            case "CellRow8":
                rowNumber = 8;
                break;
            default:
                rowNumber = -1;
                break;
        }

        return rowNumber;
    }

    string ConvertRowNumberToTag(int rowNumber) {
        string returnTag = "";
        switch (rowNumber)
        {
            case 0:
                returnTag = "CellRow0";
                break;
            case 1:
                returnTag = "CellRow1";
                break;
            case 2:
                returnTag = "CellRow2";
                break;
            case 3:
                returnTag = "CellRow3";
                break;
            case 4:
                returnTag = "CellRow4";
                break;
            case 5:
                returnTag = "CellRow5";
                break;
            case 6:
                returnTag = "CellRow6";
                break;
            case 7:
                returnTag = "CellRow7";
                break;
            case 8:
                returnTag = "CellRow8";
                break;
            default:
                returnTag = "CellRowError";
                break;
        }

        return returnTag;
    }

    void CheckForSuperRow ()
    {
        for (int i = 0; i < config.y_size; i++) {
            List<Cell> row = GetRowCells(i);
                
            bool allSameSize = true;
            int sizes = -1;
            Cell lastCell = null;
            foreach (Cell cell in row) {
                if (sizes == -1)
                    sizes = cell.tile.size;
                else if (sizes != cell.tile.size)
                    allSameSize = false;

                lastCell = cell;
            }

            if (allSameSize && lastCell)
            {
                if (!lastCell.transform.Find("SuperRowButton"))
                {
                    GameObject button = Instantiate(Resources.Load("Prefabs/SuperRowButton")) as GameObject;
                    button.transform.parent = lastCell.transform;
                    button.transform.name = "SuperRowButton";
                    button.transform.localPosition = new Vector3(button.transform.localPosition.x, 0f, button.transform.localPosition.z);
                }
            }
            else if (!allSameSize && lastCell)
            {
                Transform superRowButton = lastCell.transform.Find("SuperRowButton");
                if (superRowButton)
                {
                    Destroy(superRowButton.gameObject);
                }
            }
        }
    }
    #endregion

    public void GoMad() {
        goMad = true;
    }
}
