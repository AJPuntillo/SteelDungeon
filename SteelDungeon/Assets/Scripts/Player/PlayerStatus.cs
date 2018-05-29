using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour {

	void Die()
    {
        GameObject.FindWithTag("PlayerUI").transform.GetChild(3).gameObject.SetActive(true);
    }
}
