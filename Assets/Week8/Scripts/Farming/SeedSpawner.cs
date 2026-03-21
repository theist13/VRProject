using UnityEngine;

public class SeedSpawner : MonoBehaviour
{
    [SerializeField] private SeedItem seedPrefab;
    [SerializeField] private Transform spawnPoint;

    [Header("Limit Settings")]
    [SerializeField] private float spawnCooldown = 10f;
    [SerializeField] private int maxSeedInScene = 1;

    private float lastSpawnTime = -999f;

    public void SpawnSeed()
    {
        // 1. เช็ค cooldown
        if (Time.time < lastSpawnTime + spawnCooldown)
        {
            Debug.Log("ยังไม่ถึงเวลา spawn");
            return;
        }

        // 2. เช็คจำนวนเมล็ดในฉาก
        int currentSeedCount = FindObjectsOfType<SeedItem>().Length;

        if (currentSeedCount >= maxSeedInScene)
        {
            Debug.Log("เมล็ดเต็มฉากแล้ว ไม่ spawn เพิ่ม");
            return;
        }

        // 3. Spawn
        Instantiate(seedPrefab, spawnPoint.position, spawnPoint.rotation);
        lastSpawnTime = Time.time;

        Debug.Log("Spawn seed สำเร็จ");
    }
}