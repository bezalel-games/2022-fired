using GreatArcStudios;
using UnityEngine;

public class FleePlayer : CharacterAI
{
    // min distance from player, if farther then  character go back. 
    [SerializeField]
    private float maxDistanceFromPlayer = 10.0f;

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
        if (PauseManager.Paused)
        {
            return;
        }
        MoveCharacter();
    }

    //the function from which the character moves
    protected override void MoveCharacter()
    {
        if (IsFacing())
        {
            RunAway(Goal);
        }

        else if (Agent.remainingDistance < stoppingDistance)
        {
            Agent.destination = RandomNavmeshLocation();
        }
        else if (Distance(transform, Goal) > maxDistanceFromPlayer)
        {
            Seek(player);
        }

    }

    // does player face character?
    // private bool IsFacing()
    // {
    //     var playerTransform = player.transform;
    //     float angleToPlayer = Vector3.Angle(playerTransform.forward, (transform.position - playerTransform.position));
    //     // Debug.Log(angleToPlayer);
    //     return angleToPlayer < angleMax;
    // }
}
