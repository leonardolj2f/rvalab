using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalEffect : MonoBehaviour
{
    public GameObject avatar; // Referência ao avatar (inicialmente desativado)
    public GameObject portal; // Referência ao objeto do portal com o Particle System
    public AudioSource portalSound; // Som opcional para o portal
    public float portalDuration = 3f; // Duração do efeito do portal

    private bool hasTriggered = false; // Garantir que só acontece uma vez

    void OnTriggerEnter(Collider other)
    {
        if (hasTriggered || !other.CompareTag("Player")) return;

        hasTriggered = true;

        StartCoroutine(ActivatePortal());
    }

    private IEnumerator ActivatePortal()
    {
        // Ativar o portal e tocar o som (se existir)
        portal.SetActive(true);
        if (portalSound != null)
        {
            portalSound.Play();
        }

        // Esperar a duração do efeito do portal
        yield return new WaitForSeconds(portalDuration);

        // Desativar o portal e ativar o avatar
        portal.SetActive(false);
        avatar.SetActive(true);
    }
}
