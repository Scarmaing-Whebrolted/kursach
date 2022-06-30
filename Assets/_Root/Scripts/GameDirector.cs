namespace LastCard
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Logic;
    using UnityEngine;
    using System.Linq;
    using UnityEngine.SceneManagement;

    public class GameDirector : MonoBehaviour
    {
        [SerializeField]
        private RulesResolver rulesResolver;
        
        public CardsDeck cardsDeck;

        public CardsPile cardsPile;

        [SerializeField]
        private UserPlaceholder userHolder;
        
        [SerializeField]
        private List<BotPlaceholder> botHolders;

        [SerializeField]
        private BotPlayer botPrefab;

        [SerializeField]
        private UserPlayer userPrefab;

        public List<Player> Players = new List<Player>();
        public int PlayerIndex;
        private bool gameIsFinished = false;

        public int BotsCount;
        public int InitialCardsCount;
        public int MaximalPointsCount;

        // public void LoadGame(SaveData data)
        // {
        //     Players = data.Players;
        //     PlayerIndex = data.PlayerIndex;
        //     BotsCount = data.BotsCount;
        //     InitialCardsCount = data.InitialCardsCount;
        //     MaximalPointsCount = data.MaximalPointsCount;
        // }

        private void Awake() 
        {
            rulesResolver.Init(cardsPile, cardsDeck);
            cardsPile.Init(cardsDeck);
        }

        private void Start()
        {
            MainMenuMaster menu = MainMenuMaster.mainMenuMaster;

            if (MainMenuMaster.mainMenuMaster.GameIsLoading)
            {
                cardsPile = menu.loader.Data.Pile;
                cardsDeck = menu.loader.Data.Deck;
                
                SpawnPlayers();

                MainMenuMaster.mainMenuMaster.GameIsLoading = false;
            }
            else
            {
                SpawnPlayers();
                DistributeCards();
            }

            StartGame();
        }

        private void Update() 
        {
            if (Input.GetKey("escape"))
            {
                SceneManager.LoadScene("MainMenu");
            }
        }

        private void SpawnPlayers()
        {
            MainMenuMaster menu = MainMenuMaster.mainMenuMaster;

            if (menu.GameIsLoading)
            {
                UserPlayer userToInstantiate = menu.loader.Data.Players.Find(player => player is UserPlayer) 
                    as UserPlayer;
                SpawnUser(userToInstantiate);
            }
            else
            {
                SpawnUser(userPrefab);
            }

            if (menu.GameIsLoading)
            {
                List<Player> savedPlayers = menu.loader.Data.Players;
                savedPlayers.Remove(savedPlayers.Find(player => player is UserPlayer));

                SpawnBots(savedPlayers.Select(player => (BotPlayer)player).ToList());
            }
            else
            {
                SpawnBots(botPrefab);
            }

            ImprovePlayersNames();
        }

        private void SpawnUser(UserPlayer userPlayer)
        {
            UserPlayer user = userHolder.PlaceUser(userPlayer);
            user.Init(rulesResolver, cardsDeck, cardsPile);
            Players.Add(user);
        }

        private void SpawnBots(List<BotPlayer> botPrefabs)
        {
            for (var i = 0; i < MainMenuMaster.mainMenuMaster.BotsCount; i++)
            {
                BotPlayer bot = botHolders[i].PlaceBot(botPrefabs[i]);
                bot.Init(rulesResolver, cardsDeck, cardsPile);
                bot.name += $": {i + 1}";
                Players.Add(bot);
            }
        }

        private void SpawnBots(BotPlayer inputBot)
        {
            for (var i = 0; i < MainMenuMaster.mainMenuMaster.BotsCount; i++)
            {
                BotPlayer bot = botHolders[i].PlaceBot(inputBot);
                bot.Init(rulesResolver, cardsDeck, cardsPile);
                bot.name += $": {i + 1}";
                Players.Add(bot);
            }
        }

        private void ImprovePlayersNames()
        {
            string stringToDelete = "(Clone)";

            foreach (Player player in Players)
            {
                player.name = player.name.Remove(player.name.IndexOf(stringToDelete), stringToDelete.Length);
            }
        }

        private void DistributeCards()
        {
            foreach (Player player in Players)
            {
                List<Card> cards = cardsDeck.GetCards(MainMenuMaster.mainMenuMaster.InitialCardsCount);

                player.AddCards(cards);
            }

            Card pileCard = cardsDeck.GetCard();
            pileCard.flipper.Flip();
            cardsPile.PushCard(pileCard);
        }

        private async void StartGame()
        {
            PlayerIndex = GetStartPlayerIndex();
            Player player = Players[PlayerIndex];
            
            while (!gameIsFinished)
            {
                player = Players[PlayerIndex];

                if (!CheckSkippingTurn(player))
                {
                    player.OnCardSelected += OnPlayerSelectedCard;
                    player.OnCardsMissing += OnPlayerMissingCards;

                    Task turnTask = player.MakeTurn();
                    await turnTask;

                    player.OnCardSelected -= OnPlayerSelectedCard;
                    player.OnCardsMissing -= OnPlayerMissingCards;
                }

                if (!cardsPile.Reversed)
                {
                    PlayerIndex = GetNextPlayerIndex(PlayerIndex);
                }
                else
                {
                    PlayerIndex = GetNextPlayerIndexReversed(PlayerIndex);
                }

                gameIsFinished = CheckIsCompleted(player);
            }

            Debug.Log("The game is ending...");

            EndGame(GetWinner());
        }

        private void EndGame(Player winner)
        {
            GameMaster.GM.SetWinner(winner);
            Players.Remove(winner);
            ExcludeLosers();
            GameMaster.GM.SetRunnerUpps(Players);
            GameMaster.GM.EndGame();
        }

        private bool CheckIsCompleted(Player player)
        {
            Debug.Log($"Check {player} is completed - {player.GetCardsCount()}");

            if (player.GetCardsCount() == 0)
            {
                Debug.Log("Player's cards ended up");

                return true;
            }
            else if ((cardsDeck.CardsLeft == 0) && NobodyCanMakeTurn())
            {
                Debug.Log("Nobody can make turn");

                return true;
            }

            return false;
        }

        private void ExcludeLosers()
        {
            List<Player> tempPlayers = new List<Player>(Players);
            bool loserCanBeRemoved = true;

            while (loserCanBeRemoved)
            {
                loserCanBeRemoved = false;
                Player playerToRemove = null;

                foreach (Player player in tempPlayers)
                {
                    if (player.GetPointsNumber() > MainMenuMaster.mainMenuMaster.MaximalPointsCount)
                    {
                        playerToRemove = player;
                        loserCanBeRemoved = true;
                        break;
                    }
                }

                if (loserCanBeRemoved)
                {
                    Players.Remove(playerToRemove);
                }
            }
        }

        private bool NobodyCanMakeTurn()
        {
            foreach (Player player in Players)
            {
                if (!player.DontTurn)
                {
                    return false;
                }               
            }

            return true;
        }

        private Player GetWinner()
        {
            Player winner = Players.FirstOrDefault();

            for (var i = 1; i < Players.Count; i++)
            {
                if (Players[i].GetPointsNumber() < winner.GetPointsNumber())
                {
                    winner = Players[i];
                }
                else if (Players[i].GetPointsNumber() == winner.GetPointsNumber())
                {
                    return null;
                }
            }

            return winner;
        }

        private bool CheckSkippingTurn(Player player)
        {
            if (cardsPile.SkipTurn)
            {
                cardsPile.SkipTurn = false;
                Card cardFromDeck = cardsDeck.GetCard();

                if (cardFromDeck != null)
                {
                    player.AddCards(new List<Card>() { cardFromDeck });
                }

                return true;
            }            
            else if (!player.CanMakeTurn)
            {
                player.CanMakeTurn = true;

                return true;
            }
            
            return false;
        }

        private bool OnPlayerSelectedCard(Player player, Card card)
        {
            if (!rulesResolver.CanPushCard(card))
            {
                return false;
            }
            
            if (card.nominal == Nominal.Jack)
            {
                Players[GetNextPlayerIndex(PlayerIndex)].CanMakeTurn = false;
            }

            cardsPile.PushCard(card);
            player.DontTurn = false;

            return true;
        }

        private void OnPlayerMissingCards(Player player)
        {
            Card lastPileCard = cardsPile.PeekCard();

            if (cardsPile.IsIncrementing && !player.
                ContainsCard(card => (card.nominal == lastPileCard.nominal + 1) && (lastPileCard.suit == card.suit)))
            {
                Debug.Log($"{player.name} doesn't have any card to increment pile!");

                for (var i = 0; i < (int)lastPileCard.nominal && (cardsDeck.CardsLeft != 0); i++)
                {
                    player.AddCards(new List<Card>() { cardsDeck.GetCard() });
                    
                    Debug.Log($"{player.name} takes cards");
                }

                cardsPile.IsIncrementing = false;
            }
            else if (cardsDeck.CardsLeft != 0)
            {
                Debug.Log($"{player.name} takes cards");
                player.AddCards(new List<Card>() { cardsDeck.GetCard() });

                return;
            }

            player.DontTurn = true;
        }

        private int GetStartPlayerIndex()
        {
            System.Random random = new System.Random();

            return random.Next(0, Players.Count - 1);
        }

        private int GetNextPlayerIndex(int index)
        {
            return (index + 1) % Players.Count;
        }

        private int GetNextPlayerIndexReversed(int index)
        {
            return (index - 1 + Players.Count) % Players.Count;
        }
    }
}
