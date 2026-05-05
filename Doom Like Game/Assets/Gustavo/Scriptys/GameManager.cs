using UnityEngine;
using TMPro; // Importante para controlar o texto

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI killText;

    [Header("Configurações de Progresso")]
    [SerializeField] private int killsToSpawnBoss = 10;
    [SerializeField] private int currentKills = 0;

    [Header("Boss")]
    // Mudamos para uma Array para aceitar várias opções de Boss
    [SerializeField] private GameObject[] bossPrefabs;
    [SerializeField] private Transform bossSpawnPoint;
    private bool bossSpawned = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        UpdateUI();
    }

    public void AddKill()
    {
        currentKills++;
        UpdateUI();

        if (currentKills >= killsToSpawnBoss && !bossSpawned)
        {
            SpawnBoss();
        }
    }

    void UpdateUI()
    {
        if (killText != null)
        {
            killText.text = "KILLS: " + currentKills + " / " + killsToSpawnBoss;
        }
    }

    void SpawnBoss()
    {
        bossSpawned = true;

        if (killText != null) killText.text = "O BOSS CHEGOU!";

        // Verifica se existem prefabs na lista e se tem um ponto de spawn
        if (bossPrefabs.Length > 0 && bossSpawnPoint != null)
        {
            // Escolhe um índice aleatório entre 0 e o tamanho da lista
            int randomIndex = Random.Range(0, bossPrefabs.Length);

            // Instancia o boss sorteado
            Instantiate(bossPrefabs[randomIndex], bossSpawnPoint.position, bossSpawnPoint.rotation);

            Debug.Log("Boss variante " + randomIndex + " spawnado!");
        }
        else
        {
            Debug.LogError("ERRO: Liste os prefabs do Boss no GameManager!");
        }
    }
}