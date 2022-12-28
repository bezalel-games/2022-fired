using Avrahamy.EditorGadgets;
using BitStrap;
using UnityEngine;
using UnityEngine.Events;
using Logger = Nemesh.Logger;

namespace Flames
{
    public class BurnedEvent : MonoBehaviour
    {
        [SerializeField]
        protected UnityEvent onBurned;

        [SerializeField]
        protected bool changeMaterialColor = true;

        [ConditionalHide("changeMaterialColor")]
        [SerializeField]
        protected Color changeToColor;

        protected Renderer MyRenderer;
        
        protected virtual void Start()
        {
            onBurned.AddListener(MaterialChange);
            TryGetComponent(out MyRenderer);
        }

        private void MaterialChange()
        {
            if (changeMaterialColor)
            {
                MyRenderer.material.color = changeToColor;
            }
        }
        
        [Button]
        public void ObjectBurned()
        {
            var c = new Color(1f, 0.26f, 0.05f);
            Logger.Log("Burned", c, this);
            onBurned.Invoke();
        }
    }
}
