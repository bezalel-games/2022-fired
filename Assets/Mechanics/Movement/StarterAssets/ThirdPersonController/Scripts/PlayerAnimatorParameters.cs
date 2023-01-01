using System;
using BitStrap;
using UnityEngine;

namespace StarterAssets
{
    [Serializable]
    public struct PlayerAnimatorParameters
    {
        [Header("Booleans")]
        [SerializeField]
        public BoolAnimationParameter useAnalogParameter;

        [Space]
        [SerializeField]
        public BoolAnimationParameter groundedParameter;

        [SerializeField]
        public BoolAnimationParameter sprintingParameter;

        [SerializeField]
        public BoolAnimationParameter walkingParameter;

        [SerializeField]
        public BoolAnimationParameter jumpingParameter;

        [SerializeField]
        public BoolAnimationParameter fallingParameter;

        [Space]
        [Header("Floats")]
        [SerializeField]
        public FloatAnimationParameter analogSpeedParameter;

        [SerializeField]
        public FloatAnimationParameter motionSpeedParameter;

        [Space]
        [SerializeField]
        public FloatAnimationParameter directionXParameter;

        [SerializeField]
        public FloatAnimationParameter directionZParameter;

        [SerializeField]
        public FloatAnimationParameter rotationParameter;
    }
}
