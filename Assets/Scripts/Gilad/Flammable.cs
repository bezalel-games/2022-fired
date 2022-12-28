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

        private static int _numBurned = 0;
        
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

        private float _timeOnFire = 0f;
        
        private int _powerLevel = 0;

        private Vector3[] _startSizes;

        private float _timeCount = 0f;

        private void Awake()
        {
            if (onBurnedEvent != null)
            {
                onBurnedEvent = GetComponentInParent<BurnedEvent>();
            }
        }

        // Start is called before the first frame update
        void Start()
        {
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
            onBurnedEvent.ObjectBurned();
            GrowFire(-numOfHits);
            this.enabled = false;
            _numBurned++;
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
            
            for (int i = 0; i < flames.Length; i++)
            {
                if (putOut)
                {
                    flames[i].SetActive(false);
                }
                else
                {
                    flames[i].SetActive(true);
                }
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
            GrowFire(-waterPower);
        }

        private void FireDetect()
        {
            GrowFire(firePower);
        }

        public bool IsOnFire()
        {
            return _powerLevel > 0;
        }

        public bool IsDoneBurning()
        {
            return enabled = false;
        }

        public static bool IsAllBurned()
        {
            return _numBurned == allFlammables.Count;
        }
    }
}
