using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Flee : MonoBehaviour
{
    // [SerializeField]
    //     private Transform goal;
    //
    //     [SerializeField]
    //     private float stoppingDistance = 0.5f;
    //    
    //     [SerializeField]
    //     private float whenToRunDistance = 0.5f;
    //
    [SerializeField]
    private bool autoBreaking;
    //
    //     private UnityEngine.AI.NavMeshAgent _agent;
    //
    //     void Start()
    //     {
    //         _agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    //         _agent.autoBraking = autoBreaking;
    //         _agent.destination = goal.position;
    //     }
    //
    //     void GotoNextPoint()
    //     {
    //         if (Vector3.Distance(transform.position, goal.position) < whenToRunDistance)
    //         {
    //             Vector3 go = goal.position - transform.position;
    //             _agent.SetDestination(transform.position - go);
    //         }
    //         // _agent.destination = goal.position;
    //     }
    //
    //
    //     void Update()
    //     {
    //         GotoNextPoint();
    //     }
     [SerializeField]
    private NavMeshAgent _agent;
    
    public GameObject   player;
    [SerializeField]
    private Transform goal;

    private Vector3 goalVec;
    private float EnemyDistanceRun = 1f;
    [SerializeField]
    private float angleMax = 45;

    [SerializeField] private float radiusToGo = 1f;
    
    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.autoBraking = autoBreaking;
        _agent.destination = goal.position;
    }
    
    void Update()
    {
        UpdateNavmesh();
        
    }
    private float Distance(Transform in_player, Transform me)
    {
        Vector3 FixedPlayer = in_player.transform.position;
        FixedPlayer.y = 0;
        Vector3 FixedMe = me.position;
        FixedMe.y = 0;
        float distance = Vector3.Distance(FixedMe ,FixedPlayer);
        return distance;
    }
    private void RunAway()
    {
        Vector3 dirToPlayer = transform.position - player.transform.position;
        transform.rotation = Quaternion.LookRotation(dirToPlayer);

        Vector3 newPos = transform.position + dirToPlayer;

        _agent.SetDestination(newPos);        
    }

    private void UpdateNavmesh()
    {
        if(Distance(player.transform,this.transform) < EnemyDistanceRun)
        {
            RunAway();
            if(!ISFacing())
            {
            GetToPlayer();
            }
            else
            {
            RunAway();
            }
        }
        else
        {
            _agent.SetDestination( RandomNavmeshLocation(radiusToGo*5));
            // if (Vector3.Distance(goalVec,transform.position)< 0.1f)
            // goalVec = PickRandomPoint();
            // _agent.SetDestination(goalVec);
        }
    }
     private void GetToPlayer()
     {
         transform.rotation = Quaternion.LookRotation(player.transform.position);
         _agent.SetDestination(player.transform.position);
     }
     private bool ISFacing()
     {
         float angleToPlayer = Vector3.Angle(player.transform.forward,( transform.position - player.transform.position));
         // Debug.Log(angleToPlayer);
         return angleToPlayer < angleMax;
     }
    public Vector3 RandomNavmeshLocation(float radius) {
            Vector3 randomDirection = Random.insideUnitSphere * radius;
            randomDirection += transform.position;
            NavMeshHit hit;
            Vector3 finalPosition = Vector3.zero;
            // finalPosition += transform.position;
            if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1)) {
                finalPosition = hit.position;            
            }
            return finalPosition;
        }   
    Vector3 PickRandomPoint () {
        var point = Random.insideUnitSphere * (3*radiusToGo);

        point.y = 0;
        point += transform.position;
        return point;
    }

}
