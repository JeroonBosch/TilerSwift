using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RenderMoves : MonoBehaviour {
    public GameObject movesText;
	
	// Update is called once per frame
	void Update () {
        movesText.GetComponent<Text>().text = "Moves: " + RootController.Instance.GameManager().turnsUsed + "/" + RootController.Instance.GameManager().turnsAllowed;
    }
}
