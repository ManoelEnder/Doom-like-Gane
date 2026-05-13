using UnityEngine;
using UnityEngine.SceneManagement; // Essencial para gerenciar cenas

public class TrocaCena : MonoBehaviour
{
    // Método público para ser visível no botão
    public void CarregarCena(string nomeDaCena)
    {
        SceneManager.LoadScene(nomeDaCena);
    }

    Time.scale = 1f;

}