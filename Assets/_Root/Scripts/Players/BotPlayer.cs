namespace LastCard
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using UnityEngine;
    using System.Linq;
    using System;
    using UnityEngine.UI;

    public class BotPlayer : Player
    {
        public Outline border;
        public Text cardsCount;

        public override void AddCards(List<Card> additionalCards)
        {
            base.AddCards(additionalCards);

            cardsCount.text = cards.Count.ToString();
        }

        public override async Task MakeTurn()
        {
            Debug.Log("Bot turn");

            if (cards.Count != 1 && !resolver.CanPushCard(GetCardToPush()))
            {
                border.enabled = true;
                Task turnDuration = Task.Delay(TimeSpan.FromSeconds(2));
                await turnDuration;
                border.enabled = false;
            }
            
            // border.enabled = true;
            // Task turnDuration = Task.Delay(TimeSpan.FromSeconds(2));
            // await turnDuration;
            // border.enabled = false;
            bool turnIsMade = false;

            List<Card> tempCards = new List<Card>(cards);

            while (tempCards.Count != 0)
            {
                Card minCard = GetMinimalCard(tempCards);
                tempCards.Remove(minCard);

                if (SendCardSelected(minCard))
                {
                    minCard.flipper.Flip();
                    RemoveCard(minCard);

                    if ((cards.Count != 0) && (minCard.nominal == Nominal.Three))
                    {
                        Card cardToPush = GetCardToPush();
                        pile.PushCard(cardToPush);
                        RemoveCard(cardToPush);
                    }

                    cardsCount.text = cards.Count.ToString();
                    turnIsMade = true;
                    await Task.CompletedTask;
                    break;
                }
            }

            if (!turnIsMade)
            {
                Debug.Log("Bot takes card");
                TakeCards();
                cardsCount.text = cards.Count.ToString();
            }

            await Task.CompletedTask;
        }

        public Card GetCardToPush()
        {
            foreach (Card card in cards)
            {
                if (resolver.CanPushCard(card))
                {
                    card.flipper.Flip();
                    
                    return card;
                }
            }

            return null;
        }

        private Card GetMinimalCard(List<Card> botCards)
        {
            return botCards.Find(card => card.nominal == botCards.Min(card => card.nominal));
        }
    }
}
