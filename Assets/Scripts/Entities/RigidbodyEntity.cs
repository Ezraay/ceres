using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Rigidbody))]
public class RigidbodyEntity : MonoBehaviour {
    new Rigidbody rigidbody;

    void Awake () {
        rigidbody = GetComponent<Rigidbody> ();
    }

    void Update () {

    }
}