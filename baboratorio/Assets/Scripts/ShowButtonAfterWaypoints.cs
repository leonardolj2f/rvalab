using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowButtonWithParticles : MonoBehaviour
{
    public GameObject button; // Referência ao botão
    public AvatarGuide avatarGuide; // Referência ao script do AvatarGuide
    public GameObject particlePrefab; // Prefab do sistema de partículas
    public Transform particleSpawnPoint; // Ponto onde o sistema de partículas será instanciado

    private bool buttonShown = false; // Controle para garantir que o efeito aconteça apenas uma vez

    void Start()
    {
        button.SetActive(false); // Esconde o botão no início
    }

    void Update()
    {
        // Verifica se o avatar terminou todos os waypoints e voltou ao ponto inicial
        if (!buttonShown && avatarGuide.HasFinishedGuiding())
        {
            button.SetActive(true); // Ativa o botão
            PlayParticles(); // Executa o efeito de partículas
            buttonShown = true; // Garante que o efeito não será repetido
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
