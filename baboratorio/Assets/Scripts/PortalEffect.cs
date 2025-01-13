using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PortalEffect : MonoBehaviour
{
    public GameObject avatar; // Referência ao avatar (inicialmente desativado)
    public GameObject portal; // Referência ao objeto do portal com o Particle System
    public AudioSource portalSound; // Som opcional para o portal
    public float portalDuration = 3f; // Duração do efeito do portal
    public XRBaseController leftController;  // Referência ao controle esquerdo
    public XRBaseController rightController; // Referência ao controle direito

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

        // Disparar evento háptico nos controladores
        TriggerHapticFeedback(0.5f, 8f); // Intensidade e duração

        // Esperar a duração do efeito do portal
        yield return new WaitForSeconds(portalDuration);

        // Desativar o portal e ativar o avatar
        portal.SetActive(false);
        avatar.SetActive(true);
    }

    void TriggerHapticFeedback(float intensity, float duration)
    {
        if (leftController != null)
        {
            leftController.SendHapticImpulse(intensity, duration);
        }
        if (rightController != null)
        {
            rightController.SendHapticImpulse(intensity, duration);
        }
    }
}
