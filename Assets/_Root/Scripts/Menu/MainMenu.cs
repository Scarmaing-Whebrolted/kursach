using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace LastCard
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField]
        private GameSettings settings;
        
        [SerializeField]
        private Button playButton;

        [SerializeField]
        private Button exitButton;

        [SerializeField]
        private Slider botsSlider;

        [SerializeField]
        private Slider cardsSlider;

        [SerializeField]
        private InputField pointsField;

        private void Update() 
        {
            if (Input.GetKey("escape") && GameMaster.GM.GameStarted)
            {
                SceneManager.LoadScene("Game");
            }
        }

        public void PlayGame()
        {
            if (settings.MaximalScore >= 3)
            {
                GameMaster.GM.GameStarted = true;
                SceneManager.LoadScene("Game");
            }
            else
            {
                pointsField.text = "3";
            }
        }

        public void ExitGame()
        {
            Application.Quit();
        }

        public void AssignBotsCount()
        {
            settings.BotsCount = (int)botsSlider.value;
        }

        public void AssignCardsCount()
        {
            settings.InitialCardsCount = (int)cardsSlider.value;
        }

        public void AssignPointsCount()
        {
            settings.MaximalScore = int.Parse(pointsField.text);
        }
    }
}
