using System;
using Avrahamy.Math;
using UnityEngine;
using UnityEngine.Pool;
using Logger = Nemesh.Logger;

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
            Logger.Log(fireBallObGameObject.name,this);
        }

        private Shooter CreateBall()
        {
            var ball = Instantiate(fireBallObGameObject);
            var fireShooter = ball.GetComponent<Shooter>();
            if (fireShooter == null)
            {
                // Logger.LogException("no shooter script for object: " + ball.name);
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
            Logger.Log(fireBallObGameObject.name, this);
            _pool.Get();
        }

        public void StartShoot()
        {

        }


    }
}
