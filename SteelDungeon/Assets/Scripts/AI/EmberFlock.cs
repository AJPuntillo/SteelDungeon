using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmberFlock : MonoBehaviour
{
    public int maxBoids;
    public GameObject boid;
    List<GameObject> boids = new List<GameObject>();

    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < maxBoids; i++)
        {
            boids.Add(Instantiate(boid, Vector3.zero, transform.rotation));
        }

        foreach (GameObject boid in boids)
        {
            boid.SetActive(true);
            boid.GetComponent<EmberBoid>().flock = this;
        }
    }

    public List<GameObject> GetBoids()
    {
        return boids;
    }
}
