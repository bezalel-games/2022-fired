using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class CharacterAI : MonoBehaviour
{
    protected Transform goal;
    [SerializeField]
    protected Transform player;
    [SerializeField]
    protected float stoppingDistance = 0.5f;
    [SerializeField]
    protected float radius = 2f;
    [SerializeField]
    protected bool autoBreaking;
    
    protected NavMeshAgent _agent;
    [SerializeField] protected CharacterRadius radiusWithCol;
    protected abstract void MoveCharacter();
    // Start is called before the first frame update
    protected virtual void  Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.autoBraking = autoBreaking;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }

    
}
