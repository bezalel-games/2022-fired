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
    private PassiveTimer timeToAppearPolice;
    private bool endPolice = false;
    private bool startPolice = false;
    [SerializeField] private GameObject[] fireMans;
    private PassiveTimer timeToAppearFireMan;
    private bool endFireMan = false;
    

    private int fireManIndex;
    private int policeIndex;
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
        timeToAppearFireMan = new PassiveTimer(5);
        timeToAppearPolice = new PassiveTimer(10);
        timeToAppearFireMan.Start();
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

        if (!endFireMan)
        {
            FireManSet();
            
        }

        if (!endPolice && startPolice)
        {
            PoliceSet();
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
                startPolice = true;
                // foreach (var car in policeCar)
                // {
                //     car.SetActive(true);
                // }
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

    private void FireManSet()
    {
        if (timeToAppearFireMan.IsSet)
        {
            if (!timeToAppearFireMan.IsActive)
            {
                activateFireMan();
                timeToAppearFireMan.Clear();
                timeToAppearFireMan.Start();
                
            }
        }
    }private void PoliceSet()
    {
        if (timeToAppearPolice.IsSet)
        {
            if (!timeToAppearPolice.IsActive)
            {
                activatePolice();
                timeToAppearPolice.Clear();
                timeToAppearPolice.Start();
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

    private void activateFireMan()
    {
        if (fireManIndex < fireMans.Length)
        {
            fireMans[fireManIndex].SetActive(true);
            fireManIndex++;
        }
        else
        {
            endFireMan = true;
        }
    }private void activatePolice()
    {
        if (policeIndex < policeCar.Length)
        {
            policeCar[fireManIndex].SetActive(true);
            policeIndex++;
        }
        else
        {
            endPolice = true;
        }
    }
}
