using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Avrahamy;

public class PoliceCar : CharacterAI
{
    [Space(2)]
    [Header("PoliceCar")]
    [SerializeField]
    private PassiveTimer timeToGo;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        Goal = player;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        MoveCharacter();
    }

    protected override void MoveCharacter()
    {
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
