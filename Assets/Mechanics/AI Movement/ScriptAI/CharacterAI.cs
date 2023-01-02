// using BitStrap;

using UnityEngine;
using UnityEngine.AI;
using Gilad;
using System.Linq;

public abstract class CharacterAI : MonoBehaviour
{
    [Header("CharacterAI Base")]
    // the player
    [SerializeField]
    protected Transform player;

    // the distance from which the 
    [SerializeField]
    protected float stoppingDistance = 15f;

    // the radius of said character 
    [SerializeField]
    protected float radius = 2f;

    // to auto brake
    [SerializeField]
    protected bool autoBreaking;

    // radius that gives us the fire object (more about that in CharacterRadius)
    [SerializeField]
    protected CharacterRadius radiusWithCol;

    [SerializeField]
    [HideInInspector]
    protected float distanceToStopFromFire = 15f;

    [Space]
    [Header("Movement Animation Parameters")]
    [SerializeField]
    protected float speed;

    [SerializeField]
    private float speedChangeRate = 10f;

    // the agent (the character)
    protected NavMeshAgent Agent;

    // the goal of said character
    protected Transform Goal;

    protected MovementAnimationControl MyAnimationController;

    protected bool HasAnimator;

    //the function from which we decide how the character would move
    protected abstract void MoveCharacter();

    // Start is called before the first frame update
    protected virtual void Start()
    {
        HasAnimator = TryGetComponent(out MyAnimationController);
        TryGetComponent(out Agent);
        Agent.autoBraking = autoBreaking;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (!HasAnimator)
        {
            return;
        }

        var velocity = Agent.velocity;
        var currentHorizontal = new Vector3(velocity.x, 0.0f, velocity.z);
        float inputMagnitude = currentHorizontal.magnitude;
        float speedOffset = 0.1f;

        if (inputMagnitude < speed - speedOffset ||
            inputMagnitude > speed + speedOffset)
        {
            speed = Mathf.Lerp(inputMagnitude,
                this.speed,
                Time.deltaTime * speedChangeRate);

            speed = Mathf.Round(speed * 1000f) / 1000f;
        }
        else
        {
            speed = speed * inputMagnitude;
        }

        MyAnimationController.TargetDirection = velocity;
        var motionSpeed = speed;
        MyAnimationController.Speed = motionSpeed;
        
        // TODO: add rotation parameter like in ThirdPersonController
    }

    // this function returns random point in the radius of the character
    protected Vector3 RandomNavmeshLocation()
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += transform.position;
        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;
        if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1))
        {
            finalPosition = hit.position;
        }

        return finalPosition;
    }

    //makes character run from goal
    protected void RunAway(Transform runFrom)
    {
        var position = transform.position;
        Vector3 dirToFire = position - runFrom.position;
        // transform.rotation = Quaternion.LookRotation(dirToFire);
        Vector3 newPos = position + dirToFire;
        Agent.SetDestination(newPos);
    }

    protected void Seek(Transform other)
    {
        var position = other.position;
        // transform.rotation = Quaternion.LookRotation(position);
        Agent.SetDestination(position);
    }

    //calculate distance between two transforms
    protected static float Distance(Transform inPlayer, Transform me)
    {
        Vector3 fixedPlayer = inPlayer.transform.position;
        fixedPlayer.y = 0;
        Vector3 fixedMe = me.position;
        fixedMe.y = 0;
        float distance = Vector3.Distance(fixedMe, fixedPlayer);
        return distance;
    }

    protected Transform FindFire(Transform trans)
    {
        var listOfFlames = Flammable.AllBurning;
        if (listOfFlames.Count == 0)
        {
            return null;
        }

        var max = listOfFlames.First(t => t != null); // TODO: use some sort of API
        if (max == null) return null;
        var curMax = Distance(max.transform, trans);
        foreach (var curFire in Flammable.AllFlammables)
        {
            var cur = Distance(curFire.transform, trans);
            if ((Distance(max.transform, trans)) < curMax)
            {
                curMax = cur;
                max = curFire;
            }

        }

        return max.gameObject.transform;
    }


}
