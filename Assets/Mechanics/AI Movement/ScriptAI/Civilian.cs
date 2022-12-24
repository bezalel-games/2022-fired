using UnityEngine;


public class Civilian : CharacterAI
{
    // min distance from player, if farther then  civilian go back. 
    [SerializeField]
    private float maxDistanceFromPlayer = 10.0f;

    [SerializeField]
    private float angleMax = 45;

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
        var goal = radiusWithCol.FindFire(transform);  // TODO: use the API instead!
        if (goal != null)
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


    private bool IsFacing()
    {
        float angleToPlayer = Vector3.Angle(player.transform.forward, (transform.position - player.transform.position));
        // Debug.Log(angleToPlayer);
        return angleToPlayer < angleMax;
    }
}
