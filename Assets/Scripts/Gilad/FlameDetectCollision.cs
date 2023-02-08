using System;
using BitStrap;
using UnityEngine;

namespace Gilad
{
    class FlameDetectCollision : FlameDetect
    {
        [Space]
        [Header("Flame Detection For Collider. (ignores isFire)")]
        [LayerSelector]
        [SerializeField]
        protected int fireLayer;

        [LayerSelector]
        [SerializeField]
        protected int waterLayer;
        
        

        private void OnCollisionEnter(Collision collision)
        {
            var layer = collision.gameObject.layer;
            if (layer == fireLayer)
            {
                flammable.Detect(true);
                collision.gameObject.SetActive(false);
            }
            else if (layer == waterLayer)
            {
                flammable.Detect(false);
                collision.gameObject.SetActive(false);
            }
        }

        protected override void OnTriggerEnter(Collider other)
        {
            var layer = other.gameObject.layer;
            if (layer == fireLayer)
            {
                flammable.Detect(true);
                other.gameObject.SetActive(false);
            }
            else if (layer == waterLayer)
            {
                flammable.Detect(false);
                other.gameObject.SetActive(false);
            }
        }
    }
}
