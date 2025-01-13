using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Video;

public class AvatarGuide : MonoBehaviour
{
    public Transform[] keyPoints; // Locais principais (3 pontos)
    public Transform initialPosition; // Posi��o inicial do avatar
    //private Quaternion initialRotation; // Rota��o inicial do avatar

    public float proximityRadius = 5f; // Raio de detec��o do jogador
    public Transform player; // Refer�ncia ao jogador (XR Rig)
    public float stopTime = 2f; // Tempo de pausa em cada ponto
    private NavMeshAgent agent; // Controle do movimento do avatar
    private AudioSource audioSource; // Componente de �udio
    private int currentPoint = 0;
    private bool guiding = false;
    private bool hasGuided = false; // Controle para evitar repetir a a��o

    float rotationSpeed = 0.5f; // Velocidade de transi��o da rota��o


    public AudioClip introAudio; // �udio de introdu��o
    public AudioClip[] waypointAudios; // �udios para cada waypoint
    public AudioSource footstepAudioSource; // Som de passos


    public GameObject[] videoObjects; // Objetos associados aos v�deos
    public float videoActivationDistance = 3f; // Dist�ncia para ativar o Video Player


    private Animator animator; // Refer�ncia ao Animator do Avatar

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>(); // Obt�m o Animator

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
            Debug.LogError("Avatar precisa de um Animator para a anima��o.");
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
            return; // Se j� guiou, n�o faz nada

        if (!guiding && Vector3.Distance(player.position, transform.position) <= proximityRadius)
        {
            PlayWelcomeAnimation(); // Tocar anima��o de boas-vindas
            StartCoroutine(GuideRoutine());
            guiding = true;
        }

        // Atualiza a anima��o com base no movimento do avatar
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

        // Toca som de passos enquanto o agente est� se movendo
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
            return; // S� ativa os v�deos ap�s o avatar terminar os waypoints

        for (int i = 0; i < videoObjects.Length; i++)
        {
            // Ativar apenas quando o jogador estiver pr�ximo
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

        // Reproduzir �udio de introdu��o
        if (introAudio != null)
        {
            audioSource.clip = introAudio;
            audioSource.Play();
            // Iniciar a anima��o de falar
            //animator.SetTrigger("StartTalking");
            yield return new WaitWhile(() => audioSource.isPlaying); // Esperar o �udio terminar
           // animator.SetTrigger("StopTalking");
        }


        // Mover entre os pontos principais
        for (int i = 0; i < keyPoints.Length; i++)
        {
            agent.SetDestination(keyPoints[i].position);

            yield return new WaitUntil(() =>
                !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance);


            //yield return new WaitForSeconds(stopTime);

            // Reproduzir �udio do waypoint, se existir
            if (i < waypointAudios.Length && waypointAudios[i] != null)
            {
                audioSource.clip = waypointAudios[i];
                audioSource.Play();
                animator.SetTrigger("StartTalking");

                yield return new WaitWhile(() => audioSource.isPlaying); // Esperar o �udio terminar

                animator.SetTrigger("StopTalking");
            }

            yield return new WaitForSeconds(stopTime); // Esperar no waypoint
        }


        // Retornar ao ponto inicial
        agent.SetDestination(initialPosition.position);
        yield return new WaitUntil(() =>
            !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance);


        // Transi��o suave para a rota��o inicial
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
        return hasGuided; // Retorna true se o avatar j� terminou de guiar
    }


    // Fun��o que controla a anima��o do avatar
    void UpdateAnimation()
    {
        if (agent.velocity.magnitude > 0.1f) // Se o avatar estiver se movendo
        {
            animator.SetBool("IsWalking", true); // Ativa a anima��o de caminhada
        }
        else
        {
            animator.SetBool("IsWalking", false); // Desativa a anima��o de caminhada (volta ao estado idle)
        }
    }
}
