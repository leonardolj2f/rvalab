using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HandTracking : MonoBehaviour
{
    private InputDevice leftHand;
    private InputDevice rightHand;

    void Start()
    {
        // Obt�m dispositivos de m�o
        var leftHandDevices = new List<InputDevice>();
        var rightHandDevices = new List<InputDevice>();

        // Busca os dispositivos conectados
        InputDevices.GetDevicesAtXRNode(XRNode.LeftHand, leftHandDevices);
        InputDevices.GetDevicesAtXRNode(XRNode.RightHand, rightHandDevices);

        if (leftHandDevices.Count > 0)
            leftHand = leftHandDevices[0];
        if (rightHandDevices.Count > 0)
            rightHand = rightHandDevices[0];
    }

    void Update()
    {
        // Verificar se a m�o esquerda est� sendo rastreada
        if (leftHand.isValid)
        {
            leftHand.TryGetFeatureValue(CommonUsages.trigger, out float leftTriggerValue);
            if (leftTriggerValue > 0.5f)
            {
                Debug.Log("Pinch detected on left hand!");
            }
        }

        // Verificar se a m�o direita est� sendo rastreada
        if (rightHand.isValid)
        {
            rightHand.TryGetFeatureValue(CommonUsages.trigger, out float rightTriggerValue);
            if (rightTriggerValue > 0.5f)
            {
                Debug.Log("Pinch detected on right hand!");
            }
        }
    }
}