using System;
using Avrahamy.EditorGadgets;
using UnityEngine;
using UnityEngine.Events;

namespace Flames
{
    public class BurnedEvent : MonoBehaviour
    {
        [SerializeField]
        private UnityEvent onBurned;

        [SerializeField]
        private bool changeMaterialColor = true;

        [ConditionalHide("changeMaterialColor")]
        [SerializeField]
        private Color changeToColor;

        private Renderer _myRenderer;
        
        private void Awake()
        {
            onBurned.AddListener(MaterialChange);
            TryGetComponent(out _myRenderer);
        }

        private void MaterialChange()
        {
            if (changeMaterialColor)
            {
                _myRenderer.material.color = changeToColor;
            }
        }

        public void ObjectBurned()
        {
            onBurned.Invoke();
        }
    }
}
