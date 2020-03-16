using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerScript : MonoBehaviour
{
    public GameObject praticlePrefab;
    public GameObject praticleManager;
    public float delay = 0.1f;
    public float timer = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(Input.GetKey(KeyCode.Space) && timer > delay){
            Vector3 spawnPos = transform.position + new Vector3(Random.Range(-0.2f,0.2f),Random.Range(-0.2f,0.2f),Random.Range(-0.2f,0.2f));
            GameObject newParticle = Instantiate(praticlePrefab, spawnPos, new Quaternion(0,0,0,0));
            praticleManager.GetComponent<ParticleManager>().addParticle(newParticle);
            timer = 0;
        }
    }
}
