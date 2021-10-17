using UnityEngine;

namespace Core.Audio
{
    public class Sounds : MonoBehaviour
    {
        public AudioData BulletsAudioData;
        public AudioData ExplosionAudioData;

        private AudioService audioService;

        private AudioClip[] BulletSoundsToPlay;
        private AudioClip[] ExplosionSoundsToPlay;

        public void ShootGun()
        {
            PlaySound(GetRandomBulletSound());
        }

        public void SetAsteroidDestroyedSoundAndPlay(Asteroids.AsteroidSize asteroid)
        {
            switch (asteroid)
            {
                case Asteroids.AsteroidSize.AsteroidBig:
                    PlaySound(ExplosionSoundsToPlay[0]);
                    break;
                case Asteroids.AsteroidSize.AsteroidMedium:
                    PlaySound(ExplosionSoundsToPlay[1]);
                    break;
                case Asteroids.AsteroidSize.AsteroidSmall:
                    PlaySound(ExplosionSoundsToPlay[2]);
                    break;
            }
        }

        public AudioClip GetRandomBulletSound()
        {
            var random = Random.Range(0, BulletSoundsToPlay.Length);
            return BulletSoundsToPlay[random];
        }

        private void Awake()
        {
            audioService = transform.parent.GetComponent<AudioService>();
        }

        private void OnValidate()
        {
            ExplosionSoundsToPlay = ExplosionAudioData.ClipsToPlay;
            BulletSoundsToPlay = BulletsAudioData.ClipsToPlay;
        }

        private void PlaySound(AudioClip clipToPlay)
        {
            audioService.soundsAudioSource.PlayOneShot(clipToPlay);
        }
    }
}
