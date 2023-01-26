using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gilad;
using Logger = Nemesh.Logger;
using Avrahamy;
using BitStrap;

public class flyingEnemy : CharacterAI
{
    [Space(2)]
    [Header("Flying Enemy")]
    [SerializeField]
    [Range(0, 100)]
    private float initPerceantege;
    
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

    
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        Goal = player;
        var goal = ToXZ(RandomNavmeshLocation());
        Agent.SetDestination(goal);
        percentage = initPerceantege;
       
    }

    protected Vector3 ToXZ(Vector3 vector)
    {
        return new Vector3(vector.x, transform.position.y, vector.y);
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

    
    // protected override void MoveCharacter()
    // {
    //     if (!timeToGo.IsSet)
    //     {
    //         timeToGo.Start();
    //     }
    //     if (timeToGo.IsActive)
    //     {
    //         return;
    //     }
    //
    //     // Goal = Distance(player, transform) < minDistanceFromPlayer ||
    //     //        Distance(player, transform) > maxDistanceFromPlayer
    //     //     ? player
    //     //     : radiusWithCol.FindFire(transform);
    //     Goal = Distance(player, transform) < minDistanceFromPlayer
    //         ? player
    //         : FindFire(transform);
    //     if (Goal != null) // TODO: use an API
    //     {
    //
    //         if (percentage > 0)
    //         {
    //             HandleFire();
    //         }
    //         else
    //         {
    //             _shooter.StopShooting();
    //             if (timeToGo.IsSet)
    //             {
    //                 if (timeToGo.IsActive)
    //                 {
    //                 }
    //                 else
    //                 {
    //                     timeToGo.Clear();
    //                     RunAway(Goal);
    //                 }
    //             }
    //             else
    //             {
    //                 timeToGo.Start();
    //             }
    //         }
    //
    //     }
    //     else if (Agent.remainingDistance < stoppingDistance)
    //     {
    //         _shooter.StopShooting();
    //         var t = Agent.SetDestination(ToXZ(RandomNavmeshLocation()));
    //         Logger.Log($"{Agent.destination} : {t}", this);
    //
    //     }
    //     timeToGo.Start();
    // }
    
    //the function from which the character moves
    protected override void MoveCharacter()
    {
        if (!timeToGo.IsSet)
        {
            timeToGo.Start();
        }

        if (!timeToGo.IsActive)
        {
            if (stayNearInitPos && Vector3.Distance(initPos, transform.position) < theAreaToCover)
            {
                Agent.SetDestination(initPos);
                return;
            }

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

            timeToGo.Clear();
            wentRandom = false;
        }

        if (Goal != null && GoalOnfire()) // TODO: use an API
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
            if (!Agent.pathPending && Agent.remainingDistance < stoppingDistance || !wentRandom)
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
        if (Agent.remainingDistance < distanceToStopFromFire + 1f)
        {
            transform.LookAt(Goal.position);  // TODO: use slerp/lerp to rotate gradually
            Agent.updateRotation = false;

            if (IsFacing())
            {
                if (!(timeBetweenShots.IsSet && timeBetweenShots.IsActive))  // TODO patch
                {
                    ExtinguishFire();
                }
                else
                {
                    Seek(Goal);
                }
            }
            else
            {
                Seek(Goal);
            }
        }
        else
        {
            Agent.updateRotation = true;
            Seek(Goal);
        }
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

    protected override void Seek(Transform other)
    {
        Agent.SetDestination(ToXZ(other.position));
    }

    protected override void RunAway(Transform runFrom)
    {
        var position = transform.position;
        Vector3 dirToFire = position - runFrom.position;
        // transform.rotation = Quaternion.LookRotation(dirToFire);
        Vector3 newPos = position + dirToFire;
        Agent.SetDestination(ToXZ(newPos));
    }
}
