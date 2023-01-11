using System;
using System.Linq;
using Avrahamy;
using BitStrap;
using UnityEngine;
using UnityEngine.Pool;
using Logger = Nemesh.Logger;

namespace Flames
{
    public class ExplosionPool : OptimizedBehaviour
    {
        public static ExplosionPool Instance { get; private set; }
        public LinkedPool<ShotEndsEvent> Pool { get; private set; }

        [SerializeField]
        [RequiredReference]
        private GameObject onShotEndObject;

        [ReadOnly(onlyInPlaymode = true)]
        [SerializeField]
        private int initialPoolSize = 10;

        private void Awake()
        {
            if (Instance is not null)
            {
                Destroy(Instance);
            }
            Instance = this;
            Pool = new LinkedPool<ShotEndsEvent>(CreateEvent, BorrowEvent, ReleaseEvent, null, false, initialPoolSize);
        }

        private void ReleaseEvent(ShotEndsEvent obj)
        {
            obj.gameObject.SetActive(true);
        }

        private void BorrowEvent(ShotEndsEvent obj)
        {
            obj.InitObject();
            obj.gameObject.SetActive(true);
        }

        private ShotEndsEvent CreateEvent()
        {
            var explosionObj = Instantiate(onShotEndObject, transform);
            if (!explosionObj.TryGetComponent(out ShotEndsEvent shotEnd))
            {
                Logger.LogException("ShotEndsEvent not found");
            }

            shotEnd.MyPool = Pool;
            explosionObj.SetActive(true);
            return shotEnd;
        }

        [Button]
        private void GetDebug()
        {
            if (Pool == null)
            {
                return;
            }

            Pool.Get(out ShotEndsEvent exp);
        }
    }
}
