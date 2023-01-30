// using BitStrap;

using UnityEngine;
using UnityEngine.AI;
using Gilad;
using System.Linq;
using Avrahamy;
using Avrahamy.EditorGadgets;
using GreatArcStudios;
using Logger = Nemesh.Logger;

[SelectionBase]
public abstract class CharacterAI : OptimizedBehaviour
{
    [Space]
    [Header("Movement Animation Parameters")]
    [SerializeField]
    [ReadOnly]
    protected float speed;

    [SerializeField]
    protected float walkSpeed = 2f;

    [SerializeField]
    protected float runSpeed = 10f;

    [SerializeField]
    private float speedChangeRate = 10f;
    
    [Header("CharacterAI Base")]
    // the player
    [SerializeField]
    protected Transform player;

    // the distance from which the 
    [SerializeField]
    protected float stoppingDistance = 0.5f;

    // the radius of said character 
    [SerializeField]
    protected float radius = 2f;

    // to auto brake
    [SerializeField]
    protected bool autoBreaking;

    [SerializeField]
    protected float angleMax = 45;
    [SerializeField] protected bool stayNearInitPos;
    protected PassiveTimer timeToInit;
    protected Vector3 initPos;
    [SerializeField]
    protected float theAreaToCover = 45f;

    // radius that gives us the fire object (more about that in CharacterRadius)
    // [SerializeField]
    // protected CharacterRadius radiusWithCol;

    [SerializeField]
    protected float distanceToStopFromFire = 2f;

    [SerializeField]
    [ReadOnly]
    // the goal of said character
    protected Transform Goal;

    [SerializeField]
    [ReadOnly]
    protected Flammable fireGoal;

    protected MovementAnimationControl MyAnimationController;

    [SerializeField]
    private bool debugDest;

    [SerializeField]
    [ReadOnly]
    private Vector3 dest;

    [SerializeField]
    private double minDistanceToFire = 20f;
    
    protected bool HasAnimator;

    protected int Attempts = 2;

    // the agent (the character)
    protected NavMeshAgent Agent;

    //the function from which we decide how the character would move
    protected abstract void MoveCharacter();

    // Start is called before the first frame update
    protected virtual void Start()
    {
        HasAnimator = TryGetComponent(out MyAnimationController);
        TryGetComponent(out Agent);
        Agent.autoBraking = autoBreaking;
        Agent.stoppingDistance = stoppingDistance;
        Agent.speed = runSpeed;
        timeToInit = new PassiveTimer(Random.Range(0f, 1f));
        timeToInit.Start();
        initPos = transform.position;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (debugDest)
        {
            dest = Agent.destination;
            Debug.DrawRay(dest, Vector3.up, Color.blue, 1.0f);
        }

        if (!HasAnimator || PauseManager.Paused)
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

        MyAnimationController.TargetDirection = transform.InverseTransformDirection(currentHorizontal);
        var motionSpeed = speed;
        MyAnimationController.Speed = motionSpeed;
        MyAnimationController.Walk = inputMagnitude <= walkSpeed + 0.1f;
        // TODO: add rotation parameter like in ThirdPersonController
    }

    // this function returns random point in the radius of the character
    protected Vector3 RandomNavmeshLocation()
    {
        Vector3 center = transform.position;

        if (!RandomPoint(center, radius, out var res))
        {
            if (debugDest)
            {
                Logger.Log(Attempts, this);
            }
        }

        return res;
    }

    //makes character run from goal
    protected virtual void RunAway(Transform runFrom)
    {
        var position = transform.position;
        Vector3 dirToFire = position - runFrom.position;
        Vector3 newPos = position + dirToFire; // TODO: NavMesh.SamplePosition
        Agent.SetDestination(newPos);
        Agent.speed = runSpeed;
    }

    protected virtual void Seek(Transform other)
    {
        // var position = other.position;
        Agent.SetDestination(other.position);
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
        if (max == null)
        {
            return null;
        }
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

        if (curMax < minDistanceToFire)
        {
            fireGoal = max;
            return max.gameObject.transform;
        }
        
        fireGoal = null;
        return null;
    }

    protected virtual bool IsFacing()
    {
        float angleToPlayer = Vector3.Angle(transform.forward, (Goal.position - transform.position).normalized);
        return Mathf.Abs(angleToPlayer) < angleMax;
    }

    // public float range = 10.0f;
    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        for (int i = 0; i < Attempts; i++)
        {
            var randomPoint = center + new Vector3(
                x: Random.Range(-range, range),
                y: 0f,
                z: Random.Range(-range, range)
            );
            if (NavMesh.SamplePosition(randomPoint, out var hit, Agent.height * 2f, NavMesh.AllAreas))
            {
                result = hit.position;
                Attempts = 2;
                return true;
            }
        }

        Attempts *= 2;
        result = center;
        return false;
    }


}
