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


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        Goal = null;
        Agent.destination = RandomNavmeshLocation();
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
        Goal = radiusWithCol.FindFire(transform);
        if (Goal != null)  // TODO: use and API - also this is done in all of the inheritances?
        {
            if (percentage > 0)
            {
                HandleFire();
            }
            else
            {
                RunAway(Goal);
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
            ExtinguishFire();
        }
        else
        {
            Seek(Goal);
        }

        if (_timer < timeToExtinguish)
        {
             _timer += Time.deltaTime;
             shooter.StopShooting();
        }
        
    }

    private void ExtinguishFire()
    {
        _timer = 0;
        percentage -= costToExtinguishFire;
        shooter.StartShooting();
        // here we need to call a function that put the fire of
    }
}
