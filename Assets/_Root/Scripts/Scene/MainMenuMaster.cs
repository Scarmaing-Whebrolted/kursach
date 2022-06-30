using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace LastCard
{
    public class MainMenuMaster : MonoBehaviour
    {
        public static MainMenuMaster mainMenuMaster;
        public int BotsCount = 1;
        public int InitialCardsCount = 4;
        public int MaximalPointsCount = 20;
        public bool GameIsLoading { get; set; } = false;
        public GameLoader loader;   

        [SerializeField]
        private Slider botsSlider;

        [SerializeField]
        private Slider cardsSlider;

        [SerializeField]
        private InputField pointsField;

        private bool gameIsBegun = false;

        private void Awake() 
        {
            if (mainMenuMaster == null)
            {
                mainMenuMaster = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Update() 
        {
            if (gameIsBegun && Input.GetKeyDown("escape"))
            {
                SceneManager.LoadScene("Game");
            }
        }

        public void StartGame()
        {
            if (MaximalPointsCount >= 3)
            {
                gameIsBegun = true;
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

        public void SetBotsCount()
        {
            BotsCount = (int)botsSlider.value;
        }

        public void SetCardsCount()
        {
            InitialCardsCount = (int)cardsSlider.value;
        }

        public void SetMaximalPointsCount()
        {
            MaximalPointsCount = int.Parse(pointsField.text);
        }
    }
}
