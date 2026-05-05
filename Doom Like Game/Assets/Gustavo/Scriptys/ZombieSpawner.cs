using UnityEngine;
using System.Collections.Generic;

public class ZombieSpawner : MonoBehaviour
{
    [Header("Lista de Variações")]
    [SerializeField] private List<GameObject> zombiePrefabs; // Arraste todas as versões do boss/zumbi para cá

    [SerializeField] private float spawnInterval = 5f;
    [SerializeField] private int maxZombiesFromThisSpawner = 10;

    private int currentZombiesCreated = 0;
    private float timer;

    void Update()
    {
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
        if (zombiePrefabs.Count > 0)
        {
            // Escolhe um índice aleatório da lista
            int randomIndex = Random.Range(0, zombiePrefabs.Count);

            // Instancia a variação escolhida
            Instantiate(zombiePrefabs[randomIndex], transform.position, transform.rotation);

            currentZombiesCreated++;
        }
    }
}