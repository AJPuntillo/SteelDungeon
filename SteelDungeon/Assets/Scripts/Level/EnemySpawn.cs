using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//**EnemySpawn Class
//* Handles the type of enemy that will spawn

public class EnemySpawn : MonoBehaviour {

    //Enemy Variables
    [Header("Enemy Variables")]
    public GameObject[] enemies;             //Array of enemy-types
    public GameObject spawnedEnemy;          //Currently seleted enemy

	void Start ()
    {
        //If an enemy can spawn then choose the type of enemy from the enemies array
        GenerateEnemyType();
    }

    void GenerateEnemyType()
    {
        int rand = Random.Range(0, 9);

        switch (rand)
        {
            case 0:
            case 1:
                spawnedEnemy = (GameObject)Instantiate(enemies[0], transform);  //Slime
                break;
            case 2:
            case 3:
                spawnedEnemy = (GameObject)Instantiate(enemies[1], transform);  //Wizard
                break;
            case 4:
            case 5:
                spawnedEnemy = (GameObject)Instantiate(enemies[2], transform);  //Orc
                break;
            case 6:
            case 7:
                spawnedEnemy = (GameObject)Instantiate(enemies[3], transform);  //Statue
                break;
            case 8:
                spawnedEnemy = (GameObject)Instantiate(enemies[4], transform);  //Deer
                break;
        }
    }
}
