using System;
using System.Linq;
using BitStrap;
using UnityEngine;
using UnityEngine.Pool;
using Logger = Nemesh.Logger;

namespace Flames
{
    public class ExplosionPool : MonoBehaviour
    {
        public static ExplosionPool Instance { get; private set; }
        public LinkedPool<ShotEndsEvent> Pool { get; private set; }

        [SerializeField]
        [RequiredReference]
        private GameObject onShotEndObject;

        private void Awake()
        {
            if (Instance is not null)
            {
                Destroy(this);
                return;
            }
            Instance = this;
            Pool = new LinkedPool<ShotEndsEvent>(CreateEvent, BorrowEvent, ReleaseEvent, null, false, 40);
        }

        private void ReleaseEvent(ShotEndsEvent obj)
        {
            Logger.Log("Returned explosion", obj);
        }

        private void BorrowEvent(ShotEndsEvent obj)
        {
            Logger.Log("Borrowed explosion", obj);
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
            exp.ObjectBurned();

        }
    }
}
