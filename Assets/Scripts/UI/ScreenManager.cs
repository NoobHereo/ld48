using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace game.UI
{
    public class ScreenManager : MonoBehaviour
    {
        public Button PlayButton;
        public Button CreditsButton;
        public Button HelpButton;
        public Button QuitButton;

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