using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool canSpawn;

    private PlayerInput playerInput;
    private BoxCollider playerCollider;
    private Vector3 GetPlayerDefaultSpawnPos => Vector3.zero;

    [SerializeField] private MeshRenderer[] PlayerSkins;

    WaitForSeconds playerRespawnTime = new WaitForSeconds(0.5f);
    WaitForSeconds playerProtectionBlinkTime = new WaitForSeconds(0.25f);

    private void Awake()
    {
        playerCollider = GetComponent<BoxCollider>();
        playerInput = GetComponent<PlayerInput>();

        InitializePlayerConditions();
    }

    private void InitializePlayerConditions()
    {
        DisablePlayerPhysics();
        StartCoroutine(SetupPlayerConditionsTimer());
    }

    private IEnumerator SetupPlayerConditionsTimer()
    {
        yield return playerRespawnTime;

        if (!canSpawn)
            yield break;

        ActivatePlayer();
        SetupRespawnProtectionTimer();
    }

    int spawnProtectionCounter = 0;
    private const int SPAWN_PROTECTION_COUNTER_TARGET = 5;

    private void ResetSpawnProtectionCounter()
    {
        spawnProtectionCounter = 0;
    }
    private void SetupRespawnProtectionTimer()
    {
        StartCoroutine(StartRespawnProtectionTimer());
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
            yield break;
        }

        StartCoroutine(StartRespawnProtectionTimer());
    }

    public void ActivatePlayer()
    {
        ResetSpawnProtectionCounter();
        ToggleControls(true);
        RespawnPlayer();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Asteroid")
        {
            ActivateDeath();
        }
    }

    private void ActivateDeath()
    {
        DeactivatePlayer();
    }

    public void DeactivatePlayer()
    {
        ToggleControls(false);
        DisablePlayerPhysics();
        LoseLive();
        SetPlayerSkin(false);
        InitializePlayerConditions();
    }
    private void ToggleControls(bool isControlsOn)
    {
        if (isControlsOn)
            playerInput.EnableControls();
        else playerInput.DisableControls();
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
        for (int i = 0; i < PlayerSkins.Length; i++)
        {
            PlayerSkins[i].enabled = isSkinOn;
        }
    }
    private void LoseLive()
    {
        Actions.OnLiveLost?.Invoke();
    }

    private void RespawnPlayer()
    {
        transform.position = GetPlayerDefaultSpawnPos;
    }     
}
