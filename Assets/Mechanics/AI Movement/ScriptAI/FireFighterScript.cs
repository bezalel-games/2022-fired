using Avrahamy;
using Gilad;
using UnityEngine;

public class FireFighterScript : CharacterAI
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


    [SerializeField] private WaterShooter shooter;
    private float _timer;  // TODO: switch to PassiveTimer!
    private bool fireShoot;
    [SerializeField]private PassiveTimer timeToGo;


    // Start is called before the first frame update
    protected override void Start()
    {
        fireShoot = false;
        base.Start();
        Goal = null;
        Agent.SetDestination(RandomNavmeshLocation());
        percentage = initPerceantege;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if(timeToInit.IsSet && !timeToInit.IsActive)
        {
            return;
        }
        MoveCharacter();
    }

    //the function from which the character moves
    protected override void MoveCharacter()
    {
        Goal = FindFire(transform);
        if (Goal != null)  // TODO: use and API - also this is done in all of the inheritances?
        {
            if (percentage > 0)
            {
                HandleFire();
            }
            else
            {
                if (timeToGo.IsSet)
                {
                    if (timeToGo.IsActive)
                    {
                    }
                    else
                    {
                        timeToGo.Clear();
                        shooter.StopShooting();
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
            Agent.destination = RandomNavmeshLocation();
        }
    }

    private void HandleFire()
    {
        if (Agent.remainingDistance < stoppingDistance && _timer >= timeToExtinguish)
        {
            if( IsFacing())
                ExtinguishFire();
            else
            {
                transform.rotation = Quaternion.LookRotation(Goal.position);
            }
        }
        else
        {
            Seek(Goal);
            // shooter.StopShooting();
        }

        if (_timer < timeToExtinguish)
        {
             _timer += Time.deltaTime;
        }
        
    }

    private void ExtinguishFire()
    {
        _timer = 0;
        percentage -= costToExtinguishFire;
        shooter.StartShooting();
        // here we need to call a function that put the fire of
    }
    
    // private bool IsFacing()
    // {
    //     float angleToPlayer = Vector3.Angle(Goal.transform.forward, (transform.position - Goal.transform.position));
    //     // Debug.Log(angleToPlayer);
    //     return angleToPlayer < angleMax;
    // }
}
