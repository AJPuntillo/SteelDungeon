using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour {

    public GameObject text;
    public GameObject particle;
    public GameObject[] items;

    // Use this for initialization
    void Start()
    {
        text.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            text.SetActive(true);
        }
        if (other.tag == "PlayerWeapon" && text.activeSelf)
        {
            Instantiate(particle, transform.position, transform.rotation);
            Invoke("SpawnItem", 0.2f);
            gameObject.SetActive(false);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            text.SetActive(false);
        }
    }

    void SpawnItem()
    {
        int rand = Random.Range(0, items.Length);
        Instantiate(items[rand], transform.position, transform.rotation, transform.parent);

        GameObject p =Instantiate(particle, transform.position, transform.rotation);

        ParticleSystem pS = p.GetComponent<ParticleSystem>();
        var col = pS.colorOverLifetime;

        Gradient grad = new Gradient();
        grad.SetKeys(new GradientColorKey[] { new GradientColorKey(items[rand].GetComponent<Item>().color, 0.0f), new GradientColorKey(items[rand].GetComponent<Item>().color, 1.0f) }, new GradientAlphaKey[] { new GradientAlphaKey(items[rand].GetComponent<Item>().color.a, 0.5f), new GradientAlphaKey(0.0f, 2.0f) });

        col.color = grad;

        
    }
}
