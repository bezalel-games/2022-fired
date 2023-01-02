using UnityEngine;

namespace Gilad
{
    public class WaterShooter : MonoBehaviour
    {
        [SerializeField]
        private Cannon cannon;

        [SerializeField]
        private float shootDuration = 0.2f;

        private float _timePassed = 0f;

        private bool _isShooting;
        // Start is called before the first frame update


        // Update is called once per frame
        void Update()
        {
            // if (Input.GetKeyDown(KeyCode.Return))
            // {
            //     _isShooting = !_isShooting;
            // }
            if (_isShooting)
            {
                ShootCannon();
            }
        }

        private void ShootCannon()
        {
            if (_timePassed >= shootDuration)
            {
                cannon.ShootBall();
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
