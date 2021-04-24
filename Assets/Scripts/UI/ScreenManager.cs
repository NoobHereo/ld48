using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace game.UI
{
    public class ScreenManager : MonoBehaviour
    {
        public Button PlayButton, CreditsButton, HelpButton, QuitButton;

        private void Start()
        {
            PlayButton.onClick.AddListener(onPlayClick);
            CreditsButton.onClick.AddListener(onCreditsClick);
            HelpButton.onClick.AddListener(onHelpClick);
            QuitButton.onClick.AddListener(onQuitClick);
        }

        private void onPlayClick()
        {
            SceneManager.LoadScene(1);
        }

        private void onCreditsClick()
        {

        }

        private void onHelpClick()
        {

        }

        private void onQuitClick()
        {
            Application.Quit();
        }
    }
}