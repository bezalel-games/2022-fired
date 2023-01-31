using System;
using System.Collections.Generic;
using Flames;
using UnityEngine;
namespace Gilad
{
    public class Flammable : MonoBehaviour
    {

        public static List<Flammable> AllFlammables => allFlammables;

        private static readonly List<Flammable> allFlammables = new List<Flammable>();

        public static readonly HashSet<Flammable> AllBurning = new HashSet<Flammable>();
        
        public static readonly HashSet<Flammable> AllBurned = new HashSet<Flammable>();

        public static int NumBurned = 0;

        //flames to light
        [SerializeField]
        private GameObject[] flames;

        // how many its it takes to fully start the fire
        [SerializeField]
        private int numOfHits = 1;

        // on what time do we do a auto grow
        [SerializeField]
        private float hitDuration = 0.5f;

        // how many hits does a fire ball worth
        [SerializeField]
        private int firePower = 1;

        // how many hits does a water ball worth
        [SerializeField]
        private int waterPower = 1;

        //how many hits does time worth
        [SerializeField]
        private int timePower = 1;

        [SerializeField]
        [Tooltip("After this time the object is considered burned")]
        private float lifeTime = 20f;
        
        [SerializeField]
        private BurnedEvent onBurnedEvent;
        
        [SerializeField]
        private bool removeFlamesOnBurned;

        [SerializeField]
        [Tooltip("how much of the timeOnFire to keep after extinguishing. " +
                 "0 means reset completely, 1 means keep as was before extinguished")]
        private float timeOnFireExtinguishRatio = 1f;

        [SerializeField]
        private bool useRatioOnPowerLevel;

        private float _timeOnFire = 0f;

        private int _powerLevel = 0;

        private Vector3[] _startSizes;

        private float _timeCount = 0f;
        private bool _onBurnedEventExists;
        private bool _startedBurning = false;

        private void Awake()
        {
            if (onBurnedEvent == null)
            {
                onBurnedEvent = GetComponentInParent<BurnedEvent>();
            }

            _onBurnedEventExists = onBurnedEvent != null;
        }

        // Start is called before the first frame update
        void Start()
        {
            if (!gameObject.activeSelf) return;
            allFlammables.Add(this);
            if (numOfHits <= 0)
            {
                throw new Exception("num of hits must be larger than 0");
            }

            _startSizes = new Vector3[flames.Length];
            for (int i = 0; i < flames.Length; i++)
            {
                _startSizes[i] = flames[i].transform.localScale / numOfHits;
                flames[i].transform.localScale = Vector3.zero;
                flames[i].SetActive(false);
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (_powerLevel > 0)
            {
                _timeOnFire += Time.deltaTime;
                if (_timeOnFire >= lifeTime)
                {
                    ShutDown();
                    return;
                }

                CheckTime();
            }
        }

        private void ShutDown()
        {
            if (_onBurnedEventExists)
            {
                onBurnedEvent.ObjectBurned();
            }

            if (removeFlamesOnBurned)
            {
                GrowFire(-numOfHits);
            }

            NumBurned++;
            AllBurning.Remove(this);
            AllBurned.Add(this);
            enabled = false;
        }

        private void CheckTime()
        {
            _timeCount += Time.deltaTime;
            if (_timeCount >= hitDuration)
            {
                _timeCount = 0f;
                GrowFire(timePower);
            }
        }

        private void GrowFire(int power)
        {
            _powerLevel += power;
            _powerLevel = _powerLevel <= numOfHits ? _powerLevel : numOfHits;
            _powerLevel = _powerLevel >= 0 ? _powerLevel : 0;
            SetSizes();
        }

        private void SetSizes()
        {
            var putOut = _powerLevel == 0;
            if (putOut)
            {
                AllBurning.Remove(this);
            }
            else if (enabled)
            {
                AllBurning.Add(this);
            }
            for (int i = 0; i < flames.Length; i++)
            {
                flames[i].SetActive(!putOut);
                flames[i].transform.localScale = _startSizes[i] * _powerLevel;
            }
        }

        public void Detect(bool isFire)
        {
            if (isFire)
            {
                FireDetect();
            }
            else
            {
                WaterDetect();
            }
        }

        private void WaterDetect()
        {
            if (!enabled) return;
            GrowFire(-waterPower);
        }

        private void FireDetect()
        {
            if (!enabled) return;
            if (!_startedBurning)
            {
                StartBurning();
            }
            GrowFire(firePower);
        }

        private void StartBurning()
        {
            _startedBurning = true;
            _timeOnFire = timeOnFireExtinguishRatio * _timeOnFire;
            if (useRatioOnPowerLevel)
            {
                _powerLevel = (int) (numOfHits * (_timeOnFire / lifeTime));
            }
        }

        public bool IsOnFire()
        {
            return _powerLevel > 0;
        }

        public bool IsDoneBurning()
        {
            return enabled == false;
        }

        public static float BurningRatio()
        {
            if (allFlammables.Count == 0)
            {
                return 0;
            }
            return (float)NumBurned / allFlammables.Count;
        }
    }
}
