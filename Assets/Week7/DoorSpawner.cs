using UnityEngine;

namespace Week7
{
    public class DoorSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject doorPrefab;
        [SerializeField] private Transform spawnPoint;
        private bool spawned = false;

        private void Start()
        {
            ScoreManager.Instance.OnScoreGoalReached += SpawnDoor;
        }

        private void SpawnDoor()
        {
            if (spawned) return;

            Instantiate(doorPrefab, spawnPoint.position, spawnPoint.rotation);
            spawned = true;
        }
    }
}

