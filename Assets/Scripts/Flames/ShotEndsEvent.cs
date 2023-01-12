using System;
using Avrahamy;
using Avrahamy.Audio;
using Avrahamy.EditorGadgets;
using BitStrap;
using Gilad;
using GreatArcStudios;
using Nemesh;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;
using Logger = Nemesh.Logger;

namespace Flames
{
    [RequireComponent(typeof(AudioSource))]
    public class ShotEndsEvent : OptimizedBehaviour, IPoolable
    {
        [SerializeField]
        private PassiveTimer timeToExplosion = new PassiveTimer(0);

        [SerializeField]
        private PassiveTimer timeOfExplosion = new PassiveTimer(1);
        
        [SerializeField]
        private UnityEvent onExplosion;

        [SerializeField]
        private bool useCodedExplosion= true;

        [SerializeField]
        private ParticleController explosionParticleSystem;

        [SerializeField]
        private AudioEvent onExplosionSound;

        private AudioSource _myAudioSource;

        public LinkedPool<ShotEndsEvent> MyPool { get; set; } = null;

        public bool UseCodedExplosion
        {
            get => useCodedExplosion;
            set => useCodedExplosion = value;
        }

        private void Awake()
        {
            TryGetComponent(out _myAudioSource);
        }

        private void Start()
        {
            InitObject();
            onExplosion.AddListener(ExplodingObject);
        }

        private void Update()
        {
            if (PauseManager.Paused)
            {
                return;
            }
            
            if (timeToExplosion.IsSet && !timeToExplosion.IsActive)
            {
                timeToExplosion.Clear();
                onExplosion.Invoke();
            }
            if (timeOfExplosion.IsSet && !timeOfExplosion.IsActive)
            {
                ReleaseSelf();
            }
        }

        public void InitObject()
        {
            timeToExplosion.Start();
        }

        private void ExplodingObject()
        {
            if (UseCodedExplosion)
            {
                explosionParticleSystem.Play();
                onExplosionSound.Play(_myAudioSource);
                timeOfExplosion.Start();
            }
        }

        public void ReleaseSelf()
        {
            timeOfExplosion.Clear();
            timeToExplosion.Clear();
            explosionParticleSystem.Stop();
            
            MyPool?.Release(this);
        }
    }
}
