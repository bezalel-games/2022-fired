using UnityEngine;

namespace Mechanics.Infection
{
    public class Cheker : MonoBehaviour
    {
        [SerializeField] private Gilad.Flammable flammable;
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                print(flammable.IsOnFire());
            }
        }
    }
}
