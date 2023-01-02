using System;
using Avrahamy;
using Avrahamy.EditorGadgets;
using BitStrap;
using UnityEngine;
using UnityEngine.Events;

namespace Mechanics.UI
{
    public class TimeManager : MonoBehaviour
    {
        [SerializeField]
        [BitStrap.ReadOnly]
        private int index;

        [SerializeField]
        [BitStrap.ReadOnly]
        private float time;

        [InlineScriptableObject]
        public TimerWithEvent[] timers;


        private void Start()
        {
            if (timers.Length > 0)
            {
                timers[index].timer.Start();
            }
        }

        private void Update()
        {
            time = Time.time;
            if (index >= timers.Length)
            {
                return;
            }

            var timer = timers[index].timer;
            if (timer.IsSet && !timer.IsActive)
            {
                timers[index].onTimerEndEvent.Invoke();
                timer.Clear();
                index += 1;
                if (index < timers.Length)
                {
                    timers[index].timer.Start();
                }
            }
        }
    }

    public class TimerWithEvent : ScriptableObject
    {
        [SerializeField]
        public PassiveTimer timer;

        [SerializeField]
        public UnityEvent onTimerEndEvent;
    }
}