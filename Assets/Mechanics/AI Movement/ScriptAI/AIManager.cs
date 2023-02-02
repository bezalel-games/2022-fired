using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Avrahamy;

public class AIManager : MonoBehaviour
{
    [Header("Police Car manager")]
    [SerializeField]
    private PassiveTimer TimeToShowFireTruck;
    
    // does it work by time
    [SerializeField] private bool ByTime;
    [SerializeField] private bool ByTimeChopper;

    //PoliceCar game object
    [SerializeField] private GameObject FireTruck;

    [SerializeField]
    private bool turnFireTruckOn = false;
    [SerializeField] 
    private PassiveTimer timeToAppearChopper;
    [SerializeField] private GameObject chopper;

    [SerializeField] private GameObject []policeCar;
    // Start is called before the first frame update
    void Start()
    {
        if (ByTime)
        {
            TimeToShowFireTruck.Start();
        }

        if (turnFireTruckOn)
        {
            ActivateFireTruck();
        }
        timeToAppearChopper.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if(ByTime)
        {
            FireTruckSetIfTime();
        }

        if (ByTimeChopper)
        {
            ChopperSet();
        }

    }

    private void ChopperSet()
    {
        if (timeToAppearChopper.IsSet)
        {
            if (!timeToAppearChopper.IsActive)
            {
                timeToAppearChopper.Clear();
                chopper.SetActive(true);
                foreach (var car in policeCar)
                {
                    car.SetActive(true);
                }
            }

            return;
        }
    }

    private void FireTruckSetIfTime()
    {
        if (TimeToShowFireTruck.IsSet)
        {
            if (!TimeToShowFireTruck.IsActive)
            {
                ActivateFireTruck();
                TimeToShowFireTruck.Clear();
            }
        }
    }

    public void ActivateFireTruck()
    {
        FireTruck.SetActive(true);
    }
    
    public void DeactivateFireTruck()
    {
        FireTruck.SetActive(false);
    }

    public void ResetTimer()
    {
        TimeToShowFireTruck.Clear();
        TimeToShowFireTruck.Start();
    }
}
