using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections; // Necessário para a Corrotina

public class PlayerHealth : MonoBehaviour
{
    [Header("Configurações de Vida")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth;

    [Header("Interface")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private GameObject gameOverPanel;

    [Header("Áudio")]
    [SerializeField] private AudioSource musicSource;

    [Header("Efeito de Sangue (Novo)")]
    [SerializeField] private Image bloodOverlay; // Arraste a imagem de sangue aqui
    [SerializeField] private float fadeDuration = 0.5f; // Tempo para o sangue sumir
    private Coroutine bloodCoroutine;

    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateUI();

        if (musicSource == null) musicSource = GetComponent<AudioSource>();

        // Garante que o sangue comece invisível
        if (bloodOverlay != null)
        {
            Color c = bloodOverlay.color;
            c.a = 0f;
            bloodOverlay.color = c;
        }
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        UpdateUI();

        // --- ADICIONADO: DISPARA O EFEITO DE SANGUE ---
        if (bloodOverlay != null)
        {
            if (bloodCoroutine != null) StopCoroutine(bloodCoroutine);
            bloodCoroutine = StartCoroutine(FadeBlood());
        }

        Debug.Log("Player recebeu dano! Vida atual: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // --- ADICIONADO: CORROTINA DO EFEITO VISUAL ---
    IEnumerator FadeBlood()
    {
        Color c = bloodOverlay.color;
        c.a = 0.7f; // Intensidade do sangue ao levar dano
        bloodOverlay.color = c;

        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            c.a = Mathf.Lerp(0.7f, 0f, elapsed / fadeDuration);
            bloodOverlay.color = c;
            yield return null;
        }
    }

    void UpdateUI()
    {
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth / maxHealth;
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

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}