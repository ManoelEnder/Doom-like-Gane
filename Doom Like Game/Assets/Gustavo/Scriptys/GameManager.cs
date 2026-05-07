using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI killText;
    [SerializeField] private GameObject winPanel; // Arraste o WinPanel aqui

    [Header("Controle de Horda")]
    public int enemiesAlive = 0;
    public int maxEnemiesAllowed = 10;
    public int resumeSpawnAt = 5;
    private bool isPausedByLimit = false;

    [Header("Progresso")]
    [SerializeField] private int killsToSpawnBoss = 10;
    [SerializeField] private int currentKills = 0;
    private bool bossSpawned = false;
    private bool gameWon = false;

    [Header("Boss")]
    [SerializeField] private GameObject[] bossPrefabs;
    [SerializeField] private Transform bossSpawnPoint;

    void Awake() { Instance = this; }

    public void AddKill()
    {
        if (gameWon) return;

        currentKills++;
        enemiesAlive--;
        UpdateUI();

        if (isPausedByLimit && enemiesAlive <= resumeSpawnAt)
        {
            isPausedByLimit = false;
        }

        if (currentKills >= killsToSpawnBoss && !bossSpawned)
        {
            SpawnBoss();
        }
    }

    // NOVA FUNÇÃO: Chamada quando o Boss morre
    public void BossDefeated()
    {
        gameWon = true;
        if (winPanel != null) winPanel.SetActive(true);

        // Para o jogo e libera o mouse
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public bool CanSpawn()
    {
        if (bossSpawned || gameWon) return false; // Para de spawnar zumbis quando o boss chega ou ganha
        if (enemiesAlive >= maxEnemiesAllowed || isPausedByLimit)
        {
            isPausedByLimit = true;
            return false;
        }
        return true;
    }

    public void RegisterSpawn() { enemiesAlive++; }

    void UpdateUI()
    {
        if (killText != null)
            killText.text = "KILLS: " + currentKills + " / " + killsToSpawnBoss;
    }

    void SpawnBoss()
    {
        bossSpawned = true;
        if (killText != null) killText.text = "DERROTE O BOSS!";

        if (bossPrefabs.Length > 0 && bossSpawnPoint != null)
        {
            int randomIndex = Random.Range(0, bossPrefabs.Length);
            // Criamos o Boss
            Instantiate(bossPrefabs[randomIndex], bossSpawnPoint.position, bossSpawnPoint.rotation);
        }
    }
}