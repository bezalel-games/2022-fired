using System;
using System.Collections;
using System.Collections.Generic;
using BitStrap;
using UnityEngine;

public class OptimizeParticles : MonoBehaviour
{
    [SerializeField]
    public ParticleController particleController;

    private bool _hasController;

    private void Awake()
    {
        
        _hasController = TryGetComponent(out ParticleSystem myParticles);
        if (!_hasController)
        {
            return;
        }

        particleController.RootParticleSystem = myParticles;
        particleController.Stop();

    }

    private void OnEnable()
    {
        if (!_hasController)
        {
            return;
        }
        particleController.Play();
    }

    private void OnDisable()
    {
        if (!_hasController)
        {
            return;
        }
        particleController.Stop();
    }
}
