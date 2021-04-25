using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

namespace game.UI
{
    public struct TimeFormat
    {
        public float time;
        public string unit;
    }

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

            TimeFormat timeFormat = GetTimeFormat(data.time);
            TimeText.text = "Time Spent: " + timeFormat.time.ToString() + ", " + timeFormat.unit;
        }

        private TimeFormat GetTimeFormat(float elapsedMS)
        {
            float timeSec = elapsedMS / 1000; // Seconds
            float timeMin;
            float timeHr;
            float timeDd;

            float time = 0;
            string unit = "milliseconds";

            if (timeSec > 60)
            {
                timeMin = timeSec / 60;
                time = timeMin;
                unit = "Minutes";
                if (timeMin > 60)
                {
                    timeHr = timeMin / 60;
                    time = timeHr;
                    unit = "Hours";
                    if (timeHr > 24)
                    {
                        timeDd = timeHr / 24;
                        time = timeDd;
                        unit = "Days";
                    }
                }
            }

            TimeFormat format = new TimeFormat()
            {
                time = time,
                unit = unit
            };

            return format;
        }

        private void onReturnClick()
        {
            SceneManager.LoadScene(0);
        }
    }
}