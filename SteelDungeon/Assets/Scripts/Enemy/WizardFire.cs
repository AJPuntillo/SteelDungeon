using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardFire : MonoBehaviour {

    public GameObject firePoint;
    public GameObject projectile;
    private Enemy enemy;
    private GameObject player;

	// Use this for initialization
	void Start () {
        enemy = transform.parent.gameObject.GetComponent<Enemy>();
	}
	
	// Update is called once per frame
	void Update () {
        enemy.Attack();
	}

    public void AttackStart()
    {
        Instantiate(projectile, firePoint.transform.position, firePoint.transform.rotation, transform.parent);
    }
}
