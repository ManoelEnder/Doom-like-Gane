using UnityEngine;
using UnityEngine.SceneManagement; // Essencial para gerenciar cenas

public class TrocaCena : MonoBehaviour
{
    // Método público para ser visível no botão
    public void CarregarCena(string nomeDaCena)
    {
        SceneManager.LoadScene(nomeDaCena);
        Time.timeScale = 1f; // Certifique-se de que o tempo esteja normal ao carregar a cena
    }

}
