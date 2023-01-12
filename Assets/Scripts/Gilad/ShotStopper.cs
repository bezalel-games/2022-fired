using System;
using UnityEngine;

namespace Gilad
{
    public class ShotStopper : MonoBehaviour
    {

        [SerializeField] private float stopTime = 1f;

        [SerializeField] private ThrowBall throwBall;

        [SerializeField] private GameObject headFlame;

        private float _timeToStart;


        // Update is called once per frame
        void Update()
        {
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
            throwBall.IsShooting = true;
            headFlame.SetActive(true);
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
            throwBall.IsShooting = false;
            headFlame.SetActive(false);
        }
    }
}
