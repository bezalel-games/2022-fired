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
    private float angleMax = 45;

    [SerializeField]
    [ReadOnly]
    private float percentage;
    
    
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        Goal = player;
        Agent.SetDestination(player.transform.position);
        percentage = initPerceantege;
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
        if (!timeToGo.IsSet)
        {
            timeToGo.Start();
        }
        if (timeToGo.IsActive)
        {
            return;
        }

        // Goal = Distance(player, transform) < minDistanceFromPlayer ||
        //        Distance(player, transform) > maxDistanceFromPlayer
        //     ? player
        //     : radiusWithCol.FindFire(transform);
        Goal = Distance(player, transform) < minDistanceFromPlayer
            ? player
            : FindFire(transform);
        if (Goal != null) // TODO: use an API
        {

            if (percentage > 0)
            {
                HandleFire();
            }
            else
            {
                _shooter.StopShooting();
                if (timeToGo.IsSet)
                {
                    if (timeToGo.IsActive)
                    {
                    }
                    else
                    {
                        timeToGo.Clear();
                        RunAway(Goal);
                    }
                }
                else
                {
                    timeToGo.Start();
                }
            }

        }
        else if (Agent.remainingDistance < stoppingDistance)
        {
            _shooter.StopShooting();
            var t = Agent.SetDestination(RandomNavmeshLocation());
            Logger.Log($"{Agent.destination} : {t}", this);

        }
        timeToGo.Start();
    }

    private void HandleFire()
    {
        
        if (Agent.remainingDistance < stoppingDistance + 1f)
        {
            transform.LookAt(Goal.position);  // TODO: use slerp/lerp to rotate gradually
            Agent.updateRotation = false;

            if (IsFacing())
            {
                if (timeBetweenShots.IsSet && timeBetweenShots.IsActive || !timeBetweenShots.IsSet)  // TODO patch
                {
                    ExtinguishFire();
                }
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

    private bool IsFacing()
    {
        float angleToPlayer = Vector3.Angle(transform.forward, (Goal.position - transform.position).normalized);
        return Mathf.Abs(angleToPlayer) < angleMax;
    }
}
