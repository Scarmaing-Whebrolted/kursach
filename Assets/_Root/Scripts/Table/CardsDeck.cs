namespace LastCard
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using System.Linq;

    public class CardsDeck : MonoBehaviour
    {
        [SerializeField]
        private int maxCardsAmount = 52;

        [SerializeField]
        private List<Card> cardsPrefabs;

        private List<Card> cards;

        public int CardsLeft => cards.Count;

        private void Awake()
        {
            ShuffleCards(cardsPrefabs);
            MainMenuMaster menu = MainMenuMaster.mainMenuMaster;
            
            if (menu.GameIsLoading)
            {
                cards = CreateCards(menu.loader.Data.Deck.cards);
            }
            else
            {
                cards = CreateCards();
            }
        }

        private List<Card> CreateCards()
        {
            List<Card> result = new List<Card>();
            
            for (var i = 0; i < maxCardsAmount; i++)
            {
                Card newCard = Instantiate(cardsPrefabs[i], transform);
                newCard.flipper.Flip();
                result.Add(newCard);
            }
            
            return result;
        }

        private List<Card> CreateCards(List<Card> loadedCards)
        {
            List<Card> result = new List<Card>();
            
            for (var i = 0; i < loadedCards.Count; i++)
            {
                Card newCard = Instantiate(loadedCards[i], transform);
                newCard.flipper.Flip();
                result.Add(newCard);
            }
            
            return result;
        }

        private void ShuffleCards(List<Card> cardsList)
        {
            System.Random random = new System.Random();

            for (var i = 0; i < cardsList.Count; i++)
            {
                int randomIndex = random.Next(0, cardsList.Count - 1);
                Card card = cardsList[i];
                cardsList[i] = cardsList[randomIndex];
                cardsList[randomIndex] = card;
            }
        }

        public bool ContainsCard(Card card)
        {
            return cards.Contains(card);
        }

        public List<Card> GetCards(int amount)
        {
            List<Card> result = new List<Card>();

            while ((cards.Count != 0) && (amount != result.Count))
            {
                Card newCard = cards[cards.Count - 1];
                result.Add(newCard);
                cards.Remove(newCard);
            }

            return result;
        }

        public Card GetCard()
        {
            if (cards.Count != 0)
            {
                Card result = cards[cards.Count - 1];
                cards.Remove(result);

                return result;
            }

            return null;
        }
    }
}
