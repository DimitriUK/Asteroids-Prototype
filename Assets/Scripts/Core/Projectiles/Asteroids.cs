using UnityEngine;

namespace Core.Gameplay
{
    public class Asteroids : MonoBehaviour, IDamageable
    {
        public AsteroidService asteroid;
        private Rigidbody asteroidRigidbody;

        [SerializeField] private GameObject[] AsteroidSubGroup = null;

        private const float BIG_ASTEROID_SPEED = 400;
        private const float MED_ASTEROID_SPEED = 500;
        private const float SMALL_ASTEROID_SPEED = 600;

        public enum AsteroidSize
        {
            AsteroidBig,
            AsteroidMedium,
            AsteroidSmall
        }

        public AsteroidSize AsteroidSizeLocal;

        private void Awake()
        {
            Initialization();
        }

        private void Initialization()
        {
            asteroidRigidbody = GetComponent<Rigidbody>();
        }
        private void OnEnable()
        {
            InitializeSpeed();
        }

        private void InitializeSpeed()
        {
            asteroidRigidbody.velocity = Vector3.zero;
            var speed = ApplyAsteroidSpeed(AsteroidSizeLocal);

            Quaternion randomRotation = Random.rotation;
            randomRotation.x = 0;
            randomRotation.y = 0;

            transform.rotation = randomRotation;

            asteroidRigidbody.AddRelativeForce(transform.up * speed, ForceMode.Impulse);
        }

        private float ApplyAsteroidSpeed(AsteroidSize asteroidSize)
        {
            var speed = 0f;

            switch (asteroidSize)
            {
                case AsteroidSize.AsteroidBig:
                    speed = BIG_ASTEROID_SPEED;
                    break;

                case AsteroidSize.AsteroidMedium:
                    speed = MED_ASTEROID_SPEED;
                    break;

                case AsteroidSize.AsteroidSmall:
                    speed = SMALL_ASTEROID_SPEED;
                    break;
            }

            return speed;
        }

        public void OnHit()
        {
            Actions.OnAsteroidDestroyed?.Invoke(AsteroidSizeLocal);
            ApplyHit();
        }

        private void ApplyHit()
        {
            ActivateNextStageAsteroids(CheckNextStage());
            DisableAsteroid();
        }

        private bool CheckNextStage()
        {
            if (AsteroidSubGroup.Length != 0)
            {
                return true;
            }

            else return false;
        }

        private void ActivateNextStageAsteroids(bool isFinalAsteroid)
        {
            if (!isFinalAsteroid)
                return;

            for (int i = 0; i < AsteroidSubGroup.Length; i++)
            {
                AsteroidSubGroup[i].transform.localPosition = new Vector2(transform.localPosition.x, transform.localPosition.y);
                AsteroidSubGroup[i].SetActive(true);
            }
        }

        private void DisableAsteroid()
        {
            gameObject.SetActive(false);
        }
    }
}