using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;


public class civilian : CharacterAI
{
    // min distance from player, if farther then  civilian go back. 
    [SerializeField]
    private float MinDistanceFromPlayer = 10.0f;
    [SerializeField]
    private float angleMax = 45;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        goal = null;
        _agent.destination = RandomNavmeshLocation();
    }

   

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        MoveCharacter();
    }

    protected override void MoveCharacter()
    {
        goal = radiusWithCol.FindFire(transform);
        if(goal != null)
            RunAway();
        else if (_agent.remainingDistance < stoppingDistance)
        {
            _agent.destination = RandomNavmeshLocation();
        }
        else if (Distance(transform, goal) < MinDistanceFromPlayer)
        {
            transform.rotation = Quaternion.LookRotation(player.transform.position);
            _agent.destination = player.position;
        }
        
    }
    
    private void RunAway()
    {
        Vector3 dirToPlayer = transform.position - goal.transform.position;
        transform.rotation = Quaternion.LookRotation(dirToPlayer);

        Vector3 newPos = transform.position + dirToPlayer;
        // _agent.SetDestination(newPos);        
        _agent.destination = newPos;        
    }
    
    private float Distance(Transform in_player, Transform me)
    {
        Vector3 FixedPlayer = in_player.transform.position;
        FixedPlayer.y = 0;
        Vector3 FixedMe = me.position;
        FixedMe.y = 0;
        float distance = Vector3.Distance(FixedMe ,FixedPlayer);
        return distance;
    }
    
    public Vector3 RandomNavmeshLocation() {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += transform.position;
        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;
        if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1)) {
            finalPosition = hit.position;            
        }
        return finalPosition;
    }  
    
    private bool ISFacing()
    {
        float angleToPlayer = Vector3.Angle(player.transform.forward,( transform.position - player.transform.position));
        // Debug.Log(angleToPlayer);
        return angleToPlayer < angleMax;
    }
}
