using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterRadius : MonoBehaviour
{
    // private Transform fire;
    private int cout = 0;

    public HashSet<Transform> fire;
    // Start is called before the first frame update
    void Start()
    {
        fire = new HashSet<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Transform FindFire(Transform transform)
    {
        if (fire.Count == 0)
        {
            return null;
        }
        var max = fire.First(t => t!=null);
        // () => transform.position()
        var curMax = Vector3.Distance(max.position, transform.position);
        foreach (var curFire in fire)
        {
            var cur = Vector3.Distance(curFire.position, transform.position);
            if((Vector3.Distance(max.position, transform.position))<curMax)
            {
                curMax = cur;
                max = curFire;
            }
        }
        return max;
    }

    private void OnTriggerEnter(Collider other)
    {
        print("yap");
            fire.Add(other.transform);
    }

    private void OnTriggerExit(Collider other)
    {
        print("nap" +cout);
        cout++;
            fire.Remove(other.transform);
    }
}
