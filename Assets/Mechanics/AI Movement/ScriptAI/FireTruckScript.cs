using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gilad;
using Avrahamy;
using BitStrap;
using UnityEngine.AI;
public class FireTruckScript : CharacterAI
{
    [Space(2)]
    [Header("FireTruck")]
    [SerializeField]
    [Range(0, 100)]
    private float initPerceantege;
    
    //how much does it take to extinguish fire in percent;
    [SerializeField]
    [Range(0, 100)]
    private float costToExtinguishFire;

    [SerializeField]
    private WaterShooter _shooter;

    [SerializeField]
    private float minDistanceFromPlayer = 8.0f;

    [SerializeField]
    private float maxDistanceFromPlayer = 15.0f;


    [SerializeField] private bool prioritizePlayer;
    [SerializeField]
    private PassiveTimer timeToGo;

    [SerializeField]
    private PassiveTimer shootDuration;

    [SerializeField]
    private PassiveTimer timeBetweenShots;

    [SerializeField] private GameObject cannonHolder;
    [SerializeField] private GameObject cannonHolder2;
    [SerializeField]
    [ReadOnly]
    private float percentage;
    [SerializeField]
    private Transform[] points;
    private bool wentRandom;
    private int destPoint = 0;
    
    
    
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        Goal = player;
        Agent.SetDestination(RandomNavmeshLocation());
        percentage = initPerceantege;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if(timeToInit.IsSet && timeToInit.IsActive)
        {
            return;
        }
        MoveCharacter();
    }

    //the function from which the character moves
    protected override void MoveCharacter()
    {
        if (!timeToGo.IsSet)
        {
            timeToGo.Start();
        }

        if (!timeToGo.IsActive)
        {
            // if (!Agent.pathPending && (
            //         Agent.remainingDistance < stoppingDistance ||
            //         Agent.pathStatus == NavMeshPathStatus.PathInvalid))
            // {
            //     GotoNextPoint();
            // }
            // else
            {
                var oldGoal = Goal;
                if(prioritizePlayer)
                {
                    Goal = Distance(player, transform) < minDistanceFromPlayer
                        ? player
                        : FindFire(transform);
                }
                else
                {
                    Goal = FindFire(transform);
                }
                if(Goal != null)
                {
                    Goal = Distance(player, transform) < Distance(Goal, transform) ? player : Goal;
                }
                else
                {
                    Goal = Distance(player, transform) < minDistanceFromPlayer
                        ? player
                        : null;
                }
                timeToGo.Clear();
                wentRandom = false;
                if (Goal != null && Goal != oldGoal)
                {
                    Seek(Goal);
                }
            }
            if (Goal != null && (GoalOnfire() || Goal == player)) // TODO: use an API
            {
            
                if (HandleFire())
                {
                }
                else
                {
                    wentRandom = false;
                    RunAway(Goal);
                }

            }
        else
        
            _shooter.StopShooting();
            if (!Agent.pathPending && (
                    Agent.remainingDistance < stoppingDistance ||
                    Agent.pathStatus == NavMeshPathStatus.PathInvalid))
            {
                GotoNextPoint();
                timeToGo.Clear();
                wentRandom = true;
            }
        }
        
    }

    //return true if go after the fire
    private bool HandleFire()
    {
        if (percentage <= 0)
        {
            _shooter.StopShooting();
            return false;
        }
        // bool toSeek = true;
        if (!Agent.pathPending && Agent.remainingDistance < distanceToStopFromFire + 1f)
        {
            
            cannonHolder2.transform.LookAt(new Vector3(Goal.position.x,cannonHolder2.transform.position.y,  Goal.position.z));
            if (IsFacing())
            {
                cannonHolder.transform.LookAt(new Vector3(Goal.position.x,cannonHolder.transform.position.y,  Goal.position.z));

                // cannonHolder.transform.LookAt(new Vector3(cannonHolder.transform.position.x,Goal.position.y,  cannonHolder.transform.position.z));
                if (!(timeBetweenShots.IsSet && timeBetweenShots.IsActive))  // TODO patch
                {
                    ExtinguishFire();
                }
                
            }
        }
        else
        {
            Agent.updateRotation = true;
        }

        // if (toSeek)
        // {
        //     Seek(Goal);
        // }
        if (shootDuration.IsSet && !shootDuration.IsActive)
        {
            percentage -= costToExtinguishFire;
            shootDuration.Clear();
            _shooter.StopShooting();
            timeBetweenShots.Start();
        }

        if (timeBetweenShots.IsSet && !timeBetweenShots.IsActive)
        {
            timeBetweenShots.Clear();
        }

        return true;
    }

    private bool GoalOnfire()
    {
        return fireGoal != null && fireGoal.IsOnFire() && !fireGoal.IsDoneBurning();
    }
    private void ExtinguishFire()
    {
        if (shootDuration.IsSet)
        {
            if (shootDuration.IsActive)
            {
                Seek(Goal);
                _shooter.StartShooting();
            }
        }
        else
        {
            shootDuration.Start();
            _shooter.StartShooting();
        }
        // here we need to call a function that put the fire of
    }
    
    void GotoNextPoint()
    {
        // Returns if no points have been set up
        if (points.Length == 0)
            return;
        // Set the agent to go to the currently selected destination.
        if (NavMesh.SamplePosition(points[destPoint].position, out var hit, Agent.height * 2f, Agent.areaMask))
        {
            Agent.SetDestination(hit.position);
            destPoint = (destPoint + 1) % points.Length;
        }
        // Agent.SetDestination(points[destPoint].position);

        // Choose the next point in the array as the destination,
        // cycling to the start if necessary.
        
        // destPoint = (destPoint + 1) % points.Length;
    }

    protected override bool IsFacing()
    {
        var forw = new Vector3(cannonHolder2.transform.forward.x, 0, cannonHolder2.transform.forward.z);
        var goalPos = new Vector3(Goal.position.x, 0, Goal.position.z);
        var transPos = new Vector3(cannonHolder2.transform.position.x, 0, cannonHolder2.transform.position.z);
        float angleToPlayer = Vector3.Angle(forw, (goalPos - transPos).normalized);
        return Mathf.Abs(angleToPlayer) < angleMax;
        // return base.IsFacing();
    }
}
