using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIManager
{
    public enum UIEventEnum
    {
        PlayerHealth,
        BurnedPercentage
    }
    
    public static readonly UnityEvent<float> PlayerHealth = new UnityEvent<float>();
    public static readonly UnityEvent<float> BurnedPercentage = new UnityEvent<float>();
}
