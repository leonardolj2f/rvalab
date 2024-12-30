using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AvatarMovement : MonoBehaviour
{
    public Transform[] waypoints; // Pontos para onde o avatar ir� andar
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
        // Verifica se o avatar chegou pr�ximo ao destino
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            MoveToNextWaypoint(); // Passa para o pr�ximo waypoint
        }
    }

    void MoveToNextWaypoint()
    {
        if (waypoints.Length == 0) return;

        // Atualiza o waypoint atual
        currentWaypoint = (currentWaypoint + 1) % waypoints.Length;

        // Faz o avatar olhar para o pr�ximo waypoint antes de come�ar a andar
        LookAtTarget(waypoints[currentWaypoint]);

        // Define o pr�ximo destino no NavMeshAgent
        agent.SetDestination(waypoints[currentWaypoint].position);
    }

    void LookAtTarget(Transform target)
    {
        if (target == null) return;

        // Calcula a dire��o para o pr�ximo waypoint
        Vector3 direction = target.position - transform.position;
        direction.y = 0; // Mant�m a rota��o no plano horizontal

        // Faz o avatar girar suavemente para olhar para o pr�ximo waypoint
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }
}
