using UnityEngine;

namespace Core.Audio
{
    public class AudioService : MonoBehaviour
    {
        public Music music;
        public Sounds sounds;

        [HideInInspector] public AudioSource musicAudioSource;
        [HideInInspector] public AudioSource soundsAudioSource;

        public void StartMusic()
        {
            music.StartMusicForNewGame(musicAudioSource);
        }

        private void Awake()
        {
            Initialise();
        }

        private void Initialise()
        {
            musicAudioSource = music.GetComponent<AudioSource>();
            soundsAudioSource = sounds.GetComponent<AudioSource>();
        }

        private void OnEnable()
        {
            Actions.OnAsteroidDestroyed += music.IncreaseMusicSpeed;
            Actions.OnAsteroidDestroyed += sounds.SetAsteroidDestroyedSoundAndPlay;
            Actions.OnBulletFired += sounds.ShootGun;
        }

        private void OnDisable()
        {
            Actions.OnAsteroidDestroyed -= music.IncreaseMusicSpeed;
            Actions.OnAsteroidDestroyed -= sounds.SetAsteroidDestroyedSoundAndPlay;
            Actions.OnBulletFired -= sounds.ShootGun;
        }
    }
}
