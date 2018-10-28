using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public float bulletSpeed;

	void Update () {
        transform.Translate(Vector3.forward * bulletSpeed * Time.deltaTime);
	}
}
