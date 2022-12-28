using System;
using System.Collections;
using System.Collections.Generic;
using BitStrap;
using Gilad;
using StarterAssets;
using UnityEngine;

public class ThrowBall : MonoBehaviour
{
    [SerializeField]
    private TriggerAnimationParameter throwTrigger;

    [InspectorName("Throw Boolean - NOT USED RIGHT NOW")]
    [SerializeField]
    private BoolAnimationParameter throwBoolean;

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
        if (shouldThrow)  // TODO: and not already in state!
        {
            throwTrigger.Set(_animator);
        }
    }

    public void ThrowAnimationTrigger()
    {
        _cannon.ShootBall();
    }
}
