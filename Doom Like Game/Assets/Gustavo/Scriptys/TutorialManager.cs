using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [Header("Painéis de Interface")]
    [SerializeField] private GameObject mainMenuPanel; // O painel principal com os botões
    [SerializeField] private GameObject tutorialPanel; // O painel que explica os controles

    // Chame esta função no botão "Buttons and Tutorial"
    public void OpenTutorial()
    {
        if (mainMenuPanel != null && tutorialPanel != null)
        {
            mainMenuPanel.SetActive(false); // Esconde o menu
            tutorialPanel.SetActive(true);  // Mostra o tutorial
        }
    }

    // Chame esta função no botão de "Voltar" dentro do tutorial
    public void CloseTutorial()
    {
        if (mainMenuPanel != null && tutorialPanel != null)
        {
            tutorialPanel.SetActive(false); // Esconde o tutorial
            mainMenuPanel.SetActive(true);  // Volta para o menu
        }
    }
}