using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RenderGoals : MonoBehaviour {
    public GameObject goalText;

    private GameObject[] texts;

	public void CreateGoals (Goal[] goals)
    {
        texts = new GameObject[goals.Length];
        RectTransform rt = transform.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(rt.sizeDelta.x, 25 * goals.Length);
        rt.localPosition = new Vector3(rt.localPosition.x, rt.localPosition.y - 25f * (goals.Length - 1), rt.localPosition.z);

        for (int i = 0; i < goals.Length; i++)
        {
            GameObject goalGO = Instantiate(goalText);
            texts[i] = goalGO;
            goalGO.transform.SetParent(this.transform, false);
            goalGO.transform.localPosition = new Vector3(140f, (rt.sizeDelta.y - 12.5f) - i * 25f, 0);
        }
	}

    public void UpdateGoals(Goal[] goals)
    {
        for (int i = 0; i < goals.Length; i++)
        {
            texts[i].GetComponent<Text>().text = goals[i].GetText();

            if (goals[i].currentAmount >= goals[i].requiredAmount)
                texts[i].GetComponent<Text>().color = Color.green;
        }
    }
}
