using Avrahamy;
using GreatArcStudios;
using UnityEngine;

namespace Gilad
{
    public class WaterShooter : OptimizedBehaviour
    {
        [SerializeField]
        private Cannon[] cannons;

        [SerializeField]
        private PassiveTimer shootDuration = new(0.2f);

        void Update()
        {
            if (PauseManager.Paused)
            {
                return;
            }

            if (shootDuration.IsSet)
            {
                ShootCannon();
            }
        }

        private void ShootCannon()
        {
            if (shootDuration.IsActive)
            {
                return;
            }

            foreach (var cannon in cannons)
            {
                cannon.ShootBall();
            }

            shootDuration.Start();
        }

        public void StartShooting()
        {
            shootDuration.Start();
            shootDuration.SetRemainingTimeAndPreserveStartTime(0);
        }

        public void StopShooting()
        {
            shootDuration.Clear();
        }


    }
}
