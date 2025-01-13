using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class MovementUnlocker : MonoBehaviour
{
    public GameObject moveProviderObject; // Objeto com o Move Provider
    public Transform playerTransform; // Transform do jogador (XR Rig)
    public Collider secondaryAreaCollider; // Collider da área secundária
    private bool hasTeleported = false;

    void Start()
    {
        // Desativa o Move Provider no início
        if (moveProviderObject != null)
        {
            moveProviderObject.SetActive(false);
        }
    }

    void Update()
    {
        if (hasTeleported) return; // Se já teletransportou, não verifica novamente

        // Verifica se o jogador está dentro da área secundária
        if (secondaryAreaCollider.bounds.Contains(playerTransform.position))
        {
            hasTeleported = true;
            moveProviderObject.SetActive(true); // Ativa o Move Provider
            Debug.Log("Movimento desbloqueado!");
        }
    }
}
