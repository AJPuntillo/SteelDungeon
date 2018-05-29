using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//**UNUSED CODE
//* Maybe revist another day, but this AI implentation does not work well with out level layouts

public class Enemy_WallAvoidance : MonoBehaviour {

    //Private
    private Vector3 velocity = Vector3.zero;
    private Vector3 orientation = Vector3.up;
    private Vector3 dir = Vector3.zero;
    private Vector3 newTarget;
    private float dt;
    private float offset = 0.075f;
    private float rayLength = 0.2f;
    private bool targetFound = false;

    //Public
    public float speed;
    public float maxAngularSpeed;
    public int score = 0;

    //Components
    private Rigidbody2D rb;
    private SpriteRenderer sprite;

    public Transform rayR;
    public Transform rayL;

    //Objects
    private GameObject target;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindWithTag("Player");

        rayR.localPosition = Vector3.right * offset;
        rayL.localPosition = Vector3.left * offset;
    }

    // Update is called once per frame
    void Update()
    {
        dt = Time.deltaTime;

        Vector3 pos = transform.position;

        RaycastHit2D hitR = Physics2D.Raycast(rayR.position, orientation, rayLength);
        RaycastHit2D hitL = Physics2D.Raycast(rayL.position, orientation, rayLength);

        Debug.DrawRay(rayR.position, orientation * rayLength, Color.white);
        Debug.DrawRay(rayL.position, orientation * rayLength, Color.white);

        //Check to see if the missile is close to a wall
        if (hitL.collider != null && hitL.collider.CompareTag("Obstacle"))
        {
            newTarget = hitL.point + hitL.normal * 2.0f;
            targetFound = true;
        }
        else if (hitR.collider != null && hitR.collider.CompareTag("Obstacle"))
        {
            newTarget = hitR.point + hitR.normal * 2.0f;
            targetFound = true;
        }

        //Change direction depending on how close the missile is to an obstacle
        if (targetFound)
        {
            dir = newTarget - transform.position;
            targetFound = false;
        }
        else
        {
            dir = target.transform.position - transform.position;
        }

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

        velocity = orientation * speed;
        pos += velocity * dt;

        transform.position = pos;
    }

    void UpdateOrientation()
    {
        Vector3 angle = new Vector3(0, 0, -Mathf.Atan2(orientation.x, orientation.y) * Mathf.Rad2Deg);
        transform.eulerAngles = angle;
    }
}
