using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour {

    public GameObject heart;
    private GameObject player;
    private GameObject[] hearts;

    int heartAmount = 0;

	// Use this for initialization
	void Start () {
        player = GameObject.FindWithTag("Player_RB");
    }
	
	// Update is called once per frame
	void Update () {
        Vector3 offset = new Vector3(0, 0, 0);
        if (heartAmount < (int)player.GetComponent<Player>().health)
        {
            if (hearts != null)
            {
                for (int i = 0; i < hearts.Length; i++)
                {
                    Destroy(hearts[i]);
                }
            }
            hearts = new GameObject[(int)player.GetComponent<Player>().health];
            for (int i = 0; i < player.GetComponent<Player>().health; i++)
            {
                hearts[i] = Instantiate(heart);
                hearts[i].name = "Heart";
                hearts[i].transform.localPosition += offset;
                hearts[i].transform.SetParent(transform, false);
                offset.x += 30;
                heartAmount++;
            }
        }
        if(heartAmount == (int)player.GetComponent<Player>().maxHealth)
        {
            for (int i = 0; i < (int)player.GetComponent<Player>().health; i++)
            {
                hearts[i].transform.GetChild(0).GetComponent<Image>().color = new Color(255.0f / 255.0f, 245.0f / 255.0f, 103.0f / 255.0f);
            }
        }
        else 
        {
            for (int i = 0; i < player.GetComponent<Player>().health; i++)
            {
                hearts[i].transform.GetChild(0).GetComponent<Image>().color = Color.white;
            }
        }

        if (heartAmount > (int)player.GetComponent<Player>().health)
        {            
            for (int i = 0; i < hearts.Length; i++)
            {
                Destroy(hearts[i]);
            }
            heartAmount = 0;
        }
    }
}
