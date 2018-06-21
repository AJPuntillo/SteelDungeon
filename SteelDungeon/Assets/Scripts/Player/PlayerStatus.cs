using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour {

	void Die()
    {
        if (GameObject.FindWithTag("PlayerUI") != null)
        {
            GameObject.FindWithTag("PlayerUI").transform.GetChild(3).gameObject.SetActive(true);
            GameObject.FindWithTag("Player_RB").GetComponent<Player>().gameOver = true;
        }
    }
}
