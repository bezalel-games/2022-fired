using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class civilian : CharacterAI
{
    // Start is called before the first frame update
    [SerializeField]
    private float MinDistance = 1.0f;
    protected override void Start()
    {
        base.Start();
    }

   

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    protected override void MoveTo()
    {
        goal = radiusWithCol.FindFire(transform);
        if(Distance(transform, goal)< MinDistance)
            RunAway();
    }
    
    private void RunAway()
    {
        Vector3 dirToPlayer = transform.position - goal.transform.position;
        transform.rotation = Quaternion.LookRotation(dirToPlayer);

        Vector3 newPos = transform.position + dirToPlayer;

        _agent.SetDestination(newPos);        
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

    

    // private void OnCollisionEnter(Collision collision)
    // {
    //     if(collision.gameObject )
    // }
}
