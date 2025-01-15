using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRPokeInteractor))]
public class PokeHapticFeedback : MonoBehaviour
{
    public float hapticIntensity = 0.5f;  // Intensidade do feedback h�ptico
    public float hapticDuration = 0.1f;   // Dura��o do feedback h�ptico

    private XRPokeInteractor pokeInteractor;

    void Start()
    {
        pokeInteractor = GetComponent<XRPokeInteractor>();
        pokeInteractor.hoverEntered.AddListener(OnPokeStart);
    }

    private void OnPokeStart(HoverEnterEventArgs args)
    {
        XRBaseController controller = pokeInteractor.GetComponent<XRBaseController>();
        if (controller != null)
        {
            controller.SendHapticImpulse(hapticIntensity, hapticDuration);
        }
    }

    private void OnDestroy()
    {
        if (pokeInteractor != null)
        {
            pokeInteractor.hoverEntered.RemoveListener(OnPokeStart);
        }
    }
}
