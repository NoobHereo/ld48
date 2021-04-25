using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

namespace game.UI
{
    public class DeathScreenManager : MonoBehaviour
    {
        //============= COMPONENTS =============//
        public TextMeshProUGUI ScoreText, LevelText, KillsText, TimeText;
        public Button ReturnButton;

        private void Start()
        {
            ReturnButton.onClick.AddListener(onReturnClick);
        }

        public void SetScreenVariables()
        {

        }

        private void onReturnClick()
        {
            SceneManager.LoadScene(0);
        }
    }
}