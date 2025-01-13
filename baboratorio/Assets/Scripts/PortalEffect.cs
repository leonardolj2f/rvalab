using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PortalEffect : MonoBehaviour
{
    public GameObject avatar; // Refer�ncia ao avatar (inicialmente desativado)
    public GameObject portal; // Refer�ncia ao objeto do portal com o Particle System
    public AudioSource portalSound; // Som opcional para o portal
    public float portalDuration = 3f; // Dura��o do efeito do portal
    public XRBaseController leftController;  // Refer�ncia ao controle esquerdo
    public XRBaseController rightController; // Refer�ncia ao controle direito

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

        // Disparar evento h�ptico nos controladores
        TriggerHapticFeedback(0.5f, 8f); // Intensidade e dura��o

        // Esperar a dura��o do efeito do portal
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
