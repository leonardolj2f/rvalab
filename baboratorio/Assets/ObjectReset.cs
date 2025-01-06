using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;

public class ObjectReset : MonoBehaviour
{
    public float disappearDuration = 1f; // Tempo de desaparecimento
    public GameObject particleEffectPrefab; // Prefab das part�culas
    public AudioClip disappearSound; // Som ao desaparecer
    public AudioSource audioSource; // Componente de �udio

    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private XRGrabInteractable grabInteractable;
    private Renderer objectRenderer;
    private Color originalColor;

    void Start()
    {
        // Salva a posi��o e rota��o iniciais
        initialPosition = transform.position;
        initialRotation = transform.rotation;

        // Obt�m o componente de intera��o
        grabInteractable = GetComponent<XRGrabInteractable>();

        // Obt�m o Renderer e salva a cor original
        objectRenderer = GetComponent<Renderer>();
        originalColor = objectRenderer.material.color;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            StartCoroutine(ResetObject());
        }
    }

    private IEnumerator ResetObject()
    {
        // Instancia o efeito de part�culas na posi��o atual do objeto
        if (particleEffectPrefab != null)
        {
            Instantiate(particleEffectPrefab, transform.position, Quaternion.identity);
        }

        // Toca o som de desaparecimento, se houver
        if (audioSource != null && disappearSound != null)
        {
            audioSource.PlayOneShot(disappearSound);
        }

        // Fade-out do objeto
        yield return StartCoroutine(FadeOut());

        // Move o objeto para a posi��o inicial e reseta a rota��o
        transform.position = initialPosition;
        transform.rotation = initialRotation;

        // Fade-in do objeto
        yield return StartCoroutine(FadeIn());
    }


    private IEnumerator FadeOut()
    {
        float fadeDuration = 0.5f; // Tempo de fade-out
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            SetObjectAlpha(alpha);
            yield return null;
        }
        SetObjectAlpha(0f); // Garante que a opacidade final � 0
    }

    private IEnumerator FadeIn()
    {
        float fadeDuration = 0.5f; // Tempo de fade-in
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            SetObjectAlpha(alpha);
            yield return null;
        }
        SetObjectAlpha(1f); // Garante que a opacidade final � 1
    }

    private void SetObjectAlpha(float alpha)
    {
        Color newColor = originalColor;
        newColor.a = alpha;
        objectRenderer.material.color = newColor;
    }
}
