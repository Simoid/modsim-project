using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour
{
    public Vector3 position;
    public Vector3 velocity;
    public Vector3 totalForce;
    public float density;
    public float mass;
    public float pressure;
    public float radius;

    public void setDensity(float inputDensity){
        density = inputDensity;
    }

    public void setVelocity(Vector3 inputVelocity){
        velocity = inputVelocity;
    }

    public void setPressure(float inputPressure){
        pressure = inputPressure;
    }

}
