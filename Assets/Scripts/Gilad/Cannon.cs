using System;
using Avrahamy.Math;
using UnityEngine;
using UnityEngine.Pool;

namespace Gilad
{
    public class Cannon : MonoBehaviour
    {
        private LinkedPool<Shooter> _pool;

        [SerializeField]
        private float force = 1f;

        [SerializeField]
        private Transform forward;

        [SerializeField]
        private GameObject fireBallObGameObject;

        // Start is called before the first frame update
        void Start()
        {
            _pool = new LinkedPool<Shooter>(CreateBall, GetBall, ReleaseBall, null, false, 20);
        }

        //todo need to remove
        private void Update()
        {
            // if (Input.GetKeyDown(KeyCode.Space))
            // {
            //     ShootBall();
            // }
        }

        private Shooter CreateBall()
        {
            var ball = Instantiate(fireBallObGameObject);
            var fireShooter = ball.GetComponent<Shooter>();
            if (fireShooter == null)
            {
                throw new Exception("no shooter script for object: " + ball.name);
            }

            fireShooter.Create();
            fireShooter.FirePool = _pool;
            return fireShooter;
        }

        private void GetBall(Shooter shooter)
        {
            var position = forward.position;
            shooter.gameObject.transform.position = position;
            shooter.gameObject.SetActive(true);
            shooter.Shoot((position - transform.position).GetWithMagnitude(force));
        }

        private void ReleaseBall(Shooter shooter)
        {
            shooter.ShutDown();
        }

        public void ShootBall()
        {
            _pool.Get();
        }

        public void StartShoot()
        {

        }


    }
}
