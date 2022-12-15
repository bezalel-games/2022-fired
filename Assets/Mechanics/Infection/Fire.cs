using System;
using UnityEngine;

namespace Mechanics.Infection
{
    public class Fire : MonoBehaviour
    {

        private Transform _toFollow;

        private bool _putOut = false;

        public Transform ToFollow
        {
            set
            {
                _toFollow = value;
                transform.localScale = Vector3.zero;
            }
            get => _toFollow;
        }

        [SerializeField] private float moveSpeed = 9f;

        [SerializeField] private float spreadSpeed = 5f;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (ToFollow != null)
            {
                var transform1 = FollowObject();
                ChangeScale(transform1);
            }
        }

        private void ChangeScale(Transform transform1)
        {
            var localScale = transform1.localScale;

            if (_putOut)
            {
                transform1.localScale *= 0.9f;
                if (transform1.localScale.y < 0.01f)
                {
                    var flammable = _toFollow.GetComponent<Flammable>();
                    if (flammable != null)
                    {
                        flammable.FireOn = false;
                    }
                }
                return;
            }
            var scale = localScale;
            var scaleDiff = ToFollow.localScale * 2f - scale;
            localScale += scaleDiff * Time.deltaTime * spreadSpeed;
            transform1.localScale = localScale;
        }

        private Transform FollowObject()
        {
            var transform1 = transform;
            var position = transform1.position;
            var dist = ToFollow.position - position;
            position += dist * Time.deltaTime * moveSpeed;
            transform1.position = position;
            return transform1;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Water")
            {
                _putOut = true;
            }
        }
    }
}
