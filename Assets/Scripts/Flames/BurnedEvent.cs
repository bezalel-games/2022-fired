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
        protected bool activateAnimationBool = true;

        // [ConditionalHide("activateAnimationBool")]
        [SerializeField]
        private BoolAnimationParameter boolAnimatorParameter;

        [ConditionalHide("activateAnimationBool")]
        [SerializeField]
        protected Animator myAnimator;

        [SerializeField]
        protected bool changeMaterialColor = true;

        [ConditionalHide("changeMaterialColor")]
        [SerializeField]
        protected Color changeToColor;

        protected Renderer MyRenderer;
        private bool _hasAnimator;
        private bool _hasRenderer;

        protected virtual void Start()
        {
            onBurned.AddListener(MaterialChange);
            _hasRenderer = TryGetComponent(out MyRenderer);
            if (myAnimator == null)
            {
                _hasAnimator = TryGetComponent(out myAnimator);
            }
        }

        private void MaterialChange()
        {
            if (changeMaterialColor && _hasRenderer)
            {
                foreach (var material in MyRenderer.materials)
                {
                    material.color = changeToColor;
                }
            }

            if (activateAnimationBool && _hasAnimator)
            {
                boolAnimatorParameter.Set(myAnimator, true);
            }
        }

        [Button]
        public void ObjectBurned()
        {
            onBurned.Invoke();
        }
    }
}
