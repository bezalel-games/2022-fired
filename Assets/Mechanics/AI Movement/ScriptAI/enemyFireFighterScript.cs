using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyFireFighterScript : CharacterAI
{
    [SerializeField][Range(0,100)] private float Perceantege;

    //how much does it take to extinguish fire in percent;
    [SerializeField] private float costToExtinguishFire;
    [SerializeField] private float timeToExtinguish = 0;
    private float timer;
    [SerializeField]
    private float MinDistanceFromPlayer = 5.0f;
    [SerializeField]
    private float MaxDistanceFromPlayer = 15.0f;
    
    
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        goal = player;
        _agent.destination = RandomNavmeshLocation();
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
        goal = Distance(player, transform) < MinDistanceFromPlayer || Distance(player, transform) > MaxDistanceFromPlayer ? player : radiusWithCol.FindFire(transform);
        if (goal != null)
        {
            if (Perceantege > 0)
            {
                HandleFire();
            }
            else
            {
                RunAway(goal);
            }
        }
        else if (_agent.remainingDistance < stoppingDistance)
        {
            _agent.destination = RandomNavmeshLocation();
        }
    }

    private void HandleFire()
    {
        Seek(goal);
        if (_agent.remainingDistance < stoppingDistance && timer >= timeToExtinguish)
        {
            ExtinguishFire();
        }
        if (timer < timeToExtinguish)
            timer += Time.deltaTime;

    }

    private void ExtinguishFire()
    {
        timer = 0;
        Perceantege -= costToExtinguishFire;
        // here we need to call a function that put the fire of
    }
}
