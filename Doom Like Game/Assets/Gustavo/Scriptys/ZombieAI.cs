using System;
using System.Security.Cryptography;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.XR;
using static Unity.Burst.Intrinsics.X86;

public class ZombieAI : MonoBehaviour
{

    private NavMeshAgent agent;
    private Animator anim;
    private Transform player;

    [Header("Configurações de Nascimento")]
   [SerializeField] private float spawnAnimationDuration = 3.0f; // Tempo que ele leva pra sair do chão
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

        // Inicia o processo de nascimento
        StartCoroutine(HandleSpawn());
    }

    System.Collections.IEnumerator HandleSpawn()
    {
        agent.enabled = false; // Desliga o NavMesh para não bugar enquanto sobe
        isSpawning = true;

        // Toca a animação (certifique-se que o nome no Animator é exatamente este)
        anim.Play("Spawn");

        yield return new WaitForSeconds(spawnAnimationDuration);

        isSpawning = false;
        agent.enabled = true; // Liga o NavMesh para começar a perseguição
    }

    void Update()
    {
        // Se estiver nascendo ou se o player morreu, não faz nada
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
        anim.SetTrigger("IsAtack");
        transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
    }
}