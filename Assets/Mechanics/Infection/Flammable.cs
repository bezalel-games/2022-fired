using System;
using UnityEngine;
using Logger = Nemesh.Logger;

namespace Mechanics.Infection
{
    public class Flammable : MonoBehaviour
    {

        [SerializeField] private float fireTotalTime = 10f;

        private float _fireCurrentTime = 0f;

        private bool _fireOn = false;

        public bool FireOn
        {
            get => _fireOn;
            set
            {
                if (!value && _fire != null)
                {
                    FireFactory.ReleaseFire(_fire);
                }
                _fireOn = value;
            }
        }

        private GameObject _fire;
        private void OnTriggerEnter(Collider other)
        {
            if (_fireOn) return;
            Logger.Log($"On Fire: {gameObject.name}",Color.red, other);
            var pointOfCollision = other.ClosestPoint(transform.position);
            _fire = FireFactory.GetNewFire();
            _fire.transform.position = pointOfCollision;
            _fireOn = true;
            var fireScript = _fire.GetComponent<Fire>();
            if (fireScript != null)
            {
                fireScript.ToFollow = transform;
            }
        }

        private void Update()
        {
            if (_fireOn)
            {
                _fireCurrentTime += Time.deltaTime;
                if (_fireCurrentTime > fireTotalTime)
                {
                    FireOn = false;
                    _fireCurrentTime = 0f;
                }
            }
        }
    }
}
