using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    // Slime properties
    [Header("Enemy Properties")]
    public float maxHealth = 1;
    public float health = 1;
    public float damage = 1;
    public float defense = 1;
    public float attackSpeed = 1;
    public float stunDuration = 0.5f;
    public float moveSpeed = 50;
    public bool hurtOnTouch = false;

    // Enemy status and timers
    [HideInInspector]
    public bool attacking = false;
    [HideInInspector]
    public bool dead = false;
    private bool stunned = false;
    private float stunTimer = 0;
    private float attackTimer = 0;
    public bool flipped = false;

    // Enemy components
    private GameObject hitBox;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rigidBody2D;

	// Use this for initialization
	void Start () {
        // Get Components
        animator = transform.Find("Sprite").GetComponent<Animator>();
        spriteRenderer = transform.Find("Sprite").GetComponent<SpriteRenderer>();
        hitBox = transform.Find("HitBox").gameObject;        
        rigidBody2D = GetComponent<Rigidbody2D>();
        hitBox.GetComponent<BoxCollider2D>().enabled = false;
    }
	
	// Update is called once per frame
	void Update () {
        // Check if slime has died
        CheckDeadStatus();
        // Update stun timer
        UpdateStunTimer();
        // Update attack timer
        UpdateAttackTimer();
        // Update facing direction and walking animation
        UpdateDirection();    

        // Attack
        //Attack();        
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if(other.tag == "PlayerWeapon")
        {
            Hit(other.gameObject.transform.parent.GetComponent<Player>().damage);
        }

        // code for players getting hurt if they touch an enemy
        if (hurtOnTouch)
        {
            if (other.tag == "Player")
            {
                if (other.gameObject.transform.parent.GetComponent<Player>() != null)
                {
                    other.gameObject.transform.parent.GetComponent<Player>().Hit(damage, transform);
                }
            }
        }
    }

    void CheckDeadStatus()
    {
        if (health <= 0)
        {
            Die();
        }
    }

    void UpdateStunTimer()
    {
        stunTimer -= Time.deltaTime;
        if (stunTimer <= 0)
        {
            stunTimer = 0;
            stunned = false;
        }
    }

    void UpdateAttackTimer()
    {
        if (attacking)
        {
            attackTimer -= Time.deltaTime;
        }
        if (attackTimer <= 0 && attacking)
        {
            attackTimer = 0;
            attacking = false;
            animator.speed = 1;
        }
    }

    public void Attack()
    {
        if (!attacking && !dead)
        {
            attacking = true;
            attackTimer = animator.GetCurrentAnimatorClipInfo(0)[0].clip.length * attackSpeed;
            animator.speed = animator.speed / attackSpeed;
            animator.SetTrigger("Attack");
        }
    }

    void UpdateDirection()
    {
        // Direction
        if (rigidBody2D.velocity.x < -0.1)
        {
            FaceLeft();
        }
        else if (rigidBody2D.velocity.x > 0.1)
        {
            FaceRight();
        }

        // Animation
        if (rigidBody2D.velocity != Vector2.zero)
        {
            animator.SetBool("Walking", true);
        }
        else
        {
            animator.SetBool("Walking", false);
        }
    }

    IEnumerator FlashRed(float duration)
    {
        while (duration > 0f)
        {
            duration -= Time.deltaTime;
            if(spriteRenderer.color == Color.red)
            {
                spriteRenderer.color = Color.white;
            }
            else
            {
                spriteRenderer.color = Color.red;
            }
            yield return new WaitForSeconds(0.05f);
        }
        spriteRenderer.color = Color.white;
    }

    IEnumerator FlashBlue(float duration)
    {
        while (duration > 0f)
        {
            duration -= Time.deltaTime;
            if (spriteRenderer.color == Color.blue)
            {
                spriteRenderer.color = Color.white;
            }
            else
            {
                spriteRenderer.color = Color.blue;
            }
            yield return new WaitForSeconds(0.05f);
        }
        spriteRenderer.color = Color.white;
    }

    public void Hit(float enemyDamage)
    {
        if (!stunned && !dead)
        {
            stunned = true;
            float trueDamage = enemyDamage - defense;
            if (trueDamage <= 0)
            {
                trueDamage = 1;
                if (health - trueDamage == 0)
                {
                    StartCoroutine(FlashRed(0.1f));
                }
                else
                {
                    StartCoroutine(FlashBlue(0.1f));
                }
            }
            else
            {
                StartCoroutine(FlashRed(0.1f));
            }
            health -= trueDamage;
            stunTimer = stunDuration;
        }
    }

    void Die()
    {
        animator.SetBool("Dead", true);
        dead = true;
        hitBox.SetActive(false);
        damage = 0;
        transform.Find("Sprite").GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
        rigidBody2D.velocity = Vector2.zero;
        tag = "None";
        transform.GetChild(0).tag = "None";
    }

    public void FaceRight()
    {
        if (flipped)
        {
            spriteRenderer.flipX = true;
            hitBox.transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            spriteRenderer.flipX = false;
            hitBox.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    public void FaceLeft()
    {
        if (flipped)
        {
            spriteRenderer.flipX = false;
            hitBox.transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            spriteRenderer.flipX = true;
            hitBox.transform.localScale = new Vector3(-1, 1, 1);
        }
    }
}