using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Avrahamy;
using GreatArcStudios;
using Gilad;

public class PoliceCar : CharacterAI
{
    [Space(2)]
    [Header("PoliceCar")]
    [SerializeField]
    private PassiveTimer timeToGo;
    [SerializeField]
    private Flammable m_Flammable;
    private bool toMove;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        Goal = player;
        toMove = true;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if(timeToInit.IsSet && timeToInit.IsActive || PauseManager.Paused)
        {
            return;
        }
        if (!m_Flammable.IsOnFire())
        {
            toMove = true;
            Agent.enabled = true;
            MoveCharacter();
        }
        else
        {
            toMove = false;
            // Agent.isStopped= true;
            Agent.enabled = false;
        }
    }

    protected override void MoveCharacter()
    {
        if (m_Flammable.IsDoneBurning())
        {
            toMove = false;
            Agent.isStopped = true;
            Agent.enabled = false;
            return;
        }

        if (!toMove)
        {
            return;
        }
        if (timeToGo.IsSet)
        {
            if (timeToGo.IsActive)
            {
                if (Agent.remainingDistance < stoppingDistance)
                {
                    // do somthing
                }
            }
            else
            {
                Seek(Goal);
                timeToGo.Clear();
               
            }
        }
        else
        {
            timeToGo.Start();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        print("game");
    }
}
