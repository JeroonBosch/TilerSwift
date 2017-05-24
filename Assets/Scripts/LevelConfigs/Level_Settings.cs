using UnityEngine;

public abstract class Level_Settings : MonoBehaviour
{
    protected int _x_size; //rows
    public int x_size { get { return _x_size; } set { _x_size = value; } }

    protected int _y_size; //columns
    public int y_size { get { return _y_size; } set { _y_size = value; } }

    protected int _amountOfGoals;
    public int amountOfGoals { get { return _amountOfGoals; } set { _amountOfGoals = value; } }

    protected int _turnsAllowed;
    public int turnsAllowed { get { return _turnsAllowed; } set { _turnsAllowed = value; } }

    protected Goal[] goals;
    public Goal[] goalArray { get { return goals; } }

    protected iVec2[] iceCells;
    public iVec2[] iceCellArray { get { return iceCells; } }

    protected bool _hasSpecialTileSetup;
    public bool hasSpecialTileSetup { get { return _hasSpecialTileSetup; } }

    protected virtual void Awake()
    {
        SetData();
        goals = new Goal[_amountOfGoals];
        iceCells = new iVec2[0];
    }

    protected virtual void SetData()
    {
        _x_size = 5; //rows
        _y_size = 5; //columns
        _amountOfGoals = 1;
        _turnsAllowed = 10;
        _hasSpecialTileSetup = false;
    }
}
