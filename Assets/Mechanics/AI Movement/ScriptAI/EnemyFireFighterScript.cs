using Avrahamy;
using BitStrap;
using Gilad;
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
            timeToGo.Clear();
            wentRandom = false;
        }

        // Goal = Distance(player, transform) < minDistanceFromPlayer ||
        //        Distance(player, transform) > maxDistanceFromPlayer
        //     ? player
        //     : radiusWithCol.FindFire(transform);
        // Goal = Distance(player, transform) < minDistanceFromPlayer
        //     ? player
        //     : FindFire(transform);
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
            if (!Agent.pathPending && Agent.remainingDistance < stoppingDistance|| !wentRandom)
            {
                var t = Agent.SetDestination(RandomNavmeshLocation());
                timeToGo.Clear();
                wentRandom = true;
            }
        }
        
        // timeToGo.Start();
        
        
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
