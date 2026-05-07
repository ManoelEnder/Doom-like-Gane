using UnityEngine;

public class MusicController : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip actionMusic; // Arraste a música de ação aqui no Inspector

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // Configura a música e toca
        if (actionMusic != null)
        {
            audioSource.clip = actionMusic;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    // Função para parar a música (podes chamar quando o player morrer)
    public void StopMusic()
    {
        audioSource.Stop();
    }
}