using UnityEngine;

public class GerenciadorPainel : MonoBehaviour
{
    // Arraste o seu Panel para este campo no Inspetor
    public GameObject meuPainel;

    // Método para ligar o painel
    public void AbrirPainel()
    {
        if (meuPainel != null)
        {
            meuPainel.SetActive(true);
        }
    }

    // Método para fechar (caso queira usar em um botão de "X")
    public void FecharPainel()
    {
        if (meuPainel != null)
            meuPainel.SetActive(false);
    }
}