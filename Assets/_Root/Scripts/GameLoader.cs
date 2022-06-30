using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace LastCard
{
    public class GameLoader : MonoBehaviour
    {
        public SaveData Data { get; set; }
        
        public void Load()
        {
            if (File.Exists($"{Application.persistentDataPath}/LastCardGameSave.dat"))
            {
                using (Stream output = File.Open($"{Application.persistentDataPath}/LastCardGameSave.dat", FileMode.Open))
                {
                    BinaryFormatter fm = new BinaryFormatter();
                    Data = (SaveData)fm.Deserialize(output);
                    SetInitialParameters(Data);
                    MainMenuMaster.mainMenuMaster.GameIsLoading = true;

                    Debug.Log("Data has been loaded");
                }
            }
            else
            {
                Debug.Log("File isn't found");
            }
        }  

        private void SetInitialParameters(SaveData data)
        {
            MainMenuMaster menu = MainMenuMaster.mainMenuMaster;

            if (menu.GameIsLoading)
            {
                menu.BotsCount = data.BotsCount;
                menu.InitialCardsCount = data.InitialCardsCount;
                menu.MaximalPointsCount = data.MaximalPointsCount;
            }
        }
    }
}
