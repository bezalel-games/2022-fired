using System;
using Avrahamy;
using Avrahamy.EditorGadgets;
using UnityEngine;
using UnityEngine.Serialization;

namespace Flames
{
    public class KeepTransformRotation : OptimizedBehaviour
    {
        [FormerlySerializedAs("keepTransformRotationConstant")]
        [SerializeField]
        private bool keepYRotationConstant;

        private float _originalUp;

        public bool KeepYRotationConstant
        {
            get => keepYRotationConstant;
            set => keepYRotationConstant = value;
        }

        private void Awake()
        {
            _originalUp = transform.eulerAngles.y;
        }

        private void FixedUpdate()
        {
            if (KeepYRotationConstant && transform.hasChanged)
            {
                var angles = transform.eulerAngles;
                transform.eulerAngles = new Vector3(angles.x, _originalUp, angles.z);
            }
        }
    }
}
