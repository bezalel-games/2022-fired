using Avrahamy;
using Gilad;
using UnityEngine;

public class EnemyFireFighterScript : CharacterAI
{
    [SerializeField]
    [Range(0, 100)]
    private float initPerceantege;
    private float percentage;

    //how much does it take to extinguish fire in percent;
    [SerializeField]
    [Range(0, 100)]
    private float costToExtinguishFire;

    [SerializeField]
    private float timeToExtinguish = 0;

    [SerializeField] private WaterShooter _shooter;
    [SerializeField]
    private float minDistanceFromPlayer = 5.0f;

    [SerializeField]
    private float maxDistanceFromPlayer = 15.0f;
    private float angleMax = 30;
    [SerializeField]private PassiveTimer timeToGo;
    [SerializeField]private PassiveTimer whenStopShoot;
    

    [SerializeField]private PassiveTimer _timer; // TODO: use the PassiveTimer object!

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
        
        // Goal = Distance(player, transform) < minDistanceFromPlayer ||
        //        Distance(player, transform) > maxDistanceFromPlayer
        //     ? player
        //     : radiusWithCol.FindFire(transform);
        Goal = Distance(player, transform) < minDistanceFromPlayer
            ? player : FindFire(transform);
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
            Agent.SetDestination(RandomNavmeshLocation()); 
        }
        
    }

    private void HandleFire()
    {
        
        if (Agent.remainingDistance < stoppingDistance && (!_timer.IsSet || !_timer.IsActive))
        {
            if(IsFacing())
                ExtinguishFire();
            else
            {
               transform.rotation = Quaternion.LookRotation(Goal.position);
            }
        }
        else
        {
            Seek(Goal);
            // _shooter.StopShooting();
        }
        if (whenStopShoot.IsSet && !whenStopShoot.IsActive)
        {
            percentage -= costToExtinguishFire;
            whenStopShoot.Clear();
            _shooter.StopShooting();
            _timer.Start();
        }
        
        if (_timer.IsSet && !_timer.IsActive)
        {
            _timer.Clear();
        }


    }

    private void ExtinguishFire()
    {
        if (whenStopShoot.IsSet)
        {
            if (whenStopShoot.IsActive)
            {
                Seek(Goal);
                _shooter.StartShooting();
                
            }
        }
        else
        {
            whenStopShoot.Start();
            _shooter.StartShooting();
        }
        

        
        
        // here we need to call a function that put the fire of
        
    }
    
    private bool IsFacing()
    {
        float angleToPlayer = Vector3.Angle(Goal.transform.forward, (transform.position - Goal.transform.position));
        // Debug.Log(angleToPlayer);
        return angleToPlayer < angleMax;
    }
}
