using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ButtonAction : MonoBehaviour
{
    // Referência ao objeto que queremos ativar
    public GameObject objetoParaAtivar;

    // Referência ao AudioSource para tocar o som
    public AudioSource audioSource;
    public GameObject particlePrefab; // Prefab do sistema de partículas
    public Transform particleSpawnPoint; // Ponto onde o sistema de partículas será instanciado


    // Este método será chamado quando o botão for pressionado
    public void AtivarObjeto()
    {
        if (objetoParaAtivar != null)
        {
            objetoParaAtivar.SetActive(true);  // Ativa o objeto
            PlayParticles(); // Executa o efeito de partículas
        }

        if (audioSource != null)
        {
            // Toca o som
            audioSource.Play();
        }
    }

    private void PlayParticles()
    {
        if (particlePrefab != null)
        {
            // Instancia o prefab de partículas no local desejado
            Instantiate(particlePrefab, particleSpawnPoint.position, Quaternion.identity);
        }
    }
}
