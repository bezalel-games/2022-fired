using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gilad;
using Logger = Nemesh.Logger;
using Avrahamy;
using BitStrap;
using GreatArcStudios;
using UnityEngine.AI;

public class flyingEnemy : CharacterAI
{
    [Space(2)]
    [Header("Flying Enemy")]
    [SerializeField]
    [Range(0, 100)]
    private float initPerceantege;
    [SerializeField]
    private float stoppingFromPlayerIfFire = 5;
    
    //how much does it take to extinguish fire in percent;
    [SerializeField]
    [Range(0, 100)]
    private float costToExtinguishFire;

    [SerializeField]
    private float timeToExtinguish = 0; 
   

    [SerializeField]
    private WaterShooter _shooter;

    [SerializeField]
    private float minDistanceFromPlayer = 5.0f;

    [SerializeField]
    private float maxDistanceFromPlayer = 15.0f;

    [SerializeField]
    private PassiveTimer timeToGo;

    [SerializeField]
    private PassiveTimer shootDuration;

    [SerializeField]
    private PassiveTimer timeBetweenShots;

    [SerializeField]
    [ReadOnly]
    private float percentage;
    
    private bool wentRandom;
    [SerializeField] private bool prioritizePlayer = true;
    [SerializeField] private GameObject cannonHolder;

    
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        Goal = null;
        var goal = ToXZ(RandomNavmeshLocation());
        Agent.SetDestination(goal);
        percentage = initPerceantege;
       
    }

    protected Vector3 ToXZ(Vector3 vector)
    {
        // return new Vector3(vector.x, transform.position.y, vector.z);
        return new Vector3(vector.x, 0, vector.z);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        
        if(timeToInit.IsSet && timeToInit.IsActive || PauseManager.Paused)
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
            if (stayNearInitPos && Vector3.Distance(initPos, transform.position) > theAreaToCover)
            {
                Agent.SetDestination(ToXZ(initPos));
                return;
            }

            var oldGoal = Goal;
            if (prioritizePlayer)
            {
                Goal = Distance(player, transform) < minDistanceFromPlayer
                    ? player
                    : FindFire(transform);
            }
            else
            {
                Goal = FindFire(transform);
            }

            if (Goal != null)
            {
                Goal = Distance(player, transform) < Distance(Goal, transform) ? player : Goal;
            }
            else
            {
                // Goal = Distance(player, transform) < minDistanceFromPlayer
                //     ? player
                //     : null;
                Goal = player;
            }

            timeToGo.Clear();
            wentRandom = false;
            // if (Goal != null && Goal != oldGoal)
            if (Goal != null )
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
        {
            _shooter.StopShooting();
            if (!Agent.pathPending && (Agent.remainingDistance < stoppingDistance || !wentRandom))
            {
                var t = Agent.SetDestination(ToXZ(RandomNavmeshLocation()));
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
        if (!Agent.pathPending && (Agent.remainingDistance < distanceToStopFromFire + 1f && Goal != player || Goal == player && Agent.remainingDistance < stoppingFromPlayerIfFire))
        {
            transform.LookAt(new Vector3(Goal.position.x, transform.position.y, Goal.position.z));  // TODO: use slerp/lerp to rotate gradually
            Agent.updateRotation = false;

            // if (IsFacing())
            {
                cannonHolder.transform.LookAt(Goal.position);
                if (!(timeBetweenShots.IsSet && timeBetweenShots.IsActive))  // TODO patch
                {
                    ExtinguishFire();
                    // toSeek = false;
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

    protected virtual void Seek(Transform other)
    {
        if (NavMesh.SamplePosition(new Vector3(other.position.x, transform.position.y, other.position.z), out var hit, Agent.height * 2f, Agent.areaMask))
        {
            Agent.SetDestination(hit.position);
        }
        // Agent.SetDestination(ToXZ(other.position));
    }

    protected override void RunAway(Transform runFrom)
    {
        
        // Agent.SetDestination(ToXZ(newPos));
        
        var position = transform.position;
        Vector3 dirToFire = position - runFrom.position;
        Vector3 newPos = position + dirToFire; // TODO: NavMesh.SamplePosition
        if (NavMesh.SamplePosition(new Vector3(newPos.x, transform.position.y, newPos.z), out var hit, Agent.height * 2f, Agent.areaMask))
        {
            Agent.SetDestination(hit.position);
        }
        Agent.speed = runSpeed;
    }

    protected override bool IsFacing()
    {
        float angleToPlayer = Vector3.Angle(ToXZ(transform.forward), (ToXZ(Goal.position) - ToXZ(transform.position)).normalized);
        return Mathf.Abs(angleToPlayer) < 60f;
    }
}
