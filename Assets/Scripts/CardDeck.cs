
using System;
using System.Collections.Generic;
using UnityEngine;

public class CardDeck : MonoBehaviour
{
    public Stack<Card> Deck { get; set; } = new Stack<Card>();
    
    public CardDeck()
    {
        int suitCount = 4;
        int rankCount = 13;

        for (var i = 1; i <= rankCount; i++)
        {
            for (var j = 1; j <= suitCount; j++)
            {
                Deck.Push(new Card(i, GetSuit(j)));
            }
        }

        int firstJoker = 14;
        int secondJoker = 15;

        Deck.Push(new Card(firstJoker,"NONE"));
        Deck.Push(new Card(secondJoker,"NONE"));
    }

    public string GetSuit(int i)
    {
        switch (i)
        {
            case 1:
                return "DIAMONDS";
            case 2:
                return "HEARTS";
            case 3:
                return "SPADES";
            case 4:
                return "CLUBS";
            default:
                return "None";
        }
        
    }

    public void ShuffleCards()
    {
        int totalCardsCount = 52;
        Card[] cardArray = new Card[totalCardsCount];
        System.Random rand = new System.Random();

        for (var i = 1; i <= totalCardsCount; i++)
        {
            int randomCard = rand.Next(52);

            while (cardArray[randomCard] != null)
            {
                randomCard = rand.Next(totalCardsCount);
            }

            cardArray[randomCard] = Deck.Pop();
        }

        for (var i = 0; i < totalCardsCount; i++)
        {
            Deck.Push(cardArray[i]);
        }
    }
}