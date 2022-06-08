using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageCards : MonoBehaviour
{
    public GameObject card;

    void Start() 
    {
        DisplayCards();
    }

    public void DisplayCards()
    {
        for (var i = 0; i < 5; i++)
        {
            AddCard();
        }
    }

    private void AddCard(int rank = 0)
    {
        GameObject c = Instantiate(card, new Vector3(0, 0, 0), Quaternion.identity);
        c.transform.SetParent(GameObject.Find("AllyField").transform, false);
    }
}
