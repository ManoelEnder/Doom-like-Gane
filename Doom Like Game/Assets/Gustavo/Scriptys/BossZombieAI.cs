using UnityEngine;
using UnityEngine.AI;
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
    [SerializeField] private float attackRange = 2.8f;
    [SerializeField] private float hitDetectionRange = 3.2f;
    [SerializeField] private float recoverTimeHit = 0.8f;
    [SerializeField] private float recoverTimeMiss = 3.5f; // Punição maior para o Boss //

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
            StartCoroutine(BossAttackRoutine());
        }
    }

    IEnumerator BossAttackRoutine()
    {
        isRecovering = true;
        agent.isStopped = true;

        anim.SetTrigger("IsAtack");
        transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));

        yield return new WaitForSeconds(0.7f);

        if (CheckBossHit())
        {
            yield return new WaitForSeconds(recoverTimeHit);
        }
        else
        {
            // BOSS FICA TONTO/RECUPERANDO
            anim.SetBool("IsStun", true);
            yield return new WaitForSeconds(recoverTimeMiss);
            anim.SetBool("IsStun", false);
        }

        isRecovering = false;
        if (agent.enabled) agent.isStopped = false;
    }

    bool CheckBossHit()
    {
        RaycastHit hit;
        Vector3 origin = transform.position + Vector3.up * 1.2f;
        if (Physics.Raycast(origin, transform.forward, out hit, hitDetectionRange))
        {
            if (hit.collider.CompareTag("Player")) return true;
        }
        return false;
    }
}