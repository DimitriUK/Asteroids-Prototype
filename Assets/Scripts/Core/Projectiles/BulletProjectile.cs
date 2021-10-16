using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BulletProjectile : MonoBehaviour, IDamageable, IPooledProjectile
{
    [SerializeField] private Rigidbody rigidbody;

    [SerializeField] private float desiredSpeed;
    [SerializeField] private float maxSpeed;

    private void Awake()
    {
        InitializeProjectile();
    }

    private void InitializeProjectile()
    {
        rigidbody = GetComponent<Rigidbody>();
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

        rigidbody.velocity = Vector3.ClampMagnitude(rigidbody.velocity, maxSpeed);
        rigidbody.AddRelativeForce(Vector3.forward * desiredSpeed, ForceMode.Impulse);
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
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
    }
}