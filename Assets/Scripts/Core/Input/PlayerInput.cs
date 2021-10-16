using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private PlayerCombat playerCombat;

    private Rigidbody rigidBody;

    public bool IsControllable { get; set; }

    private const float MAX_ENGINE_SPEED = 10;
    private const float MAX_TORQUE_SPEED = 20;

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        InitializeStartingPlayerConditions();
        AssignComponents();
    }

    private void InitializeStartingPlayerConditions()
    {
        IsControllable = false;
    }

    private void AssignComponents()
    {
        rigidBody = GetComponent<Rigidbody>();
        playerCombat = GetComponent<PlayerCombat>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
            FireProjectile();
    }

    private void FixedUpdate()
    {
        if (Input.anyKey)
            MoveandTurnShip();
    }

    private void MoveandTurnShip()
    {
        if (!IsControllable)
            return;

        MoveShip(); TurnShip();
    }

    private float GetHorizontalAxisValue()
    {
        var horizontal = Input.GetAxis("Horizontal");
        var result = horizontal;

        return result;
    }

    private float GetVerticalAxisValue()
    {
        var vertical = Input.GetAxis("Vertical");
        var result = vertical;

        if (result < 0)
            return 0;

        return vertical;
    }

    private void MoveShip()
    {
        rigidBody.AddForce(transform.forward * MAX_ENGINE_SPEED * GetVerticalAxisValue());
    }
    private void TurnShip()
    {
        rigidBody.AddTorque(transform.up * MAX_TORQUE_SPEED * GetHorizontalAxisValue());
    }
    private void FireProjectile()
    {
        if (!IsControllable)
            return;

        playerCombat.FireProjectile();
    }

    public void EnableControls()
    {
        IsControllable = true;
    }
    public void DisableControls()
    {
        IsControllable = false;
    }

}