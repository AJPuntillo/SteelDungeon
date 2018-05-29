using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFSM : MonoBehaviour {

    // BOSS OPTIONS
    public float decisionSpeed = 2.0f;
    public float runTime = 2;
    public float chaseTime = 3;
    public int attackTime = 1;
    public bool rage = false;
    private bool enraged = false;
    public GameObject projectile;

    // TIMERS
    private float decisionTimer = 0;
    private float runTimer = 0;
    private float chaseTimer = 0;
    private int attackCounter = 0;    

    // COMPONENTS
    private GameObject player;
    private Enemy enemy;
    private Rigidbody2D rigidB2D;

    // STATES
    private enum ACTIONS {IDLE, ATTACK, RUN, CHASE, SHOOT};
    private ACTIONS currentAction = ACTIONS.IDLE;

	// Use this for initialization
	void Start () {
        player = GameObject.FindWithTag("Player_RB");
        enemy = GetComponent<Enemy>();
        rigidB2D = GetComponent<Rigidbody2D>();
        decisionTimer = decisionSpeed;
	}
	
	// Update is called once per frame
	void Update () {
        // Make sure enemy and player are alive;
        if (!enemy.dead && !player.GetComponent<Player>().dead)
        {
            // Count down decision timer
            decisionTimer -= Time.deltaTime;
            // IDLE
            if (currentAction == ACTIONS.IDLE)
            {
                if (decisionTimer <= 0)
                {
                    SelectAction();
                }
            }
            // ATTACK
            if (currentAction == ACTIONS.ATTACK)
            {
                Attack();
            }
            // RUN
            if (currentAction == ACTIONS.RUN)
            {
                Run();
            }
            // CHASE
            if (currentAction == ACTIONS.CHASE)
            {
                Chase();
            }
            // SHOOT
            if (currentAction == ACTIONS.SHOOT)
            {
                Shoot();
            }
            // At half health the boss will gain stats
            UpdateRage();
        }
        else
        {
            // Boss taunts if you're dead
            rigidB2D.velocity = Vector2.zero;
            transform.GetChild(0).GetComponent<Animator>().SetBool("Gesture", true);
        }
        if(enemy.dead)
        {
            transform.GetChild(0).GetComponent<Animator>().SetBool("Gesture", false);
        }
	}

    public void ResetDecisionTimer()
    {
        if (attackCounter <= 0)
        {
            currentAction = ACTIONS.IDLE;
            rigidB2D.velocity = Vector2.zero;
            decisionTimer = decisionSpeed;
        }
        else 
        {
            attackCounter--;
        }
    }

    void SelectAction()
    {
        transform.GetChild(0).GetComponent<Animator>().SetBool("Gesture", false);
        // GET DISTANCE FROM PLAYER
        float distance = Vector3.Distance(player.transform.position, transform.position);
        // IF CLOSE TO PLAYER PICK BETWEEN ATTCKING OR RUNNING AWAY
        if (distance <= 0.4)
        {
            int chance = Random.Range(0, 2);
            if (chance == 0)
            {
                attackCounter = attackTime;
                currentAction = ACTIONS.ATTACK;
            }
            else if (chance == 1)
            {
                runTimer = runTime;
                currentAction = ACTIONS.RUN;
            }
        }
        // IF FAR FROM PLAYER PICK BETWEEN SHOOTING OR CHASING THEM
        else if (distance > 0.4)
        {
            int chance = Random.Range(0, 2);
            if (chance == 0)
            {
                chaseTimer = chaseTime;
                currentAction = ACTIONS.CHASE;
            }
            else if (chance == 1)
            {
                currentAction = ACTIONS.SHOOT;
            }
        }
    }

    void Attack()
    {
        if (attackCounter > 0)
        {
            if (player.transform.position.x > transform.position.x)
            {
                enemy.FaceRight();
            }
            else
            {
                enemy.FaceLeft();
            }
            rigidB2D.velocity = Vector2.zero;
            enemy.Attack();
        }
        else
        {
            ResetDecisionTimer();
        }
    }

    void Shoot()
    {
        if (player.transform.position.x > transform.position.x)
        {
            enemy.FaceRight();
        }
        else
        {
            enemy.FaceLeft();
        }
        rigidB2D.velocity = Vector2.zero;
        enemy.Attack();
        GameObject shot = Instantiate(projectile, transform);
        shot.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.red;
    }

    void Run()
    {
        // GET THE DISTANCES FROM THE PLAYER AND FROM THE CENTER
        float distancePlayer = Vector3.Distance(player.transform.position, transform.position);
        float distanceCenter = Vector3.Distance(Vector3.zero, transform.position);

        // IF THE PLAYER IS CLOSER THAN THE CENTER THEN RUN TO THE CENTER ELSE RUN TO THE PLAYER
        Vector3 dir;
        if (distancePlayer < distanceCenter)
        {
            dir = Vector3.zero - player.transform.position;
        }
        else
        {
            dir = transform.position - player.transform.position;
        }
        Vector3 velocity = dir.normalized * enemy.moveSpeed;
        rigidB2D.velocity = velocity;

        // WHEN RUN TIMER IS 0 STOP RUNNING
        if (runTimer <= 0)
        {
            runTimer = 0;
            rigidB2D.velocity = Vector2.zero;
            currentAction = ACTIONS.SHOOT;
        }

        runTimer -= Time.deltaTime;
    }

    void Chase()
    {
        // GET THE DISTANCES FROM THE PLAYER
        float distancePlayer = Vector3.Distance(player.transform.position, transform.position);

        if (distancePlayer <= 0.3)
        {
            rigidB2D.velocity = Vector2.zero;
            enemy.Attack();
        }
        else
        {
            Vector3 dir = player.transform.position - transform.position;
            Vector3 velocity = dir.normalized * enemy.moveSpeed;
            rigidB2D.velocity = velocity;
        }

        // WHEN RUN TIMER IS 0 STOP RUNNING
        if (chaseTimer <= 0)
        {
            chaseTimer = 0;
            rigidB2D.velocity = Vector2.zero;
            attackCounter = attackTime;
            currentAction = ACTIONS.ATTACK;
        }

        chaseTimer -= Time.deltaTime;
    }

    void UpdateRage()
    {
        if(enemy.health <= enemy.maxHealth / 2 && !enraged)
        {
            rage = true;
            rigidB2D.velocity = Vector2.zero;
            transform.GetChild(0).GetComponent<Animator>().SetBool("Gesture", true);
            attackCounter = 0;
            ResetDecisionTimer();
        }
        if(rage)
        {
            enemy.damage = Mathf.RoundToInt(enemy.damage * 1.5f);
            enemy.attackSpeed = 0.5f;
            decisionSpeed *= 0.5f;
            enemy.moveSpeed *= 1.5f;
            enemy.defense *= 1.5f;
            attackTime = 2;
            rage = false;
            enraged = true;
        }
    }
}
