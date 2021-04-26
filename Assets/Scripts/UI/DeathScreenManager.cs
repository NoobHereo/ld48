using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

namespace game.UI
{

    public class DeathScreenManager : MonoBehaviour
    {
        //============= COMPONENTS =============//
        public TextMeshProUGUI ScoreText, LevelText, KillsText, ItemsText, TimeText;
        public Button ReturnButton;

        //============= PROPERTIES =============//
        private GameData gameData;

        private void Start()
        {
            StatDataManager.Singleton.OnPlayerDeath();
            ReturnButton.onClick.AddListener(onReturnClick);
            gameData = StatDataManager.Singleton.GetGameData();
            SetScreenVariables(gameData);
        }

        public void SetScreenVariables(GameData data)
        {
            ScoreText.text = "Score: " + data.score.ToString();
            LevelText.text = "Levels: " + data.level.ToString();
            KillsText.text = "Kills: " + data.kills.ToString();
            ItemsText.text = "Collected Items: " + data.items.ToString();
        }     

        private void onReturnClick()
        {
            SceneManager.LoadScene(0);
        }
    }
}