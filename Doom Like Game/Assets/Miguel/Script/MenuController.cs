using UnityEngine;
using UnityEngine.SceneManagement; // Necessário para trocar de cena

public class MenuController : MonoBehaviour
{
    [Header("Configurações de Cena")]
    public string nomeDaCenaParaCarregar;

    void Update()
    {
        // 1. Sair do Jogo (Ex: Tecla Escape/ESC)
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SairDoJogo();
        }

        // 2. Mudar de Cena (Ex: Tecla Espaço ou Enter)
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
        {
            CarregarCena();
        }
    }

    public void CarregarCena()
    {
        // Carrega a cena definida na variável nomeDaCenaParaCarregar
        SceneManager.LoadScene(nomeDaCenaParaCarregar);
    }

    public void SairDoJogo()
    {
        Debug.Log("O botão de sair foi apertado!");

        // Fecha o aplicativo (só funciona no jogo buildado)
        Application.Quit();

        // Se estiver rodando dentro do Editor da Unity, isso para o PlayMode
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}