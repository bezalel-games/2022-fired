using System;
using Avrahamy;
using BitStrap;
using UnityEngine;
using Logger = Nemesh.Logger;

namespace Gilad
{
    public class FlameRestartManager : OptimizedBehaviour
    {
        private static FlameRestartManager _instance;

        public static FlameRestartManager Instance
        {
            get => _instance;
            set => _instance = value;
        }

        private static Transform _waterCannonParent;

        // private static FlameRestartManager _instance;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(this);
                return;
            }
            Instance = this;
            Flammable.AllBurning.Clear();
            Flammable.AllFlammables.Clear();
            Flammable.NumBurned = 0;
        }

    }
}
