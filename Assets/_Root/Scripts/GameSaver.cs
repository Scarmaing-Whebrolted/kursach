using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;

namespace LastCard
{
    public class GameSaver : MonoBehaviour
    {
        public void Save(GameDirector director)
        {
            SaveData data = new SaveData()
            {
                InitialCardsCount = director.InitialCardsCount,
                MaximalPointsCount = director.MaximalPointsCount,
                PlayerIndex = director.PlayerIndex,
                BotsCount = director.BotsCount,
                Players = director.Players,
                Deck = director.cardsDeck,
                Pile = director.cardsPile
            };

            using (Stream input = File.Create($"{Application.persistentDataPath}/LastCardGameSave.dat"))
            {
                BinaryFormatter fm = new BinaryFormatter();
                fm.Serialize(input, data);
                Debug.Log("Data has been saved");
            }
        }        
    }

    [Serializable]
    public class SaveData
    {
        public List<Player> Players { get; set; }
        public int PlayerIndex { get; set; }
        public int InitialCardsCount { get; set; }
        public int MaximalPointsCount { get; set; }
        public int BotsCount { get; set; }
        public CardsPile Pile { get; set; }
        public CardsDeck Deck { get; set; }
    }
}
