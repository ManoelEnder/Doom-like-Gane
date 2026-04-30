using UnityEngine;
using UnityEngine.AI; // Necessário para usar o NavMesh

public class ZombieMovement : MonoBehaviour
{
    public Transform player;
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        // Atualiza o caminho para a posição atual do player a cada frame
        if (player != null)
        {
            agent.SetDestination(player.position);
        }
    }
}