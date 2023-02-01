using System;
using System.Collections.Generic;
using Flames;
using GreatArcStudios;
using UnityEngine;

namespace Gilad
{
    public class ShotStopper : MonoBehaviour
    {

        [SerializeField]
        private float stopTime = 1f;

        [SerializeField]
        private ThrowBall throwBall;

        [SerializeField]
        private GameObject headFlame;

        [SerializeField]
        private bool disableOtherFlames = true;
        
        [SerializeField]
        private List<GameObject> otherFlames;

        private float _timeToStart;


        // Update is called once per frame
        void Update()
        {
            if (PauseManager.Paused)
            {
                return;
            }

            if (_timeToStart > 0f)
            {
                _timeToStart -= Time.deltaTime;
                if (_timeToStart <= 0f)
                {
                    EnableShooting();
                }
            }
        }

        private void EnableShooting()
        {
            _timeToStart = 0f;
            throwBall.CanShoot = true;
            headFlame.SetActive(true);
            var explosion = ExplosionPool.Instance.Pool.Get();
            explosion.transform.position = headFlame.transform.position;
            if (disableOtherFlames)
            {
                foreach (var flame in otherFlames)
                {
                    flame.SetActive(true);
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_timeToStart <= 0)
            {
                DisableShooting();
            }
        }

        private void DisableShooting()
        {
            _timeToStart = stopTime;
            throwBall.CanShoot = false;
            headFlame.SetActive(false);
            if (disableOtherFlames)
            {
                foreach (var flame in otherFlames)
                {
                    flame.SetActive(false);
                }
            }
        }
    }
}
