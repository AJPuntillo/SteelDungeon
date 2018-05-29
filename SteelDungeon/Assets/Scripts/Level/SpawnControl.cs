using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//**SpawnControl Class
//* Handles the spawn chance of enemies and chests within the stage
//* Also tracks if all enemies have been defeated, allowing the player to advance

public class SpawnControl : MonoBehaviour {

    public EnemySpawn[] enemyList;          //Array of all enemys in the stage
    public GameObject chestSpawn;                //The possible chest to spawn in the stage
    public GameObject chest;
    public BoxCollider2D endCollider;       //The End collider, allows the player to advance to the next stage
    private int enemyCount = 0;

	// Use this for initialization
	void Start () {
        //Disable the End collider, preventing the player from leaving the stage without clearing it
        endCollider.enabled = false;

        //Determine if a chest will spawn with a 50/50 chance
        SpawnChest();

        //Loop through the enemy list and determine if an enemy will spawn with a 50/50 chance
        SpawnEnemy();

    }

    // Update is called once per frame
    void Update () {
        //Check to see if all enemies are defeated
        if (AreEnemiesDefeated())
            endCollider.enabled = true;
    }

    void SpawnChest()
    {
        int rand = Random.Range(0, 2);

        if (rand == 1)
            Instantiate(chest, chestSpawn.transform);
    }

    void SpawnEnemy()
    {
        for (int i = 0; i < enemyList.Length; i++)
        {
            int rand = Random.Range(0, 2);

            if (rand == 1)
            {
                enemyList[i].gameObject.SetActive(false);
                enemyCount++;
            }
        }

        //If all the enemies are chosen to now spawn, force spawn the first enemy
        if (enemyCount == enemyList.Length)
            enemyList[0].gameObject.SetActive(true);
    }

    bool AreEnemiesDefeated()
    {
        //When an enemy is defeated, their Tag becomes "None"
        //Therefore when all the enemies are defeated, this search return true
        if (GameObject.FindWithTag("Enemy") == null)
            return true;
        else
            return false;

    }
}
