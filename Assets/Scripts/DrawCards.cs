using System.Collections;
using System.Collections.Generic;
using UnityEngine; 

public class DrawCards : MonoBehaviour
{
    public GameObject Card1;
    public GameObject Card2;
    public GameObject AllyField;
    public GameObject EnemyField;
    public GameObject DropZone;

    List<GameObject> cards = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        cards.Add(Card1);
        cards.Add(Card2);
    }

    public void PlaceFiveCardsBothSides()
    {
        if (AllyField.transform.childCount != 5 && EnemyField.transform.childCount != 5)
        {
            for (var i = 0; i < 5; i++)
            {
                GameObject allyCard = Instantiate(cards[Random.Range(0, cards.Count)],
                    new Vector3(0, 0, 0), Quaternion.identity);
                allyCard.transform.SetParent(AllyField.transform, false);
            }

            for (var i = 0; i < 5; i++)
            {
                GameObject enemyCard = Instantiate(cards[Random.Range(0, cards.Count)],
                    new Vector3(0, 0, 0), Quaternion.identity);
                enemyCard.transform.SetParent(EnemyField.transform, false);
                enemyCard.GetComponent<CardFlipper>().Flip();
            }
        }
    }

    public void ClearCards()
    {
        AllyField.transform.DetachChildren();
        EnemyField.transform.DetachChildren();
        DropZone.transform.DetachChildren();
    }
}
