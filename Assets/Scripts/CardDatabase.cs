using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDatabase : MonoBehaviour
{
    public static List<Card> Deck = new List<Card>();

    private void Awake() 
    {
        //Deck.Add(new Card())
    }
}
