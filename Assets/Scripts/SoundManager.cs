using UnityEngine;

namespace game
{
    public class SoundManager : MonoBehaviour
    {
        public AudioSource SFXSource;

        private void Start()
        {
            SFXSource = GetComponent<AudioSource>();
            SFXSource.playOnAwake = false;
            SFXSource.loop = false;
        }

        public void PlaySFX(AudioClip sfx)
        {
            Debug.Log("Playing sound: " + sfx.name);
            SFXSource.clip = sfx;
            SFXSource.Play();
        }
    }
}