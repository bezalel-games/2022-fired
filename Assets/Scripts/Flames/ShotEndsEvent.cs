using System;
using Avrahamy.EditorGadgets;
using BitStrap;
using Gilad;
using Nemesh;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;
using Logger = Nemesh.Logger;

namespace Flames
{
    public class ShotEndsEvent : BurnedEvent, IPoolable
    {
        [Space(2)]
        [Header("Bullet")]
        [Space]
        [SerializeField]
        private bool doExplosion = true;

        public LinkedPool<ShotEndsEvent> MyPool { get; set; } = null;

        protected override void Start()
        {
            base.Start();
            InitObject();
        }

        public void InitObject()
        {

            changeMaterialColor = false;
            onBurned.AddListener(ExplodingObject);
        }

        protected void OnDisable()
        {
            ReleaseSelf();
        }

        private void ExplodingObject()
        {
            if (doExplosion)
            {
                Logger.Log("Do Some sort of explosion", Color.red, this);
                gameObject.SetActive(false);
            }
        }

        public void ReleaseSelf()
        {
            MyPool?.Release(this);
        }
    }
}
