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

        protected virtual void Start()
        {
            onBurned.AddListener(MaterialChange);
            TryGetComponent(out MyRenderer);
            if (myAnimator == null)
            {
                activateAnimationBool = TryGetComponent(out myAnimator);
            }
        }

        private void MaterialChange()
        {
            if (changeMaterialColor)
            {
                foreach (var material in MyRenderer.materials)
                {
                    material.color = changeToColor;
                }
            }

            if (activateAnimationBool)
            {
                boolAnimatorParameter.Set(myAnimator, true);
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
