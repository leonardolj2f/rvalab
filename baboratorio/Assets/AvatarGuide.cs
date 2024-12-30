using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class AvatarGuide : MonoBehaviour
{
    public Transform[] keyPoints; // Locais principais (3 pontos)
    public Transform initialPosition; // Posi��o inicial do avatar
    //private Quaternion initialRotation; // Rota��o inicial do avatar

    public float proximityRadius = 5f; // Raio de detec��o do jogador
    public Transform player; // Refer�ncia ao jogador (XR Rig)
    public float stopTime = 20f; // Tempo de pausa em cada ponto
    private NavMeshAgent agent; // Controle do movimento do avatar
    private int currentPoint = 0;
    private bool guiding = false;

    float rotationSpeed = 0.5f; // Velocidade de transi��o da rota��o
    // Refer�ncia ao Animator do Avatar
    private Animator animator;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>(); // Obt�m o Animator

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
        if (!guiding && Vector3.Distance(player.position, transform.position) <= proximityRadius)
        {
            guiding = true;
            StartCoroutine(GuideRoutine());
        }

        // Atualiza a anima��o com base no movimento do avatar
        UpdateAnimation();
    }

    IEnumerator GuideRoutine()
    {
        // Mover entre os pontos principais
        for (int i = 0; i < keyPoints.Length; i++)
        {
            agent.SetDestination(keyPoints[i].position);
            yield return new WaitUntil(() => agent.remainingDistance <= agent.stoppingDistance);
            yield return new WaitForSeconds(stopTime);
        }

        // Retornar ao ponto inicial
        agent.SetDestination(initialPosition.position);
        yield return new WaitUntil(() => agent.remainingDistance <= agent.stoppingDistance);

        // Transi��o suave para a rota��o inicial
        while (Quaternion.Angle(transform.rotation, initialPosition.rotation) > 1f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, initialPosition.rotation, rotationSpeed * Time.deltaTime);
            yield return null;
        }

        // Finalizar o guia
        guiding = false;
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
