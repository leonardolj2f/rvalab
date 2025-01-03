using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TeleportAreaTrigger : MonoBehaviour
{
    public TeleportManager teleportManager; // Referência ao TeleportManager

    private void OnTriggerEnter(Collider other)
    {
        // Verifica se o objeto que entrou é o XR Rig (Player)
        if (other.CompareTag("Player"))
        {
            // Notifica o TeleportManager que o player chegou
            teleportManager.OnTeleportToSecondary();
            print("hello esta aqui");
        }
    }
}
