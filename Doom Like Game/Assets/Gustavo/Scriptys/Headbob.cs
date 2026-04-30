using UnityEngine;

public partial class Headbob : MonoBehaviour
{
    [Header("Configurações")]
    [SerializeField] private float walkingBobSpeed = 14f;
    [SerializeField] private float bobWeight = 0.05f; // Força do balanço

    [Header("Referências")]
    [SerializeField] private CharacterController playerController; // Arraste o Player aqui

    private float timer = 0f;
    private float defaultPosY = 0f;

    void Start()
    {
        // Salva a posição inicial da câmera
        defaultPosY = transform.localPosition.y;
    }

    void Update()
    {
        // Verifica se o player está se movendo no chão
        if (Mathf.Abs(playerController.velocity.x) > 0.1f || Mathf.Abs(playerController.velocity.z) > 0.1f)
        {
            // Player está andando
            timer += Time.deltaTime * walkingBobSpeed;

            // Calcula a nova posição usando Seno
            float newY = defaultPosY + Mathf.Sin(timer) * bobWeight;
            transform.localPosition = new Vector3(transform.localPosition.x, newY, transform.localPosition.z);
        }
        else
        {
            // Player parado: volta para a posição inicial suavemente
            timer = 0;
            transform.localPosition = new Vector3(
                transform.localPosition.x,
                Mathf.Lerp(transform.localPosition.y, defaultPosY, Time.deltaTime * walkingBobSpeed),
                transform.localPosition.z
            );
        }
    }
}