using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondarySpaceTrigger : MonoBehaviour
{
    public AvatarGuide avatarGuide; // Refer�ncia ao script do avatar
    public AudioClip secondarySound; // Som a ser tocado
    public Animator avatarAnimator; // Refer�ncia ao Animator do avatar
    public string animationTriggerName = "Wave"; // Nome do trigger da anima��o
    private AudioSource audioSource;
    private bool hasTriggered = false; // Vari�vel de controle

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
        if (other.CompareTag("Player") && avatarGuide.HasFinishedGuiding() && !hasTriggered) // Verifica se � o player e se o avatar j� guiou
        {
            hasTriggered = true; // Marca como j� executado
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

        // Executar anima��o
        if (avatarAnimator != null)
        {
            avatarAnimator.SetTrigger(animationTriggerName);
        }
    }
}
