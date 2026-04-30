using UnityEngine;
using TMPro; // Importante para controlar o texto

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI killText; // Arraste o seu texto aqui no Inspector

    [Header("Configurações de Progresso")]
    [SerializeField] private int killsToSpawnBoss = 10;
    [SerializeField] private int currentKills = 0;

    [Header("Boss")]
    [SerializeField] private GameObject bossPrefab;
    [SerializeField] private Transform bossSpawnPoint;
    private bool bossSpawned = false;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        UpdateUI(); // Começa com 0 na tela
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
            // Atualiza o texto na tela
            killText.text = "KILLS: " + currentKills + " / " + killsToSpawnBoss;
        }
    }

    void SpawnBoss()
    {
        bossSpawned = true;
        // Mensagem especial na tela quando o boss chega
        if (killText != null) killText.text = "O BOSS CHEGOU!";

        if (bossPrefab != null && bossSpawnPoint != null)
        {
            Instantiate(bossPrefab, bossSpawnPoint.position, bossSpawnPoint.rotation);
        }
    }
}