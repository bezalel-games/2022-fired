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
    private float MaxDistanceFromPlayer = 10.0f;
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

    //the function from which the character moves
    protected override void MoveCharacter()
    {
        goal = radiusWithCol.FindFire(transform);
        if(goal != null)
            RunAway(goal);
        else if (_agent.remainingDistance < stoppingDistance)
        {
            _agent.destination = RandomNavmeshLocation();
        }
        else if (Distance(transform, goal) < MaxDistanceFromPlayer)
        {
            Seek(player);
        }
    }
    
    
   
    
    private bool ISFacing()
    {
        float angleToPlayer = Vector3.Angle(player.transform.forward,( transform.position - player.transform.position));
        // Debug.Log(angleToPlayer);
        return angleToPlayer < angleMax;
    }
}
