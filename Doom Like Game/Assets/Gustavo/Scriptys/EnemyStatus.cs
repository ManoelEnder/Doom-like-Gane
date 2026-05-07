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

        // Se for um Zumbi comum
        if (gameObject.CompareTag("Zombie"))
        {
            GameManager.Instance.AddKill();
        }

        // NOVO: Se for o Boss (Crie a Tag "Boss" no Unity e coloque no Prefab do Boss)
        if (gameObject.CompareTag("Boss"))
        {
            GameManager.Instance.BossDefeated();
        }

        Destroy(gameObject);
    }

}
