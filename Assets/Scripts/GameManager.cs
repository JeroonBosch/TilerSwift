using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    private Level_Settings config;
    private RenderGoals goalRenderer;

    private int _turnsAllowed = 1;
    public int turnsAllowed { get { return _turnsAllowed; } set { _turnsAllowed = value; } }
    private int _turnsUsed = 0;
    public int turnsUsed { get { return _turnsUsed; } set { _turnsUsed = value; } }

    private int _blocksDestroyed;
    public int blocksDestroyed { get { return _blocksDestroyed; } set { _blocksDestroyed = value; }}
    private int _redBlocksDestroyed;    //5
    private int _yellowBlocksDestroyed; //4
    private int _blueBlocksDestroyed;   //3
    private int _greenBlocksDestroyed;  //2
    private int _purpleBlocksDestroyed; //1

    private void Start()
    {
        LoadLevelData();
        goalRenderer = GameObject.Find("GoalsPanel").GetComponent<RenderGoals>();
        goalRenderer.CreateGoals(config.goalArray);
    }
    private void Update()
    {
        CheckForLoss();
        CheckForWin();
        if (config)
        {
            goalRenderer.UpdateGoals(config.goalArray);

            for (int i = 0; i < config.goalArray.Length; i++)
                config.goalArray[i].LevelModifierUpdate();
        }
            
    }

    private void CheckForLoss()
    {
        if (_turnsUsed > _turnsAllowed)
            RootController.Instance.Quit(); //TO-DO todo replace
    }

    private void CheckForWin()
    {
        bool win = false;
        if (config)
        {
            if (config.goalArray.Length > 0) { 
                for (int i = 0; i < config.goalArray.Length; i++) {
                    if (config.goalArray[i].currentAmount >= config.goalArray[i].requiredAmount)
                        win = true;
                    else
                    {
                        win = false;
                        return;
                    }
                }
            }
        }

        if (win)
            GameObject.Find("Grid").GetComponent<GridController>().GoMad();
    }

    public void LoadLevelData()
    {
        config = RootController.Instance.GetConfigFile();
        _turnsAllowed = config.turnsAllowed;
    }

    public void ResetLevel()
    {
        LoadLevelData();
        _turnsUsed = 0;
}
}
