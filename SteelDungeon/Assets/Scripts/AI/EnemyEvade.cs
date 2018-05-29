using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEvade : MonoBehaviour {

    private Enemy enemy;
    private GameObject target;
    private Rigidbody2D rigidB2D;

    private float runTimer = 0;

    // Use this for initialization
    void Start () {
        enemy = GetComponent<Enemy>();
        target = GameObject.FindWithTag("Player");
        rigidB2D = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        float distance = Vector3.Distance(transform.position, target.transform.position);

        if (distance < 0.3)
        {
            Vector3 dir = transform.position - target.transform.position;
            Vector3 velocity = dir.normalized * enemy.moveSpeed;
            rigidB2D.velocity = velocity;
            runTimer = 1;
        }
        
        if(runTimer <= 0)
        {
            runTimer = 0;
            rigidB2D.velocity = Vector3.zero;
        }

        runTimer -= Time.deltaTime;

        if(enemy.dead)
        {
            rigidB2D.velocity = Vector3.zero;
        }
    }
}
