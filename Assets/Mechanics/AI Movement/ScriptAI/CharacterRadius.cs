using System.Collections.Generic;
using System.Linq;
using Gilad;
using UnityEngine;
using Logger = Nemesh.Logger;

public class CharacterRadius : MonoBehaviour
{
    // private Transform fire;
    private int _count = 0;

    public HashSet<Transform> Fire { get; set; } = new();
    private List<GameObject> Flames;

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
        for (int i = Flames.Count -1; i >=0; i--)
        {
            Flammable flammable =Flames[i].GetComponent<Flammable>();
            if (flammable.IsDoneBurning())
            {
                Fire.Remove(Flames[i].transform);
                Flames.Remove(Flames[i]);
            }
        }
        var max = Fire.First(t => t != null);  // TODO: use some sort of API
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
        Flammable flame = other.gameObject.GetComponent<Flammable>();
        if (flame.IsOnFire())
        {
            Fire.Add(other.transform);
            Flames.Add(other.gameObject);
        }
            
    }

    private void OnTriggerExit(Collider other)
    {
        Logger.Log("trigger exited" + _count, this);
        _count++;
        Fire.Remove(other.transform);
        Flames.Remove(other.gameObject);
    }
}
