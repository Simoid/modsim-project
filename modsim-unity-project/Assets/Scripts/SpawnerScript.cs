using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerScript : MonoBehaviour
{
    public GameObject praticlePrefab;
    public GameObject praticleManager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Space)){
            Vector3 spawnPos = transform.position + new Vector3(Random.Range(-1,1),Random.Range(-1,1),Random.Range(-1,1));
            GameObject newParticle = Instantiate(praticlePrefab, spawnPos, new Quaternion(0,0,0,0));
            praticleManager.GetComponent<ParticleManager>().addParticle(newParticle);
        }
    }
}
