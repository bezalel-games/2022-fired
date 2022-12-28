using System;
using Avrahamy;
using Avrahamy.Math;
using Avrahamy.Utils;
using UnityEngine;
using UnityEngine.Pool;
using Logger = Nemesh.Logger;
using Random = UnityEngine.Random;

namespace Gilad
{
    public class Shooter : MonoBehaviour
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

        private void Awake()
        {
            TryGetComponent(out _myMeshRendered);
        }

        public void Create()
        {
            _rigidbody = gameObject.GetComponent<Rigidbody>();
            _rigidbody.isKinematic = true;
        }

        // Update is called once per frame
        void Update()
        {
            if (lifeTimer.IsSet)
            {
                if (!lifeTimer.IsActive)
                {
                    lifeTimer.Clear();
                    gameObject.SetActive(false);
                }
                else
                {
                    var rigidVelocity = _rigidbody.velocity;
                    var vel = new Vector4(-rigidVelocity.x, Math.Abs(rigidVelocity.y), -rigidVelocity.z);
                    _myMeshRendered.material.SetVector(LerpVec, vel);
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
