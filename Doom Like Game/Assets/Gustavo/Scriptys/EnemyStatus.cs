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
        if (isDead) return; // Evita contar duas vezes
        isDead = true;

        // IMPORTANTE: Verifique se as tags batem com o que está no Unity
        if (gameObject.CompareTag("Zombie"))
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.AddKill();
            }
        }
        else if (gameObject.CompareTag("Boss"))
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.BossDefeated();
            }
        }

        Destroy(gameObject);
    }

}
