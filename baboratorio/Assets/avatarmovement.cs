using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AvatarMovement : MonoBehaviour
{
    public Transform[] waypoints; // Pontos para onde o avatar irá andar
    private NavMeshAgent agent;
    private int currentWaypoint = 0;

    void Start()
    {
        // Configura o NavMeshAgent e define o primeiro destino
        agent = GetComponent<NavMeshAgent>();
        if (waypoints.Length > 0)
        {
            MoveToNextWaypoint();
        }
    }

    void Update()
    {
        // Verifica se o avatar chegou próximo ao destino
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            MoveToNextWaypoint(); // Passa para o próximo waypoint
        }
    }

    void MoveToNextWaypoint()
    {
        if (waypoints.Length == 0) return;

        // Atualiza o waypoint atual
        currentWaypoint = (currentWaypoint + 1) % waypoints.Length;

        // Faz o avatar olhar para o próximo waypoint antes de começar a andar
        LookAtTarget(waypoints[currentWaypoint]);

        // Define o próximo destino no NavMeshAgent
        agent.SetDestination(waypoints[currentWaypoint].position);
    }

    void LookAtTarget(Transform target)
    {
        if (target == null) return;

        // Calcula a direção para o próximo waypoint
        Vector3 direction = target.position - transform.position;
        direction.y = 0; // Mantém a rotação no plano horizontal

        // Faz o avatar girar suavemente para olhar para o próximo waypoint
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }
}
