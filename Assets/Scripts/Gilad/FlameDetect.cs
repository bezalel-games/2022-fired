using System;
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
            other.gameObject.SetActive(false);
        }
    }
}
