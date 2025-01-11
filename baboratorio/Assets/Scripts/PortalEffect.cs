using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalEffect : MonoBehaviour
{
    public GameObject avatar; // Refer�ncia ao avatar (inicialmente desativado)
    public GameObject portal; // Refer�ncia ao objeto do portal com o Particle System
    public AudioSource portalSound; // Som opcional para o portal
    public float portalDuration = 3f; // Dura��o do efeito do portal

    private bool hasTriggered = false; // Garantir que s� acontece uma vez

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

        // Esperar a dura��o do efeito do portal
        yield return new WaitForSeconds(portalDuration);

        // Desativar o portal e ativar o avatar
        portal.SetActive(false);
        avatar.SetActive(true);
    }
}
