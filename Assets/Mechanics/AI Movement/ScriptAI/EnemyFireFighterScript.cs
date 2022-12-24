using UnityEngine;

public class EnemyFireFighterScript : CharacterAI
{
    [SerializeField]
    [Range(0, 100)]
    private float perceantege;

    //how much does it take to extinguish fire in percent;
    [SerializeField]
    private float costToExtinguishFire;

    [SerializeField]
    private float timeToExtinguish = 0;

    [SerializeField]
    private float minDistanceFromPlayer = 5.0f;

    [SerializeField]
    private float maxDistanceFromPlayer = 15.0f;

    private float _timer; // TODO: use the PassiveTimer object!

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        Goal = player;
        Agent.destination = RandomNavmeshLocation();
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
        Goal = Distance(player, transform) < minDistanceFromPlayer ||
               Distance(player, transform) > maxDistanceFromPlayer
            ? player
            : radiusWithCol.FindFire(transform);
        if (Goal != null) // TODO: use an API
        {
            if (perceantege > 0)
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
        Seek(Goal);
        if (Agent.remainingDistance < stoppingDistance && _timer >= timeToExtinguish)
        {
            ExtinguishFire();
        }

        if (_timer < timeToExtinguish)
            _timer += Time.deltaTime;

    }

    private void ExtinguishFire()
    {
        _timer = 0;
        perceantege -= costToExtinguishFire;
        // here we need to call a function that put the fire of
    }
}
