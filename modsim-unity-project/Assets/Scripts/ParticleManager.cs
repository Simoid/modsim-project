using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public GameObject[] particles;
    public float neighborRadius = 10;
    public float restDensity = 10;
    public float gasConstant = 8.314f;
    public float viscosityConst = 1;
    public float particleMass = 10;
    public float smoothingRadius = 10;
    public float gravityConst = 9.82f;



    // Start is called before the first frame update
    void Start()
    {
        particles = GameObject.FindGameObjectsWithTag("Particle");

        //Init particles
        for(int i = 0; i < particles.Length; i++){
            particles[i].GetComponent<Particle>().mass = particleMass;
            particles[i].GetComponent<Particle>().density = 0; //todo
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
            Vector3 pressureForce = Vector3.zero;
            Vector3 viscForce = Vector3.zero;
            Vector3 gravityForce = Vector3.zero;

            for(int j = 0; j < particles.Length; j++){
                if(i != j){
                    pressureForce += calculatePressureForce(i, j);
                    viscForce += calculateViscForce(i, j);
                }
            }
            //Todo: With gravityForce the particles fall with massive speed, so I multiplied it with 0.001f, fix this later...
            gravityForce = new Vector3(0, -1, 0) * particles[i].GetComponent<Particle>().density * gravityConst; // mass should be density??
            Vector3 combinedForce = pressureForce + viscForce + gravityForce;
            particles[i].GetComponent<Particle>().combinedForce = combinedForce;
            //particles[i].GetComponent<Particle>().velocity += Time.deltaTime * combinedForce / particles[i].GetComponent<Particle>().density;
            particles[i].GetComponent<Rigidbody>().velocity += Time.deltaTime  * combinedForce / particles[i].GetComponent<Particle>().density;
            //particles[i].transform.position += Time.deltaTime * particles[i].GetComponent<Particle>().velocity;
        }
    }

    void calculateCollision(){

    }

    void setDensity(int index){
        float totalDensity = 0;
        for(int j = 0; j < particles.Length; j++) {
            float distanceBetween = Vector3.Distance(particles[index].transform.position, particles[j].transform.position);
            if (distanceBetween < neighborRadius){ // Should index != j not be here????
                //totalDensity += particles[j].GetComponent<Particle>().mass * calculatePoly6Kernel(distanceBetween, smoothingRadius);
                totalDensity += particleMass * (315.0f / (64.0f * Mathf.PI * Mathf.Pow(smoothingRadius, 9))) * Mathf.Pow(Mathf.Pow(smoothingRadius,2) - Mathf.Pow(distanceBetween,2), 3);
            }
        }
        float res_density = Mathf.Max(totalDensity, restDensity);
        Debug.Log(restDensity);
        particles[index].GetComponent<Particle>().density = res_density;
        /*
        
         *  Todo: Here I set the density to restDensity if the 'totalDensity' is 0,
         *  because otherwise we get null values. But this method might be wrong.
             
        if(Mathf.Abs(totalDensity) < 1e-10)
        {
            particles[index].GetComponent<Particle>().density = restDensity;
        }
        */
    }

    void setPressure(int index){
        float totalPressure = gasConstant * (particles[index].GetComponent<Particle>().density - restDensity);
        particles[index].GetComponent<Particle>().pressure = totalPressure;
    }

    // calc force from pressure
    Vector3 calculatePressureForce(int i, int j){
        Vector3 distanceVector = particles[j].transform.position - particles[i].transform.position;
        //Debug.Log("d "+distanceVector);
        //Vector3 calculatedKernel = calculateSpikyKernel(distanceVector, smoothingRadius);
        //Debug.Log("m :" + particles[i].GetComponent<Particle>().mass);
        //Debug.Log("d :" + particles[i].GetComponent<Particle>().density);
        //return -(particles[i].GetComponent<Particle>().mass / particles[i].GetComponent<Particle>().density) * calculatedKernel; 
        if(distanceVector.magnitude < smoothingRadius){
            return -1 * (distanceVector.normalized) * particleMass * (particles[i].GetComponent<Particle>().pressure * particles[j].GetComponent<Particle>().pressure)
                    / (2.0f * particles[j].GetComponent<Particle>().density) 
                    * (15.0f /(Mathf.PI * Mathf.Pow(smoothingRadius, 6))) * Mathf.Pow(smoothingRadius - distanceVector.magnitude, 3);
        }else{
            return Vector3.zero;
        }
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
            return 315.0f / (64.0f * Mathf.PI * Mathf.Pow(h, 9)) * Mathf.Pow(Mathf.Pow(h, 2) - Mathf.Pow(abs_r, 2), 3);
        }else{
            return 0;
        }
    }

    // calc kernel
    Vector3 calculateSpikyKernel(Vector3 r, float h){
        float r_mag = r.magnitude;
        Vector3 r_norm = r.normalized; 

        if(0 <= r_mag && r_mag <= h ){
            return -45.0f / (Mathf.PI * Mathf.Pow(h, 6)) * r_norm * Mathf.Pow(h - Mathf.Pow(r_mag, 2), 2);
        } else {
            return Vector3.zero;
        }
    }

    // calc visc kernel
    float calculateViscKernel(float r, float h){
        float abs_r = Mathf.Abs(r);
 
        if(0 <= abs_r && abs_r <= h ) {
            return 45.0f / (Mathf.PI * Mathf.Pow(h, 6)) * (h - abs_r);
        } else {
            return 0;
        }
    }

    //OLD FUNC
    float OLDcalculatePoly6KernelOld(float r, float h){
        float abs_r = Mathf.Abs(r);

        if(0 <= abs_r && abs_r <= h ) {
            return Mathf.Pow(Mathf.Pow(h, 2) - Mathf.Pow(abs_r, 2), 3);
        } else {
            return 0;
        }
    }

    void setGameObjectPosition(){
        return;
    }
}
