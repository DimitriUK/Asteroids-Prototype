using System.Collections;
using UnityEngine;

namespace Core.Audio
{
    public class Music : MonoBehaviour
    {
        public bool IsMusicOn;

        public AudioData MusicAudioData;

        private AudioClip[] soundsToPlay;
        private AudioService audioService;

        private float musicTimerDelay;

        private int currentAsteroidsLeft = 0;
        private int initialAsteroidsAmount = 0;

        private const float MUSIC_TIME_DELAY_DEFAULT = 1.5f;

        public void StartMusicForNewGame(AudioSource musicAudioSource)
        {
            StartCoroutine(StartMusicTimer(musicAudioSource));
            IsMusicOn = true;
        }

        public void SetupMusicTimeDelay(int amountOfAsteroids)
        {
            ResetMusic();
            currentAsteroidsLeft = amountOfAsteroids;
            initialAsteroidsAmount = amountOfAsteroids;
            musicTimerDelay = CalculateSubtraction();
        }

        public IEnumerator StartMusicTimer(AudioSource musicAudioSource)
        {
            if (!musicAudioSource.isPlaying)
                musicAudioSource.PlayOneShot(soundsToPlay[0]);

            yield return new WaitForSeconds(musicTimerDelay);

            if (!musicAudioSource.isPlaying)
                musicAudioSource.PlayOneShot(soundsToPlay[1]);

            yield return new WaitForSeconds(musicTimerDelay);

            StartCoroutine(StartMusicTimer(musicAudioSource));
        }

        public void IncreaseMusicSpeed(Asteroids.AsteroidSize asteroidSize)
        {
            currentAsteroidsLeft--;
            musicTimerDelay = CalculateSubtraction();
        }
        public void StopMusic()
        {
            IsMusicOn = false;
            ResetMusic();
        }

        private void Awake()
        {
            audioService = transform.parent.GetComponent<AudioService>();
        }

        private void OnValidate()
        {
            soundsToPlay = MusicAudioData.ClipsToPlay;
        }

        private float CalculateSubtraction()
        {
            var result = MUSIC_TIME_DELAY_DEFAULT * currentAsteroidsLeft / initialAsteroidsAmount + 0.5f;
            return result;
        }

        private void ResetMusic()
        {
            StopAllCoroutines();
        }
    }
}
