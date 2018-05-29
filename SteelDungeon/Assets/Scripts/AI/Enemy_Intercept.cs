using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//**Enemy_Intercept Class
//* AI intended for the Wizard's projectiles.
//* Chases the player and tries to intercept based on the player's velocity.
//* Has an angular limit to prevent sharp turns.

public class Enemy_Intercept : MonoBehaviour {

    //References
    private GameObject target;                  //Reference to the player
    private Enemy enemyScript;                  //Reference to the enemy

    //Intercept variables
    private float dt;                           //Delta Time
    private float projectileSpeed = 0.6f;       //Speed of the projectile
    private float maxAngularSpeed = 120;        //The maximum angle for the projectile to turn
    Vector3 velocity = Vector3.zero;            //Velocity
    Vector3 orientation = Vector3.up;           //Orientation

    //Player's variables
    Vector3 playerVel = Vector3.zero;           //Velocity of the player

    // Use this for initialization
    void Start()
    {
        //Initialize the references
        target = GameObject.FindWithTag("Player_RB");
    }

    // Update is called once per frame
    void Update()
    {
        //Delta Time
        dt = Time.deltaTime;

        //If the target is moving, try to intercept, but if not then chase normally
        if (target.GetComponent<Rigidbody2D>().velocity.x > 0 || target.GetComponent<Rigidbody2D>().velocity.y > 0)
            Intercept();
        else
            Chase();
    }

    //Update the orientation of the sprite
    void UpdateOrientation()
    {
        Vector3 angle = new Vector3(0, 0, -Mathf.Atan2(orientation.x, orientation.y) * Mathf.Rad2Deg);
        transform.eulerAngles = angle;
    }

    //Try to intercept the target by predicting its position based on their velocity
    void Intercept()
    {
        //Grab the player's velocity from it's Rigidbody2D
        playerVel = new Vector3(target.GetComponent<Rigidbody2D>().velocity.x, target.GetComponent<Rigidbody2D>().velocity.y, 0.0f);

        //Calculate relative velocity
        Vector3 vr = playerVel - velocity;

        //Caluculate relative distance
        Vector3 sr = target.transform.position - transform.position;

        //Calculate time to close
        float tc = sr.magnitude / vr.magnitude;

        //Predicted position of the target
        Vector3 st = target.transform.position + playerVel * tc;

        //Desired direction
        Vector3 dir = st - transform.position;
        dir.Normalize();

        //Angle
        float dot = Vector3.Dot(dir, orientation.normalized);
        float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;

        //Compare
        if (angle > maxAngularSpeed * dt)
        {
            bool rightDir = Vector3.Dot(new Vector3(orientation.y, -orientation.x, 0), dir) > 0;

            //If rightDir is true, then flip the direction in the opposite direction
            int flipDir;
            if (rightDir)
                flipDir = -1;
            else
                flipDir = 1;

            orientation = Quaternion.Euler(0, 0, maxAngularSpeed * dt * flipDir) * orientation;
        }
        else
        {
            orientation = dir;
        }

        UpdateOrientation();

        Vector3 pos = transform.position;
        velocity = orientation * projectileSpeed;
        pos += velocity * dt;

        transform.position = pos;
    }

    //Chase towards the target's position
    void Chase()
    {
        Vector3 dir = target.transform.position - transform.position;
        dir.Normalize();


        float d = Vector3.Dot(dir, orientation.normalized);
        float a = Mathf.Acos(d) * Mathf.Rad2Deg;


        if (a > maxAngularSpeed * dt)
        {
            bool rightDir = Vector3.Dot(new Vector3(orientation.y, -orientation.x, 0), dir) > 0;

            //If rightDir is true, then flip the direction in the opposite direction
            int flipDir;
            if (rightDir)
                flipDir = -1;
            else
                flipDir = 1;

            orientation = Quaternion.Euler(0, 0, maxAngularSpeed * dt * flipDir) * orientation;
        }
        else
        {
            orientation = dir;
        }
        UpdateOrientation();

        Vector3 pos = transform.position;
        Vector3 velocity = orientation * projectileSpeed;
        pos += velocity * dt;

        transform.position = pos;
    }
}
