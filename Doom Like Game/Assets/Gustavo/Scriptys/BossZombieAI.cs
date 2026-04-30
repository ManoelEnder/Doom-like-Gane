using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
using System.Collections;

public class BossZombieAI : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator anim;
    private Transform player;

    [Header("Configurações de Nascimento")]
    [SerializeField] private float spawnAnimationDuration = 3.0f;
    private bool isSpawning = true;

    [Header("Combate")]
    [SerializeField] private float attackRange = 2.2f;
    [SerializeField] private float attackCooldown = 1.5f;
    private float nextAttackTime;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) player = playerObj.transform;

        // Inicia o nascimento
        StartCoroutine(HandleSpawn());
    }

    IEnumerator HandleSpawn()
    {
        isSpawning = true;
        if (agent != null) agent.enabled = false;

        // ATIVA O TRIGGER QUE VOCÊ VIU NA IMAGEM
        anim.SetTrigger("Spawn");

        yield return new WaitForSeconds(spawnAnimationDuration);

        isSpawning = false;
        if (agent != null) agent.enabled = true;
    }

    void Update()
    {
        if (isSpawning || player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > attackRange)
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);
            anim.SetBool("IsWalking", true);
        }
        else
        {
            agent.isStopped = true;
            anim.SetBool("IsWalking", false);

            if (Time.time >= nextAttackTime)
            {
                Attack();
                nextAttackTime = Time.time + attackCooldown;
            }
        }
    }

    void Attack()
    {
        anim.SetTrigger("IsAtack"); // Nome que estava na sua imagem anterior
        transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
    }
}
