using System;
using System.Collections;
using System.Collections.Generic;
using Avrahamy;
using BitStrap;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Animator))]
public class Showcase : MonoBehaviour
{
    [AnimatorField("animator")] // TODO: just get the animator like openAI
    [SerializeField]
    private BoolAnimationParameter boolParam;

    [SerializeField]
    [AnimatorField("animator")]
    private FloatAnimationParameter floatParam;
    
    [SerializeField]
    private PassiveTimer timer;
    
    [SerializeField]
    private Animator animator;
    
    private void OnValidate()
    {
        DebugLog.Log($"float: {floatParam.name}, bool: {boolParam.name}", Color.blue);
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
        DebugLog.Log("Normal Log", this);
        DebugLog.Log("Colored Log", Color.green, this);
        DebugLog.LogError("Error", this);
        DebugLog.LogWarning("Warning", this);
        timer.Start();
    }

    private void Update()
    {
        if (timer.IsActive && timer.IsSet)
            return;
        timer.Clear();
        timer.Start();
        var color = Random.ColorHSV();
        DebugLog.Log("Timer reached", color);
    }

}
