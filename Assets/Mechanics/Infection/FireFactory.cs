using UnityEngine;
using UnityEngine.Pool;

namespace Mechanics.Infection
{
    public class FireFactory : MonoBehaviour
    {
        private static FireFactory _instance;

        private LinkedPool<GameObject> _allFires;

        [SerializeField] private GameObject fireGameObject;


        // Start is called before the first frame update
        void Start()
        {
            // hey
            _instance = this;
            _allFires = new LinkedPool<GameObject>(CreateFire, null, null, null, false, 200);
        }

        private GameObject CreateFire()
        {
            return Instantiate(fireGameObject);
        }

        public static GameObject GetNewFire()
        {
            var fire = _instance._allFires.Get();
            fire.SetActive(true);
            return fire;
        }

        public static void ReleaseFire(GameObject fire)
        {
            fire.SetActive(false);
            _instance._allFires.Release(fire);
            
        }
    }
}
