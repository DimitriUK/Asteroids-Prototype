using System.Collections;
using UnityEngine;

namespace Core.Gameplay
{
    [RequireComponent(typeof(Rigidbody))]
    public class BulletProjectile : MonoBehaviour, IDamageable, IPooledProjectile
    {
        [SerializeField] private Rigidbody bulletRigidbody;

        [SerializeField] private float desiredSpeed = 0;
        [SerializeField] private float maxSpeed = 0;

        private void Awake()
        {
            InitializeProjectile();
        }

        private void InitializeProjectile()
        {
            bulletRigidbody = GetComponent<Rigidbody>();
        }

        void OnEnable()
        {
            StartDespawnTimer();
        }

        void StartDespawnTimer()
        {
            StartCoroutine(DespawnTimer());
        }
        public void DespawnProjectile()
        {
            gameObject.SetActive(false);
        }

        WaitForSeconds despawnTime = new WaitForSeconds(2);

        private IEnumerator DespawnTimer()
        {
            while (true)
            {
                yield return despawnTime;
                DespawnProjectile();
            }
        }

        private void FixedUpdate()
        {

            bulletRigidbody.velocity = Vector3.ClampMagnitude(bulletRigidbody.velocity, maxSpeed);
            bulletRigidbody.AddRelativeForce(Vector3.forward * desiredSpeed, ForceMode.Impulse);
        }

        private void OnTriggerEnter(Collider other)
        {
            IDamageable damageable = other.GetComponent<IDamageable>();

            if (damageable == null)
                return;

            damageable.OnHit();

            OnHit();
        }

        public void OnHit()
        {
            gameObject.SetActive(false);
        }

        public void OnProjectileSpawn()
        {
            bulletRigidbody.velocity = Vector3.zero;
            bulletRigidbody.angularVelocity = Vector3.zero;
        }
    }
}