namespace LastCard
{
    using System.Collections.Generic;
    using UnityEngine;
    using System.Linq;
    using System;

    public class CardsPile : MonoBehaviour
    {
        [SerializeField]
        private Transform cardsHolder;

        private List<Card> cards = new List<Card>();
        private CardsDeck deck;

        public bool IsIncrementing = false;
        public bool HasAliasThree { get; set; } = false;
        public bool SkipTurn { get; set; } = false;
        public bool Reversed { get; set; } = false;

        private void Awake() 
        {
            MainMenuMaster menu = MainMenuMaster.mainMenuMaster;

            if (menu.GameIsLoading)
            {
                AddCards(menu.loader.Data.Pile.cards);
            }
        }

        public void Init(CardsDeck cardsDeck)
        {
            deck = cardsDeck;
        }
        
        public Card PeekCard()
        {
            if (cards.Count - 1 < 0)
            {
                return null;
            }

            return cards.Last();
        }

        private void AddCards(List<Card> cardsToAdd)
        {
            for (var i = 0; i < cardsToAdd.Count; i++)
            {
                Card newCard = Instantiate(cardsToAdd[i], transform);
                newCard.flipper.Flip();
                cards.Add(newCard);
            }
        }

        public void PushCard(Card card)
        {
            Debug.Log(card.name);

            if (HasAliasThree)
            {
                HasAliasThree = false;
            }

            switch (card.nominal)
            {
                case Nominal.Four:
                    IsIncrementing = true;
                    break;
                case Nominal.Two:
                    SkipTurn = true;
                    break;
                case Nominal.Three:
                    HasAliasThree = true;
                    break;
                case Nominal.Ace:
                    Reversed = !Reversed;
                    break;
                case Nominal.Ten:
                    IsIncrementing = false;
                    break;
                default:
                    break;
            }

            cards.Add(card);
            card.transform.SetParent(cardsHolder.transform, false);
        }
    }
}
