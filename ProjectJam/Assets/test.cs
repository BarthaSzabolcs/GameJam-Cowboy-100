using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class test : MonoBehaviour
{
    private NavMeshAgent agent;

    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            agent.destination = RandomDestination();
        }
    }

    private void OnDrawGizmos()
    {
        if (agent != null)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawSphere(agent.destination, 1);
        }
    }

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private Vector3 RandomDestination()
    {
        var randomVector = new Vector3(Random.Range(0, 1), Random.Range(0, 1), 0).normalized;
        randomVector *= 5;

        return transform.position + Vector3.up * 5;
    }
}
