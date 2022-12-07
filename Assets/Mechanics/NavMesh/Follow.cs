using UnityEngine;
using UnityEngine.AI;

public class Follow : MonoBehaviour
{
    [SerializeField]
    private Transform goal;

    [SerializeField]
    private float stoppingDistance = 0.5f;

    [SerializeField]
    private bool autoBreaking;

    private NavMeshAgent _agent;

    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.autoBraking = autoBreaking;
        _agent.destination = goal.position;
    }

    void GotoNextPoint()
    {
        _agent.destination = goal.position;
    }


    void Update()
    {
        GotoNextPoint();
    }

}
