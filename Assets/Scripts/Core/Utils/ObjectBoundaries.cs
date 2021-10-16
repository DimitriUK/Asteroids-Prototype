using System;
using UnityEngine;

namespace Core.Utils
{
    public class ObjectBoundaries : MonoBehaviour
    {
        private Camera mainCam;

        private const int OUT_OF_BOUNDS_LEFT_SIDE_X_OFFSET = 10;

        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            mainCam = AssignCamera();
        }

        private Camera AssignCamera()
        {
            var cam = Camera.main ? Camera.main : null;

            if (cam == null)
            {
                throw new ArgumentException("The Main Camera was not found in the scene. Set the Main Camera GameObject's tag to 'MainCamera'");
            }

            return cam;
        }

        private void FixedUpdate()
        {
            GetScreenPositionAndCheckBoundaries();
        }

        private void GetScreenPositionAndCheckBoundaries()
        {
            Vector3 screenPosition = GetScreenPosition();

            bool isOutOfBoundsOnLeftOfScreen = screenPosition.x < 0 ? true : false;
            bool isOutOfBoundsOnRightOfScreen = screenPosition.x > Screen.width ? true : false;
            bool isOutOfBoundsOnTopOfScreen = screenPosition.y < 0 ? true : false;
            bool isOutOfBoundsOnBottomOfScreen = screenPosition.y > Screen.height ? true : false;

            if (isOutOfBoundsOnLeftOfScreen) TeleportToRightOfScreen(screenPosition);
            if (isOutOfBoundsOnRightOfScreen) TeleportToLeftOfScreen(screenPosition);
            if (isOutOfBoundsOnTopOfScreen) TeleportToTopofScreeen(screenPosition);
            if (isOutOfBoundsOnBottomOfScreen) TeleportToBottomOfScreen(screenPosition);
        }

        public Vector3 GetScreenPosition()
        {
            var screenPosition = mainCam.WorldToScreenPoint(transform.position);
            return screenPosition;
        }

        void TeleportToLeftOfScreen(Vector3 screenPosition)
        {
            Vector3 targetScreenPosition = new Vector3(0, screenPosition.y, screenPosition.z);
            Vector3 targetWorldPosition = mainCam.ScreenToWorldPoint(targetScreenPosition);

            MoveObjectToTargetPosition(targetWorldPosition);
        }

        void TeleportToRightOfScreen(Vector3 screenPosition)
        {
            Vector3 targetScreenPosition = new Vector3(Screen.width - OUT_OF_BOUNDS_LEFT_SIDE_X_OFFSET, screenPosition.y, screenPosition.z);
            Vector3 targetWorldPosition = mainCam.ScreenToWorldPoint(targetScreenPosition);

            MoveObjectToTargetPosition(targetWorldPosition);
        }

        void TeleportToTopofScreeen(Vector3 screenPosition)
        {
            Vector3 targetScreenPosition = new Vector3(screenPosition.x, Screen.height, screenPosition.z);
            Vector3 targetWorldPosition = mainCam.ScreenToWorldPoint(targetScreenPosition);

            MoveObjectToTargetPosition(targetWorldPosition);
        }

        void TeleportToBottomOfScreen(Vector3 screenPosition)
        {
            Vector3 targetScreenPosition = new Vector3(screenPosition.x, Screen.height - Screen.height, screenPosition.z);
            Vector3 targetWorldPosition = mainCam.ScreenToWorldPoint(targetScreenPosition);

            MoveObjectToTargetPosition(targetWorldPosition);
        }

        private void MoveObjectToTargetPosition(Vector3 targetWorldPos)
        {
            transform.position = targetWorldPos;
        }
    }
}