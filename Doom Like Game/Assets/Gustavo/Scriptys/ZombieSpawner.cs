using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    public GameObject zombiePrefab;
    public float spawnInterval = 5f;
    public int maxZombiesFromThisSpawner = 10;

    private int currentZombiesCreated = 0;
    private float timer;

    void Update()
    {
        // Só spawna se ainda não atingiu o limite
        if (currentZombiesCreated < maxZombiesFromThisSpawner)
        {
            timer += Time.deltaTime;

            if (timer >= spawnInterval)
            {
                Spawn();
                timer = 0;
            }
        }
    }

    void Spawn()
    {
        if (zombiePrefab != null)
        {
            Instantiate(zombiePrefab, transform.position, transform.rotation);
            currentZombiesCreated++;
        }
    }
}