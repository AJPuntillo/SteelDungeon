using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour {

    public void AttackStart()
    {
        transform.parent.Find("HitBox").GetComponent<BoxCollider2D>().enabled = true;
    }

    public void AttackFinish()
    {
        transform.parent.Find("HitBox").GetComponent<BoxCollider2D>().enabled = false;
    }

    public void Reset()
    {
        if(transform.parent.gameObject.GetComponent<BossFSM>() != null)
        {
            transform.parent.gameObject.GetComponent<BossFSM>().ResetDecisionTimer();
        }
    }
}
