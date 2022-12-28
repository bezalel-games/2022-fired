using System;
using UnityEngine;
using UnityEngine.Pool;

namespace Gilad
{
    public class Shooter : MonoBehaviour
    {

        private Rigidbody _rigidbody;

        public LinkedPool<Shooter> FirePool { get; set; }

        [SerializeField] private float lifeTime = 2f;


        private float _lifeSpent = 0f;
        // Start is called before the first frame update
        void Start()
        {
        }

        public void Create()
        {
            _rigidbody = gameObject.GetComponent<Rigidbody>();
            _rigidbody.isKinematic = true;
        }
        // Update is called once per frame
        void Update()
        {
            _lifeSpent += Time.deltaTime;
            if (TimeLife())
            {
                gameObject.SetActive(false);
            }
        }

        private void OnDisable()
        {
            FirePool.Release(this);
        }

        private bool TimeLife()
        {
            return _lifeSpent > lifeTime;
        }

        public void Shoot(Vector3 direction)
        {
            _rigidbody.isKinematic = false;
            _rigidbody.AddForce(direction, ForceMode.Impulse);
        }

        public void ShutDown()
        {
            _rigidbody.isKinematic = true;
            _rigidbody.velocity = Vector3.zero;
            _lifeSpent = 0f;
        }

    }
}
