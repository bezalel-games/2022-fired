using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Managers
{
    public class OpeningListener : MonoBehaviour
    {
        [SerializeField]
        private InputAction startKey;
        
        [Space]
        [SerializeField]
        public UnityEvent onStartKeyPressed;

        private void OnEnable()
        {
            startKey.Enable();
            startKey.started += OnStartKey;
        }

        private void OnDisable()
        {
            startKey.started -= OnStartKey;
            startKey.Disable();
        }

        private void OnStartKey(InputAction.CallbackContext obj)
        {
            onStartKeyPressed.Invoke();
        }
    }
}
