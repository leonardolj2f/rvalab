using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TeleportationControl : MonoBehaviour
{
    public GameObject teleportationArea; // Refer�ncia ao GameObject da Teleportation Area
    public Transform player; // Refer�ncia ao jogador (XR Rig)
    public Transform secondarySpace; // Refer�ncia ao espa�o secund�rio
    public float activationDistance = 2f; // Dist�ncia para considerar que o player entrou no espa�o
    private bool hasDeactivated = false; // Controle para desativa��o �nica

    private AvatarGuide avatarGuide; // Refer�ncia ao script do AvatarGuide

    void Start()
    {
        avatarGuide = FindObjectOfType<AvatarGuide>();
        teleportationArea.SetActive(true); // Garantir que come�a ativo
    }

    void Update()
    {
        CheckPlayerInSecondarySpace();
    }

    private void CheckPlayerInSecondarySpace()
    {
        if (!hasDeactivated && Vector3.Distance(player.position, secondarySpace.position) <= activationDistance)
        {
            teleportationArea.SetActive(false); // Desativa a �rea de teleporte
            hasDeactivated = true;
        }

        // Reativar quando o avatar terminar o guia
        if (hasDeactivated && avatarGuide.HasFinishedGuiding())
        {
            teleportationArea.SetActive(true); // Reativa a �rea de teleporte
        }
    }
}
