using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//**EndStage Class
//* Attached to the End Trigger on every stage.
//* When the player touches the trigger, this script is called and calls the GameManager's NextLevel method.

public class EndStage : MonoBehaviour {

    private GameObject gameManager;         //Used to create a reference to the active GameManager
    private bool isColliding = false;       //Used to only check collision once (So that the BossInt can't be incremented more than once)

	// Use this for initialization
	void Start () {
        //Find the active GameManager and set it to this GameObject
        gameManager = GameObject.FindWithTag("GameManager");
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        //If the collision has already been triggered then return
        if (isColliding)
            return;

        //Call the GameManager's NextLevel method
        if (col.gameObject.tag == "Player_RB" || col.gameObject.tag == "Player")
            StartCoroutine(gameManager.GetComponent<GameManager>().NextLevel());

        isColliding = true;
    }
}
