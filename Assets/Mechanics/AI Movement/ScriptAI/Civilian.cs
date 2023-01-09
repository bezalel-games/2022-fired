using UnityEngine;
using Avrahamy;
using Logger = Nemesh.Logger;
using Gilad;


public class Civilian : CharacterAI
{
    // min distance from player, if farther then  civilian go back. 
    [SerializeField]
    private float maxDistanceFromPlayer = 20.0f;
    [SerializeField]
    private float minDistanceFromPlayer = 5.0f;
    [SerializeField]
    private Flammable m_Flammable;

    
    [SerializeField]private PassiveTimer timeToGo;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        Goal = player;
        Agent.SetDestination(RandomNavmeshLocation());
    }


    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if(timeToInit.IsSet && timeToInit.IsActive)
        {
            return;
        }
        
        
        if(!m_Flammable.IsOnFire())
        {
            // Agent.isStopped= true;
            MoveCharacter();
        }
        else
        {
            // Agent.isStopped= true;
            if(m_Flammable.IsDoneBurning())
                Agent.enabled = false;
        }
    }

    //the function from which the character moves
    protected override void MoveCharacter()
    {
        if (timeToGo.IsSet)
        {
            if (timeToGo.IsActive)
            {
                if (Agent.remainingDistance < stoppingDistance)
                {
                    Agent.SetDestination(RandomNavmeshLocation());
                }
            }
            else
            {
                Goal = FindFire(transform);
                if (Goal != null )
                {
                    RunAway(Goal);
                    timeToGo.Clear();
                }
                else
                {
                    Agent.SetDestination(RandomNavmeshLocation());
                    timeToGo.Clear();
                }
            }
        }
        else
        {
            timeToGo.Start();
        }
        // Goal = radiusWithCol.FindFire(transform);  // TODO: use the API instead!
        // if (Goal != null && Distance(transform, Goal) < minDistanceFromPlayer)
        // {
        //     timeToGo.Clear();
        //     RunAway(Goal);
        // }
        // // else if (Distance(transform, player) < minDistanceFromPlayer)
        // // {
        // //     RunAway(player);
        // // }
        // else if (Agent.remainingDistance < stoppingDistance)
        // {
        //     Agent.destination = RandomNavmeshLocation();
        // }
        // else if (Distance(transform, player) > maxDistanceFromPlayer)
        // {
        //     Agent.destination = RandomNavmeshLocation();
        //     // Seek(player);
        // }

    }
    
}
