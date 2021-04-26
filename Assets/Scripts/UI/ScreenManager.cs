using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

namespace game.UI
{
    public class ScreenManager : MonoBehaviour
    {
        //============= COMPONENTS =============//
        public Button PlayButton, CreditsButton, HelpButton, QuitButton;
        public Animation PlayerFallAnimation, BackgroundAnimation, PlayerWalkAnimation;
        public GameObject Logo;
        public AudioClip PlayerHitGroundSFX;
        public AudioClip PlayerMoveSFX;
        private SoundManager soundManager;

        private void Start()
        {
            soundManager = GameObject.FindGameObjectWithTag("SoundManager").gameObject.GetComponent<SoundManager>();
            PlayButton.onClick.AddListener(onPlayClick);
            CreditsButton.onClick.AddListener(onCreditsClick);
            HelpButton.onClick.AddListener(onHelpClick);
            QuitButton.onClick.AddListener(onQuitClick);
        }

        private void onPlayClick()
        {
            PlayButton.gameObject.SetActive(false);
            CreditsButton.gameObject.SetActive(false);
            HelpButton.gameObject.SetActive(false);
            QuitButton.gameObject.SetActive(false);
            Logo.SetActive(false);
            soundManager.PlaySFX(PlayerHitGroundSFX);
            StartCoroutine(startFallAnim());
        }

        private IEnumerator startFallAnim()
        {
            yield return new WaitForEndOfFrame();

            PlayerWalkAnimation.clip = PlayerWalkAnimation.GetClip("PlayerStartFallAnimation");
            PlayerWalkAnimation.Play();
            StartCoroutine(startBackgroundAnimation());
        }

        private IEnumerator startBackgroundAnimation()
        {
            yield return new WaitForSeconds(3f);
            BackgroundAnimation.clip = BackgroundAnimation.GetClip("TitleviewFadeOut");
            BackgroundAnimation.Play();
            StartCoroutine(startPlayerWalkAnimation());
        }

        private IEnumerator startPlayerWalkAnimation()
        {
            soundManager.PlaySFX(PlayerMoveSFX);
            PlayerWalkAnimation.clip = PlayerWalkAnimation.GetClip("PlayerWalkAnimation");
            PlayerWalkAnimation.Play();
            yield return new WaitForSeconds(2f);
            SceneManager.LoadScene(1);
        }

        private void onCreditsClick()
        {
            // TODO: Add something here.
        }

        private void onHelpClick()
        {
            // TODO: Add something here.
        }

        private void onQuitClick()
        {
            Application.Quit();
        }
    }
}