using UnityEngine;

namespace Gilad
{
    public class WaterShooter : MonoBehaviour
    {
        [SerializeField]
        private Cannon[] cannons;

        [SerializeField]
        private float shootDuration = 0.2f;

        private float _timePassed = 0f;

        private bool _isShooting;
        void Update()
        {
            if (_isShooting)
            {
                ShootCannon();
            }
        }

        private void ShootCannon()
        {
            if (_timePassed >= shootDuration)
            {
                foreach (var cannon in cannons)
                {
                    cannon.ShootBall();
                }
                _timePassed = 0f;
            }
            else
            {
                _timePassed += Time.deltaTime;
            }
        }

        public void StartShooting()
        {
            _isShooting = true;
            _timePassed = shootDuration;
        }

        public void StopShooting()
        {
            _isShooting = false;
            _timePassed = 0f;
        }


    }
}
