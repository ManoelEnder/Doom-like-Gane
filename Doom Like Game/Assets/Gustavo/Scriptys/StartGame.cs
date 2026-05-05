using UnityEngine;

using UnityEngine.SceneManagement;  // Necessário para carregar cenas

public class StartGame : MonoBehaviour

{

    // Função chamada quando o botão é clicado

    public void StartTheGame()

    {

        // Carrega a cena do jogo (certifique-se de que a cena "Game" está adicionada nas configurações do build)

        SceneManager.LoadScene("CenaGu");

    }

}