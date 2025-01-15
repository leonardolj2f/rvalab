using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondarySpaceTrigger : MonoBehaviour
{
    public AvatarGuide avatarGuide; // Referência ao script do avatar
    public AudioClip secondarySound; // Som a ser tocado
    public Animator avatarAnimator; // Referência ao Animator do avatar
    public string animationTriggerName = "Wave"; // Nome do trigger da animação
    private AudioSource audioSource;
    private bool hasTriggered = false; // Variável de controle

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("Faltando AudioSource no GameObject do trigger!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && avatarGuide.HasFinishedGuiding() && !hasTriggered) // Verifica se é o player e se o avatar já guiou
        {
            hasTriggered = true; // Marca como já executado
            PlaySecondaryReaction();
        }
    }

    private void PlaySecondaryReaction()
    {
        // Tocar som
        if (secondarySound != null && audioSource != null)
        {
            audioSource.PlayOneShot(secondarySound);
        }

        // Executar animação
        if (avatarAnimator != null)
        {
            avatarAnimator.SetTrigger(animationTriggerName);
        }
    }
}
