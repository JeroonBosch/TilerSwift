using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileCage : MonoBehaviour {

    private Transform cageBar1;
    private Transform cageBar2;
    private Transform cageBar3;

    private int _hitpoints;
    public int hitpoints { get { return _hitpoints; } set { _hitpoints = value; Debug.Log("hitpoints now: " + _hitpoints); } }

    // Use this for initialization
    void Awake () {
        hitpoints = 3;
        foreach (Transform child in transform)
        {
            if (child.name == "CageBar1")
            {
                cageBar1 = child;
            }
            else if (child.name == "CageBar2")
            {
                cageBar2 = child;
            }
            else if (child.name == "CageBar3")
            {
                cageBar3 = child;
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (hitpoints == 0)
        {
            cageBar3.gameObject.SetActive(false);
            cageBar2.gameObject.SetActive(false);
            cageBar1.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }
        else if (hitpoints == 1) {
            cageBar2.gameObject.SetActive(false);
            cageBar1.gameObject.SetActive(false);
        }
        else if (hitpoints == 2)
        {
            cageBar1.gameObject.SetActive(false);
        }
    }
}
