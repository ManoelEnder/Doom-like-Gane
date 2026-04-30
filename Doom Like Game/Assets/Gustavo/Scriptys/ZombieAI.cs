using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class ZombieAI : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform player;

    [Header("Configurações de Combate")]
    public float attackRange = 1.5f;
    public float attackCooldown = 2.0f;
    private float nextAttackTime;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // Encontra o player uma única vez no início
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) player = playerObj.transform;
    }

    void Update()
    {
        if (player == null) return;

        // O destino é ATUALIZADO constantemente para a posição do player
        agent.SetDestination(player.position);

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Verifica se está perto o suficiente para atacar
        if (distanceToPlayer <= attackRange)
        {
            if (Time.time >= nextAttackTime)
            {
                Attack();
                nextAttackTime = Time.time + attackCooldown;
            }
        }
    }

    void Attack()
    {
        // Aqui entra o gatilho da sua animação ou dano
        Debug.Log("Zumbi acertou o player!");

        // Opcional: Faz o zumbi parar brevemente ao atacar para dar chance ao player
        // agent.isStopped = true; 
        // Invoke("ResumeChase", 1.0f);
    }

    void ResumeChase()
    {
        if (agent != null) agent.isStopped = false;
    }
}