using System;
using UnityEngine;

namespace Flames
{
    public class FlameDirection : MonoBehaviour
    {
        private static readonly int LerpVec = Shader.PropertyToID("_LerpVector");

        private MeshRenderer _myMeshRendered;
        private Vector3 _myVelocity;
        private bool _hasMeshRenderer;

        public MeshRenderer MyMeshRendered
        {
            get => _myMeshRendered;
            set
            {
                _myMeshRendered = value;
                _hasMeshRenderer = value != null;
            }
        }

        public Vector3 MyVelocity
        {
            get => _myVelocity;
            set => _myVelocity = value;
        }

        private void FixedUpdate()
        {
            if (!_hasMeshRenderer)
            {
                return;
            }

            var currentVelocity = MyVelocity;
            if (currentVelocity.Equals(Vector3.zero))
            {
                currentVelocity = Vector3.up;
            }

            var vel = new Vector4(-currentVelocity.x, Math.Abs(currentVelocity.y), -currentVelocity.z);
            MyMeshRendered.material.SetVector(LerpVec, vel);
        }
    }
}
