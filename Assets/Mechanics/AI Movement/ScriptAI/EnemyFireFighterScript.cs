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
    

    private float _timer; // TODO: use the PassiveTimer object!

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        Goal = player;
        Agent.destination = player.transform.position;
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
            ? player : radiusWithCol.FindFire(transform);
        if (Goal != null) // TODO: use an API
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
                        _shooter.StopShooting();
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
            Agent.destination = RandomNavmeshLocation(); 
        }
    }

    private void HandleFire()
    {
        if (Agent.remainingDistance < stoppingDistance && _timer >= timeToExtinguish&& IsFacing())
        {
            ExtinguishFire();
        }
        else
        {
            Seek(Goal);
            // _shooter.StopShooting();
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
        // here we need to call a function that put the fire of
        _shooter.StartShooting();
    }
    
    private bool IsFacing()
    {
        float angleToPlayer = Vector3.Angle(Goal.transform.forward, (transform.position - Goal.transform.position));
        // Debug.Log(angleToPlayer);
        return angleToPlayer < angleMax;
    }
}
