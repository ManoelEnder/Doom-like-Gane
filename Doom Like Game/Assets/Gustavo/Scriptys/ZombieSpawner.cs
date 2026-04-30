using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    public GameObject zombiePrefab;
    public float spawnInterval = 5f;
    public int maxZombiesFromThisSpawner = 10; // Limite por ponto de spawn

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
        // Instancia o zumbi exatamente na posição do Spawner
        Instantiate(zombiePrefab, transform.position, transform.rotation);
        currentZombiesCreated++;

        // Opcional: Adicione um efeito de partículas aqui
        Debug.Log("Zumbi saindo do chão em: " + gameObject.name);
    }
}