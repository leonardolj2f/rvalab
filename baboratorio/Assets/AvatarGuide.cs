using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Video;

public class AvatarGuide : MonoBehaviour
{
    public Transform[] keyPoints; // Locais principais (3 pontos)
    public Transform initialPosition; // Posição inicial do avatar
    //private Quaternion initialRotation; // Rotação inicial do avatar

    public float proximityRadius = 5f; // Raio de detecção do jogador
    public Transform player; // Referência ao jogador (XR Rig)
    public float stopTime = 2f; // Tempo de pausa em cada ponto
    private NavMeshAgent agent; // Controle do movimento do avatar
    private AudioSource audioSource; // Componente de áudio
    private int currentPoint = 0;
    private bool guiding = false;
    private bool hasGuided = false; // Controle para evitar repetir a ação

    float rotationSpeed = 0.5f; // Velocidade de transição da rotação


    public AudioClip introAudio; // Áudio de introdução
    public AudioClip[] waypointAudios; // Áudios para cada waypoint
    public AudioSource footstepAudioSource; // Som de passos


    public GameObject[] videoObjects; // Objetos associados aos vídeos
    public float videoActivationDistance = 3f; // Distância para ativar o Video Player


    private Animator animator; // Referência ao Animator do Avatar

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>(); // Obtém o Animator

        // Inicializa todos os Video Players como inativos
        foreach (var videoObject in videoObjects)
        {
            videoObject.SetActive(false);
        }

        if (agent == null)
        {
            Debug.LogError("Avatar precisa de um NavMeshAgent para mover.");
        }

        if (animator == null)
        {
            Debug.LogError("Avatar precisa de um Animator para a animação.");
        }
    }

    void Update()
    {

        StartGuidingIfPlayerIsClose();
        HandleFootstepSound(); // Controlar som de passos
        CheckVideoActivation();
    }

    private void StartGuidingIfPlayerIsClose()
    {
        if (hasGuided)
            return; // Se já guiou, não faz nada

        if (!guiding && Vector3.Distance(player.position, transform.position) <= proximityRadius)
        {
            PlayWelcomeAnimation(); // Tocar animação de boas-vindas
            StartCoroutine(GuideRoutine());
            guiding = true;
        }

        // Atualiza a animação com base no movimento do avatar
        UpdateAnimation();
    }

    private void PlayWelcomeAnimation()
    {
        if (animator != null)
        {
            animator.SetTrigger("BoasVindas");
        }
    }

    private void HandleFootstepSound()
    {

        footstepAudioSource.volume = Mathf.Clamp(agent.velocity.magnitude / rotationSpeed, 0.2f, 1.0f);

        // Toca som de passos enquanto o agente está se movendo
        if (agent.velocity.magnitude > 0.1f && !footstepAudioSource.isPlaying)
        {
            footstepAudioSource.Play();
        }
        else if (agent.velocity.magnitude <= 0.1f && footstepAudioSource.isPlaying)
        {
            footstepAudioSource.Pause();
        }
    }

    private void CheckVideoActivation()
    {
        if (!hasGuided)
            return; // Só ativa os vídeos após o avatar terminar os waypoints

        for (int i = 0; i < videoObjects.Length; i++)
        {
            // Ativar apenas quando o jogador estiver próximo
            if (Vector3.Distance(player.position, videoObjects[i].transform.position) <= videoActivationDistance)
            {
                videoObjects[i].SetActive(true);
            }
            else
            {
                videoObjects[i].SetActive(false);
            }
        }
    }


    IEnumerator GuideRoutine()
    {

        // Reproduzir áudio de introdução
        if (introAudio != null)
        {
            audioSource.clip = introAudio;
            audioSource.Play();
            // Iniciar a animação de falar
            //animator.SetTrigger("StartTalking");
            yield return new WaitWhile(() => audioSource.isPlaying); // Esperar o áudio terminar
           // animator.SetTrigger("StopTalking");
        }


        // Mover entre os pontos principais
        for (int i = 0; i < keyPoints.Length; i++)
        {
            agent.SetDestination(keyPoints[i].position);

            yield return new WaitUntil(() =>
                !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance);


            //yield return new WaitForSeconds(stopTime);

            // Reproduzir áudio do waypoint, se existir
            if (i < waypointAudios.Length && waypointAudios[i] != null)
            {
                audioSource.clip = waypointAudios[i];
                audioSource.Play();
                animator.SetTrigger("StartTalking");

                yield return new WaitWhile(() => audioSource.isPlaying); // Esperar o áudio terminar

                animator.SetTrigger("StopTalking");
            }

            yield return new WaitForSeconds(stopTime); // Esperar no waypoint
        }


        // Retornar ao ponto inicial
        agent.SetDestination(initialPosition.position);
        yield return new WaitUntil(() =>
            !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance);


        // Transição suave para a rotação inicial
        while (Quaternion.Angle(transform.rotation, initialPosition.rotation) > 1f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, initialPosition.rotation, rotationSpeed * Time.deltaTime);
            yield return null;
        }

        hasGuided = true;

        // Finalizar o guia
        guiding = false;
    }

    public bool HasFinishedGuiding()
    {
        return hasGuided; // Retorna true se o avatar já terminou de guiar
    }


    // Função que controla a animação do avatar
    void UpdateAnimation()
    {
        if (agent.velocity.magnitude > 0.1f) // Se o avatar estiver se movendo
        {
            animator.SetBool("IsWalking", true); // Ativa a animação de caminhada
        }
        else
        {
            animator.SetBool("IsWalking", false); // Desativa a animação de caminhada (volta ao estado idle)
        }
    }
}
