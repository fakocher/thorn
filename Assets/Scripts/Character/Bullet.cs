using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public float speed = 100;
    public float damage = 10;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        transform.Translate(Vector3.right * speed * 0.5f * Time.deltaTime);
	}

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Enemy") && other.gameObject.GetComponent<Zombie>() != null) {
            other.gameObject.GetComponent<Zombie>().health -= damage;
            Destroy(gameObject);
        }
    }
}
