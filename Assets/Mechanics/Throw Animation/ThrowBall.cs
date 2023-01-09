using System;
using System.Collections;
using System.Collections.Generic;
using BitStrap;
using Gilad;
using StarterAssets;
using UnityEngine;
using Logger = Nemesh.Logger;

public class ThrowBall : MonoBehaviour
{
    [HideInInspector]
    [SerializeField]
    private TriggerAnimationParameter throwTrigger;

    [SerializeField]
    private BoolAnimationParameter throwBoolean;

    [SerializeField]
    private FloatAnimationParameter throwSpeed;
    
    [SerializeField]
    private float throwAnimationSpeed = 3f;

    private Animator _animator;
    private StarterAssetsInputs _input;
    private Cannon _cannon;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _input = GetComponent<StarterAssetsInputs>();
        _cannon = GetComponentInChildren<Cannon>();
    }

    private void OnEnable()
    {
        _input.onFireEvent.AddListener(OnThrow);
    }

    private void OnDisable()
    {
        _input.onFireEvent.RemoveListener(OnThrow);
    }
    
    [Button]
    private void OnThrow()
    {
        OnThrow(true);
    }

    private void OnThrow(bool shouldThrow)
    {
        // if (shouldThrow)  // TODO: and not already in state!
        // {
        //     throwTrigger.Set(_animator);
        // }
        throwBoolean.Set(_animator, shouldThrow);
        throwSpeed.Set(_animator, throwAnimationSpeed);
    }

    public void ThrowAnimationTrigger()
    {
        // Logger.Log("Shooting");
        _cannon.ShootBall();
    }
}
