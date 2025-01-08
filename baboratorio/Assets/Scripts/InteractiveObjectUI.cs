using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.XR.Interaction.Toolkit;

public class InteractiveObjectUI : MonoBehaviour
{
    public Transform player; // Referência ao jogador
    public float proximityRadius = 2f; // Distância para mostrar o UI
    public GameObject popupUI; // Referência ao UI Pop-up
    public float fadeDuration = 0.5f; // Duração do fade in/out

    private CanvasGroup canvasGroup;
    private bool isPlayerNearby = false; // Estado atual do jogador (perto/longe)
    private bool isFading = false; // Para evitar múltiplas corrotinas

    private XRGrabInteractable grabInteractable; // Referência ao componente XR Grab Interactable

    private void Start()
    {
        if (popupUI != null)
        {
            canvasGroup = popupUI.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = popupUI.AddComponent<CanvasGroup>();
            }
            canvasGroup.alpha = 0f; // Começa invisível
            popupUI.SetActive(false);
        }

        // Tenta obter o componente XRGrabInteractable
        grabInteractable = GetComponent<XRGrabInteractable>();
        if (grabInteractable != null)
        {
            // Registra a função OnGrab como listener do evento de grab
            grabInteractable.selectEntered.AddListener(OnGrab);
        }
    }

    private void Update()
    {
        CheckProximity();
    }

    private void CheckProximity()
    {
        float distance = Vector3.Distance(player.position, transform.position);
        bool playerIsClose = distance <= proximityRadius;

        if (playerIsClose && !isPlayerNearby)
        {
            isPlayerNearby = true;
            StartCoroutine(FadeIn());
        }
        else if (!playerIsClose && isPlayerNearby)
        {
            isPlayerNearby = false;
            StartCoroutine(FadeOut());
        }
    }

    private IEnumerator FadeIn()
    {
        isFading = true;
        popupUI.SetActive(true);

        float timer = 0f;
        while (timer <= fadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            timer += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = 1f;
        isFading = false;
    }

    private IEnumerator FadeOut()
    {
        isFading = true;

        float timer = 0f;
        while (timer <= fadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration);
            timer += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = 0f;
        popupUI.SetActive(false);
        isFading = false;
    }

    // Função chamada quando o objeto é pego (grabbed)
    private void OnGrab(SelectEnterEventArgs args)
    {
        if (isPlayerNearby)
        {
            StartCoroutine(FadeOut()); // Desativa o UI ao pegar o objeto
            isPlayerNearby = false; // Evita que ele reapareça enquanto o objeto estiver pego
        }
    }

    private void OnDestroy()
    {
        // Remove o listener para evitar erros ao destruir o objeto
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.RemoveListener(OnGrab);
        }
    }
}
