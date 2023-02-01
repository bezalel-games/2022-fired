using Avrahamy;
using Avrahamy.Math;
using BitStrap;
using Gilad;
using GreatArcStudios;
using UnityEngine;
using Logger = Nemesh.Logger;

public class EnemyFireFighterScript : CharacterAI
{
    [Space(2)]
    [Header("FireFighter")]
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

    [SerializeField]
    [ReadOnly]
    private float percentage;

    private bool wentRandom;
    
    
    
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        Goal = null;
        Agent.SetDestination(RandomNavmeshLocation());
        percentage = initPerceantege;
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
                Agent.SetDestination(initPos);
                return;
            }
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
        {
            _shooter.StopShooting();
            if (!Agent.pathPending &&  (Agent.remainingDistance < stoppingDistance || !wentRandom))
            {
                var t = Agent.SetDestination(RandomNavmeshLocation());
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
            var position = Goal.position;
            transform.LookAt(new Vector3(position.x, transform.position.y, position.z));  // TODO: use slerp/lerp to rotate gradually
            Agent.updateRotation = false;

            if (IsFacing())
            {
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
            else
            {
                // _shooter.StopShooting();
                // shootDuration.Clear();
                // timeBetweenShots.Start();
            }
        }
        else
        {
            shootDuration.Start();
            _shooter.StartShooting();
        }
        // here we need to call a function that put the fire of
    }
    

}
