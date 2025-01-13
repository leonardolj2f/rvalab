using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TeleportationControl : MonoBehaviour
{
    public GameObject teleportationArea; // Referência ao GameObject da Teleportation Area
    public Transform player; // Referência ao jogador (XR Rig)
    public Transform secondarySpace; // Referência ao espaço secundário
    public float activationDistance = 2f; // Distância para considerar que o player entrou no espaço
    private bool hasDeactivated = false; // Controle para desativação única

    private AvatarGuide avatarGuide; // Referência ao script do AvatarGuide

    void Start()
    {
        avatarGuide = FindObjectOfType<AvatarGuide>();
        teleportationArea.SetActive(true); // Garantir que começa ativo
    }

    void Update()
    {
        CheckPlayerInSecondarySpace();
    }

    private void CheckPlayerInSecondarySpace()
    {
        if (!hasDeactivated && Vector3.Distance(player.position, secondarySpace.position) <= activationDistance)
        {
            teleportationArea.SetActive(false); // Desativa a área de teleporte
            hasDeactivated = true;
        }

        // Reativar quando o avatar terminar o guia
        if (hasDeactivated && avatarGuide.HasFinishedGuiding())
        {
            teleportationArea.SetActive(true); // Reativa a área de teleporte
        }
    }
}
