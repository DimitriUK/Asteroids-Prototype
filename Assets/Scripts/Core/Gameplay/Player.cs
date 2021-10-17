using System.Collections;
using UnityEngine;

namespace Core.Gameplay
{
    public class Player : MonoBehaviour
    {
        public bool CanSpawn;
        public bool IsControllable { get; set; }

        [HideInInspector] public Gun playerGun;

        [SerializeField] private MeshRenderer[] playerSkins = null;

        private BoxCollider playerCollider;
        private Rigidbody rigidBody;

        private WaitForSeconds playerRespawnTime = new WaitForSeconds(0.5f);
        private WaitForSeconds playerProtectionBlinkTime = new WaitForSeconds(0.25f);
        private Vector3 getPlayerDefaultSpawnPosition => Vector3.zero;

        private int spawnProtectionCounter = 0;

        private const float MAX_ENGINE_SPEED = 10;
        private const float MAX_TORQUE_SPEED = 20;

        private const int SPAWN_PROTECTION_COUNTER_TARGET = 5;

        public void SetupPlayerFirstTime()
        {
            DisableControls();
            DisablePlayerPhysics();
            SetPlayerSkin(false);
        }

        public void EnableControls()
        {
            IsControllable = true;
        }

        public void DisableControls()
        {
            IsControllable = false;
        }

        public void ActivatePlayer()
        {
            ResetSpawnProtectionCounter();
            ToggleControls(true);
            RespawnPlayer();
        }

        public void DeactivatePlayer()
        {
            ToggleControls(false);
            DisablePlayerPhysics();
            LoseLive();
            SetPlayerSkin(false);
            InitializePlayerConditions();
        }

        public void ActivateRespawnShield()
        {
            playerGun.DisableGun();
            ResetSpawnProtectionCounter();
            SetupRespawnProtection();
        }

        public void MoveandTurnShip(float verticalAxisValue, float horizontalAxisValue)
        {
            if (!IsControllable)
                return;

            MoveShip(verticalAxisValue); TurnShip(horizontalAxisValue);
        }

        private void Awake()
        {
            playerGun = GetComponent<Gun>();
            playerCollider = GetComponent<BoxCollider>();
            rigidBody = GetComponent<Rigidbody>();
            InitializePlayerConditions();
        }

        private void InitializePlayerConditions()
        {
            DisablePlayerPhysics();
            StartCoroutine(SetupPlayerConditionsTimer());
        }

        private void SetupRespawnProtection()
        {
            DisablePlayerPhysics();
            StartCoroutine(StartRespawnProtectionTimer());
        }

        private IEnumerator SetupPlayerConditionsTimer()
        {
            yield return playerRespawnTime;

            if (!CanSpawn)
                yield break;

            ActivatePlayer();
            ActivateRespawnShield();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Asteroid")
            {
                DeactivatePlayer();
            }
        }

        private void ToggleControls(bool isControlsOn)
        {
            if (isControlsOn)
                EnableControls();
            else DisableControls();
        }

        private void EnablePlayerPhysics()
        {
            playerCollider.enabled = true;
        }

        private void DisablePlayerPhysics()
        {
            playerCollider.enabled = false;
        }

        private void SetPlayerSkin(bool isSkinOn)
        {
            for (int i = 0; i < playerSkins.Length; i++)
            {
                playerSkins[i].enabled = isSkinOn;
            }
        }

        private void LoseLive()
        {
            Actions.OnLiveLost?.Invoke();
        }

        private void RespawnPlayer()
        {
            transform.position = getPlayerDefaultSpawnPosition;
        }

        private void MoveShip(float verticalAxisValue)
        {
            rigidBody.AddForce(transform.forward * MAX_ENGINE_SPEED * verticalAxisValue);
        }

        private void TurnShip(float horizontalAxisValue)
        {
            rigidBody.AddTorque(transform.up * MAX_TORQUE_SPEED * horizontalAxisValue);
        }

        private void ResetSpawnProtectionCounter()
        {
            spawnProtectionCounter = 0;
        }

        private IEnumerator StartRespawnProtectionTimer()
        {
            spawnProtectionCounter++;

            SetPlayerSkin(false);

            yield return playerProtectionBlinkTime;

            SetPlayerSkin(true);

            yield return playerProtectionBlinkTime;

            if (spawnProtectionCounter == SPAWN_PROTECTION_COUNTER_TARGET)
            {
                EnablePlayerPhysics();
                playerGun.EnableGun();
                yield break;
            }

            StartCoroutine(StartRespawnProtectionTimer());
        }
    }
}
