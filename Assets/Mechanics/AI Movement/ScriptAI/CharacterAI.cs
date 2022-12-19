using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class CharacterAI : MonoBehaviour
{
    [SerializeField]
    protected Transform goal;

    [SerializeField]
    protected float stoppingDistance = 0.5f;

    [SerializeField]
    protected bool autoBreaking;

    [SerializeField]
    protected NavMeshAgent _agent;

    // [SerializeField] protected Collider radiusOfCharacter;
    [SerializeField] protected CharacterRadius radiusWithCol;
    protected abstract void MoveTo();
    // Start is called before the first frame update
    protected virtual void  Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.autoBraking = autoBreaking;
        _agent.destination = goal.position;
        // radiusWithCol = new CharacterRadius();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }

    
}
