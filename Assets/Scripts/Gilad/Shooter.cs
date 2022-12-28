using System;
using Avrahamy;
using UnityEngine;
using UnityEngine.Pool;
using Logger = Nemesh.Logger;

namespace Gilad
{
    public class Shooter : MonoBehaviour
    {
        private static readonly int LerpVec = Shader.PropertyToID("_LerpVector");

        [SerializeField]
        private PassiveTimer lifeTimer = new(2f);

        public LinkedPool<Shooter> FirePool { get; set; }

        private Rigidbody _rigidbody;
        private Material _myMaterial;
        private MeshRenderer _myMeshRendered;

        private void Awake()
        {
            TryGetComponent(out _myMeshRendered);
            // _myMaterial = GetComponent<MeshRenderer>().material;
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
                    Logger.Log("Fireball dead", this);
                    lifeTimer.Clear();
                    gameObject.SetActive(false);
                }
                else
                {
                    var vel = Vector3.Reflect(_rigidbody.velocity, Vector3.up);
                    _myMeshRendered.material.SetVector(LerpVec, vel);
                }

            }
        }

        private void OnDisable()
        {
            FirePool.Release(this);
        }

        public void Shoot(Vector3 direction)
        {
            _rigidbody.isKinematic = false;
            _rigidbody.AddForce(direction, ForceMode.Impulse);
            lifeTimer.Start();
        }

        public void ShutDown()
        {
            _rigidbody.isKinematic = true;
            _rigidbody.velocity = Vector3.zero;
        }

    }
}
