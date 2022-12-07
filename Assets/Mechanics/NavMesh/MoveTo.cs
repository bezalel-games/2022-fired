using System.Collections;
using System.Collections.Generic;
using UnityEngine;

  // MoveTo.cs
using UnityEngine.AI;

    
public class MoveTo : MonoBehaviour {
       
   public Transform goal;
       
   void Start () {
      NavMeshAgent agent = GetComponent<NavMeshAgent>();
      agent.destination = goal.position; 
   }
   
}
