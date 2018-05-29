using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{

    public float damage = 2;
    public float life = 3;
    private float scale = 1;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        life -= Time.deltaTime;
        if (life <= 1)
        {
            scale -= Time.deltaTime;
        }
        transform.localScale = new Vector3(scale, scale, scale);
        if (life <= 0)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (other.gameObject.transform.parent.GetComponent<Player>() != null)
            {
                other.gameObject.transform.parent.GetComponent<Player>().Hit(damage);
            }
            Destroy(gameObject);
        }
    }
}