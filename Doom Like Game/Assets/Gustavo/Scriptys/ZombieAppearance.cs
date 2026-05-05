using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ZombieAppearance : MonoBehaviour
{
    [Header("Variações de Corpo/Malha")]
    [SerializeField]private List<GameObject> bodyVariants; // Arraste diferentes modelos de corpo aqui

    [Header("Variações de Cor/Material")]
    [SerializeField] private Renderer zombieRenderer; // O SkinnedMeshRenderer do zumbi
    [SerializeField] private List<Material> skinMaterials; // Lista de diferentes texturas/cores

    [Header("Acessórios (Opcional)")]
    [SerializeField] private List<GameObject> accessories; // Chapéus, ferramentas, etc.
    void Start()
    {
        ApplyRandomLook();
    }

    void ApplyRandomLook()
    {
        // 1. Escolhe um corpo aleatório (se houver mais de um)
        if (bodyVariants.Count > 0)
        {
            // Desativa todos primeiro
            foreach (var body in bodyVariants) body.SetActive(false);

            // Ativa um aleatório
            int bodyIndex = Random.Range(0, bodyVariants.Count);
            bodyVariants[bodyIndex].SetActive(true);
        }

        // 2. Escolhe um material aleatório
        if (zombieRenderer != null && skinMaterials.Count > 0)
        {
            int matIndex = Random.Range(0, skinMaterials.Count);
            zombieRenderer.material = skinMaterials[matIndex];
        }

        // 3. Chance aleatória de ter um acessório (ex: 30% de chance)
        if (accessories.Count > 0)
        {
            foreach (var item in accessories) item.SetActive(false); // Limpa tudo

            if (Random.value > 0.7f) // 0.7 significa 30% de chance de aparecer algo
            {
                int accIndex = Random.Range(0, accessories.Count);
                accessories[accIndex].SetActive(true);
            }
        }
    }
}