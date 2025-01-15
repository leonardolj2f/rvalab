using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class AvatarWaveDetection : MonoBehaviour
{
    public Transform playerHead; // Refer�ncia � cabe�a do jogador (XR Camera)
    public float waveHeightThreshold = 1.5f; // Altura m�nima para considerar a sauda��o
    public AudioClip waveAudio; // Som da sauda��o
    private AudioSource audioSource;
    private Animator animator;
    private bool hasWaved = false; // Evita sauda��es repetidas

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        DetectHandWave();
    }

    private void DetectHandWave()
    {
        // Verificar posi��o dos controladores esquerdo e direito
        InputDevice leftHandDevice = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        InputDevice rightHandDevice = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);

        Vector3 leftHandPosition, rightHandPosition;
        bool leftHandAvailable = leftHandDevice.TryGetFeatureValue(CommonUsages.devicePosition, out leftHandPosition);
        bool rightHandAvailable = rightHandDevice.TryGetFeatureValue(CommonUsages.devicePosition, out rightHandPosition);

        if (!hasWaved && (leftHandAvailable || rightHandAvailable))
        {
            if ((leftHandAvailable && leftHandPosition.y > playerHead.position.y + waveHeightThreshold) ||
                (rightHandAvailable && rightHandPosition.y > playerHead.position.y + waveHeightThreshold))
            {
                PlayWaveAnimation();
            }
        }
    }

    private void PlayWaveAnimation()
    {
        if (animator != null)
        {
            animator.SetTrigger("BoasVindas");
        }

        if (waveAudio != null && audioSource != null)
        {
            audioSource.clip = waveAudio;
            audioSource.Play();
        }

        hasWaved = true;
    }
}
