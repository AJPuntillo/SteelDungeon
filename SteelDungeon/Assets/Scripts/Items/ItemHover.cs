using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHover : MonoBehaviour {

    bool up = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if(transform.localPosition.y >= 0.05f)
        {
            up = true;
        }
        else if (transform.localPosition.y <= -0.05f)
        {
            up = false;
        }

        if (up)
        {
            transform.localPosition =
                    new Vector3(transform.localPosition.x, transform.localPosition.y - Time.deltaTime / 10, transform.localPosition.z);
        }
        else
        {
            transform.localPosition =
                new Vector3(transform.localPosition.x, transform.localPosition.y + Time.deltaTime / 10, transform.localPosition.z);
        }
    }
}
