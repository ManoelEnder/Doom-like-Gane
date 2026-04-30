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

    [Header("Configurações de Ataque")]
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float attackCooldown = 1.5f;
    private float nextAttackTime;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) player = playerObj.transform;
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > attackRange)
        {
            // MODO PERSEGUIÇÃO
            agent.isStopped = false;
            agent.SetDestination(player.position);

            // Ativa animação de andar e desativa ataque
            anim.SetBool("IsWalking", true);
        }
        else
        {
            // MODO ATAQUE
            agent.isStopped = true; // Para de andar para atacar
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
        // Dispara a animação de ataque
        // Se no Animator for Trigger, use SetTrigger. Se for Bool, use SetBool.
        anim.SetTrigger("IsAtack");

        Debug.Log("Zumbi atacou!");

        // Faz o zumbi encarar o player no ataque
        transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
    }
}