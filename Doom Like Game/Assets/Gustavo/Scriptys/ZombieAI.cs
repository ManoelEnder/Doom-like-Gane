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

    [Header("Configurações")]
    [SerializeField] private float attackRange = 2.2f; // Ajuste conforme o tamanho do braço do modelo
    [SerializeField] private float attackCooldown = 1.5f;
    private float nextAttackTime;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        // Encontra o player pela Tag (certifique-se que seu Player tem a tag "Player")
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) player = playerObj.transform;
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > attackRange)
        {
            // --- ESTADO: PERSEGUIR ---
            agent.isStopped = false;
            agent.SetDestination(player.position);

            anim.SetBool("IsWalking", true);
        }
        else if (distance <= attackRange)
        {
            // --- ESTADO: ATACAR ---
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
        // Usa o nome EXATO que está na sua imagem
        anim.SetTrigger("IsAtack");

        // Gira para o player não fugir do foco durante o golpe
        Vector3 lookPos = new Vector3(player.position.x, transform.position.y, player.position.z);
        transform.LookAt(lookPos);

        Debug.Log("Zumbi desferiu um golpe!");
    }
}