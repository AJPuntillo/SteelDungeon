using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawn : MonoBehaviour {

    public GameObject bossSpawn;                //The possible chest to spawn in the stage
    public GameObject boss;

    // Use this for initialization
    void Start () {
        Instantiate(boss, bossSpawn.transform);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
