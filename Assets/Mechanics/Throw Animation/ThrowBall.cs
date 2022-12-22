using System;
using System.Collections;
using System.Collections.Generic;
using BitStrap;
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

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _input = GetComponent<StarterAssetsInputs>();
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
    private void OnThrow(bool shouldThrow = true)
    {
        if (shouldThrow)  // TODO: and not already in state!
        {
            throwTrigger.Set(_animator);
        }
    }
}
