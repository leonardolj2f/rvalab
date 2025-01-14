using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowButtonWithParticles : MonoBehaviour
{
    public GameObject button; // Refer�ncia ao bot�o
    public AvatarGuide avatarGuide; // Refer�ncia ao script do AvatarGuide
    public GameObject particlePrefab; // Prefab do sistema de part�culas
    public Transform particleSpawnPoint; // Ponto onde o sistema de part�culas ser� instanciado

    private bool buttonShown = false; // Controle para garantir que o efeito aconte�a apenas uma vez

    void Start()
    {
        button.SetActive(false); // Esconde o bot�o no in�cio
    }

    void Update()
    {
        // Verifica se o avatar terminou todos os waypoints e voltou ao ponto inicial
        if (!buttonShown && avatarGuide.HasFinishedGuiding())
        {
            button.SetActive(true); // Ativa o bot�o
            PlayParticles(); // Executa o efeito de part�culas
            buttonShown = true; // Garante que o efeito n�o ser� repetido
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
