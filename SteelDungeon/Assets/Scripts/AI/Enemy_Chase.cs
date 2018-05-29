using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//**Enemy_Chase Class
//* Simple chase AI, intended for the Slime enemy
//* The enemy finds the position of the player and begins to chase them at a certain range.
//* When the enemy gets close enough, it will attack.
//* If the player leaves the enemy's outer radius, it the enemy will stop chasing.

public class Enemy_Chase : MonoBehaviour {

    //References
    private GameObject target;              //Reference to the player
    private Enemy enemyScript;              //Reference to the enemy
    private Rigidbody2D rb2d;               //Enemy's Rigidbody2D

    //Chase Variables
    [Header("Chase Variables")]
    public float LOS = 0.8f;                //Enemy Line of Sight (The range in which they will spot the player)
    public float AttackRange = 0.15f;       //Enemy's Attack Range;

    // Use this for initialization
    void Start()
    {
        //Initialize the references
        target = GameObject.FindWithTag("Player");
        enemyScript = GetComponent<Enemy>();
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        //Find the distance between the player and enemy
        float distance = Vector2.Distance(transform.position, target.transform.position);

        //If the player is within the distance radius then chase
        if (distance < LOS && !enemyScript.dead && !enemyScript.attacking)
        {

            Vector2 dir = target.transform.position - transform.position;
            Vector2 velocity = dir.normalized * enemyScript.moveSpeed;
            rb2d.velocity = velocity;

        }
        //If the player is out of the radius then stop moving
        else if (distance >= LOS)
        {
            rb2d.velocity = Vector2.zero;
        }

        //If the player is within attacking range then stop moving and attack
        if (distance < AttackRange)
        {
            rb2d.velocity = Vector2.zero;
            enemyScript.Attack();
        }
    }
}
