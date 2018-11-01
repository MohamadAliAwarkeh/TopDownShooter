using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour 
{
    
    public float bulletSpeed;
    public float bulletLifeTime;

    public GameObject explosionEffect;

    private bool hasExploded = false;

	void Update () 
	{
        transform.Translate(Vector3.forward * bulletSpeed * Time.deltaTime);

        bulletLifeTime -= Time.deltaTime;
        if (bulletLifeTime <= 0)
        {
            Destroy(gameObject);
        }
	}

    void OnCollisionEnter(Collision theCol)
    {
        if (theCol.gameObject.CompareTag("Wall") && !hasExploded)
        {
            Explode();
            hasExploded = true;
        }
    }

    void Explode()
    {
        //Explosion effect
        Instantiate(explosionEffect, transform.position, transform.rotation);

        //Get nearby Objects
        //Add force and damage

        //Destroy gameobject
        Destroy(gameObject);
    }
}
