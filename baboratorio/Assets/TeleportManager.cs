using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TeleportManager : MonoBehaviour
{
    public TeleportationArea primaryTeleportArea; // Área do Plane Principal
    public TeleportationArea secondaryTeleportArea; // Área do Plane Secundário

    private bool hasTeleportedToSecondary = false;

    void Start()
    {
        // Desativar o teletransporte no Plane Principal inicialmente
        if (primaryTeleportArea != null)
        {
            primaryTeleportArea.enabled = false;
        }
    }

    public void OnTeleportToSecondary()
    {
        if (!hasTeleportedToSecondary)
        {
            hasTeleportedToSecondary = true;

            // Ativar o teletransporte no Plane Principal
            if (primaryTeleportArea != null)
            {
                primaryTeleportArea.enabled = true;
                print("puta");
            }
        }
    }
}
