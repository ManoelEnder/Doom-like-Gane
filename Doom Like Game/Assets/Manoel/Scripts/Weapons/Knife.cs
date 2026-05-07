using UnityEngine;

public class Knife : MonoBehaviour
{
    public float damage = 50f;
    public float range = 3f;

    public Camera fpsCam;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }
    }

    void Attack()
    {
        RaycastHit hit;

        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            Enemy enemy = hit.transform.GetComponent<Enemy>();

            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }
    }
}