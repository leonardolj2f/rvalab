using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusicDucking : MonoBehaviour
{
    public AudioSource backgroundMusic; // O AudioSource da música de fundo
    public AudioSource[] importantSounds; // Array de AudioSources importantes que fazem o ducking
    public float reducedVolume = 0.2f; // Volume reduzido da música de fundo durante o ducking
    public float checkInterval = 0.1f; // Intervalo de tempo para verificar o estado dos outros sons

    private float originalVolume;

    void Start()
    {
        if (backgroundMusic != null)
        {
            originalVolume = backgroundMusic.volume;
        }
        InvokeRepeating(nameof(CheckImportantSounds), 0f, checkInterval);
    }

    void CheckImportantSounds()
    {
        bool isAnySoundPlaying = false;

        // Verifica se algum dos sons importantes está a tocar
        foreach (AudioSource source in importantSounds)
        {
            if (source != null && source.isPlaying)
            {
                isAnySoundPlaying = true;
                break;
            }
        }

        // Ajusta o volume da música de fundo com base no estado dos sons importantes
        if (isAnySoundPlaying)
        {
            backgroundMusic.volume = reducedVolume;
        }
        else
        {
            backgroundMusic.volume = originalVolume;
        }
    }
}
