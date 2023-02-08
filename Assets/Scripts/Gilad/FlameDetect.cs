using System;
using Flames;
using UnityEngine;

namespace Gilad
{
    public class FlameDetect : MonoBehaviour
    {
        // Start is called before the first frame update
        [SerializeField] protected Flammable flammable;

        [SerializeField] private bool isFire = true;

        protected virtual void OnTriggerEnter(Collider other)
        {
            flammable.Detect(isFire);

            other.gameObject.SetActive(false);
        }
    }
}
