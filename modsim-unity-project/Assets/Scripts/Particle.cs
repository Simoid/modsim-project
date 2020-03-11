using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour
{
    public Vector3 position;
    public Vector3 velocity;
    public Vector3 combinedForce;
    public float density;
    public float mass;
    public float pressure;
    public float radius;

    void OnCollisionEnter(Collision other)
    {
        velocity = GetComponent<Rigidbody>().velocity;
        Debug.Log(other);
    }

    void calculateCollisions(){
    }
}
