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
    
    // this function returns random point in the radius of the character
    protected Vector3 RandomNavmeshLocation() {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += transform.position;
        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;
        if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1)) {
            finalPosition = hit.position;            
        }
        return finalPosition;
    }  
    
    //makes character run from goal
    protected void RunAway(Transform runFrom)
    {
        Vector3 dirToFire = transform.position - runFrom.position;
        transform.rotation = Quaternion.LookRotation(dirToFire);
        Vector3 newPos = transform.position + dirToFire;       
        _agent.destination = newPos;        
    }

    protected void Seek(Transform other)
    {
        transform.rotation = Quaternion.LookRotation(other.position);
        _agent.destination = other.position;
    }
    //calculate distance between two transforms
    protected float Distance(Transform in_player, Transform me)
    {
        Vector3 FixedPlayer = in_player.transform.position;
        FixedPlayer.y = 0;
        Vector3 FixedMe = me.position;
        FixedMe.y = 0;
        float distance = Vector3.Distance(FixedMe ,FixedPlayer);
        return distance;
    }

    
}
