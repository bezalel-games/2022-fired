using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// MoveTo.cs
using UnityEngine.AI;


public class MoveTo : MonoBehaviour
{

    [SerializeField]
    private Transform goal;

    private NavMeshAgent _agent;

    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.destination = goal.position;
    }

}
