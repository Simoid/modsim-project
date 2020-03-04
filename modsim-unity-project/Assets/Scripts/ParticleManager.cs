using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    float calculateDensity(Particle particle, Particle[] particles){
        return 0;
    }

    float calculatePoly6Kernel(float r, float h){
        float abs_r = Mathf.Abs(r);

        if(0 <= abs_r && abs_r <= h ){
            return Mathf.Pow(Mathf.Pow(h, 2) - Mathf.Pow(abs_r, 2), 3);
        }else{
            return 0;
        }
    }
}
