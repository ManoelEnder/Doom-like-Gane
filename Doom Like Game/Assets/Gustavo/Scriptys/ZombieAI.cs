using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class ZombieAI : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator anim;
    private Transform player;

    [Header("Configurações de Nascimento")]
    [SerializeField] private float spawnAnimationDuration = 3.0f;
    private bool isSpawning = true;

    [Header("Combate")]
    [SerializeField] private float attackRange = 2.2f;
    [SerializeField] private float hitDetectionRange = 2.5f;
    [SerializeField] private float recoverTimeHit = 1.0f;
    [SerializeField] private float recoverTimeMiss = 2.5f;
    [SerializeField] private float damageDealt = 10f; // Dano do zumbi normal

    private bool isRecovering = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) player = playerObj.transform;
        StartCoroutine(HandleSpawn());
    }

    IEnumerator HandleSpawn()
    {
        isSpawning = true;
        if (agent != null) agent.enabled = false;
        anim.SetTrigger("Spawn");
        yield return new WaitForSeconds(spawnAnimationDuration);
        isSpawning = false;
        if (agent != null) agent.enabled = true;
    }

    void Update()
    {
        if (isSpawning || isRecovering || player == null) return;

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
            StartCoroutine(AttackRoutine());
        }
    }

    IEnumerator AttackRoutine()
    {
        isRecovering = true;
        agent.isStopped = true;

        anim.SetTrigger("IsAtack");
        transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));

        yield return new WaitForSeconds(0.6f); // Momento do impacto

        if (CheckHit())
        {
            // --- AQUI APLICA O DANO ---
            PlayerHealth ph = player.GetComponent<PlayerHealth>();
            if (ph != null) ph.TakeDamage(damageDealt);

            yield return new WaitForSeconds(recoverTimeHit);
        }
        else
        {
            anim.SetBool("IsStun", true);
            yield return new WaitForSeconds(recoverTimeMiss);
            anim.SetBool("IsStun", false);
        }

        isRecovering = false;
        if (agent.enabled) agent.isStopped = false;
    }

    bool CheckHit()
    {
        RaycastHit hit;
        Vector3 origin = transform.position + Vector3.up;
        if (Physics.Raycast(origin, transform.forward, out hit, hitDetectionRange))
        {
            if (hit.collider.CompareTag("Player")) return true;
        }
        return false;
    }
}