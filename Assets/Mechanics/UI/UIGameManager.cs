using System;
using BitStrap;
using UnityEngine;
using UnityEngine.Events;
using Logger = Nemesh.Logger;

namespace Mechanics.UI
{
    public class UIGameManager : MonoBehaviour
    {
        [SerializeField]
        public UnityEvent<float> onPlayerChangeHealth;
        
        [SerializeField]
        public UnityEvent<float> onBurnedPercentageChange;
        
        [SerializeField]
        private bool printOnCalls = true;

        private void Awake()
        {
            UIManager.PlayerHealth.AddListener(hp => onPlayerChangeHealth.Invoke(hp));
            UIManager.BurnedPercentage.AddListener(p => onBurnedPercentageChange.Invoke(p));

            onBurnedPercentageChange.AddListener(OnBurnedDefault);
            onPlayerChangeHealth.AddListener(OnHealthDefault);
        }

        private void OnHealthDefault(float hp)
        {
            if (printOnCalls)
            {
                Logger.Log($"Current hp {hp}", Color.blue, this);
            }
        }

        private void OnBurnedDefault(float p)
        {
            if (printOnCalls)
            {
                Logger.Log($"Burned {p}", Color.yellow, this);
            }
        }

        [Button]
        private void DebugPlayerEvent()
        {
            UIManager.PlayerHealth.Invoke(0.5f);
        }
    }
}