using UnityEngine;

namespace game
{
    public class SoundManager : MonoBehaviour
    {
        public AudioSource SFXSource;
        public AudioSource MusicSource;

        private void Start()
        {
            SFXSource.playOnAwake = false;
            SFXSource.loop = false;
        }

        public void PlaySFX(AudioClip sfx)
        {
            Debug.Log("Playing sound: " + sfx.name);
            SFXSource.clip = sfx;
            SFXSource.Play();
        }

        public void PauseMusic()
        {
            MusicSource.Pause();
        }

        public void ResumeMusic()
        {
            MusicSource.Play();
        }
    }
}