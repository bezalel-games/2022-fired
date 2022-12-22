using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class CharacterAI : MonoBehaviour
{
    // the goal of said character
    protected Transform goal;
    
    // the player
    [SerializeField]
    protected Transform player;
    
    // the distance from which the 
    [SerializeField]
    protected float stoppingDistance = 0.5f;
    
    // the radius of said character 
    [SerializeField]
    protected float radius = 2f;
    
    // to auto brake
    [SerializeField]
    protected bool autoBreaking;
    
    // the agent (the character)
    protected NavMeshAgent _agent;
    
    // radius that gives us the fire object (more about that in CharacterRadius)
    [SerializeField] protected CharacterRadius radiusWithCol;
    
    //the function from which we decide how the character would move
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
