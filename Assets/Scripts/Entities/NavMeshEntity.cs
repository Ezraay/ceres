using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent (typeof (NavMeshAgent))]
public class NavMeshEntity : MonoBehaviour {
    NavMeshAgent agent;

    void Awake () {
        agent = GetComponent<NavMeshAgent> ();
    }

    void Update () {

    }
}