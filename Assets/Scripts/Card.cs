using UnityEngine;

public class Card : MonoBehaviour
{
    public int Rank;
    public string Name;
    public string Suit;
    public string Colour;
    
    public Card(int rank, string suit)
    {
        Rank = rank;
        Suit = suit;

        switch (rank)
        {
            case 1:
                Name = "A";
                break;
            case 11:
                Name = "J";
                break;
            case 12:
                Name = "K";
                break;
            case 13:
                Name = "Q";
                break;
            case 14:
                Name = "JOKER";
                Colour = "RED";
                break;
            case 15:
                Name = "JOKER";
                Colour = "BLACK";
                break;
            default:
                Name = rank.ToString();
                break;
        }

        if (Suit == "HEARTS" || Suit == "DIAMONDS")
        {
            Colour = "RED";
        }
        else
        {
            if (Suit == "CLUBS" || Suit == "SPADES")
            {
                Colour = "BLACK";
            }
        }
    }
}