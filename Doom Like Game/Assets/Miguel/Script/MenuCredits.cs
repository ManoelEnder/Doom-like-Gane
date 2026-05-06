using UnityEngine;

public class MenuCredits : MonoBehaviour
{
    [Header("Painéis")]
    public GameObject painelCreditos; // Arraste seu Panel de créditos aqui no Inspector

    // Função para abrir o painel
    public void AbrirCreditos()
    {
        if (painelCreditos != null)
        {
            painelCreditos.SetActive(true);
        }
    }

    // Função para fechar o painel (botão de "Voltar")
    public void FecharCreditos()
    {
        if (painelCreditos != null)
        {
            painelCreditos.SetActive(false);
        }
    }
}