using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;
using System.Linq;

//using TMPro;

namespace LastCard
{
    public class GameMaster : MonoBehaviour
    {
        public static GameMaster GM;

        [SerializeField]
        private GameDirector director;

        public string Winner;
        public List<string> RunnerUpps;
        public GameSaver saver;

        private void Awake() 
        {
            director.BotsCount = MainMenuMaster.mainMenuMaster.BotsCount;
            director.InitialCardsCount = MainMenuMaster.mainMenuMaster.InitialCardsCount;
            director.MaximalPointsCount = MainMenuMaster.mainMenuMaster.MaximalPointsCount;

            if (GM == null)
            {
                GM = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void Save()
        {
            saver.Save(director);
        }

        public void SetWinner(Player player)
        {
            Winner = $"Player {player.name} - {player.GetPointsNumber()} Win!";
        }

        public void EndGame()
        {
            SceneManager.LoadScene("GameEnding");
        }

        public void SetRunnerUpps(List<Player> runnerUpps)
        {
            RunnerUpps = runnerUpps.Select(runnerUp => $"{runnerUp.name} - {runnerUp.GetPointsNumber()} points")
                .ToList();
        }
    }
}
