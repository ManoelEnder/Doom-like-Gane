using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [Header("Configurações de Vida")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth;

    [Header("Interface")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private GameObject gameOverPanel; // Arraste um painel de Game Over aqui

    [Header("Áudio")]
    [SerializeField] private AudioSource musicSource;

    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateUI();

        if (musicSource == null) musicSource = GetComponent<AudioSource>();
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        UpdateUI();

        // Opcional: Tocar som de dor ou efeito de sangue na tela
        Debug.Log("Player recebeu dano! Vida atual: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void UpdateUI()
    {
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth/maxHealth;
        }
    }

    void Die()
    {
        isDead = true;
        Debug.Log("Player morreu!");

        if (musicSource != null)
        {
            musicSource.Stop();
        }

        // Ativa a tela de Game Over
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        // Para o tempo do jogo
        Time.timeScale = 0f;

        // Libera o mouse para clicar nos botões do menu
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}