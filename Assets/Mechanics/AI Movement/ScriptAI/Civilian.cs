using UnityEngine;
using Avrahamy;
using Logger = Nemesh.Logger;
using Gilad;
using GreatArcStudios;
using UnityEngine.AI;


public class Civilian : CharacterAI
{
    // min distance from player, if farther then  civilian go back. 
    [SerializeField]
    private float maxDistanceFromPlayer = 20.0f;

    [SerializeField]
    private float minDistanceFromPlayer = 5.0f;

    [SerializeField]
    private Flammable m_Flammable;

    [SerializeField]
    private PassiveTimer timeToGo;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        Goal = player;
        Agent.SetDestination(RandomNavmeshLocation());
        Agent.speed = walkSpeed;
    }


    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (timeToInit.IsSet && timeToInit.IsActive || PauseManager.Paused)
        {
            return;
        }


        if (!m_Flammable.IsOnFire())
        {
            MoveCharacter();
        }
        else // TODO: keep moving when on fire!
        {
            Agent.speed = runSpeed;
            if (m_Flammable.IsDoneBurning())
            {
                MyAnimationController.DropDead = true;
                Agent.enabled = false;
            }
        }
    }

    //the function from which the character moves
    protected override void MoveCharacter()
    {
        if (timeToGo.IsSet)
        {
            if (!timeToGo.IsActive)
            {
                Goal = FindFire(transform);
                if (Goal != null)
                {
                    RunAway(Goal);
                    timeToGo.Clear();
                }
                else if (Agent.pathStatus != NavMeshPathStatus.PathComplete ||
                         !Agent.hasPath ||
                         (Agent.remainingDistance < stoppingDistance && !Agent.pathPending))
                {

                    Agent.SetDestination(RandomNavmeshLocation());
                    // Agent.speed = walkSpeed;  # TODO: do we want to return to walking after running?
                    timeToGo.Clear();
                }
            }

        }
        else
        {
            if (!Agent.pathPending) // TODO: pathPending check for all agents!
            {
                timeToGo.Start();
            }
        }
    }

}
