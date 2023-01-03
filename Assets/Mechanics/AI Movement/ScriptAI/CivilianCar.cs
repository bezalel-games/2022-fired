using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Avrahamy;
using Gilad;

public class CivilianCar : CharacterAI
{
    private bool toMove;
    [Space(2)]
    [Header("Civilian Car")]
    [SerializeField]private PassiveTimer timeToGo;

    

    [SerializeField] private float DistanceFromFireToPanic = 10f;
    [SerializeField]
    private Flammable m_Flammable;
    
    [SerializeField] private Transform[] points;
    private int destPoint = 0;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        toMove = true;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if(!m_Flammable.IsOnFire() && toMove)
        {
            MoveCharacter();
        }
    }
    
    protected override void MoveCharacter()
    {
        if (m_Flammable.IsDoneBurning())
        {
            toMove = false;
            return;
        }
            
        if (timeToGo.IsSet)
        {
            if (timeToGo.IsActive)
            {
                if (!Agent.pathPending && Agent.remainingDistance < stoppingDistance)
                    GotoNextPoint();
            }
            else
            {
                Goal = FindFire(transform);
                if (Goal != null && Agent.remainingDistance < DistanceFromFireToPanic)
                {
                    RunAway(Goal);
                    timeToGo.Clear();
                }
                else
                {
                    if (!Agent.pathPending && Agent.remainingDistance < stoppingDistance)
                        GotoNextPoint();
                    timeToGo.Clear();
                }
            }
        }
        else
        {
            timeToGo.Start();
        }
    }
    
    void GotoNextPoint() {
        // Returns if no points have been set up
        if (points.Length == 0)
            return;
        // Set the agent to go to the currently selected destination.
        Agent.SetDestination(points[destPoint].position);

        // Choose the next point in the array as the destination,
        // cycling to the start if necessary.
        destPoint = (destPoint + 1) % points.Length;
    }
}
