using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Avrahamy;

public class AIManager : MonoBehaviour
{
    [Header("Police Car manager")]
    [SerializeField]
    private PassiveTimer TimeToShowPoliceCar;
    
    // does it work by time
    [SerializeField] private bool ByTime;

    //PoliceCar game object
    [SerializeField] private GameObject PoliceCar;

    [SerializeField]
    private bool turnPoliceOn = false;
    [SerializeField]
    private PassiveTimer timeToAppear;
    [SerializeField] private GameObject chopper;
    // Start is called before the first frame update
    void Start()
    {
        if (ByTime)
        {
            TimeToShowPoliceCar.Start();
        }

        if (turnPoliceOn)
        {
            ActivatePoliceCar();
        }
        timeToAppear.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if(ByTime)
        {
            policeCarSetIfTime();
        }
        
        if (timeToAppear.IsSet )
        {
            if (!timeToAppear.IsActive)
            {
                timeToAppear.Clear();
                chopper.SetActive(true);
                
            }
            return;
        }
        
    }

    private void policeCarSetIfTime()
    {
        if (TimeToShowPoliceCar.IsSet)
        {
            if (!TimeToShowPoliceCar.IsActive)
            {
                ActivatePoliceCar();
                TimeToShowPoliceCar.Clear();
            }
        }
    }

    public void ActivatePoliceCar()
    {
        PoliceCar.SetActive(true);
    }
    
    public void DeactivatePoliceCar()
    {
        PoliceCar.SetActive(false);
    }

    public void ResetTimer()
    {
        TimeToShowPoliceCar.Clear();
        TimeToShowPoliceCar.Start();
    }
}
