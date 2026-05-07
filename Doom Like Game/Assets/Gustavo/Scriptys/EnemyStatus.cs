using UnityEngine;

public class EnemyStatus : MonoBehaviour, IShootable
{
    [SerializeField] private bool isDead = false;
    [SerializeField] private GameObject _bloodEffect;
    [SerializeField] private float _lifeMax = 2;
    private float _currentLife;

    [Header("Sons de Feedback")]
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _hitSound;   // Som de quando leva o tiro
    [SerializeField] private AudioClip _deathSound; // Som de quando morre

    void Start()
    {
        _currentLife = _lifeMax;

        // Se você esqueceu de arrastar o AudioSource, ele tenta pegar o do objeto
        if (_audioSource == null) _audioSource = GetComponent<AudioSource>();
    }

    public void Hitted(float damage, Vector3 shootPoint)
    {
        if (isDead) return;

        _currentLife -= damage;

        // --- FEEDBACK VISUAL (SANGUE) ---
        if (_bloodEffect != null)
        {
            GameObject blood = Instantiate(_bloodEffect, shootPoint, Quaternion.LookRotation(shootPoint - transform.position));
            blood.transform.SetParent(transform);
            Destroy(blood, 2f); // Destrói o efeito de sangue após 2 segundos para não pesar o jogo
        }

        // --- FEEDBACK SONORO (DANO) ---
        if (_audioSource != null && _hitSound != null)
        {
            _audioSource.PlayOneShot(_hitSound);
        }

        if (_currentLife <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;

        // --- FEEDBACK SONORO (MORTE) ---
        // Usamos PlayClipAtPoint porque o objeto será destruído e o AudioSource pararia de tocar
        if (_deathSound != null)
        {
            AudioSource.PlayClipAtPoint(_deathSound, transform.position);
        }

        // Desativa os scripts de IA para o zumbi não continuar andando "morto"
        if (GetComponent<ZombieAI>()) GetComponent<ZombieAI>().enabled = false;
        if (GetComponent<BossZombieAI>()) GetComponent<BossZombieAI>().enabled = false;

        // Lógica do GameManager
        if (gameObject.CompareTag("Zombie"))
        {
            GameManager.Instance.AddKill();
        }
        else if (gameObject.CompareTag("Boss"))
        {
            GameManager.Instance.BossDefeated();
        }

        Destroy(gameObject);
    }
}