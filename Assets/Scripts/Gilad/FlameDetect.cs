using System;
using Flames;
using UnityEngine;

namespace Gilad
{
    public class FlameDetect : MonoBehaviour
    {
        // Start is called before the first frame update
        [SerializeField] private Flammable flammable;

        [SerializeField] private bool isFire = true;

        private void OnTriggerEnter(Collider other)
        {
            flammable.Detect(isFire);
            if (ExplosionPool.Instance.Pool != null)
            {
                ExplosionPool.Instance.Pool.Get(out var exp);
                exp.transform.position = other.transform.position;
                exp.ObjectBurned();
            }

            other.gameObject.SetActive(false);
        }
    }
}
