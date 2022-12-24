using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Logger = Nemesh.Logger;

public class CharacterRadius : MonoBehaviour
{
    // private Transform fire;
    private int _count = 0;

    public HashSet<Transform> Fire { get; set; } = new();

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public Transform FindFire(Transform trans)
    {
        if (Fire.Count == 0)
        {
            return null;
        }

        var max = Fire.First(t => t != null);  // TODO: use some sort of API
        // () => transform.position()
        var curMax = Vector3.Distance(max.position, trans.position);
        foreach (var curFire in Fire)
        {
            var cur = Vector3.Distance(curFire.position, trans.position);
            if ((Vector3.Distance(max.position, trans.position)) < curMax)
            {
                curMax = cur;
                max = curFire;
            }
        }

        return max;
    }

    private void OnTriggerEnter(Collider other)
    {
        Logger.Log("trigger entered", this);
        Fire.Add(other.transform);
    }

    private void OnTriggerExit(Collider other)
    {
        Logger.Log("trigger exited" + _count, this);
        _count++;
        Fire.Remove(other.transform);
    }
}
