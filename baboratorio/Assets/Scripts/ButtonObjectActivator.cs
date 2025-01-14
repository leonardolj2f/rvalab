using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ButtonAction : MonoBehaviour
{
    // Refer�ncia ao objeto que queremos ativar
    public GameObject objetoParaAtivar;

    // Refer�ncia ao AudioSource para tocar o som
    public AudioSource audioSource;
    public GameObject particlePrefab; // Prefab do sistema de part�culas
    public Transform particleSpawnPoint; // Ponto onde o sistema de part�culas ser� instanciado


    // Este m�todo ser� chamado quando o bot�o for pressionado
    public void AtivarObjeto()
    {
        if (objetoParaAtivar != null)
        {
            objetoParaAtivar.SetActive(true);  // Ativa o objeto
            PlayParticles(); // Executa o efeito de part�culas
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
            // Instancia o prefab de part�culas no local desejado
            Instantiate(particlePrefab, particleSpawnPoint.position, Quaternion.identity);
        }
    }
}
