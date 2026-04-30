using UnityEngine;

public class EnemyStatus : MonoBehaviour, IShootable
{
    [SerializeField] private bool isDead = false;
    [SerializeField] private GameObject _bloodEffect;
    [SerializeField] private float _lifeMax = 2;
    private float _currentLife;

    void Start()
    {
        _currentLife = _lifeMax;
    }

    public void Hitted(float damage, Vector3 shootPoint)
    {
        if (isDead) return; // Evita contar a morte múltiplas vezes se levar dois tiros seguidos

        _currentLife -= damage;

        // Efeito de sangue
        GameObject blood = Instantiate(_bloodEffect, shootPoint, Quaternion.LookRotation(shootPoint - transform.position));
        blood.transform.SetParent(transform);

        if (_currentLife <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;

        // 1. VERIFICA A TAG "Zombie" (Certifique-se de ter criado a tag no Unity!)
        if (gameObject.CompareTag("Zombie"))
        {
            // 2. AVISA O GAME MANAGER
            if (GameManager.Instance != null)
            {
                GameManager.Instance.AddKill();
            }
            else
            {
                Debug.LogWarning("GameManager não encontrado na cena!");
            }
        }

        // 3. DESTROI O OBJETO
        Destroy(gameObject);
    }

}
