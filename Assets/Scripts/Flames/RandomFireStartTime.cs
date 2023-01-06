using Avrahamy.EditorGadgets;
using BitStrap;
using UnityEngine;
using Logger = Nemesh.Logger;
using Random = UnityEngine.Random;

namespace Flames
{
    public class RandomFireStartTime : MonoBehaviour
    {
        private static readonly int RandomSeed = Shader.PropertyToID("_Random_Seed");

        [SerializeField]
        private bool useManuallyEnteredSeed;

        [ConditionalHide("useManuallyEnteredSeed")]
        [SerializeField]
        private Vector2 seed;
        
        [Space]
        [Header("Current Parameters:")]
        [SerializeField]
        [BitStrap.ReadOnly]
        private bool hasRandomSeedVectorInMaterial;

        [SerializeField]
        [BitStrap.ReadOnly]
        private Vector2 currentSeed;

        private Material _myMaterial;

        private void Awake()
        {
            var hasRenderer = TryGetComponent(out Renderer myRenderer);
            _myMaterial = myRenderer.material;
            hasRandomSeedVectorInMaterial = hasRenderer && _myMaterial.HasVector(RandomSeed);
            ResetSeed();
        }

        [Button]
        public void ResetSeed()
        {
            currentSeed = useManuallyEnteredSeed ? seed : Random.insideUnitCircle;

            if (!hasRandomSeedVectorInMaterial || _myMaterial == null)
            {
                Logger.LogWarning("No Material with random seed or not in Play!", this);
                return;
            }

            if (Application.isPlaying)
            {
                _myMaterial.SetVector(RandomSeed, currentSeed);
            }
            else
            {
                Logger.LogWarning("Cant set material in Editor! because prefab stuff.", this);
            }
        }
    }
}
