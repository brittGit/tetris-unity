using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    public GameObject[] groups;

    public void spawnNext() {
        // Random Index
        int i = Random.Range(0, groups.Length);

        // Spawn Group at current Position
        // transform.position is the spawners position
        // Quaternion.identify is the default rotation
        Instantiate(groups[i], transform.position, Quaternion.identity);
    }

    // Start is called before the first frame update
    void Start() {
        // Spawn first group on start
        spawnNext();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
