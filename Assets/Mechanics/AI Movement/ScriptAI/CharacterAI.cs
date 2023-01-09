// using BitStrap;

using UnityEngine;
using UnityEngine.AI;
using Gilad;
using System.Linq;
using Avrahamy;
using Avrahamy.EditorGadgets;

public abstract class CharacterAI : MonoBehaviour
{
    [Header("CharacterAI Base")]
    // the player
    [SerializeField]
    protected Transform player;

    // the distance from which the 
    [SerializeField]
    protected float stoppingDistance = 5f;

    // the radius of said character 
    [SerializeField]
    protected float radius = 2f;

    // to auto brake
    [SerializeField]
    protected bool autoBreaking;
    
    [SerializeField]
    protected float angleMax = 45;
    
    protected PassiveTimer timeToInit;

    // radius that gives us the fire object (more about that in CharacterRadius)
    // [SerializeField]
    // protected CharacterRadius radiusWithCol;

    [SerializeField]
    [HideInInspector]
    protected float distanceToStopFromFire = 2f;

    [Space]
    [Header("Movement Animation Parameters")]
    [SerializeField]
    protected float speed;

    [SerializeField]
    private float speedChangeRate = 10f;

    // the agent (the character)
    protected NavMeshAgent Agent;

    
    [SerializeField]
    [ReadOnly]
    // the goal of said character
    protected Transform Goal;

    [SerializeField]
    [ReadOnly]
    protected Flammable fireGoal;

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
        timeToInit = new PassiveTimer(Random.Range(0f, 1f));
        timeToInit.Start();
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
        Vector3 center = transform.position;
        for (int i = 0; i < radius; i++)
        {
            Vector3 res;
            if (RandomPoint(center, 10f, out res))
            {
                center = res;
            }
            
        }
        return center;
        // for (int i = 0; i < 30; i++)
        // {
        //     if (NavMesh.SamplePosition(randomDirection, out hit, radius, NavMesh.AllAreas))
        //     {
        //        return hit.position;
        //     }
        // }
        
    }

    //makes character run from goal
    protected virtual void RunAway(Transform runFrom)
    {
        var position = transform.position;
        Vector3 dirToFire = position - runFrom.position;
        Vector3 newPos = position + dirToFire;
        Agent.SetDestination(newPos);
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
        fireGoal = max;
        return max.gameObject.transform;
    }
    
    protected bool IsFacing()
    {
        float angleToPlayer = Vector3.Angle(transform.forward, (Goal.position - transform.position).normalized);
        return Mathf.Abs(angleToPlayer) < angleMax;
    }
    
    // public float range = 10.0f;
    bool RandomPoint(Vector3 center, float range, out Vector3 result) {
        for (int i = 0; i < 30; i++) {
            Vector3 randomPoint = center + Random.insideUnitSphere * range;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, Agent.height * 2f, NavMesh.AllAreas)) {
                result = hit.position;
                return true;
            }
        }
        result = Vector3.zero;
        return false;
    }


}
