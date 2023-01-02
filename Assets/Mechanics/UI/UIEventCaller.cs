using System;
using UnityEngine;
using UnityEngine.Events;
using Logger = Nemesh.Logger;

namespace Mechanics.UI
{
    public class UIEventCaller : MonoBehaviour
    {
        [SerializeField]
        private UIManager.UIEventEnum callType;

        [SerializeField]
        private UnityEvent<float> myEvent;
        
        private void Awake()
        {
            ListenToEvent();
        }

        private void ListenToEvent()
        {
            UIManager.BurnedPercentage.RemoveListener(OnEventCall);
            UIManager.PlayerHealth.RemoveListener(OnEventCall);
            
            switch (callType)
            {
                case UIManager.UIEventEnum.PlayerHealth:
                    UIManager.PlayerHealth.AddListener(OnEventCall);
                    break;
                case UIManager.UIEventEnum.BurnedPercentage:
                    UIManager.BurnedPercentage.AddListener(OnEventCall);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void OnEventCall(float arg)
        {
            myEvent.Invoke(arg);
            Logger.Log("Invoked on this", this);
        }
    }
}