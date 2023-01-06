using System;
using BitStrap;
using UnityEngine;
using Logger = Nemesh.Logger;

namespace Gilad
{
    public class FlameRestartManager : MonoBehaviour
    {

        // private static FlameRestartManager _instance;

        private void Awake()
        {
            // if (_instance != null)
            // {
                // Destroy(this);
                // return;
            // }

            // _instance = this;
            Flammable.AllBurning.Clear();
            Flammable.AllFlammables.Clear();
            Flammable.NumBurned = 0;
        }

    }
}
