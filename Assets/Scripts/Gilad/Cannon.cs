using System;
using Avrahamy.Math;
using BitStrap;
using Flames;
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
        
        [RequiredReference]
        [SerializeField]
        private ExplosionPool explosionPool;

        [SerializeField]
        private Transform parentTransform;

        private bool _hasExplosion;

        // Start is called before the first frame update
        void Start()
        {
            // TODO: unified pool for all water cannons!
            _pool = new LinkedPool<Shooter>(CreateBall, GetBall, ReleaseBall, null, false, 20);
            _hasExplosion = explosionPool != null;
            if (parentTransform == null)
            {
                parentTransform = FlameRestartManager.Instance.transform;
            }
        }

        private Shooter CreateBall()
        {
            var ball = Instantiate(fireBallObGameObject, parentTransform);
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
            var pos = shooter.transform.position;
            shooter.ShutDown();
            if (!_hasExplosion)
            {
                return;
            }
            var explosion = explosionPool.Pool.Get();
            explosion.transform.position = pos;
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
