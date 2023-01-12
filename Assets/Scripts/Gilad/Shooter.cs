using System;
using Avrahamy;
using Avrahamy.Math;
using Avrahamy.Utils;
using Flames;
using GreatArcStudios;
using UnityEngine;
using UnityEngine.Pool;
using Logger = Nemesh.Logger;
using Random = UnityEngine.Random;

namespace Gilad
{
    [RequireComponent(typeof(FlameDirection))]
    public class Shooter : OptimizedBehaviour
    {
        private static readonly int LerpVec = Shader.PropertyToID("_LerpVector");
        private static readonly int RandomSeed = Shader.PropertyToID("_Random_Seed");

        [SerializeField]
        private PassiveTimer lifeTimer = new(2f);

        [SerializeField]
        private AnimationCurve sizeOverLifer;

        public LinkedPool<Shooter> FirePool { get; set; }

        private Rigidbody _rigidbody;
        private MeshRenderer _myMeshRendered;
        private FlameDirection _myFlameDirection;

        private void Awake()
        {
            TryGetComponent(out _myMeshRendered);
            TryGetComponent(out _myFlameDirection);
            _myFlameDirection.MyMeshRendered = _myMeshRendered;
        }

        public void Create()
        {
            _rigidbody = gameObject.GetComponent<Rigidbody>();
            _rigidbody.isKinematic = true;
        }

        // Update is called once per frame
        void Update()
        {
            if (lifeTimer.IsSet && !PauseManager.Paused)
            {
                if (!lifeTimer.IsActive || transform.position.y <= 0f)  // TODO: better system
                {
                    lifeTimer.Clear();
                    gameObject.SetActive(false);
                }
                else
                {
                    _myFlameDirection.MyVelocity = _rigidbody.velocity; 
                    transform.SetScale(sizeOverLifer.Evaluate(lifeTimer.Progress));
                }

            }
        }

        private void OnDisable()
        {
            FirePool.Release(this);
        }

        public void Shoot(Vector3 force)
        {
            _rigidbody.isKinematic = false;
            _rigidbody.AddForce(force, ForceMode.Impulse);
            _myMeshRendered.material.SetVector(RandomSeed, Random.insideUnitCircle);
            lifeTimer.Start();
            transform.SetScale(sizeOverLifer.Evaluate(0));
        }

        public void ShutDown()
        {
            _rigidbody.isKinematic = true;
            _rigidbody.velocity = Vector3.zero;
            lifeTimer.Clear();
        }
    }
}
