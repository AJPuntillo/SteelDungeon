using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour {

    // Player properties
    [Header("Player Properties")]
    public float maxHealth = 3;
    public float health = 3;
    public float damage = 2;
    public float defense = 1;
    public float attackSpeed = 1;
    public float stunDuration = 0.5f;
    public float moveSpeed = 50;
    public float attackMovementSpeedMod = 0.3f;

    // Player status and timers
    [HideInInspector]
    public bool dead = false;
    private bool stunned = false;
    private bool attacking = false;
    private float stunTimer = 0;
    private float attackTimer = 0;

    // Player Components  
    private GameObject hitBox;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rigidBody2D;

    // Use Crossplatform Input
    [HideInInspector]
    public bool UseCrossplatformInput = false;

    // Use this for initialization
    void Start () {
        // Get Components
        animator = transform.Find("Sprite").GetComponent<Animator>();
        spriteRenderer = transform.Find("Sprite").GetComponent<SpriteRenderer>();
        hitBox = transform.Find("HitBox").gameObject;
        rigidBody2D = GetComponent<Rigidbody2D>();
        hitBox.GetComponent<BoxCollider2D>().enabled = false;        
        // Set up collisions layers to ignore
        Physics2D.IgnoreLayerCollision(8, 8, true);
        Physics2D.IgnoreLayerCollision(8, 9, true);
        Physics2D.IgnoreLayerCollision(8, 10, true);
        Physics2D.IgnoreLayerCollision(9, 10, true);
    }
	
	// Update is called once per frame
	void Update () {
        // Check if player has died
        CheckDeadStatus();
        // Update stun timer
        UpdateStunTimer();
        // Update attack timer
        UpdateAttackTimer();
        // Update facing direction and walking animation
        UpdateDirection();

        if (UseCrossplatformInput)
        {
            float h = CrossPlatformInputManager.GetAxis("Horizontal");
            float v = CrossPlatformInputManager.GetAxis("Vertical");

            h *= 0.4f;
            v *= 0.4f;

            if (!attacking && !dead)
            {
                rigidBody2D.velocity = new Vector3(h * moveSpeed * Time.deltaTime, v * moveSpeed * Time.deltaTime, 0);
            }
            else if (attacking && !dead)
            {
                h *= attackMovementSpeedMod;
                v *= attackMovementSpeedMod;
                rigidBody2D.velocity = new Vector3(h * moveSpeed * Time.deltaTime, v * moveSpeed * Time.deltaTime, 0);
            }

            if (CrossPlatformInputManager.GetAxis("Attack") > 0 && !dead)
            {
                Attack();
            }

            if (CrossPlatformInputManager.GetButtonDown("Attack") && GameObject.FindWithTag("PlayerUI").transform.GetChild(3).gameObject.activeSelf == true)
            {
                MainMenu();
            }

            if (CrossPlatformInputManager.GetButtonDown("Item") && GameObject.FindWithTag("PlayerUI").transform.GetChild(3).gameObject.activeSelf == true)
            {
                RestartGame();
            }
        }
        else
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            if (!attacking && !dead)
            {
                rigidBody2D.velocity = new Vector3(h * moveSpeed * Time.deltaTime, v * moveSpeed * Time.deltaTime, 0);
            }
            else if (attacking && !dead)
            {
                h *= attackMovementSpeedMod;
                v *= attackMovementSpeedMod;
                rigidBody2D.velocity = new Vector3(h * moveSpeed * Time.deltaTime, v * moveSpeed * Time.deltaTime, 0);
            }

            if (Input.GetAxis("Attack") > 0 && !dead)
            {
                Attack();
            }

            if (Input.GetButtonDown("Attack") && GameObject.FindWithTag("PlayerUI").transform.GetChild(3).gameObject.activeSelf == true)
            {
                MainMenu();
            }

            if (Input.GetButtonDown("Item") && GameObject.FindWithTag("PlayerUI").transform.GetChild(3).gameObject.activeSelf == true)
            {
                RestartGame();
            }
        }

        if (health > maxHealth)
        {
            health = maxHealth;
        }
    }

    void CheckDeadStatus()
    {
        if (health <= 0)
        {
            health = 0;
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

    void UpdateDirection()
    {
        if (!attacking)
        {
            // Direction
            if (rigidBody2D.velocity.x < -0.1)
            {
                spriteRenderer.flipX = true;
                hitBox.transform.localScale = new Vector3(-1, 1, 1);
            }
            else if (rigidBody2D.velocity.x > 0.1)
            {
                spriteRenderer.flipX = false;
                hitBox.transform.localScale = new Vector3(1, 1, 1);
            }
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

    void Attack()
    {
        if (!attacking && !dead)
        {
            attacking = true;
            float tempAS = attackSpeed;
            if(tempAS < 0.2f)
            {
                tempAS = 0.2f;
            }
            attackTimer = animator.GetCurrentAnimatorClipInfo(0)[0].clip.length * tempAS;
            //rigidBody2D.velocity = Vector3.zero;
            animator.speed = animator.speed / tempAS;
            animator.SetTrigger("Attack");
        }
    }

    IEnumerator FlashRed(float duration)
    {
        while (duration > 0f)
        {
            duration -= Time.deltaTime;
            if (spriteRenderer.color == Color.red)
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
            if(trueDamage <= 0)
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
        rigidBody2D.velocity = Vector3.zero;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "EnemyWeapon")
        {
            Hit(other.gameObject.transform.parent.GetComponent<Enemy>().damage);
        }
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("GameScene");
    }
}