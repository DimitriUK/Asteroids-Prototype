using Core.Gameplay;
using UnityEngine;

namespace Core.Controls
{
    public class InputService : MonoBehaviour
    {
        private GameService gameService;

        private Player player;
        private Gun playerGun;

        private void Awake()
        {
            Initialise();
        }

        private void Initialise()
        {
            gameService = GetComponent<GameService>();
        }

        public void InitialisePlayer()
        {
            player = FindObjectOfType<Player>();
            playerGun = player.playerGun;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (gameService)
                    gameService.StartNewGame();
            }

            if (player == null)
                return;

            if (player.IsControllable && Input.GetButton("Fire1") && Time.time > playerGun.nextShot)
            {
                playerGun.nextShot = Time.time + playerGun.fireRate;
                FireGun();
            }
        }
        private void FireGun()
        {
            player.playerGun.CheckTriggerAndFireGun();
        }

        private void FixedUpdate()
        {
            if (player == null)
                return;

            if (Input.anyKey)
                player.MoveandTurnShip(GetVerticalAxisValue(), GetHorizontalAxisValue());
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
    }
}