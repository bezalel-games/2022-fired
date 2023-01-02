// using BitStrap;
using UnityEngine;
using UnityEngine.AI;
using Gilad;
using System.Linq;

public abstract class CharacterAI : MonoBehaviour
{
    // the goal of said character
    protected Transform Goal;

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

    // the agent (the character)
    protected NavMeshAgent Agent;

    // radius that gives us the fire object (more about that in CharacterRadius)
    [SerializeField]
    protected CharacterRadius radiusWithCol;
    
    protected MovementAnimationControl _myAnimationController;
    private bool _hasAnimator;


    //the function from which we decide how the character would move
    protected abstract void MoveCharacter();

    // Start is called before the first frame update
    protected virtual void Start()
    {
        _hasAnimator = TryGetComponent(out _myAnimationController);
        TryGetComponent(out Agent);
        Agent.autoBraking = autoBreaking;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (_hasAnimator)
        {
            _myAnimationController.TargetDirection = Agent.velocity;
            _myAnimationController.Speed = Agent.velocity.magnitude;
        }

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
        transform.rotation = Quaternion.LookRotation(dirToFire);
        Vector3 newPos = position + dirToFire;
        Agent.SetDestination(newPos);
    }

    protected void Seek(Transform other)
    {
        var position = other.position;
        transform.rotation = Quaternion.LookRotation(position);
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
