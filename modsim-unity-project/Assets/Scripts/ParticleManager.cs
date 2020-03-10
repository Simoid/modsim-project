﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public GameObject[] particles;
    public float neighborRadius = 10;
    public float restDensity = 10;
    public float gasConstant = 8.314f;
    public float viscosityConst = 1;
    public float particleMass = 5;
    public float smoothingRadius = 10;
    public float gravityConst = 9.82f;



    // Start is called before the first frame update
    void Start()
    {
        particles = GameObject.FindGameObjectsWithTag("Particle");

        //Init particles
        for(int i = 0; i < particles.Length; i++){
            particles[i].GetComponent<Particle>().mass = particleMass;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // For each particle, set density and pressure
        for(int i = 0; i < particles.Length; i++){
            setDensity(i);
            setPressure(i);
        }

        for(int i = 0; i < particles.Length; i++){
            Vector3 pressureForce;
            Vector3 viscForce;
            Vector3 gravityForce;

            for(int j = 0; j < particles.Length; j++){
                if(i != j){
                    pressureForce = calculatePressureForce(i, j);
                    viscForce = calculateViscForce(i, j);
                }
            }
            gravityForce = new Vector3(0, -1, 0) * particles[i].GetComponent<Particle>().mass * gravityConst;
        }
    }

    void setDensity(int index){
        float totalDensity = 0;
        float distanceBetween = float.MaxValue;
        for(int j = 0; j < particles.Length; j++){
            if(index != j && distanceBetween < neighborRadius){ // Should index != j not be here????
                totalDensity += particles[j].GetComponent<Particle>().mass * calculatePoly6Kernel(distanceBetween, smoothingRadius);
            }
        }
        particles[index].GetComponent<Particle>().density = totalDensity;
    }

    void setPressure(int index){
        float totalPressure = gasConstant * (particles[index].GetComponent<Particle>().density - restDensity);
        particles[index].GetComponent<Particle>().pressure = totalPressure;
    }

    // calc force from pressure
    Vector3 calculatePressureForce(int i, int j){
        Vector3 distanceVector = particles[j].transform.position - particles[i].transform.position;
        Vector3 calculatedKernel = calculateSpikyKernel(distanceVector, smoothingRadius);
        return -(particles[i].GetComponent<Particle>().mass / particles[i].GetComponent<Particle>().density) * calculatedKernel; //todo;
    }

    Vector3 calculateViscForce(int i, int j){
        float distance = calculateViscKernel(Vector3.Distance(particles[i].transform.position, particles[j].transform.position), smoothingRadius);
        Vector3 velocityDiff = particles[j].GetComponent<Particle>().velocity - particles[i].GetComponent<Particle>().velocity;
        return viscosityConst * particles[j].GetComponent<Particle>().mass * velocityDiff/particles[i].GetComponent<Particle>().density 
                * calculateViscKernel(distance, smoothingRadius); //todo;
    }

    // calc kernel
    float calculatePoly6Kernel(float r, float h){
        float abs_r = Mathf.Abs(r);

        if(0 <= abs_r && abs_r <= h ){
            return 315 / (64 * Mathf.PI * Mathf.Pow(h, 9)) * Mathf.Pow(Mathf.Pow(h, 2) - Mathf.Pow(abs_r, 2), 3);
        }else{
            return 0;
        }
    }

    // calc kernel
    Vector3 calculateSpikyKernel(Vector3 r, float h){
        float r_mag = r.magnitude;
        Vector3 r_norm = r.normalized; 

        if(0 <= r_mag && r_mag <= h ){
            return -45 / (Mathf.PI * Mathf.Pow(h, 6)) * r_norm * Mathf.Pow(h - Mathf.Pow(r_mag, 2), 2);
        }else{
            return Vector3.zero;
        }
    }

    // calc visc kernel
    float calculateViscKernel(float r, float h){
        float abs_r = Mathf.Abs(r);
 
        if(0 <= abs_r && abs_r <= h ){
            return 45 / (Mathf.PI * Mathf.Pow(h, 6)) * (h - abs_r);
        }else{
            return 0;
        }
    }

    //OLD FUNC
    float OLDcalculatePoly6KernelOld(float r, float h){
        float abs_r = Mathf.Abs(r);

        if(0 <= abs_r && abs_r <= h ){
            return Mathf.Pow(Mathf.Pow(h, 2) - Mathf.Pow(abs_r, 2), 3);
        }else{
            return 0;
        }
    }

    void setGameObjectPosition(){
        return;
    }
}
