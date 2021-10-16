using UnityEngine;

namespace Core.Utils
{
    public class ObjectSpawner : MonoBehaviour
    {
        private ObjectBoundaries ObjectBoundaries;

        private void Awake()
        {
            ObjectBoundaries = GetComponent<ObjectBoundaries>();
        }

        private void OnEnable()
        {
            InitializeSpawn();
        }

        private void InitializeSpawn()
        {
            SpawnObjectAtDestination(GetScreenSize());
        }

        private Vector2 GetScreenSize()
        {
            Vector2 screenPos = ObjectBoundaries.GetScreenPosition();

            var screenWidth = screenPos.x;
            var screenHeight = screenPos.y;

            float randomScreenWidthPos = Random.Range(-screenWidth / 60, screenWidth / 60);
            float randomScreenHeightPos = Random.Range(-screenHeight / 60, screenHeight / 60);        

            Vector2 posToSpawn = new Vector2(randomScreenWidthPos, randomScreenHeightPos);

            return posToSpawn;
        }

        private void SpawnObjectAtDestination(Vector2 spawnPos)
        {
            transform.position = spawnPos;
        }
    }
}
